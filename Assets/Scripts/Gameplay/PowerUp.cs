using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class PowerUp : MonoBehaviour
{
    public abstract void OnCollisionEnter2D(Collision2D collision);
}
