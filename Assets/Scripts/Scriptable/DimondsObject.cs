using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TRGames.ProMiner.Gameplay
{
    [CreateAssetMenu(fileName = "New Diamonds", menuName = "Dimonds Data", order = 51)]
    public class DimondsObject : ScriptableObject
    {
        [SerializeField] public Crystal[] crystals;

    }
}
