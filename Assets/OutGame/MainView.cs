using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    public Button team;
    public Button cacha;
    public Button bird;
    public Image bird_saying;
    public AudioClip bgm;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        team.onClick.RemoveAllListeners();
        cacha.onClick.RemoveAllListeners();
        bird.onClick.RemoveAllListeners();
    }
}
