using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crystal : MonoBehaviour
{
    [SerializeField] ParticleSystem crystalsParticles;

    void Start()
    {
        StartCoroutine(AnimationEnumerator());
    }

    IEnumerator AnimationEnumerator()
    {
        while (true)
        {
            transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f).OnComplete(() => transform.DOScale(new Vector3(0.25f, 0.25f, 0.25f), 0.5f));
            yield return new WaitForSeconds(1);
        }
    }

    public void Destroy()
    {
        Instantiate(crystalsParticles, transform.position + Vector3.back * 5, Quaternion.identity);
        Destroy(gameObject);
    }
}
