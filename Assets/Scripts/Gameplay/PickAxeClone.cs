using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace TRGames.ProMiner.Gameplay
{
    public class PickAxeClone : MonoBehaviour
    {
        [SerializeField] SpriteRenderer renderer;
        [SerializeField] Rigidbody2D rigid;
        [SerializeField] ParticleSystem sparks;
        [SerializeField] int lifeTime;

        PolygonCollider2D coll;

        public void Init(Sprite sprite, Vector2 forceDirection)
        {
            renderer.sprite = sprite;
            coll = gameObject.AddComponent<PolygonCollider2D>();
            coll.isTrigger = false;
            rigid.AddForce(forceDirection * PickAxe.Instance.jumpStrenght, ForceMode2D.Force);

            StartCoroutine(Lifetime());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Metaball_liquid")
            {
                Destroy(collision.gameObject);
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
                    if (g.HitCount == 2)
                    {
                        g.Destroy();
                        isDestroyed = true;
                    }
                    else
                        Instantiate(sparks, transform.position, Quaternion.identity);
                }

                rigid.velocity = Vector2.zero;
                Vector2 direction = transform.position - collision.gameObject.transform.position;

                if (!isDestroyed)
                    rigid.AddForce(direction * PickAxe.Instance.jumpStrenght, ForceMode2D.Force);
                else
                {
                    rigid.AddForce(-direction * PickAxe.Instance.jumpStrenght, ForceMode2D.Force);
                    rigid.transform.DOPunchRotation(new Vector3(0, 0, 360), 0.2f);
                }
            }
        }

        IEnumerator Lifetime()
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }
    }
}