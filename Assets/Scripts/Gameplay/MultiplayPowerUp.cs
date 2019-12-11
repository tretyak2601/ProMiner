using System.Collections;
using System.Collections.Generic;
using TRGames.ProMiner.Gameplay;
using UnityEngine;

public class MultiplayPowerUp : PowerUp
{
    [SerializeField] PickAxeClone paClone;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < 2; i++)
        {
            Vector3 direction = Vector3.zero;

            switch (i)
            {
                case 0:
                    direction = Vector3.left;
                    break;
                case 1:
                    direction = Vector3.right;
                    break;
            }

            var pa = Instantiate(paClone, PickAxe.Instance.transform.position + direction * 2, Quaternion.identity);
            pa.Init(PickAxe.Instance.GetComponent<SpriteRenderer>().sprite, direction);
        }

        Destroy(gameObject);
    }
}
