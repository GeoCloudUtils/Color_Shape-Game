using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptUtils.GameUtils
{
    public class GridConstructorSystem : MonoBehaviour
    {
        /// <summary>
        /// Number of rows
        /// </summary>
        public int rows = 0;
        /// <summary>
        /// Number of cols.
        /// </summary>
        public int cols = 0;
        /// <summary>
        /// Prefab to use for construction.
        /// NOTE: prefab should be a rectangular shape with a BoxCollider2D attached. This collider will be used to determine cell dimensions.
        /// </summary>
        public GameObject CellPrefab = null;

        private float cellWidth;
        private float cellHeigth;
        /// <summary>
        /// Check if all parameters are present, generate grid if so.
        /// </summary>
        public void Update()
        {
            if (verifyParams())
            {
                genGrid();
            }
        }
        /// <summary>
        /// Check if all parameters are present.
        /// </summary>
        /// <returns></returns>
        private bool verifyParams()
        {
            return rows != 0 && cols != 0 && CellPrefab != null;
        }
        /// <summary>
        /// Detects cell size from attached BoxCollider2D and constructs the grid.
        /// Adds GridRep to the newly created grid. If completed succesfully it will destroy its self.
        /// </summary>
        private void genGrid()
        {
            cellWidth = CellPrefab.GetComponent<BoxCollider2D>().size.x;
            cellHeigth = CellPrefab.GetComponent<BoxCollider2D>().size.y;
            for (int i = 0; i < rows; i++)
            {
                GameObject rowObject = new GameObject("row" + i);
                rowObject.transform.parent = gameObject.transform;
                rowObject.transform.localPosition = new Vector3(0, (cellHeigth * i) * -1);
                for (int j = 0; j < cols; j++)
                {
#if UNITY_EDITOR
                    GameObject cellObject = PrefabUtility.InstantiatePrefab(CellPrefab) as GameObject;
#else
                    GameObject cellObject=Instantiate<GameObject>(CellPrefab);
#endif
                    cellObject.name = "cell_" + i + "_" + j;
                    cellObject.transform.parent = rowObject.transform;
                    cellObject.transform.localPosition = new Vector3(cellWidth * j, 0f);
                }
            }
            gameObject.AddComponent<GridRepG>();
            DestroyImmediate(this);
        }
    }
}