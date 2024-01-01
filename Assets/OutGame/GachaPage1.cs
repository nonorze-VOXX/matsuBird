using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GachaPage1 : Scene_manerger
{
    private GachaView _gachaView;
    private bool IsBirdComed = false;
    private int birdType = 1;

    private List<string> birdPath = new List<string>()
    {
        "b1",
        "b2",
        "b3",
        "b4"
    };
    public  override void Start()
    {
        _gachaView = GameObject.FindObjectOfType<GachaView>();
        _gachaView.kami.onClick.AddListener(PlayKasu);
        _gachaView.backButton.onClick.AddListener(BackMain);
        Debug.Log(_gachaView.backButton);
    }

    public override void Update()
    {
        if (!_gachaView.birdCome.isPlaying && IsBirdComed)
        {
            OutGameManager.instance.SetCurrentDetail(birdType);
            OutGameManager.instance.GotoScene("BirdDetail");
        }
        
    }
    
    void changeTostoImage()
    {
        if (_gachaView.pan != null && _gachaView.tostoList.Length > 0 && !IsBirdComed)
        {
            int randomIndex = Random.Range(0, _gachaView.tostoList.Length);
            Sprite tostoImage = _gachaView.tostoList[randomIndex];
            if (randomIndex == 1)
            {
                _gachaView.birdCome.Play();
                OutGameManager.instance.AddBirdList(RandomBird());
                _gachaView.panKasu.gameObject.SetActive(false);
                IsBirdComed = true;
            }

            _gachaView.pan.sprite = tostoImage;
        } 
    }

    TeamBird RandomBird()
    {
        birdType = Random.Range(1, 5);
        string path = birdPath[birdType-1];
        _gachaView.bird.sprite = Resources.Load<Sprite>("Image/bird/" + path + "_f");
        
        return new TeamBird(path,birdType);
    }
    
    void PlayKasu()
    {
        _gachaView.panClick.Play();
        changeTostoImage();
        _gachaView.panKasu.Play();
    }

    void BackMain()
    {
        Debug.Log("ho to main");
        OutGameManager.instance.GotoScene("Main");
    }

}
