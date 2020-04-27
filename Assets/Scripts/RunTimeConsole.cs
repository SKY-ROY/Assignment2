using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunTimeConsole : MonoBehaviour
{
    public AspectRatioFitter fit;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = fit.aspectRatio.ToString();
    }


}
