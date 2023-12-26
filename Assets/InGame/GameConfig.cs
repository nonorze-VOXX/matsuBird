using UnityEngine;

namespace InGame
{
    [CreateAssetMenu(fileName = "gameConfig", menuName = "gameScriptable", order = 0)]
    public class GameConfig : ScriptableObject
    {
        public Vector2 birdSpeed;
        public Vector2 birdEnterPosition;
        public Vector2 mapSize;
        public float groundHeight;
        public Vector2 aboveFireSpeed;
        public float fireTime;
        public float fireRangeMin;
        public float fireRangeMax;
        public float fireWidth;
        public float switchTime;
    }
}