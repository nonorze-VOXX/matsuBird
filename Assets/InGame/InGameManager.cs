using System.Collections.Generic;
using UnityEngine;

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

        private void Awake()
        {
            var bird = Instantiate(birdPrefab, transform);
            gameConfig.birdEnterPosition.x = -gameConfig.mapSize.x;
            bird.transform.position = gameConfig.birdEnterPosition;
            var scriptBird = bird.GetComponent<bird>();
            scriptBird.SetInitSpeed(gameConfig.birdSpeed);
            scriptBird.SetAboveFireSpeed(gameConfig.aboveFireSpeed);
            newMap();
        }

        private void newMap()
        {
            var fire = Instantiate(firePrefab, transform);
            fire.transform.position = new Vector3(Random.Range(-gameConfig.mapSize.x, gameConfig.mapSize.x),
                gameConfig.groundHeight, 0);
            fires.Add(fire.GetComponent<Fire>());
        }
    }
}