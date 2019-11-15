using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TRGames.ProMiner.Gameplay
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] ParticleSystem destroyParticles;
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] GroundSprite sprites;
        [SerializeField] LomData lom;
        [SerializeField] Collider2D coll;

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

        public void Init(int downPosition, Color32 color)
        {
            GroundType type = GroundType.Default;

            int rand = Random.Range(0, 50);

            if (rand == 1 && downPosition != 0)
                type = GroundType.Rock;
            if (rand == 2 && downPosition != 0)
                type = GroundType.Sand;
            if (rand == 3 && downPosition != 0)
                type = GroundType.Clay;
            if ((rand == 0 || rand == 5) && downPosition != 0)
                gameObject.SetActive(false);

            switch (type)
            {
                case GroundType.Default:
                    sprite.sprite = sprites.DefaultSprite;
                    sprite.color = color;
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
        
        private void OnBecameVisible()
        {
            coll.enabled = true;
        }

        private void OnBecameInvisible()
        {
            coll.enabled = false;
        }

        private void OnDestroy()
        {
            var part = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            var main = part.main;
            main.startColor = sprite.color;
            Handheld.Vibrate();
        }
    }
}
