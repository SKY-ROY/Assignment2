using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MessageReciever : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text textElement = GetComponent<Text>();
        var plugin = new AndroidJavaClass("com.sky5698.unityplugin.PlugInClass");
        textElement.text = plugin.CallStatic<string>("GetTextFromPlugin", 100);
    }
}
