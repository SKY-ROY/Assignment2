using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PLuginWrapper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text textElement = GetComponent<Text>();
        var plugin = new AndroidJavaClass("com.sky5698.unityplugin.PlugInClass");
        textElement.text = plugin.CallStatic<string>("GetTextFromPlugin", 69);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
