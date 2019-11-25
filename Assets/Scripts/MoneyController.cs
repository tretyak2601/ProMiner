using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TRGames.ProMiner.Gameplay
{
    public class MoneyController : MonoBehaviour
    {
        [SerializeField] private Text moneyText;
        [SerializeField] private PickAxe axe;

        private int moneyCount;
        public int MoneyCount
        {
            get { return moneyCount; }
            private set
            {
                moneyCount = value;
                moneyText.text = value.ToString();
            }
        }
        
        private void Awake()
        {
            axe.OnCubeDestroyed += CubeDestroyedHandler; 
        }

        private void CubeDestroyedHandler(GroundType type, Vector2 position)
        {
            MoneyCount++;
        }
    }
}