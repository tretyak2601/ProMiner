using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateAnimation : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AnimationEnumerator());
    }

    IEnumerator AnimationEnumerator()
    {
        while (true)
        {
            transform.DOPunchRotation(new Vector3(0, 0, 5), 10f).OnComplete(() => transform.DOPunchRotation(new Vector3(0, 0, -5), 10f));
            yield return new WaitForSeconds(1);
        }
    }
}
