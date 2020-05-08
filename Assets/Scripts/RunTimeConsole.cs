using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunTimeConsole : MonoBehaviour
{
    public PhoneCamera mainCam;

    private static string existingText;

    // Update is called once per frame
    void Update()
    {
        existingText = "R:" + mainCam.ratio 
            + "\nDN:" + mainCam.deviceName 
            + "\nDW:" + mainCam.deviceWidth 
            + "\nDH:" + mainCam.deviceHeight
            + "\nCW:" + mainCam.camWidth
            + "\nCW:" + mainCam.camHeight;
        GetComponent<Text>().text = existingText;
    }
}
