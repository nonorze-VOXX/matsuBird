using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    [CreateAssetMenu(fileName = "birdPicture", menuName = "pirdPicture", order = 0)]
    public class BirdPictureObject : ScriptableObject
    {
        public List<Sprite> birdPicture;
    }
}