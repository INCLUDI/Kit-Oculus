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

    public void LaunchActivity(ActivityDetails activity)
    {
        sceneList = new List<string> { activity.scene };
        jsonPathList = new List<string> { activity.jsonPath };
        ChangeScene();
    }

    public void LaunchExperience(ExperienceDetails experience)
    {
        foreach (ActivityDetails activity in experience.activities)
        {
            sceneList.Add(activity.scene);
            jsonPathList.Add(activity.jsonPath);
        }
        ChangeScene();
    }

    public void ChangeScene()
    {
        _sceneStep++;

        if (_sceneStep >= sceneList.Count)
        {
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
