
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

public class TerrainDrawer : MonoBehaviour
{
    public Terrain terrain;
    [Range(10, 100)] public int radius;
    [Range(0, 1)] public float initialHeight;//How much height on all surface when started level
    [Range(0, 1)] public float drawHeight; //How much height set on draw

    private TerrainData terrainData;
    private int diameter; //Brush diameter: How much terrain is destroying on mouse move
    private int sqrDiameter; //Otimisation: Square root of diameter
    private float heightModifier; // Otimisation: Translates world Z coord to terrain Z coord
    private float widthModifier; // Otimisation: Translates world X coord to terrain X coord
    private Queue<Vector3> mousePoints = new Queue<Vector3>(); //Queue to put points when mouse moved and get points in Draw coroutine
    private object mousePointsLock = new object(); //Lock for mousePoints queue 
    private bool running = false; //If Draw coroutine running

    //INFO: Get points from mousePoints queue and deform mesh
    public IEnumerator Draw()
    {
        Func<bool> waitPredicate = () => { return mousePoints.Count != 0; };
        WaitUntil waitUntil = new WaitUntil(waitPredicate);
        running = true;

        while (running)
        {
            //Debug.LogFormat("Draw Tick: {0}", mousePoints.Count);

            if (mousePoints.Count != 0)
            {
                lock (mousePointsLock)
                {
                    Vector3 point = mousePoints.Dequeue();

                    int centerX = (int)(point.x * widthModifier);
                    int centerY = (int)(point.z * heightModifier);

                    DeformAt(centerX, centerY);
                }
            }

            yield return waitUntil;
        }
    }


    //INFO: Change vertices height by position and radius
    public void DeformAt(int centerX, int centerY)
    {
        int X = centerX - radius;
        int Y = centerY - radius;
        float[,] heights = terrainData.GetHeights(X, Y, diameter, diameter);
        Vector2 center = new Vector2(centerX, centerY);

        for (int x = 0; x < diameter; ++x)
        {
            for (int y = 0; y < diameter; ++y)
            {
                float currentRadiusSqr = (center - new Vector2(X + x, Y + y)).sqrMagnitude;
                if (currentRadiusSqr < sqrDiameter)
                {
                    heights[x, y] = drawHeight;
                }

            }
        }

        terrainData.SetHeightsDelayLOD(X, Y, heights);
    }

    private void Start()
    {
        //INFO: Some checks...
        if (terrain == null) { Debug.LogError("terrain is not assigned!"); }
        if (radius <= 0) { Debug.LogError("radius is 0!(Maybe forgot to set in Editor?)"); }
        if (initialHeight <= 0) { Debug.LogError("initialHeight is 0!(Maybe forgot to set in Editor?)"); }
        if (drawHeight <= 0) { Debug.LogError("drawHeight is 0!(Maybe forgot to set in Editor?)"); }

        //INFO: Get some info from terrain...
        terrainData = terrain.terrainData;
        float terrainHeightMapWidth = terrainData.heightmapResolution;
        float terrainHeightMapHeight = terrainData.heightmapResolution;
        float terrainWidth = terrainData.size.x;
        float terrainHeight = terrainData.size.z;


        //INFO: Optimisation. Cache used in Update values.
        diameter = radius + radius;
        sqrDiameter = radius * radius;
        widthModifier = (1.0f / terrainWidth) * terrainHeightMapWidth;
        heightModifier = (1.0f / terrainHeight) * terrainHeightMapHeight;

        //INFO: Nivelate all map to initial height
        float[,] heights = terrainData.GetHeights(0, 0, (int)terrainHeightMapWidth, (int)terrainHeightMapHeight);
        for (int i = 0; i < heights.GetLength(0); ++i)
        {
            for (int k = 0; k < heights.GetLength(1); ++k)
            {
                heights[k, i] = initialHeight;
            }
        }
        terrainData.SetHeights(0, 0, heights);

        //INFO: Start draw coroutine
        StartCoroutine(Draw());

        //INFO: Just some info
        Debug.LogFormat("HeightMapSize: {0}x{1}", terrainHeightMapWidth, terrainHeightMapHeight);
        Debug.LogFormat("widthModifier: {0}", widthModifier);
        Debug.LogFormat("heightModifier: {0}", heightModifier);
    }



    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                lock (mousePointsLock)
                {
                    mousePoints.Enqueue(hit.point);
                }
            }
        }
    }

    private void OnDestroy()
    {
        lock (mousePoints)
        {
            mousePoints.Clear();
        }
        running = false;
        StopCoroutine(Draw());
        terrainData.SyncHeightmap();
    }
}