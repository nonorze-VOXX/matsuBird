using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutGameManager : Singleton<OutGameManager>
{
    private Scene_manerger _fsm;
    protected override void _Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public void Start()
    {
        _fsm.Start();
    }

    private void Update()
    {
        _fsm.Update();
    }


    public void StartGame()
    {
        Debug.Log("press space");
    }
    
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        print(scene.name);
        switch (scene.name)
        {
            case "Main":
                _fsm = new MainPage();
                break;
            case "InGame":
                // :TODO
                _fsm = new TeamPage();
                break;
            case "Team":
                _fsm = new TeamPage();
                break;
            case "GaCha":
                _fsm = new GaChaPage();
                break;
            default:
                break;
        }
    }
    
    public void GotoScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
