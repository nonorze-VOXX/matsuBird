using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class TeamView : MonoBehaviour
{
    public Button backButton;
    private Image _left, _right;
    public Button _leftB, _rightB;
    public GameObject team_1, team_2, team_3;
    public GameObject bird, onePage;
    private int onePageBirdNumber = 8;
    private List<GameObject> _birdButtonArray = new List<GameObject>();
    private bool createOver = false;
    private PageView _scrool_function;
    private int _birdPageNumber;
    private List<Sprite> _bigBirdSprite = new List<Sprite>();

    private void Start()
    {
        _scrool_function = GameObject.FindObjectOfType<PageView>();
        _left = transform.Find("button/left").GetComponent<Image>();
        _right = transform.Find("button/right").GetComponent<Image>();
        _bigBirdSprite = new List<Sprite>()
        {
            Resources.Load("Image/bird/b", typeof(Sprite)) as Sprite,
            Resources.Load("Image/bird/big_b1", typeof(Sprite)) as Sprite,
            Resources.Load("Image/bird/big_b2", typeof(Sprite)) as Sprite,
            Resources.Load("Image/bird/b3", typeof(Sprite)) as Sprite,
            Resources.Load("Image/bird/big_b4", typeof(Sprite)) as Sprite
        };
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

        createOver = true;
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

    public bool IsCreateOver()
    {
        return createOver;
    }

    public void CloseCreateOver()
    {
        createOver = false;
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

    public void setLeft(bool enable)
    {
        _left.enabled = enable;
    }

    public void setRight(bool enable)
    {
        _right.enabled = enable;
    }

    public List<Sprite> GetBiSprites()
    {
        return _bigBirdSprite;
    }
}
