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
        private GameFlowState gameFlowState;

        private void Awake()
        {
            var bird = Instantiate(birdPrefab, transform);
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
                    gameFlowState = GameFlowState.Flying;
                    break;
                case GameFlowState.Flying:
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
            for (var i = -gameConfig.mapSize.x; i < gameConfig.mapSize.x; i++)
            {
                var fire = Instantiate(firePrefab, transform);
                fire.transform.position = new Vector3(i, gameConfig.groundHeight, 0);
                Debug.Log(fire.GetComponent<Wind>());
                fires.Add(fire.GetComponent<Fire>());
            }
        }
    }
}