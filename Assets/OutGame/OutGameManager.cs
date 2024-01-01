using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OutGameManager : Singleton<OutGameManager>
{
    public AudioClip duckQuack;
    public AudioSource audioSourceSE;
    public AudioSource audioSourceBGM;
    private Scene_manerger _fsm;
    private int currentDetailType = 1;

    private List<Sprite> teamBird = new List<Sprite>()
    {
        null,
        null,
        null
    };

    private List<TeamBird> birdList = new List<TeamBird>()
    {
        new TeamBird("b", 0),
        new TeamBird("b1", 1),
        new TeamBird("b2", 2),
        new TeamBird("b3", 3),
        new TeamBird("b3", 3),
        new TeamBird("b3", 3),
        new TeamBird("b3", 3),
        new TeamBird("b3", 3),
        new TeamBird("b3", 3),
        new TeamBird("b4", 4),
    };
    
    protected override void _Awake()
    {
        Debug.Log("GM Awake");
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(audioSourceSE);
        DontDestroyOnLoad(audioSourceBGM);
        SceneManager.sceneLoaded += OnSceneLoad;
        audioSourceBGM.loop = true;
        audioSourceSE.volume = 0.3f;
    }

    private void Update()
    {
        if (_fsm != null)
        {
            _fsm.Update();
        }
    }

    public void StartGame()
    {
        Debug.Log("press space");
    }
    
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
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
                _fsm = new GachaPage1();
                break;
            case "BirdDetail":
                _fsm = new DetailPage();
                break;
            default:
                break;
        }

        if (_fsm != null)
        {
            DuckQuack();
            _fsm.Start();
        }
    }
    
    public void GotoScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public List<TeamBird> GetBirdList()
    {
        return birdList;
    }
    public void AddBirdList(TeamBird teamBird)
    {
        birdList.Add(teamBird);
    }
    public List<Sprite> GetTeamBird()
    {
        return teamBird;
    }
    
    public void SetTeamBirdByID(int index, Sprite sprite)
    {
        teamBird[index] = sprite;
    }


    private void DuckQuack()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (var button in buttons)
        {
            button.onClick.AddListener(AddDuckToButton);
        }
    }

    private void AddDuckToButton()
    {
        audioSourceSE.PlayOneShot(duckQuack);
    }

    public void SetCurrentDetail(int type)
    {
        currentDetailType = type;
    }

    public int GetCurrentDetail()
    {
        return currentDetailType;
    }
    
}
