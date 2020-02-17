using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DataModel;

public class MenuManager : MonoBehaviour
{
    public GameObject carouselItem;
    public Transform carouselWrapper;

    private List<SceneConfiguration> sceneConfigurations = new List<SceneConfiguration>();

    private void Start()
    {
        TextAsset file = Resources.Load<TextAsset>("Menu");

        List<ActivityDetails> activityList = JsonUtility.FromJson<MenuItems>(file.text).activities;
        foreach (ActivityDetails activity in activityList)
        {
            GameObject item = Instantiate(carouselItem, carouselItem.transform.position, carouselItem.transform.rotation, carouselWrapper);
            item.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = activity.name;
            item.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = activity.desctiption;
            item.GetComponent<Button>().onClick.AddListener(() => { LaunchActivity(activity.scene);});
        }

        List<ExperienceDetails> experienceList = JsonUtility.FromJson<MenuItems>(file.text).experiences;
    }
    
    public void LaunchActivity(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}
