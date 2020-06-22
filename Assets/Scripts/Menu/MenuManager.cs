using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Android;
using UnityEngine.UI;
using static DataModel;

public class MenuManager : MonoBehaviour
{
    public GameObject scrollItem;
    public ScrollRect activityScrollView;
    public Transform activityContent;
    //public ScrollRect experieceScrollView;
    //public Transform experienceContent;
    //public Button activityButton;
    //public Button experienceButton;
    //public Sprite activeSprite;
    //public Sprite inactiveSprite;

    private void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        Addressables.LoadAssetsAsync<TextAsset>("_JSON", null).Completed += JsonLoaded;
    }

    void JsonLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<IList<TextAsset>> addressables)
    {
        List<TextAsset> files = addressables.Result as List<TextAsset>;

        foreach (TextAsset file in files)
        {
            ActivityConfiguration activity = JsonUtility.FromJson<ActivityConfiguration>(file.text);
            GameObject item = Instantiate(scrollItem);
            item.GetComponent<ActivityComponentFiller>().Fill(activity);
            item.transform.SetParent(activityContent);
        }
        activityScrollView.verticalNormalizedPosition = 1;

        //experieceScrollView.verticalNormalizedPosition = 1;

        //experienceButton.onClick.AddListener(() =>
        //{
        //    experieceScrollView.gameObject.SetActive(true);
        //    activityScrollView.gameObject.SetActive(false);
        //    experienceButton.GetComponent<Image>().sprite = activeSprite;
        //    activityButton.GetComponent<Image>().sprite = inactiveSprite;
        //});

        //activityButton.onClick.AddListener(() =>
        //{
        //    experieceScrollView.gameObject.SetActive(false);
        //    activityScrollView.gameObject.SetActive(true);
        //    experienceButton.GetComponent<Image>().sprite = inactiveSprite;
        //    activityButton.GetComponent<Image>().sprite = activeSprite;
        //});

        /*TextAsset playlistFile = Resources.Load<TextAsset>("Playlists");
        List<Playlist> playlists = JsonUtility.FromJson<PlaylistList>(playlistFile.text).playlists;
        foreach (Playlist playlist in playlists)
        {
            GameObject item = Instantiate(scrollItem, scrollItem.transform.position, scrollItem.transform.rotation, activityWrapper);
            item.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = name;
            List<string> activitiesNames = new List<string>();
            foreach (ActivityConfiguration activity in playlist.activities)
            {
                activitiesNames.Add(activity.name);
            }
            item.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.LaunchGame(playlist.activities); });
        }*/
    }
}