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
        [SerializeField] GroundBuilder gb;

        private void Awake()
        {
            gb.OnGroundBuilt += OffLoading;
        }

        public void OffLoading()
        {
            backGround.gameObject.SetActive(false);
        }
    }
}