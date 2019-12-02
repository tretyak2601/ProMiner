using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TRGames.ProMiner.Gameplay
{
    public class LifesController : MonoBehaviour
    {
        [SerializeField] int lifeCount;
        [SerializeField] Image lifePrefab;

        List<Image> lifeList = new List<Image>();

        private void Start()
        {
            for (int i = 0; i < lifeCount; i++)
            {
                var l = Instantiate(lifePrefab, Vector3.zero, Quaternion.identity, transform);
                l.transform.localPosition = Vector3.zero;
                lifeList.Add(l);
            }

            PickAxe.Instance.Lifes = lifeCount;

            PickAxe.Instance.OnLifeLost += () =>
            {
                Destroy(lifeList[lifeList.Count - 1].gameObject);
                lifeList.RemoveAt(lifeList.Count - 1);
            };
        }
    }
}
