using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCamTexture;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    private void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("No camera detected!");
            camAvailable = false;
            return;
        }

        for(int i=0; i<devices.Length; i++)
        {
            if(!devices[i].isFrontFacing)
            {
                backCamTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if(backCamTexture == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        backCamTexture.Play();
        background.texture = backCamTexture;

        camAvailable = true;
    }

    private void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)backCamTexture.width / (float)backCamTexture.height;
        fit.aspectRatio = ratio;

        float scaleY = backCamTexture.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orientation = -backCamTexture.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }
}

