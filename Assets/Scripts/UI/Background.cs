using System.Collections;
using System.Collections.Generic;
using TRGames.ProMiner.Gameplay;
using UnityEngine;

public class Background : MonoBehaviour
{
    private void Update()
    {
        Vector3 direction = new Vector3(PickAxe.Instance.GetComponent<Rigidbody2D>().velocity.x, PickAxe.Instance.GetComponent<Rigidbody2D>().velocity.y, 0);
        direction *= Random.Range(0.01f, 0.05f);
        transform.position -= direction *  Time.deltaTime;
    }
}
