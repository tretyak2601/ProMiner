using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TRGames.ProMiner.Gameplay
{
    public class GroundBuilder : MonoBehaviour
    {
        [SerializeField] GameObject topGroundPrefab;
        [SerializeField] Ground groundPrefab;

        [SerializeField] int startWidth;
        [SerializeField] int startHeight;

        List<Ground> topGrounds = new List<Ground>();

        const float groundWidth = 0.438f;
        const float groundHeight = 0.425f;

        void Start()
        {
            StartCoroutine(CreateGround());
        }

        IEnumerator CreateGround()
        {
            for (int i = 0; i < startWidth; i++)
            {
                for (int j = 0; j < startHeight; j++)
                {
                    var obj = Instantiate(groundPrefab, (Vector3.zero + Vector3.right * groundWidth * i) + (Vector3.down * groundHeight * j), Quaternion.identity, transform);

                    if (j == 0)
                    {
                        var top = Instantiate(topGroundPrefab, Vector3.up * groundHeight / 2, Quaternion.identity, obj.transform);
                        top.transform.localPosition = new Vector3(0, top.transform.localPosition.y, -1);
                    }

                    obj.Init(j, startHeight);
                    topGrounds.Add(obj);
                }

            }
            yield return null;

            float allWidth = startWidth * groundWidth;
            float center = allWidth / 2 - groundWidth / 2;
            transform.position -= Vector3.right * center;

            topGrounds.ForEach(g => g.gameObject.isStatic = true);
            UnityEditor.StaticOcclusionCulling.GenerateInBackground();
        }
    }
}
