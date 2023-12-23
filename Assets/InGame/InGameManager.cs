using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame
{
    internal enum GameFlowState
    {
        Prepare,
        Flying,
        Landing,
        Eating
    }

    public class InGameManager : MonoBehaviour
    {
        public GameObject birdPrefab;
        public GameConfig gameConfig;
        public Rigidbody2D birdRigidbody2D;
        public GameObject firePrefab;
        public List<Fire> fires = new();

        private float fireTime;
        private GameFlowState gameFlowState;

        private void Awake()
        {
            var bird = Instantiate(birdPrefab, transform);
            birdRigidbody2D = bird.GetComponent<Rigidbody2D>();
            gameConfig.birdEnterPosition.x = -gameConfig.mapSize.x;
            bird.transform.position = gameConfig.birdEnterPosition;
            var scriptBird = bird.GetComponent<bird>();
            scriptBird.SetInitSpeed(gameConfig.birdSpeed);
            scriptBird.SetAboveFireSpeed(gameConfig.aboveFireSpeed);
            gameFlowState = GameFlowState.Prepare;
            newMap();
        }

        private void Update()
        {
            switch (gameFlowState)
            {
                case GameFlowState.Prepare:
                    var firenumber = Random.Range(0, fires.Count);
                    foreach (var fire in fires)
                    {
                        fire.Reset();
                        fire.gameObject.SetActive(false);
                    }

                    fires[firenumber].gameObject.SetActive(true);
                    SwitchState(GameFlowState.Flying);
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
                case GameFlowState.Landing:
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
            gameFlowState = state;
            fireTime = 0;
        }
    }
}