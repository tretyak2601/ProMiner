using System.Collections;
using System.Collections.Generic;
using TRGames.ProMiner.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class WetText : MonoBehaviour
{
    [SerializeField] Text waterText;

    private void Start()
    {
        PickAxe.Instance.OnDropsStateChanged += flag => waterText.enabled = flag;
    }
}
