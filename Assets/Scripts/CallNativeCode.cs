using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class CallNativeCode : MonoBehaviour
{
    [DllImport("SharedObject1")]
    private static extern int HandShake();
    
    [DllImport("CPPToUnity")]
    private static extern int ComputerVision(int num);
    
    [DllImport("CPPToUnity")]
    private static extern int ProcessImage(ref Color32[] rawImage, int width, int height, int operationCount);

    void OnGUI()
    {
        // This Line should display "Returned response: 10"
        GUI.Label(new Rect(15, 125, 450, 100), "Returned response: " + ComputerVision(2));
        GUI.Label(new Rect(15, 150, 450, 100), "Returned response: " + HandShake());
    }
    
}