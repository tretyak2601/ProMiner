using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TRGames.ProMiner.Gameplay
{
    public class LoadingManager : MonoBehaviour
    {
        [SerializeField] Image backGround;
        [SerializeField] GameObject map;
        [SerializeField] GroundBuilder gb;

        private void Awake()
        {
            gb.OnGroundBuilt += OffLoading;
        }

        private void OffLoading()
        {
            map.SetActive(true);
            backGround.gameObject.SetActive(false);

        }
    }
}