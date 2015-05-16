using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
    float deltaTime = 0.0f;
    Texture2D background = null;
    public bool useBackground = true;
    int lastW = -1;
    int lastH = -1;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        if ((lastW != w || lastH != h || background == null) && useBackground)
        {
            lastW = w;
            lastH = h;
            background = makeTexture(w, h / 2, new Color(255.0f, 255.0f, 255.0f));
        }

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        if (background != null)
        {
            style.normal.background = background;
        }
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        Debug.Log(text);
        GUI.Label(rect, text, style);
    }

    Texture2D makeTexture(int width, int height, Color c)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = c;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}