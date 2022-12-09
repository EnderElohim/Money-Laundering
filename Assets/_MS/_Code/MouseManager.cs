using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.ForceSoftware;
    void Start()
    {
        Vector2 cursorOffset = new Vector2(cursorTexture.width /2.5f, cursorTexture.height / 6);
        Cursor.SetCursor(cursorTexture, cursorOffset, cursorMode);
    }
}
