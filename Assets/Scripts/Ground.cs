using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TRGames.ProMiner.Gameplay
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] GroundSprite sprites;
        [SerializeField] LomData lom;

        public LomData Lom { get { return lom; } }
        public GroundType GroundType { get; private set; }

        int hitCount = default;
        public int HitCount
        {
            get
            {
                return hitCount;
            }
            set
            {
                hitCount = value;

                if (value - 1 < Lom.LomSprites.Length)
                {
                    var o = Instantiate(Lom.LomSprites[value - 1], Vector3.zero, Quaternion.identity, transform);
                    o.transform.localPosition = Vector3.zero + Vector3.back * value;
                }
            }
        }

        public void Init(int downPosition, int height)
        {
            GroundType type = GroundType.Default;

            int rand = Random.Range(0, 15);

            if (rand == 9 && downPosition != 0)
                type = GroundType.Rock;
            else
            {
                float r = height / (Enum.GetNames(type.GetType()).Length - 1);

                int defaultChance = downPosition <= r ? 99 : Mathf.RoundToInt(downPosition / 0.8f);
                int sandChance = Mathf.RoundToInt(downPosition / 1.6f);
                int clayChance = Mathf.RoundToInt(downPosition / 2.4f);

                int chance = MinChance(defaultChance) + MinChance(sandChance) + MinChance(clayChance);
                int random = Random.Range(0, chance + 1);

                if (random >= 0 && random < MinChance(defaultChance))
                    type = GroundType.Default;
                else if (random >= MinChance(defaultChance) && random < MinChance(defaultChance) + MinChance(sandChance))
                    type = GroundType.Sand;
                else if (random >= MinChance(defaultChance) + MinChance(sandChance) && random < MinChance(defaultChance) + MinChance(sandChance) + MinChance(clayChance))
                    type = GroundType.Clay;
            }

            switch (type)
            {
                case GroundType.Default:
                    sprite.sprite = sprites.DefaultSprite;
                    break;
                case GroundType.Sand:
                    sprite.sprite = sprites.SandSprite;
                    break;
                case GroundType.Rock:
                    sprite.sprite = sprites.RockSprite;
                    break;
                case GroundType.Clay:
                    sprite.sprite = sprites.ClaySprite;
                    break;
            }

            GroundType = type;
        }

        private int MinChance(int chance)
        {
            if (chance > 100)
                chance = 100 - (chance - Mathf.CeilToInt(chance / 100) * 100);

            return chance;
        }
    }
}
