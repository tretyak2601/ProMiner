using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void Awake()
        {
            input.OnDragEvent += DragHandler;
            input.OnDragUp += () => force = Vector2.zero;
            StartCoroutine(ForceMovement());
        }

        private void DragHandler(Vector2 obj)
        {
            force = new Vector2(Mathf.Abs(obj.x) - 0.5f, Mathf.Abs(obj.y) - 0.5f) * dragStrenght;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<Ground>() != null)
            {
                var g = collision.gameObject.GetComponent<Ground>();
                g.HitCount++;

                if (g.GroundType == GroundType.Default && g.HitCount == 2)
                    Destroy(g.gameObject);
                else if (g.GroundType == GroundType.Sand && g.HitCount == 1)
                    Destroy(g.gameObject);
                else if (g.GroundType == GroundType.Clay && g.HitCount == 2)
                    Destroy(g.gameObject);
                else if (g.GroundType == GroundType.Rock && g.HitCount == 3)
                    Destroy(g.gameObject);

                if (enable)
                {
                    rigid.velocity = Vector2.zero;
                    Vector2 direction = transform.position - collision.gameObject.transform.position;
                    rigid.AddForce(direction * jumpStrenght, ForceMode2D.Force);
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
