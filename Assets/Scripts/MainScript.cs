using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public bool highRes = false;
    public bool isFilterOn = true;
    public Image CameraImage;
    public RectTransform rectController_UI;

    private Gyroscope gyro;
    private GameObject cameraContainer;
    private Quaternion rotation;

    private bool camAvailable;
    private WebCamTexture _webcam;
    private Texture2D _cameraTexture;
    private float camFps;
    private int renderedFrameCount = 0;

    private bool ARready = false;

    [DllImport("CPPToUnity")]
    private static extern int ProcessImage(ref Color32[] rawImage, int width, int height, int operationCount);    

    void Start()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            Debug.Log("This device doesn't have a Gyroscope");
            return;
        }
        if (highRes)
        {
            WebCamDevice[] devices = WebCamTexture.devices;

            if (devices.Length == 0)
            {
                Debug.Log("No camera detected!");
                camAvailable = false;
                return;
            }

            for (int i = 0; i < devices.Length; i++)
            {
                if (!devices[i].isFrontFacing)
                {
                    _webcam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                }
            }

            if (_webcam == null)
            {
                Debug.Log("Unable to find back camera");
                return;
            }
        }
        else
        {
            _webcam = new WebCamTexture();
        }

        //Both services are supported, activation starts here
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyro = Input.gyro;
        gyro.enabled = true;

        cameraContainer.transform.rotation = Quaternion.Euler(90f, 0, 0);
        rotation = new Quaternion(0, 0, 1, 0);

        _webcam.Play();
        _cameraTexture = new Texture2D(_webcam.width, _webcam.height);
        CameraImage.material.mainTexture = _cameraTexture;

        ARready = true;
    }

    void Update()
    {
        if(ARready)
        {
            if (_webcam.isPlaying)
            {
                var rawImage = _webcam.GetPixels32();
                if (isFilterOn)
                {
                    renderedFrameCount = ProcessImage(ref rawImage, _webcam.width, _webcam.height, renderedFrameCount);
                }
                else
                {
                    //flip vertically 180 deg(y axis)
                }

                _cameraTexture.SetPixels32(rawImage);
                PlaneRescale();
                _cameraTexture.Apply();

                //debug info
                camFps = renderedFrameCount / Time.timeSinceLevelLoad;
                Debug.Log("Execution count: " + renderedFrameCount);
                Debug.Log("***Image Rendered*** " + camFps);
            }

            transform.localRotation = gyro.attitude * rotation;
        }
        
    }

    void PlaneRescale()
    {
        DebugInfo();

        rectController_UI.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.currentResolution.width);//_webcam.height);
        rectController_UI.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.currentResolution.height);//_webcam.width);

        int orientation = -_webcam.videoRotationAngle;
        CameraImage.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }
    void DebugInfo()
    {
        float ratio = (float)_webcam.width / (float)_webcam.height;
        //w:2336; h:1080;
        Debug.Log("Camera Ratio:" + ratio.ToString());
        Debug.Log("Screen W:" + Screen.currentResolution.width);
        Debug.Log("Screen H:" + Screen.currentResolution.height);
        Debug.Log("Image W:" + rectController_UI.rect.width);
        Debug.Log("Image H:" + rectController_UI.rect.height);
        Debug.Log("Cam W:" + _webcam.width);
        Debug.Log("Cam H:" + _webcam.height);
    }
}