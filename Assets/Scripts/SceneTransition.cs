using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Start is called before the first frame update
    public void SwitchScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
