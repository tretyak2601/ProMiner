using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = UnityEngine.Random;

namespace TRGames.ProMiner.Gameplay
{
    public class GroundBuilder : MonoBehaviour
    {
        [SerializeField] private PickAxe axe;
        [SerializeField] GameObject topGroundPrefab;
        [SerializeField] Ground groundPrefab;
        [SerializeField] private GameObject groundBorderPrefab;
        [SerializeField] Color32[] groundColors;

        [SerializeField] int startWidth;
        [SerializeField] int startHeight;

        public event Action OnGroundBuilt;
        public event Action<List<KeyValuePair<int, (Ground, Vector3)>>> OnGroundAdded;
        public event Action OnSaveXML;

        public LinkedList<KeyValuePair<int, (Ground, Vector3)>> Grounds { get; private set; } = new LinkedList<KeyValuePair<int, (Ground, Vector3)>>();
        private List<Color32> colors = new List<Color32>();
        private List<Color32> usedColors = new List<Color32>();

        const float groundWidth = 0.438f;
        const float groundHeight = 0.425f;
        Color32 color = Color.green;

        private float lastYPos = default;
        private bool enableAdding = true;

        public void CreateEmpty()
        {
            StartCoroutine(CreateGround());
        }

        public IEnumerator Create(List<GroundData> data)
        {
            int index = 0;

            lock (Grounds)
            {
                foreach (var d in data)
                {
                    if (d.position != null)
                    {
                        var obj = Instantiate(groundPrefab, SerializeUtility.ToVector(d.position), Quaternion.identity, transform);
                        obj.Init(SerializeUtility.ToColor(d.color), this);
                        Grounds.AddLast(new KeyValuePair<int, (Ground, Vector3)>(0, (obj, obj.transform.position)));
                        index++;

                        if (index % 500 == 0)
                            yield return null;
                    }
                }
            }
            lastYPos = Grounds.Last.Value.Value.Item2.y - groundHeight;
            EndCreating();
        }

        IEnumerator CreateGround()
        {
            int colorLayers = startHeight / groundColors.Length;
            yield return StartCoroutine(AddNewGround());

            for (int i = 0; i < startHeight; i++)
            {
                for (int j = 0; j < startWidth; j++)
                {
                    if (i == 0)
                    {
                        Ground g = null;

                        foreach (var gr in Grounds)
                        {
                            if (gr.Key == j)
                            {
                                g = gr.Value.Item1;
                                break;
                            }
                        }

                        var top = Instantiate(topGroundPrefab, Vector3.up * groundHeight / 2, Quaternion.identity, g.transform);
                        top.transform.localPosition = new Vector3(0, top.transform.localPosition.y, -1);
                    }
                }
            }

            EndCreating();
            OnSaveXML?.Invoke();
        }

        void EndCreating()
        {
            axe.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            AddBorders();

            foreach (var g in Grounds)
                g.Value.Item1.gameObject.isStatic = true;

            StartCoroutine(CheckGroundHeight());
            OnGroundBuilt?.Invoke();
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

        IEnumerator CheckGroundHeight()
        {
            while (true)
            {
                if (enableAdding && axe.transform.position.y < lastYPos + ((startHeight * groundHeight) / 1.3))
                    StartCoroutine(AddNewGround());

                yield return new WaitForSeconds(1);
            }
        }

        IEnumerator AddNewGround()
        {
            enableAdding = false;
            int colorLayers = startHeight / groundColors.Length;
            usedColors.Clear();
            var list = new List<KeyValuePair<int, (Ground, Vector3)>>();

            for (int i = 0; i < startHeight; i++)
            {
                for (int k = 0; k < groundColors.Length; k++)
                {
                    if (i > colorLayers * k && i <= colorLayers * (k + 1) && usedColors.Count == k)
                        ChangeColor(out color);
                }

                for (int j = 0; j < startWidth; j++)
                {
                    Vector3 pos = (Vector3.zero + Vector3.right * groundWidth * j) + (Vector3.down * groundHeight * i) - Vector3.down * lastYPos;
                    var obj = Instantiate(groundPrefab, pos, Quaternion.identity, transform);
                    obj.Init(color, this);
                    list.Add(obj.listIndex);
                    Grounds.AddLast(new KeyValuePair<int, (Ground, Vector3)>(j, (obj, obj.transform.position)));
                }

                yield return new WaitForSeconds(0.1f);
            }

            //UnityEditor.StaticOcclusionCulling.GenerateInBackground();
            lastYPos = Grounds.Last.Value.Value.Item2.y - groundHeight;
            enableAdding = true;
            OnGroundAdded?.Invoke(list);
        }

        private void AddBorders()
        {
            Vector2 leftEdge = default;
            Vector2 rightEdge = default;
            foreach (var VARIABLE in Grounds)
            {
                if (VARIABLE.Value.Item2.x < leftEdge.x)
                    leftEdge = VARIABLE.Value.Item2;

                if (VARIABLE.Value.Item2.x > rightEdge.x)
                    rightEdge = VARIABLE.Value.Item2;
            }

            var left = Instantiate(groundBorderPrefab, leftEdge, Quaternion.identity);
            left.transform.localEulerAngles = new Vector3(0, 0, 90);
            left.transform.position += Vector3.up * left.GetComponent<SpriteRenderer>().size.x / 2;
            var right = Instantiate(groundBorderPrefab, rightEdge, Quaternion.identity);
            right.transform.localEulerAngles = new Vector3(0, 0, 90);
            right.transform.position += Vector3.up * right.GetComponent<SpriteRenderer>().size.x / 2;
        }
    }
}
