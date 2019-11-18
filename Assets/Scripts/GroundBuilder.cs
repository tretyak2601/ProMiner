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
        [SerializeField] Color32[] groundColors;

        [SerializeField] int startWidth;
        [SerializeField] int startHeight;

        List<Ground> topGrounds = new List<Ground>();
        List<Color32> colors = new List<Color32>();
        List<Color32> usedColors = new List<Color32>();

        const float groundWidth = 0.438f;
        const float groundHeight = 0.425f;
        Color32 color = Color.green;

        void Start()
        {
            StartCoroutine(CreateGround());
        }

        IEnumerator CreateGround()
        {
            int colorLayers = startHeight / groundColors.Length;

            for (int i = 0; i < startHeight; i++)
            {
                for (int k = 0; k < groundColors.Length; k++)
                {
                    if (i > colorLayers * k && i <= colorLayers * (k + 1) && usedColors.Count == k)
                        ChangeColor(out color);
                }

                for (int j = 0; j < startWidth; j++)
                {
                    var obj = Instantiate(groundPrefab, (Vector3.zero + Vector3.right * groundWidth * j) + (Vector3.down * groundHeight * i), Quaternion.identity, transform);

                    if (i == 0)
                    {
                        var top = Instantiate(topGroundPrefab, Vector3.up * groundHeight / 2, Quaternion.identity, obj.transform);
                        top.transform.localPosition = new Vector3(0, top.transform.localPosition.y, -1);
                    }
                    
                    obj.Init(i, color);
                    topGrounds.Add(obj);
                }
            }
            
            float allWidth = startWidth * groundWidth;
            float center = allWidth / 2 - groundWidth / 2;
            transform.position -= Vector3.right * center;

            topGrounds.ForEach(g => g.gameObject.isStatic = true);
            //UnityEditor.StaticOcclusionCulling.GenerateInBackground();
            yield return null;
        }

        void ChangeColor(out Color32 color)
        {
            while (true)
            {
                color = groundColors[Random.Range(0, 5)];

                if (!usedColors.Contains(color))
                {
                    usedColors.Add(color);
                    break;
                }
            }
        }
    }
}
