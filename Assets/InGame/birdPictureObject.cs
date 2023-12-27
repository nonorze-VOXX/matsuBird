using UnityEngine;

namespace InGame
{
    internal enum birdType
    {
        bird1,
        bird2,
        bird3,
        bird4
    }

    internal enum birdState
    {
        stand,
        fly
    }

    [CreateAssetMenu(fileName = "birdPicture", menuName = "pirdPicture", order = 0)]
    public class birdPictureObject : ScriptableObject
    {
        public Sprite[][] birdPicture;
    }
}