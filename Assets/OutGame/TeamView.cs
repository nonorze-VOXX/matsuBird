using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class TeamView : MonoBehaviour
{
    public Button backButton;
    public Button _leftB, _rightB;
    public GameObject team_1, team_2, team_3;
    public GameObject bird, onePage;
    private int onePageBirdNumber = 8;
    private List<GameObject> _birdButtonArray = new List<GameObject>();
    private PageView _scrool_function;
    private int _birdPageNumber;

    private void Start()
    {
        _scrool_function = GameObject.FindObjectOfType<PageView>();
    }

    public void CreateOnePage()
    {
        _birdPageNumber = (OutGameManager.instance.GetBirdList().Count / onePageBirdNumber) + 1;

        GameObject.Find("Canvas/Scroll View/Viewport/Content").GetComponent<RectTransform>().sizeDelta =
            new Vector2((_birdPageNumber) *800, 278.09f);

        for (int i = 0; i < _birdPageNumber; i++)
        {
            GameObject onePageGameObject =
                Instantiate(onePage, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            onePageGameObject.transform.SetParent(GameObject.Find("Canvas/Scroll View/Viewport/Content").transform,
                false);

            CreateBird(onePageGameObject, i);
        }

        Debug.Log("Create over");
    }

    public void CreateBird(GameObject target, int page)
    {
        List<TeamBird> birdList = OutGameManager.instance.GetBirdList();
        int totalBirdNumber = birdList.Count;
        int indexStart = page * onePageBirdNumber;

        for (int i = indexStart; i < indexStart + 8; i++)
        {
            if (i >= totalBirdNumber)
            {
                break;
            }

            string path = "Image/bird/" + birdList[i].GetPath();
            Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            GameObject char_item = Instantiate(bird, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            char_item.transform.SetParent(target.transform, false);
            char_item.transform.Find("bird").GetComponent<Image>().sprite = sprite;
            char_item.GetComponent<TeamBird>().SetPath(birdList[i].GetPath());
            char_item.GetComponent<TeamBird>().SetType(birdList[i].GetBirdType());
            _birdButtonArray.Add(char_item);
        }
    }

    public List<GameObject> GetBirdList()
    {
        return _birdButtonArray;
    }

    public int GetCurrentIndex()
    {
        return _scrool_function.GetCurrentIndex();
    }

    public void gotoPage(int page)
    {
        _scrool_function.pageTo(page);
    }

    public int GetBirdPageNumber()
    {
        return _birdPageNumber;
    }

    public void SetLeft(bool enable)
    {
        _leftB.gameObject.SetActive(enable);
    }

    public void SetRight(bool enable)
    {
        _rightB.gameObject.SetActive(enable);
    }

    void OnDestroy()
    {
        backButton.onClick.RemoveAllListeners(); 
        _leftB.onClick.RemoveAllListeners();
        _rightB.onClick.RemoveAllListeners();
        team_1.GetComponent<Button>().onClick.RemoveAllListeners();
        team_2.GetComponent<Button>().onClick.RemoveAllListeners();
        team_3.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
