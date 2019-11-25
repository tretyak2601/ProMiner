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
        public KeyValuePair<int, (Ground, Vector3)> listIndex { get; private set; }
        public Color Color { get; private set; }

        private GroundBuilder gb;

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

        public void Init(Color32 color, GroundBuilder gb)
        {
            this.gb = gb;
            Color = color;
            GroundType type = GroundType.Default;

            int rand = Random.Range(0, 50);

            if (rand == 1)
                type = GroundType.Rock;
            if (rand == 2)
                type = GroundType.Sand;
            if (rand == 3)
                type = GroundType.Clay;
            if ((rand == 0 || rand == 5))
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
            listIndex = new KeyValuePair<int, (Ground, Vector3)>(0, (this, this.transform.position));
        }

        private void OnBecameVisible()
        {
            coll.enabled = true;
        }

        private void OnBecameInvisible()
        {
            coll.enabled = false;
        }

        public void Destroy()
        {
            var part = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            var main = part.main;
            main.startColor = sprite.color;
            gb.Grounds.Remove(listIndex);

            GameObject.Destroy(this.gameObject);
        }

        public override string ToString()
        {
            return "Position: " + transform.position + "\n" + " Color: " + Color;
        }
    }
}
