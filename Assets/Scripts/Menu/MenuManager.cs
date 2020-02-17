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
    public Transform activityWrapper;
    public Transform experienceWrapper;

    private void Start()
    {
        TextAsset file = Resources.Load<TextAsset>("Menu");

        List<ActivityDetails> activityList = JsonUtility.FromJson<MenuItems>(file.text).activities;
        foreach (ActivityDetails activity in activityList)
        {
            GameObject item = Instantiate(carouselItem, carouselItem.transform.position, carouselItem.transform.rotation, activityWrapper);
            item.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = activity.name;
            item.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = activity.description;
            // set image
            item.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.LaunchActivity(activity);});
        }

        List<ExperienceDetails> experienceList = JsonUtility.FromJson<MenuItems>(file.text).experiences;
        /*foreach (ExperienceDetails experience in experienceList)
        {
            GameObject item = Instantiate(carouselItem, carouselItem.transform.position, carouselItem.transform.rotation, experienceWrapper);
            item.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = experience.name;
            item.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = experience.description;
            // set image
            item.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.LaunchExperience(experience); });
        }*/
    }
}
