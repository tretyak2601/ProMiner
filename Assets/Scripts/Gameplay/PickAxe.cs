using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TRGames.ProMiner.Gameplay
{
    public class PickAxe : MonoBehaviour
    {
        public static PickAxe Instance;

        [SerializeField] float dragStrenght;
        [SerializeField] int jumpStrenght;

        [SerializeField] Rigidbody2D rigid;
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] InputController input;
        [SerializeField] RageController rage;
        [SerializeField] ParticleSystem sparks;

        Vector2 force;
        bool enable = true;
        bool rageMode = false;
        bool canLostLife = true;

        public event Action OnGameOver;
        public event Action OnLifeLost;
        public event Action OnDirtDestroyed;

        int lifes = 5;
        public int Lifes
        {
            get
            {
                return lifes;
            }
            set
            {
                if (!canLostLife)
                    return;

                if (lifes > value)
                    StartCoroutine(LifeLostAnimation());

                OnLifeLost?.Invoke();
                lifes = value;

                if (lifes <= 0)
                {
                    OnGameOver?.Invoke();
                    rigid.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(this);

            input.OnDragEvent += DragHandler;
            input.OnDragUp += () =>
            {
                force = Vector2.zero;
                rigid.mass = 1;
            };
            StartCoroutine(ForceMovement());
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
            if (collision.gameObject.tag == "Metaball_liquid")
            {
                Destroy(collision.gameObject);

                if (canLostLife)
                    Lifes--;

                return;
            }

            if (collision.gameObject.tag == "Crystal")
            {
                collision.gameObject.GetComponent<Crystal>().Destroy();
                return;
            }

            if (collision.gameObject.GetComponent<Ground>() != null)
            {
                bool isDestroyed = false;

                var g = collision.gameObject.GetComponent<Ground>();
                g.HitCount++;

                g.transform.DOScale(1.2f, 0.1f).OnComplete(() => g.transform.DOScale(1f, 0.1f));
                
                if (!g.NotDestroyeble)
                {
                    if (rage.IsRaged)
                    {
                        g.Destroy();
                        isDestroyed = true;
                    }
                    else
                    {
                        if (g.HitCount == 2)
                        {
                            g.Destroy();
                            isDestroyed = true;
                            OnDirtDestroyed?.Invoke();
                        }
                        else
                            Instantiate(sparks, transform.position, Quaternion.identity);
                    }
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

#if UNITY_ANDROID && !UNITY_EDITOR
                        Vibration.Vibrate(100);
#elif UNITY_IOS && !UNITY_EDITOR
                        Vibration.VibratePop();
#endif
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

        IEnumerator LifeLostAnimation()
        {
            canLostLife = false;
            for (int i = 0; i < 5; i++)
            {
                sprite.enabled = false;
                yield return new WaitForSeconds(0.33f);
                sprite.enabled = true;
                yield return new WaitForSeconds(0.33f);
            }
            canLostLife = true;
        }
    }
}