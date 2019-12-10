using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace TRGames.ProMiner.Gameplay
{
    [CreateAssetMenu(fileName = "New GroundSprites", menuName = "Ground Data", order = 51)]
    public class GroundSprite : ScriptableObject
    {
        [SerializeField] Sprite[] defaultSprite;
        [SerializeField] Sprite[] vulcanSprite;
        [SerializeField] Sprite[] marsSprite;
        [SerializeField] Sprite[] mineSprite;
        [SerializeField] Sprite[] jungleSprite;
        [SerializeField] Sprite[] egyptSprite;
        [SerializeField] Sprite[] arcticSprite;
        [SerializeField] Sprite[] beachSprite;
        [SerializeField] Sprite[] caveSprite;

        public Sprite GetSprite(GroundType type)
        {
            Sprite s = null;

            switch (type)
            {
                case GroundType.None:
                    break;
                case GroundType.Default:
                    s = defaultSprite[Random.Range(0, defaultSprite.Length)];
                    break;
                case GroundType.Vulcan:
                    s = vulcanSprite[Random.Range(0, vulcanSprite.Length)];
                    break;
                case GroundType.Mars:
                    s = marsSprite[Random.Range(0, marsSprite.Length)];
                    break;
                case GroundType.Mine:
                    s = mineSprite[Random.Range(0, mineSprite.Length)];
                    break;
                case GroundType.Jungle:
                    s = jungleSprite[Random.Range(0, jungleSprite.Length)];
                    break;
                case GroundType.Egypt:
                    s = egyptSprite[Random.Range(0, egyptSprite.Length)];
                    break;
                case GroundType.Arctic:
                    s = arcticSprite[Random.Range(0, arcticSprite.Length)];
                    break;
                case GroundType.Beach:
                    s = beachSprite[Random.Range(0, beachSprite.Length)];
                    break;
                case GroundType.Cave:
                    s = caveSprite[Random.Range(0, caveSprite.Length)];
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