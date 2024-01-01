using System.Collections.Generic;
using UnityEngine;

public class GachaPage1 : Scene_manerger
{
    private GachaView _gachaView;

    private readonly List<string> birdPath = new()
    {
        "b1",
        "b2",
        "b3",
        "b4"
    };

    private bool IsBirdComed;

    public override void Start()
    {
        _gachaView = Object.FindObjectOfType<GachaView>();
        _gachaView.kami.onClick.AddListener(PlayKasu);
        _gachaView.backButton.onClick.AddListener(BackMain);
        Debug.Log(_gachaView.backButton);
    }

    public override void Update()
    {
    }

    private void changeTostoImage()
    {
        if (_gachaView.pan != null && _gachaView.tostoList.Length > 0 && !IsBirdComed)
        {
            var randomIndex = Random.Range(0, _gachaView.tostoList.Length);
            var tostoImage = _gachaView.tostoList[randomIndex];
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

    private TeamBird RandomBird()
    {
        var randomNumber = Random.Range(1, 5);
        var path = birdPath[randomNumber - 1];
        _gachaView.bird.sprite = Resources.Load<Sprite>("Image/bird/" + path + "_f");

        return new TeamBird(path, randomNumber);
    }

    private void PlayKasu()
    {
        _gachaView.panClick.Play();
        changeTostoImage();
        _gachaView.panKasu.Play();
    }

    private void BackMain()
    {
        OutGameManager.instance.GotoScene("Main");
    }
}