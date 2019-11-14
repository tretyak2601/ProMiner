using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TRGames.ProMiner.Gameplay
{
    [CreateAssetMenu(fileName = "New LomSprites", menuName = "Lom Data", order = 52)]
    public class LomData : ScriptableObject
    {
        [SerializeField] GameObject[] lomSprites;
        public GameObject[] LomSprites { get { return lomSprites; } }
    }
}