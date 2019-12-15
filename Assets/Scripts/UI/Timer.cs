using System;
using System.Collections;
using System.Collections.Generic;
using TRGames.ProMiner.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] int minutes;

    DateTime currentTime;
    DateTime CurrentTime
    {
        get
        {
            return currentTime;
        }
        set
        {
            timerText.text = value.ToLongTimeString().Remove(0,2);
            currentTime = value;
        }
    }

    private void Start()
    {
        CurrentTime = new DateTime(2018, 1, 1, 0, 1, 0);
        StartCoroutine(TimerEnumerator());
    }

    IEnumerator TimerEnumerator()
    {
        do
        {
            yield return new WaitForSeconds(1);
            var t = currentTime - new DateTime(1, 1, 1, 0, 0, 1);
            CurrentTime = new DateTime(2018, 1, 1, 0, t.Minutes, t.Seconds);
        }
        while (CurrentTime.Second > 0);

        PickAxe.Instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
