using System.Collections;
using System.Collections.Generic;
using TRGames.ProMiner.Gameplay;
using UnityEngine;

public class FireshootPowerUp : PowerUp
{
    [SerializeField] ParticleSystem fires;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < 3; i++)
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
                case 2:
                    direction = Vector3.down;
                    break;
            }

            var obj = Instantiate(fires, new Vector3(transform.position.x, transform.position.y, -5), Quaternion.identity);
            obj.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
            PickAxe.Instance.StartCoroutine(Move(obj));
        }

        Destroy(gameObject);
    }

    IEnumerator Move(ParticleSystem obj)
    {
        while (true)
        {
            Vector3 random = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);

            while (random.x == 0 || random.y == 0)
                random = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);

            for (int i = 0; i < 2; i++)
            {
                if (obj == null)
                    break;

                obj.transform.position += random;
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}
