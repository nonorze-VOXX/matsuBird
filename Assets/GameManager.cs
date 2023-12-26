using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private FSM fsm;

    private void Start()
    {
        fsm.Setup();
    }

    protected override void _Awake()
    {
        base._Awake();
        DontDestroyOnLoad(gameObject);
        fsm = new FSM01();
        fsm.Setup();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // switch (arg0.name)
        // {
        //     case "WelcomePage":
        fsm = new FSM02();
        //         break;
        // }
    }

    public void UpdateFSM()
    {
        fsm.Setup();
    }
}