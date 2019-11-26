﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] GameObject water;
    [SerializeField] int count;
    [SerializeField] Color color;

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var w = Instantiate(water, this.transform.position, Quaternion.identity);
            w.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-360, 360), Random.Range(-360, 360)), ForceMode2D.Force);
            //w.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
