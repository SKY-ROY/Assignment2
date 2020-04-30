using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTest : MonoBehaviour
{
    public Canvas mainCanvas;
    public RectTransform rectController;
    public RawImage background;

    // Start is called before the first frame update
    void Start()
    {
        createBackground();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createBackground()
    {
        rectController.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1080);
        rectController.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1920);
    }
}
