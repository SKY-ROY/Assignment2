using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunTimeConsole : MonoBehaviour
{
    //public AspectRatioFitter fit;
    public PhoneCamera mainCam;

    private static string existingText;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        existingText = "R:" + mainCam.ratio + "\nN:" + mainCam.deviceName + "\nW:" + mainCam.deviceWidth + "\nH:" + mainCam.deviceHeight;
        GetComponent<Text>().text = existingText;
    }
}
