using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickLongShort :MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    private float delay = 1f;
    private bool isDown = false;
    private float lastIsDownTime;
    private int currentTeamSelect = 1;
    private List<Sprite> _bigBirdSprite = new List<Sprite>();

    private void Start()
    {
        _bigBirdSprite = new List<Sprite>()
        {
            Resources.Load("Image/bird/b", typeof(Sprite)) as Sprite,
            Resources.Load("Image/bird/big_b1", typeof(Sprite)) as Sprite,
            Resources.Load("Image/bird/big_b2", typeof(Sprite)) as Sprite,
            Resources.Load("Image/bird/b3", typeof(Sprite)) as Sprite,
            Resources.Load("Image/bird/big_b4", typeof(Sprite)) as Sprite
        };
    }

    void Update ()
    {
        if (isDown && Time.time - lastIsDownTime > delay)
        {
            //long do
            int type = transform.parent.GetComponent<TeamBird>().GetBirdType();
            OutGameManager.instance.SetCurrentDetail(type);
            OutGameManager.instance.GotoScene("BirdDetail");
            lastIsDownTime = Time.time;
        }
    }
 
    public void OnPointerDown (PointerEventData eventData)
    {
        isDown = true;
        lastIsDownTime = Time.time;
    }
 
    public void OnPointerUp (PointerEventData eventData)
    {
        if (isDown && Time.time - lastIsDownTime <= delay)
        {
            //short do
            Debug.Log("short work");
            int type = transform.parent.GetComponent<TeamBird>().GetBirdType();
            Sprite sprite = _bigBirdSprite[type];
            OutGameManager.instance.SetTeamBirdByID(currentTeamSelect-1,sprite);
        }
        
        isDown = false;
    }

    public void SetCurrentTeamSelect(int current)
    {
        currentTeamSelect = current;
    }
}