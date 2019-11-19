using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TRGames.ProMiner.Gameplay
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        private void Update()
        {
            transform.position = new Vector3(canvas.transform.position.x, canvas.transform.position.y - 1, transform.position.z);
        }
    }
}