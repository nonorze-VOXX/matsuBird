using UnityEngine;

namespace InGame
{
    public enum BirdType
    {
        bird1,
        bird2,
        bird3,
        bird4
    }

    public enum BirdPictureState
    {
        stand,
        fly
    }

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
        public float initHp;
        public float flyToSkyHp;
        public float flyToSkyTime;
        public float foodHp;
        public BirdPictureState birdPictureState;
        public BirdType birdType;
        public int foodNumber;
        public float DropTime;
    }
}