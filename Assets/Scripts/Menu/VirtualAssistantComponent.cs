using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualAssistantComponent : MonoBehaviour
{
    public GameObject assistantItem;
    public Transform assistantContent;

    private List<GameObject> _assistants = new List<GameObject>();
    private int _index;

    // Start is called before the first frame update
    void Start()
    {
        _index = PlayerPrefs.GetInt("SelectedAssistantIndex");
        Sprite[] sprites = Resources.LoadAll<Sprite>("VirtualAssistants/Images");

        for (int i = 0; i < sprites.Length; i++)
        {
            GameObject item = Instantiate(assistantItem);
            item.GetComponent<VirtualAssistantComponentFiller>().Fill(sprites[i]);
            item.transform.SetParent(assistantContent);
            _assistants.Add(item);
            _assistants[i].SetActive(i == _index);
        }
    }

    public void LeftClicked()
    {
        if (_index > 0)
        {
            _assistants[_index].SetActive(false);
            _index--;
            PlayerPrefs.SetInt("SelectedAssistantIndex", _index);
            PlayerPrefs.Save();
            _assistants[_index].SetActive(true);
        }
    }

    public void RightClicked()
    {
        if (_index < _assistants.Count - 1)
        {
            _assistants[_index].SetActive(false);
            _index++;
            PlayerPrefs.SetInt("SelectedAssistantIndex", _index);
            PlayerPrefs.Save();
            _assistants[_index].SetActive(true);
        }
    }
}
