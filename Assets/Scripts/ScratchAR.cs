using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScratchAR : MonoBehaviour
{
    //Gyro
    private Gyroscope gyro;
    private GameObject cameraContainer;
    private Quaternion rotation;

    //Camera
    private WebCamTexture cam;
    public RawImage background;
    public AspectRatioFitter fit;

    private bool ARready = false;

    // Start is called before the first frame update
    void Start()
    {
        //check if both the services are active
        //Gyroscope
        if(!SystemInfo.supportsGyroscope)
        {
            Debug.Log("This device doesn't have a Gyroscope");
            return;
        }

        //Back Camera
        for(int i=0; i<WebCamTexture.devices.Length; i++)
        {
            if(!WebCamTexture.devices[i].isFrontFacing)
            {
                cam = new WebCamTexture(WebCamTexture.devices[i].name, Screen.width, Screen.height);
                break;
            }
        }

        //If no camera is present, exit
        if(cam == null)
        {
            Debug.Log("This device doesn't have a back-camera");
            return;
        }

        //Both services are supported, activation starts here
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyro = Input.gyro;
        gyro.enabled = true;

        cameraContainer.transform.rotation = Quaternion.Euler(90f, 0, 0);
        rotation = new Quaternion(0, 0, 1, 0);

        cam.Play();
        background.texture = cam;

        ARready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(ARready)
        {
            //Update Camera
            float ratio = (float)cam.width / (float)cam.height;
            fit.aspectRatio = ratio;

            float scaleY = cam.videoVerticallyMirrored ? -1.0f : 1.0f;
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

            int orientation = -cam.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);

            //Update Gyro
            transform.localRotation = gyro.attitude * rotation;
        }
    }
}
