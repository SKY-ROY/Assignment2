using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TextureTest : MonoBehaviour
{
    public GameObject friendObj;
    void Start()
    {
        var texture1 = new Texture2D(128, 128, TextureFormat.RGBA32, false);
        var texture2 = new Texture2D(128, 128, TextureFormat.RGBA32, false);

        GetComponent<Renderer>().material.mainTexture = texture1;
        friendObj.GetComponent<Renderer>().material.mainTexture = texture2;

        // RGBA32 texture format data layout exactly matches Color32 struct
        var data = texture1.GetRawTextureData<Color32>();
        Color32[] color32Array = new Color32[data.Length];

        // fill texture data with a simple pattern
        Color32 orange = new Color32(255, 165, 0, 255);
        Color32 teal = new Color32(0, 128, 128, 255);

        //Debug.Log(data);
        //Debug.Log(orange.ToString());

        //procedurally generated texture
        int index = 0;
        for (int y = 0; y < texture1.height; y++)
        {
            for (int x = 0; x < texture1.width; x++)
            {
                data[index] = ((x & y) == 0 ? orange : teal);
                color32Array[index] = data[index];//Unity.Collections.NativeArray<Color32> to a Color32 Array
                index++;
            }
        }

        if(false)
        {
            //Byte-Array for communication
            Byte[] byteArray = Color32ArrayToByteArray(color32Array);//texture1.GetRawTextureData();
            Debug.Log(byteArray.Length);
            
            texture2.LoadRawTextureData(byteArray);

            //upload to the GPU
            texture2.Apply();
        }

        if(true)
        {
            FilterMethod(texture2);
        }

        // upload to the GPU
        texture1.Apply();
    }

    void FilterMethod(Texture2D tex)
    {
        byte[] pngByteArray = ScreenshotWebcam(tex);

        //send to plugin
        var plugin = new AndroidJavaClass("com.sky5698.unityplugin.PlugInClass");
        pngByteArray = plugin.CallStatic<Byte[]>("ImageProcessingMethod", pngByteArray);

        tex.LoadImage(pngByteArray);
        tex.Apply();
    }

    static byte[] ScreenshotWebcam(Texture2D wct/*WebCamTexture wct*/)
    {
        Texture2D colorTex = new Texture2D(wct.width, wct.height, TextureFormat.RGBA32, false);
        colorTex.LoadRawTextureData(Color32ArrayToByteArray(wct.GetPixels32()));
        //colorTex.Apply();
        return colorTex.EncodeToPNG();
    }

    //Color32 to ByteArray
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
