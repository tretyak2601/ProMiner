using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TRGames.ProMiner.Gameplay
{
    [CreateAssetMenu(fileName = "New GroundSprites", menuName = "Ground Data", order = 51)]
    public class GroundSprite : ScriptableObject
    {
        [SerializeField] Sprite defaultSprite;
        [SerializeField] Sprite rockSprite;
        [SerializeField] Sprite claySprite;
        [SerializeField] Sprite sandSprite;

        public Sprite DefaultSprite { get { return defaultSprite; } }
        public Sprite RockSprite { get { return rockSprite; } }
        public Sprite ClaySprite { get { return claySprite; } }
        public Sprite SandSprite { get { return sandSprite; } }
    }

    public enum GroundType
    {
        Default,
        Rock,
        Clay,
        Sand
    }
}