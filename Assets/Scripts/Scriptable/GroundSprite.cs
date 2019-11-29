using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace TRGames.ProMiner.Gameplay
{
    [CreateAssetMenu(fileName = "New GroundSprites", menuName = "Ground Data", order = 51)]
    public class GroundSprite : ScriptableObject
    {
        [SerializeField] Sprite defaultSprite;
        [SerializeField] Sprite vulcanSprite;
        [SerializeField] Sprite marsSprite;
        [SerializeField] Sprite mineSprite;
        [SerializeField] Sprite jungleSprite;
        [SerializeField] Sprite egyptSprite;
        [SerializeField] Sprite arcticSprite;
        [SerializeField] Sprite beachSprite;
        [SerializeField] Sprite caveSprite;

        public Sprite GetSprite(GroundType type)
        {
            Sprite s = null;

            switch (type)
            {
                case GroundType.None:
                    break;
                case GroundType.Default:
                    s = defaultSprite;
                    break;
                case GroundType.Vulcan:
                    s = vulcanSprite;
                    break;
                case GroundType.Mars:
                    s = marsSprite;
                    break;
                case GroundType.Mine:
                    s = mineSprite;
                    break;
                case GroundType.Jungle:
                    s = jungleSprite;
                    break;
                case GroundType.Egypt:
                    s = egyptSprite;
                    break;
                case GroundType.Arctic:
                    s = arcticSprite;
                    break;
                case GroundType.Beach:
                    s = beachSprite;
                    break;
                case GroundType.Cave:
                    s = caveSprite;
                    break;
            }

            return s;
        }
    }

    public enum GroundType
    {
        None,
        [Description("Default")]
        Default,
        Vulcan,
        Mars,
        Mine,
        Jungle,
        Egypt,
        Arctic,
        Beach,
        Cave
    }
}