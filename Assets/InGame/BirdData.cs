using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    [CreateAssetMenu(fileName = "birdTeam", menuName = "birdTeam", order = 0)]
    public class BirdData : ScriptableObject
    {
        public List<TeamBird> birdList;
        public List<Sprite> teamBird;
    }
}