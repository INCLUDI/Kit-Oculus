using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DataModel;

public class ActivityComponentFiller : MonoBehaviour
{
    public Image image;
    public Text title;
    public Text description;
    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;
    public Sprite starFull;
    public Sprite starEmpty;

    public async void Fill(ActivityConfiguration activity)
    {
        title.text = activity.name;
        description.text = activity.description;
        image.sprite = await Addressables.LoadAssetAsync<Sprite>(activity.image).Task;
        GetComponent<Button>().onClick.AddListener(() => GameManager.instance.LoadActivity(activity));
        if (activity.difficulty >= 1)
        {
            Star1.GetComponent<Image>().sprite = starFull;
        }
        if (activity.difficulty >= 2)
        {
            Star2.GetComponent<Image>().sprite = starFull;
        }
        if (activity.difficulty == 3)
        {
            Star3.GetComponent<Image>().sprite = starFull;
        }

    }

}
