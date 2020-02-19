using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DataModel;

public class GameManager : MonoBehaviour
{
    private List<string> sceneList = new List<string>();
    private List<string> jsonPathList = new List<string>();
    private int _sceneStep;

    public static GameManager instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void LaunchGame(List<ActivityDetails> activities)
    {
        foreach (ActivityDetails activity in activities)
        {
            sceneList.Add(activity.scene);
            jsonPathList.Add(activity.jsonPath);
        }
        ChangeScene();
    }

    public void NextScene()
    {
        _sceneStep++;
        ChangeScene();
    }

    private void ChangeScene()
    {
        if (_sceneStep >= sceneList.Count)
        {
            _sceneStep = 0;
            SceneManager.LoadScene("MenuScene");
            sceneList = new List<string>();
            jsonPathList = new List<string>();
        }
        else
        {
            SceneManager.LoadScene(sceneList[_sceneStep]);
        }
    }

    public string Getjson()
    {
        return jsonPathList[_sceneStep];
    }
}
