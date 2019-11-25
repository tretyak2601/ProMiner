﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TRGames.ProMiner.Gameplay
{
    public class PickAxe : MonoBehaviour
    {
        [SerializeField] float dragStrenght;
        [SerializeField] int jumpStrenght;

        [SerializeField] Rigidbody2D rigid;
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] InputController input;

        Vector2 force;
        bool enable = true;

        public event Action<GroundType, Vector2> OnCubeDestroyed;

        private void Awake()
        {
            input.OnDragEvent += DragHandler;
            input.OnDragUp += () =>
            {
                force = Vector2.zero;
                rigid.mass = 1;
            };
            StartCoroutine(ForceMovement());
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("lastPositionX")))
            {
                transform.position = new Vector3(10, 10, 0);
                return;
            }
            else
            {
                float x;
                float y;
                float.TryParse(PlayerPrefs.GetString("lastPositionX"), out x);
                float.TryParse(PlayerPrefs.GetString("lastPositionY"), out y);
                transform.position = new Vector3(x, y, 0);
            }
        }

        private void DragHandler(Vector2 obj)
        {
            rigid.mass = 0.7f;
            force = new Vector2(Mathf.Abs(obj.x) - 0.5f, Mathf.Abs(obj.y) - 0.5f) * dragStrenght;
            
            if (obj.y < 0.5f)
                rigid.DORotate(-Vector2.Angle(Vector2.right,
                                   new Vector2(Mathf.Abs(obj.x) - 0.5f, Mathf.Abs(obj.y) - 0.5f)) - 45, 0.5f);
            else
                rigid.DORotate(Vector2.Angle(Vector2.right,
                                   new Vector2(Mathf.Abs(obj.x) - 0.5f, Mathf.Abs(obj.y) - 0.5f)) - 45, 0.5f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<Ground>() != null)
            {
                bool isDestroyed = false;

                var g = collision.gameObject.GetComponent<Ground>();
                g.HitCount++;

                g.transform.DOScale(0.15f, 0.1f).OnComplete(() => g.transform.DOScale(0.1f, 0.1f));

                if (g.GroundType == GroundType.Default && g.HitCount == 2)
                {
                    OnCubeDestroyed?.Invoke(g.GroundType, g.transform.position);
                    g.Destroy();
                    isDestroyed = true;
                }
                else if (g.GroundType == GroundType.Sand && g.HitCount == 1)
                {
                    OnCubeDestroyed?.Invoke(g.GroundType, g.transform.position);
                    g.Destroy();
                    isDestroyed = true;
                }
                else if (g.GroundType == GroundType.Clay && g.HitCount == 2)
                {
                    OnCubeDestroyed?.Invoke(g.GroundType, g.transform.position);
                    g.Destroy();
                    isDestroyed = true;
                }
                else if (g.GroundType == GroundType.Rock && g.HitCount == 3)
                {
                    OnCubeDestroyed?.Invoke(g.GroundType, g.transform.position);
                    g.Destroy();
                    isDestroyed = true;
                }

                if (enable)
                {
                    rigid.velocity = Vector2.zero;
                    Vector2 direction = transform.position - collision.gameObject.transform.position;

                    if (!isDestroyed)
                        rigid.AddForce(direction * jumpStrenght, ForceMode2D.Force);
                    else
                    {
                        rigid.AddForce(-direction * jumpStrenght, ForceMode2D.Force);
                        rigid.transform.DOPunchRotation(new Vector3(0, 0, 360), 0.2f);
                    }

                    enable = false;
                    StartCoroutine(Wait());
                }
            }
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.1f);
            enable = true;
        }

        IEnumerator ForceMovement()
        {
            while (true)
            {
                rigid.AddForce(force, ForceMode2D.Impulse);
                yield return new WaitForSeconds(Mathf.PI / 10);
            }
        }
    }
}