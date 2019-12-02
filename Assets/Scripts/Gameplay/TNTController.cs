using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TRGames.ProMiner.Gameplay
{
    public class TNTController : MonoBehaviour
    {
        [SerializeField] GameObject explosivePrefab;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                var objects = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 2), 360);

                foreach (var o in objects)
                {
                    if (o.GetComponent<Ground>() && o.GetComponent<Ground>().gt != GroundType.None)
                        o.GetComponent<Ground>().Destroy();
                }

                PickAxe.Instance.Lifes -= 1;
                Instantiate(explosivePrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
