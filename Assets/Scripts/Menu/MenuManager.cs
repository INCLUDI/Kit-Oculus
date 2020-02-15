using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        TextAsset[] files = Resources.LoadAll<TextAsset>("ChristmasScene/");

        foreach (TextAsset j in files)
        {
            SceneConfiguration sceneConfiguration = JsonUtility.FromJson<SceneConfiguration>(j.text);
            sceneConfigurations.Add(sceneConfiguration);

            GameObject item = Instantiate(carouselItem, carouselItem.transform.position, carouselItem.transform.rotation, carouselWrapper);
            item.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = sceneConfiguration.sceneName;
            item.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = sceneConfiguration.sceneName;
        }
    }

    
}
