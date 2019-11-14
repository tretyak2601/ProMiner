using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TRGames.ProMiner.Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class CameraControll : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset;

        private void LateUpdate()
        {
            Vector3 desiredPosition = target.position - offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.1f);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10);
        }
    }
}