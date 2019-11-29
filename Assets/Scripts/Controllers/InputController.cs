using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TRGames.ProMiner.Gameplay
{
    public class InputController : MonoBehaviour
    {
        public event Action<Vector2> OnDragEvent;
        public event Action OnDragUp;
        bool release = false;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                release = true;
            else if (Input.GetMouseButtonUp(0))
            {
                OnDragUp?.Invoke();
                release = false;
            }

            if (release)
                OnDragEvent?.Invoke(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        }
    }
}
