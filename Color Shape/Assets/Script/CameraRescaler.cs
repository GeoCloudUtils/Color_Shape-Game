using UnityEngine;

public class CameraRescaler : MonoBehaviour
{
    private int _screenSizeX = 0;
    private int _screenSizeY = 0;

    private void Start()
    {
        RescaleCamera();
    }

#if UNITY_EDITOR
    private void Update()
    {
        RescaleCamera();
    }
#endif
    private void OnPreCull()
    {
        if (Application.isEditor) return;
        Rect wp = Camera.main.rect;
        Rect nr = new Rect(0, 0, 1, 1);
        Camera.main.rect = nr;
        GL.Clear(true, true, Color.black);

        Camera.main.rect = wp;
    }

    private void RescaleCamera()
    {
        if (Screen.width == _screenSizeX && Screen.height == _screenSizeY) return;
        float targetaspect = 16.0f / 9.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;
        Camera camera = GetComponent<Camera>();
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;
            Rect rect = camera.rect;
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
        _screenSizeX = Screen.width;
        _screenSizeY = Screen.height;
    }
}
