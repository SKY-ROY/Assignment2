using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCamTexture;
    private Texture2D outputTexture;
    private Texture defaultBackground;

    public string deviceName;
    public string deviceWidth;
    public string deviceHeight;
    public float ratio;

    public RectTransform parentRect;
    public RawImage background;

    private void Start()
    {
        defaultBackground = background.texture;
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
                backCamTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                //flag
                if(true)
                    outputTexture = new Texture2D(backCamTexture.width, backCamTexture.height, TextureFormat.RGBA32, false);
            }
        }

        if(backCamTexture == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        backCamTexture.Play();
        //flag
        if (true)
            background.texture = outputTexture;
        else
            background.texture = backCamTexture;

        deviceName = backCamTexture.deviceName;
        deviceWidth = backCamTexture.width.ToString();
        deviceHeight = backCamTexture.height.ToString();

        camAvailable = true;
    }

    private void Update()
    {
        if (!camAvailable)
            return;
        
        //flag
        if(true)
        {
            FilterMethod(outputTexture, backCamTexture);
        }

        PlaneRescaling();

    }

    void PlaneRescaling()
    {
        ratio = (float)backCamTexture.width / (float)backCamTexture.height;
        //fit.aspectRatio = ratio;

        //float scaleY = backCamTexture.videoVerticallyMirrored ? -1f : 1f;//not needed for android
        background.rectTransform.localScale = new Vector3(1.777f * ((float)backCamTexture.width / 1920f), 0.563f/*scaleY*/, 1f);

        int orientation = -backCamTexture.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }

    /// <summary>
    /// upstream-downstream pipeline method part
    /// </summary>

    void FilterMethod(Texture2D tex1, WebCamTexture bufferWebCamTexture)
    {
        byte[] pngByteArray = ScreenshotWebcam(bufferWebCamTexture);

        //send to and recieve from plugin
        var plugin = new AndroidJavaClass("com.sky5698.unityplugin.PlugInClass");
        pngByteArray = plugin.CallStatic<Byte[]>("ImageProcessingMethod", pngByteArray, bufferWebCamTexture.height, bufferWebCamTexture.width);

        tex1.LoadImage(pngByteArray);
        tex1.Apply();
    }

    //frame texture (screenshot from camera)
    static byte[] ScreenshotWebcam(WebCamTexture wct) 
    { 
        Texture2D colorTex = new Texture2D(wct.width, wct.height, TextureFormat.RGBA32, false); 
        colorTex.LoadRawTextureData(Color32ArrayToByteArray(wct.GetPixels32())); 
        colorTex.Apply(); 
        return colorTex.EncodeToPNG(); 
    }

    //Color32Array to ByteArray
    private static byte[] Color32ArrayToByteArray(Color32[] colors)
    {
        if (colors == null || colors.Length == 0)
            return null;

        int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
        int length = lengthOfColor32 * colors.Length;
        byte[] bytes = new byte[length];

        GCHandle handle = default(GCHandle);
        try
        {
            handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(ptr, bytes, 0, length);
        }
        finally
        {
            if (handle != default(GCHandle))
                handle.Free();
        }

        return bytes;
    }
}

