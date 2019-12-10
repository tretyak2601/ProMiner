﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TRGames.ProMiner.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class Ground : MonoBehaviour
    {
        [SerializeField] ParticleSystem destroyParticles;
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] GroundSprite sprites;
        [SerializeField] LomData lom;

        [SerializeField] public GroundType gt;

        Collider2D coll;
        public LomData Lom { get { return lom; } }
        public KeyValuePair<Ground, Vector3> listIndex { get; private set; }
        public Color Color { get; private set; }

        GroundBuilder gb;

        public bool NotDestroyeble = false;

        private void Awake()
        {
            coll = GetComponent<Collider2D>();
        }

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

        private void Start()
        {
            if (gb == null)
                gb = GroundBuilder.Instance;
        }

        public void Init(GroundBuilder gb, GroundType type, bool notDestroyeble = false)
        {
            NotDestroyeble = notDestroyeble;
            this.gb = gb;
            int rand = Random.Range(0, 100);

            if ((rand == 4 || rand == 5 || rand == 6 || rand == 7 || rand == 8))
            {
                DestroyImmediate(this.gameObject, false);
                return;
            }

            sprite.sprite = sprites.GetSprite(type);

            gt = type;
            listIndex = new KeyValuePair<Ground, Vector3>(this, this.transform.position);
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
