using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCamTexture_WC;
    private Texture2D openCVTexture;
    private Texture defaultBackground;
    private int renderedFrameCount = 0;

    public string deviceName;
    public string deviceWidth;
    public string deviceHeight;
    public string camWidth;
    public string camHeight;
    public float ratio;
    public bool isFilterOn = false;

    //public AspectRatioFitter fit;
    public RectTransform rectController_UI;
    public Image backgroundImage_UI;

    [DllImport("CPPToUnity")]
    private static extern int ProcessImage(ref Color32[] rawImage, int width, int height, int operationCount);

    private void Start()
    {
        defaultBackground = backgroundImage_UI.material.mainTexture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected!");
            camAvailable = false;
            return;
        }

        for(int i=0; i<devices.Length; i++)
        {
            if(!devices[i].isFrontFacing)
            {
                backCamTexture_WC = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if(backCamTexture_WC == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        backCamTexture_WC.Play();
        openCVTexture = new Texture2D(backCamTexture_WC.width, backCamTexture_WC.height);
        if(isFilterOn)
        {
            backgroundImage_UI.material.mainTexture = openCVTexture;//render this if openCV processing is required
        }
        else
        {
            backgroundImage_UI.material.mainTexture = backCamTexture_WC;//render this if no openCV procesing is required;
        }

        //debug info
        deviceName = backCamTexture_WC.deviceName;
        camWidth = backCamTexture_WC.width.ToString();
        camHeight = backCamTexture_WC.height.ToString();
        deviceWidth = Screen.currentResolution.width.ToString();
        deviceHeight = Screen.currentResolution.height.ToString();
        camAvailable = true;
    }

    private void Update()
    {
        if (!camAvailable)
            return;

        PlaneRescaling();
    }

    void PlaneRescaling()
    {
        ratio = (float)backCamTexture_WC.width / (float)backCamTexture_WC.height;
        //fit.aspectRatio = ratio;

        if(isFilterOn)
        {
            //**************************OpenCV processing part****************************
            //Obtain pixel-data from WebCamTexture and send it to openCV for processing
            var rawImage = backCamTexture_WC.GetPixels32();
            renderedFrameCount = ProcessImage(ref rawImage, backCamTexture_WC.width, backCamTexture_WC.height, renderedFrameCount);

            //Apply the processed pixel-data as texture through gpu
            openCVTexture.SetPixels32(rawImage);
            openCVTexture.Apply();
            //*************************Ends Here******************************************
        }

        //******************UI Element rescaling after rendering image part***********
        //redrawing the UI_RawImage element to fit on screen
        rectController_UI.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, backCamTexture_WC.height);
        rectController_UI.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, backCamTexture_WC.width);

        //float scaleY = backCamTexture.videoVerticallyMirrored ? -1f : 1f;//not needed for android
        //background.rectTransform.localScale = new Vector3(1.777f * ((float)backCamTexture.width / 1920f), 0.563f/*scaleY*/, 1f);

        //rotating the UI_RawImage to match the proportions
        int orientation = -backCamTexture_WC.videoRotationAngle;
        backgroundImage_UI.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
        //*************************Ends Here******************************************

    }
}

