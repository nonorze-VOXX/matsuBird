using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    internal enum GameFlowState
    {
        Prepare,
        Flying,
        Eating
    }

    internal enum groundState
    {
        Normal,
        Fire,
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
        private readonly List<GameObject> fireWalls = new();
        private readonly List<groundState> mapState = new();

        private float fireTime;
        private GameFlowState gameFlowState;
        private Bird scriptBird;

        private void Awake()
        {
            var birdGO = Instantiate(birdPrefab, transform);
            birdRigidbody2D = birdGO.GetComponent<Rigidbody2D>();
            gameConfig.birdEnterPosition.x = -gameConfig.mapSize.x;
            birdGO.transform.position = gameConfig.birdEnterPosition;
            scriptBird = birdGO.GetComponent<Bird>();
            scriptBird.SetInitSpeed(gameConfig.birdSpeed);
            scriptBird.SetAboveFireSpeed(gameConfig.aboveFireSpeed);
            newMap();
            SwitchState(GameFlowState.Prepare);
            fireWallHint = Instantiate(fireWallPrefab, transform);
        }

        private void Update()
        {
            switch (gameFlowState)
            {
                case GameFlowState.Prepare:
                    // SwtchState(GameFlowState.Flying);
                    if (Input.GetKeyDown(KeyCode.Space))
                        SwitchState(GameFlowState.Flying);
                    if (Input.GetKeyDown(KeyCode.Q))
                        fireWallHint.SetActive(true);

                    if (fireWallHint.activeSelf)
                    {
                        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        fireWallHint.transform.position =
                            new Vector2(Mathf.Round(worldPosition.x), gameConfig.groundHeight);
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
                        for (var i = 0; i < mapState.Count; i++)
                        {
                            if (mapState[i] == groundState.FireWall)
                                continue;
                            if (mapState[i] == groundState.Fire)
                            {
                                if (i - 1 >= 0)
                                    fireIndex.Add(i - 1);
                                if (i + 1 < fires.Count)
                                    fireIndex.Add(i + 1);
                            }
                        }

                        foreach (var i in fireIndex)
                            if (mapState[i] != groundState.FireWall)
                                mapState[i] = groundState.Fire;
                        UpdateMapState();
                    }

                    break;
                case GameFlowState.Eating:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void newMap()
        {
            mapState.Clear();
            for (var i = gameConfig.fireRangeMin + gameConfig.fireWidth;
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

                mapState.Add(groundState.Normal);
                UpdateMapState();
            }
        }

        private void UpdateMapState()
        {
            for (var i = 0; i < mapState.Count; i++)
            {
                fires[i].gameObject.SetActive(mapState[i] == groundState.Fire);
                fireWalls[i].gameObject.SetActive(mapState[i] == groundState.FireWall);
            }
        }

        private void SwitchState(GameFlowState state)
        {
            switch (state)
            {
                case GameFlowState.Prepare:
                    scriptBird.Stop();
                    var firenumber = 0; //Random.Range(0, fires.Count);
                    foreach (var fire in fires) fire.gameObject.SetActive(false);

                    fires[firenumber].gameObject.SetActive(true);
                    mapState[firenumber] = groundState.Fire;
                    break;
                case GameFlowState.Flying:

                    fireWallHint.SetActive(false);
                    scriptBird.Fly();
                    fireTime = 0;
                    break;
                case GameFlowState.Eating:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            gameFlowState = state;
        }
    }
}