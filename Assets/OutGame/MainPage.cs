using UnityEngine;
using UnityEngine.EventSystems;

public class MainPage : Scene_manerger
{

    private MainView _mainView;
    private bool focusl =false;
        
    
    // Start is called before the first frame update
    public override void Start()
    {
        Debug.Log("welcome start");
        
        _mainView = GameObject.FindObjectOfType<MainView>();
        if (_mainView == null)
        {
            Debug.Log("MainPage auto set false");
        }
        
        _mainView.bird.onClick.AddListener(GoToInGame);
        _mainView.team.onClick.AddListener(delegate { SwitchPage("Team"); });
        _mainView.cacha.onClick.AddListener(delegate { SwitchPage("GaCha"); });
    }

    // Update is called once per frame
    public override void Update()
    {
        _mainView.bird_saying.enabled = focusl;
        
        if (EventSystem.current.currentSelectedGameObject != _mainView.bird.gameObject)
        {
            focusl = false;
        }
    }

    private void GoToInGame()
    {
        if (focusl)
        {
            Debug.Log(this._mainView.bird.name + " was selected");
            SwitchPage("InGame");
        }
        else
        {
            focusl = true;
        }
    }
    
    private void SwitchPage(string name)
    {
        OutGameManager.instance.GotoScene(name);
    }
    
}
