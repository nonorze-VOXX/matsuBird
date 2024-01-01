using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace InGame
{
    internal enum GameFlowState
    {
        Prepare,
        Flying,
        NextMap,
        GameOver,
        Win
    }

    internal enum groundState
    {
        Normal,
        Fire,
        Food,
        FireWall,
        NEST
    }

    public class InGameManager : MonoBehaviour
    {
        public GameObject birdPrefab;
        public GameConfig gameConfig;
        public Rigidbody2D birdRigidbody2D;
        public GameObject firePrefab;
        public GameObject nestPrefab;
        public List<Fire> fires = new();
        public GameObject fireWallPrefab;
        public GameObject foodPrefab;

        public Camera mainCamera;
        public GameObject hpBar;
        public GameObject createWall;
        public GameObject StartButton;
        public GameObject GameOverPanel;
        public GameObject GameWinPanel;
        public BirdData birdData;

        public BirdPictureObject birdPictureObject;
        private readonly List<GameObject> fireWalls = new();
        private readonly List<GameObject> foods = new();
        private readonly List<groundState> mapState = new();
        private readonly List<GameObject> nests = new();
        private float fireTime;
        private GameFlowState gameFlowState;

        private int gameoverCounter;
        private bool isPlaceWall;
        private bool isPlaceWallLv2;
        private int level;


        private Touch oldTouch;
        private Vector2 pastPosition = new(0, 0);
        private Bird scriptBird;
        private float switchTimer;


        private void Awake()
        {
            level = 0;
            createWall = GameObject.Find("CreateWall");
            StartButton = GameObject.Find("StartButton");
            GameOverPanel.SetActive(false);
            GameWinPanel.SetActive(false);
            if (GameOverPanel == null) Debug.LogError("not found game opver");
            mainCamera = Camera.main;
            gameConfig.mapSize.x = mainCamera.orthographicSize * mainCamera.aspect;
            gameConfig.birdEnterPosition.x = -gameConfig.mapSize.x * 0.9F;

            NewBird();

            newMap();
            gameFlowState = GameFlowState.Prepare;
            transform.position = Vector2.zero;
            mainCamera.transform.position = new Vector3(0, 0, -10);
            for (var i = mapState.Count / 2; i < mapState.Count; i++) mapState[i] = groundState.Normal;
            SwitchState(GameFlowState.Prepare);
            GenerateNextMap(mapState);

            gameoverCounter = 0;
            SetBirdSprite(0);
        }

        private void Update()
        {
            if (Input.touches.Length != 0)
            {
                oldTouch = Input.touches[0];
            }
            else
            {
                oldTouch = new Touch();
                oldTouch.position = new Vector2(-1, -1);
                if (isPlaceWall)
                    isPlaceWallLv2 = true;
            }


            UpdateHpBar();
            switch (gameFlowState)
            {
                case GameFlowState.Prepare:
                    // SwtchState(GameFlowState.Flying);
                    if (Input.GetKeyDown(KeyCode.Space))
                        Fly();
                    if (Input.GetKeyDown(KeyCode.Q)) ActiveFireHint();

                    if (!isPlaceWallLv2) return;
                    if (!oldTouch.position.Equals(new Vector2(-1, -1)))
                    {
                        Vector3 worldPosition;
                        if (oldTouch.position != new Vector2(-1, -1))
                            worldPosition = Camera.main.ScreenToWorldPoint(oldTouch.position);
                        else
                            return;
                        var hintpos = new Vector2(Mathf.Round(worldPosition.x), gameConfig.groundHeight);
                        var index = 0;
                        foreach (var fireWall in fireWalls)
                        {
                            if (fireWall.transform.position.x.Equals(hintpos.x))
                            {
                                mapState[index] = groundState.FireWall;
                                isPlaceWall = false;
                                isPlaceWallLv2 = false;
                                UpdateMapState();
                                break;
                            }

                            index++;
                        }
                    }

                    break;
                case GameFlowState.Flying:
                    fireTime += Time.deltaTime;
                    if (gameConfig.fireTime < fireTime)
                    {
                        fireTime = 0;
                        List<int> fireIndex = new();
                        for (var i = 0; i < mapState.Count / 2; i++)
                        {
                            if (mapState[i] == groundState.FireWall)
                                continue;
                            if (mapState[i] == groundState.Fire)
                            {
                                if (i - 1 >= 0)
                                    fireIndex.Add(i - 1);
                                if (i + 1 < fires.Count / 2)
                                    fireIndex.Add(i + 1);
                            }
                        }

                        foreach (var i in fireIndex)
                            if (mapState[i] != groundState.FireWall)
                                mapState[i] = groundState.Fire;
                        UpdateMapState();
                    }

                    if (scriptBird.gameObject.transform.position.x > gameConfig.mapSize.x)
                        SwitchState(GameFlowState.NextMap);

                    break;
                case GameFlowState.NextMap:
                    switchTimer += Time.deltaTime;
                    Vector3 newPos = Vector2.Lerp(pastPosition,
                        pastPosition + new Vector2(gameConfig.mapSize.x * 2, 0),
                        switchTimer / gameConfig.switchTime);

                    newPos.z = -10;
                    mainCamera.transform.position = newPos;
                    if (switchTimer > gameConfig.switchTime)
                        SwitchState(GameFlowState.Prepare);
                    break;
                case GameFlowState.GameOver:
                    break;
                case GameFlowState.Win:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void NewBird()
        {
            var birdGO = Instantiate(birdPrefab, transform);
            birdRigidbody2D = birdGO.GetComponent<Rigidbody2D>();
            birdGO.transform.position = gameConfig.birdEnterPosition;
            scriptBird = birdGO.GetComponent<Bird>();
            scriptBird.SetInitSpeed(gameConfig.birdSpeed);
            scriptBird.SetAboveFireSpeed(gameConfig.aboveFireSpeed);
            scriptBird.Stop();
            scriptBird.SetHp(gameConfig.initHp);
            scriptBird.AddFoodListener(FoodDisappear);
            scriptBird.AddGameOverListener(GameOver);
            scriptBird.AddGameWinListener(GameWin);
            scriptBird.gameConfig = gameConfig;
        }

        private void SetBirdSprite(int index)
        {
            if (birdData.teamBird.Count > index && birdData.teamBird[index] != null)
                for (var i = 0; i < Enum.GetNames(typeof(BirdType)).Length; i++)
                    if (birdData.teamBird[index].name.Contains((i + 1).ToString()))
                    {
                        Debug.Log("choose " + i + " bird");
                        gameConfig.birdType = (BirdType)i;
                    }
        }

        private void GameWin()
        {
            Debug.Log("wqin");
            SwitchState(GameFlowState.Win);
        }

        private void GameOver()
        {
            SwitchState(GameFlowState.GameOver);
        }

        public void Fly()
        {
            SwitchState(GameFlowState.Flying);
        }

        public void ActiveFireHint()
        {
            isPlaceWall = !isPlaceWall;
        }

        private void GenerateNextMap(List<groundState> mapState)
        {
            for (var i = 0; i < mapState.Count; i++)
                if (i < mapState.Count / 2)
                    mapState[i] = mapState[i + mapState.Count / 2];
                else
                    mapState[i] = groundState.Normal;
            var firenumber = Random.Range(fires.Count / 2 + 1, fires.Count);
            mapState[firenumber] = groundState.Fire;
            for (var i = 0; i < gameConfig.foodNumber; i++)
            {
                var foodnumber = Random.Range((int)(fires.Count * (float)(3 / 4)) + 1, fires.Count);
                foodnumber = foodnumber == firenumber ? foodnumber + 2 : foodnumber;
                if (this.mapState[foodnumber] != groundState.Fire) mapState[foodnumber] = groundState.Food;
            }

            UpdateMapState();
        }

        private void newMap()
        {
            mapState.Clear();
            for (var i = gameConfig.fireRangeMin;
                 i < gameConfig.fireRangeMax;
                 i += gameConfig.fireWidth)
            {
                var fire = Instantiate(firePrefab, transform);

                fire.transform.position = new Vector3(i, gameConfig.groundHeight, 0);
                fires.Add(fire.GetComponent<Fire>());

                var fireWall = Instantiate(fireWallPrefab, transform);
                fireWall.transform.position =
                    new Vector2(i, gameConfig.groundHeight);
                fireWall.SetActive(false);
                fireWalls.Add(fireWall);

                var food = Instantiate(foodPrefab, transform);
                food.transform.position = new Vector3(i, gameConfig.groundHeight, 0);
                food.gameObject.SetActive(false);
                foods.Add(food);

                var random = Random.Range(0, birdPictureObject.birdPicture.Count);
                var nest = Instantiate(nestPrefab, transform);
                nest.transform.position = new Vector3(i, gameConfig.groundHeight, 0);
                nest.GetComponent<SpriteRenderer>().sprite = birdPictureObject.birdPicture[random];
                nest.gameObject.SetActive(false);
                nests.Add(nest);

                mapState.Add(groundState.Normal);
                UpdateMapState();
            }
        }

        private void UpdateHpBar()
        {
            var bg = hpBar.GetComponent<RectTransform>().rect.width;
            var hpInner = hpBar.transform.GetChild(0).GetComponent<RectTransform>();
            var deltax = (scriptBird.GetHp() / gameConfig.initHp - 1) * bg;
            hpInner.sizeDelta = new Vector2(deltax, 0);
        }


        private void UpdateMapState()
        {
            for (var i = 0; i < mapState.Count; i++)
            {
                fires[i].gameObject.SetActive(mapState[i] == groundState.Fire);
                fireWalls[i].gameObject.SetActive(mapState[i] == groundState.FireWall);
                foods[i].gameObject.SetActive(mapState[i] == groundState.Food);
                nests[i].gameObject.SetActive(mapState[i] == groundState.NEST);
            }
        }

        private void FoodDisappear(GameObject arg0)
        {
            mapState[foods.IndexOf(arg0)] = groundState.Normal;
            UpdateMapState();
        }

        private void SwitchState(GameFlowState state)
        {
            gameFlowState = state;
            switch (state)
            {
                case GameFlowState.Prepare:
                    level += 1;
                    createWall.GameObject().SetActive(true);
                    StartButton.GameObject().SetActive(true);
                    mainCamera.transform.position = new Vector3(0, 0, -10);
                    // transform.position = Vector2.zero;
                    scriptBird.Stop();
                    scriptBird.transform.position =
                        new Vector2(gameConfig.birdEnterPosition.x,
                            scriptBird.transform.position.y);
                    if (level <= 10)
                        GenerateNextMap(mapState);
                    else
                        for (var i = 0; i < mapState.Count; i++)
                            if (i < mapState.Count / 2)
                                mapState[i] = mapState[i + mapState.Count / 2];
                            else
                                mapState[i] = groundState.NEST;
                    UpdateMapState();

                    break;
                case GameFlowState.Flying:
                    createWall.GameObject().SetActive(false);
                    StartButton.GameObject().SetActive(false);
                    isPlaceWall = false;
                    scriptBird.Fly();
                    fireTime = 0;
                    transform.position = new Vector2(0, 0);
                    break;
                case GameFlowState.NextMap:
                    scriptBird.Stop();
                    pastPosition = transform.position;
                    switchTimer = 0;
                    break;
                case GameFlowState.GameOver:
                {
                    if (gameoverCounter < 3)
                    {
                        Debug.Log("gameover");
                        gameoverCounter++;
                        NewBird();
                        SetBirdSprite(gameoverCounter);
                        SwitchState(GameFlowState.Prepare);
                    }
                    else
                    {
                        createWall.GameObject().SetActive(false);
                        StartButton.GameObject().SetActive(false);
                        var sprite = scriptBird.GetSprite();
                        GameOverPanel.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
                        GameOverPanel.SetActive(true);
                    }

                    break;
                }
                case GameFlowState.Win:
                {
                    createWall.GameObject().SetActive(false);
                    StartButton.GameObject().SetActive(false);
                    var sprite = scriptBird.GetSprite();
                    GameWinPanel.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
                    GameWinPanel.SetActive(true);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene("Main");
        }
    }
}