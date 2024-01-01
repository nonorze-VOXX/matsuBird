using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace InGame
{
    internal enum GameFlowState
    {
        Prepare,
        Flying,
        NextMap
    }

    internal enum groundState
    {
        Normal,
        Fire,
        Food,
        FireWall
    }

    public class InGameManager : MonoBehaviour
    {
        public GameObject birdPrefab;
        public GameConfig gameConfig;
        public Rigidbody2D birdRigidbody2D;
        public GameObject firePrefab;
        public List<Fire> fires = new();
        public GameObject fireWallPrefab;
        public GameObject fireWallHint;
        public GameObject foodPrefab;

        public Camera mainCamera;
        public GameObject hpBar;
        public Button createWall;
        public Button StartButton;
        private readonly List<GameObject> fireWalls = new();
        private readonly List<GameObject> foods = new();
        private readonly List<groundState> mapState = new();
        private float fireTime;
        private GameFlowState gameFlowState;
        private Vector2 pastPosition = new(0, 0);
        private Bird scriptBird;
        private float switchTimer;


        private void Awake()
        {
            mainCamera = Camera.main;
            Debug.Log(birdPrefab);
            Debug.Log(transform);
            var birdGO = Instantiate(birdPrefab, transform);
            birdRigidbody2D = birdGO.GetComponent<Rigidbody2D>();
            gameConfig.birdEnterPosition.x = -gameConfig.mapSize.x;
            birdGO.transform.position = gameConfig.birdEnterPosition;
            scriptBird = birdGO.GetComponent<Bird>();
            scriptBird.SetInitSpeed(gameConfig.birdSpeed);
            scriptBird.SetAboveFireSpeed(gameConfig.aboveFireSpeed);
            scriptBird.Stop();
            scriptBird.SetHp(gameConfig.initHp);
            scriptBird.AddFoodListener(FoodDisappear);
            scriptBird.gameConfig = gameConfig;
            newMap();
            gameFlowState = GameFlowState.Prepare;
            transform.position = Vector2.zero;
            mainCamera.transform.position = new Vector3(0, 0, -10);
            for (var i = mapState.Count / 2; i < mapState.Count; i++) mapState[i] = groundState.Normal;
            fireWallHint = Instantiate(fireWallPrefab, transform);
            fireWallHint.SetActive(false);
            SwitchState(GameFlowState.Prepare);
            GenerateNextMap(mapState);
            createWall = GameObject.Find("CreateWall").GetComponent<Button>();
            StartButton = GameObject.Find("StartButton").GetComponent<Button>();
        }


        private void Update()
        {
            UpdateHpBar();
            switch (gameFlowState)
            {
                case GameFlowState.Prepare:
                    // SwtchState(GameFlowState.Flying);
                    if (Input.GetKeyDown(KeyCode.Space))
                        Fly();
                    if (Input.GetKeyDown(KeyCode.Q))
                        ActiveFireHint();

                    if (fireWallHint.activeSelf)
                    {
                        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        var hintpos = new Vector2(Mathf.Round(worldPosition.x), gameConfig.groundHeight);
                        hintpos.x = Mathf.Max(hintpos.x, gameConfig.fireRangeMin);
                        fireWallHint.transform.position = hintpos;
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            var index = 0;
                            foreach (var fireWall in fireWalls)
                            {
                                if (fireWall.transform.position.x.Equals(fireWallHint.transform.position.x))
                                {
                                    mapState[index] = groundState.FireWall;
                                    fireWallHint.SetActive(false);
                                    UpdateMapState();
                                    break;
                                }

                                index++;
                            }
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Fly()
        {
            SwitchState(GameFlowState.Flying);
        }

        public void ActiveFireHint()
        {
            fireWallHint.SetActive(true);
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
            }
        }

        private void FoodDisappear(GameObject arg0)
        {
            mapState[foods.IndexOf(arg0)] = groundState.Normal;
            UpdateMapState();
        }

        private void SwitchState(GameFlowState state)
        {
            switch (state)
            {
                case GameFlowState.Prepare:
                    mainCamera.transform.position = new Vector3(0, 0, -10);
                    // transform.position = Vector2.zero;
                    scriptBird.Stop();
                    scriptBird.transform.position =
                        new Vector2(gameConfig.birdEnterPosition.x,
                            scriptBird.transform.position.y);
                    GenerateNextMap(mapState);
                    break;
                case GameFlowState.Flying:

                    fireWallHint.SetActive(false);
                    scriptBird.Fly();
                    fireTime = 0;
                    transform.position = new Vector2(0, 0);
                    break;
                case GameFlowState.NextMap:
                    scriptBird.Stop();
                    pastPosition = transform.position;
                    switchTimer = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            gameFlowState = state;
        }
    }
}