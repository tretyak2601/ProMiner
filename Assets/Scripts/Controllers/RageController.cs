using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace TRGames.ProMiner.Gameplay
{
    public class RageController : MonoBehaviour
    {
        [SerializeField] Text rageText;
        [SerializeField] Slider slider;
        [SerializeField] ParticleSystem fires;
        [SerializeField] int rageSeconds;
        
        public bool IsRaged { get; private set; }

        private int rageCount;
        public int RageCount
        {
            get
            {
                return rageCount;
            }
            set
            {
                rageCount = value;
                slider.value = value;

                if (rageCount == 100)
                {
                    IsRaged = true;
                    StartCoroutine(Raging());
                    rageText.enabled = true;
                }
            }
        }

        private void Start()
        {
            PickAxe.Instance.OnDirtDestroyed += () => RageCount++;
            StartCoroutine(Firing());
            StartCoroutine(TextAnimation());
        }

        IEnumerator Raging()
        {
            yield return new WaitForSeconds(rageSeconds);
            IsRaged = false;
            slider.value = 0;
            rageCount = 0;
            rageText.enabled = false;
        }

        IEnumerator Firing()
        {
            while (true)
            {
                if (IsRaged)
                    Instantiate(fires, PickAxe.Instance.transform.position, Quaternion.identity);

                yield return null;
            }
        }

        IEnumerator TextAnimation()
        {
            while (true)
            {
                rageText.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f).OnComplete(() => rageText.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
                yield return new WaitForSeconds(1);
            }
        }
    }
}