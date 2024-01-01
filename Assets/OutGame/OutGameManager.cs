using System.Collections.Generic;
using InGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutGameManager : Singleton<OutGameManager>
{
    public AudioClip duckQuack;
    public AudioSource audioSourceSE;
    public AudioSource audioSourceBGM;

    public BirdData birdData;

    private Scene_manerger _fsm;


    private int currentDetailType = 1;

    private void Update()
    {
        if (_fsm != null) _fsm.Update();
    }

    protected override void _Awake()
    {
        Debug.Log("GM Awake");
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(audioSourceSE);
        DontDestroyOnLoad(audioSourceBGM);
        SceneManager.sceneLoaded += OnSceneLoad;
        audioSourceBGM.loop = true;
        audioSourceSE.volume = 0.3f;
        birdData.teamBird = new List<Sprite>
        {
            null,
            null,
            null
        };
        birdData.birdList = new List<TeamBird>
        {
            new("b", 0),
            new("b1", 1),
            new("b2", 2),
            new("b3", 3),
            new("b3", 3),
            new("b3", 3),
            new("b3", 3),
            new("b3", 3),
            new("b3", 3),
            new("b4", 4)
        };
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
        return birdData.birdList;
    }

    public void AddBirdList(TeamBird teamBird)
    {
        birdData.birdList.Add(teamBird);
    }

    public List<Sprite> GetTeamBird()
    {
        return birdData.teamBird;
    }

    public void SetTeamBirdByID(int index, Sprite sprite)
    {
        birdData.teamBird[index] = sprite;
    }


    private void DuckQuack()
    {
        var buttons = FindObjectsOfType<Button>();
        foreach (var button in buttons) button.onClick.AddListener(AddDuckToButton);
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