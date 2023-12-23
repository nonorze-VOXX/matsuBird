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
                        if (fireWallHint == null)
                            fireWallHint = Instantiate(fireWallPrefab, transform);

                    if (fireWallHint != null)
                    {
                        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        fireWallHint.transform.position =
                            new Vector2(Mathf.Round(worldPosition.x), gameConfig.groundHeight);
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            fireWalls.Add(fireWallHint);
                            fireWallHint = null;
                        }
                    }

                    break;
                case GameFlowState.Flying:
                    fireTime += Time.deltaTime;
                    if (gameConfig.fireTime < fireTime)
                    {
                        fireTime = 0;
                        List<int> fireIndex = new();
                        for (var i = 0; i < fires.Count; i++)
                        {
                            if (fires[i].GetUsed() || !fires[i].gameObject.activeSelf || fires[i].GetInWall()) continue;
                            if (i - 1 >= 0)
                                fireIndex.Add(i - 1);
                            if (i + 1 < fires.Count)
                                fireIndex.Add(i + 1);
                            fires[i].SetUesd(true);
                        }

                        foreach (var i in fireIndex) fires[i].gameObject.SetActive(true);
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
            for (var i = gameConfig.fireRangeMin + gameConfig.fireWidth;
                 i < gameConfig.fireRangeMax;
                 i += gameConfig.fireWidth)
            {
                var fire = Instantiate(firePrefab, transform);
                fire.transform.position = new Vector3(i, gameConfig.groundHeight, 0);
                fires.Add(fire.GetComponent<Fire>());
            }
        }

        private void SwitchState(GameFlowState state)
        {
            switch (state)
            {
                case GameFlowState.Prepare:
                    scriptBird.Stop();
                    var firenumber = 0; //Random.Range(0, fires.Count);
                    foreach (var fire in fires)
                    {
                        fire.Reset();
                        fire.gameObject.SetActive(false);
                    }

                    fires[firenumber].gameObject.SetActive(true);
                    break;
                case GameFlowState.Flying:
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