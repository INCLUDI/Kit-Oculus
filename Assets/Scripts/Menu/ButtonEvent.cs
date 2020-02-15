using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    private string sceneName;

    private void Start()
    {
        sceneName = transform.GetChild(1).GetChild(0).GetComponent<Text>().text;
    }

    public void SceneLauncher()
    {
        SceneManager.LoadScene(sceneName);
    }
}
