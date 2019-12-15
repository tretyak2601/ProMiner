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
        public static GroundBuilder Instance;

        [SerializeField] Ground groundPrefab;

        [SerializeField] int startWidth;
        [SerializeField] int startHeight;

        public event Action OnGroundBuilt;

        public LinkedList<KeyValuePair<Ground, Vector3>> Grounds = new LinkedList<KeyValuePair<Ground, Vector3>>();

        const float groundWidth = 0.438f;
        const float groundHeight = 0.425f;
        Color32 color = Color.green;

        private float lastYPos = default;
        private bool enableAdding = true;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
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
                        obj.Init(this, GroundType.Default);
                        Grounds.AddLast(new KeyValuePair<Ground, Vector3>(obj, obj.transform.position));
                        index++;

                        if (index % 500 == 0)
                            yield return null;
                    }
                }
            }
            lastYPos = Grounds.Last.Value.Value.y - groundHeight;
            EndCreating();
        }

        void EndCreating()
        {
            foreach (var g in Grounds)
                g.Key.gameObject.isStatic = true;

            OnGroundBuilt?.Invoke();
        }

        IEnumerator AddNewGround()
        {
            enableAdding = false;
            var list = new List<KeyValuePair<Ground, Vector3>>();

            for (int i = 0; i < startHeight; i++)
            {
                for (int j = 0; j < startWidth; j++)
                {
                    Vector3 pos = (Vector3.zero + Vector3.right * groundPrefab.GetComponent<SpriteRenderer>().size.x * j) + (Vector3.down * groundPrefab.GetComponent<SpriteRenderer>().size.y * i) - Vector3.down * lastYPos;
                    var obj = Instantiate(groundPrefab, pos, Quaternion.identity, transform);
                    obj.Init(this, GroundType.Default);
                    list.Add(obj.listIndex);
                    Grounds.AddLast(new KeyValuePair<Ground, Vector3>(obj, obj.transform.position));
                }

                yield return new WaitForSeconds(0.1f);
            }

            //UnityEditor.StaticOcclusionCulling.GenerateInBackground();
            lastYPos = Grounds.Last.Value.Value.y - groundHeight;
            enableAdding = true;
        }
    }
}
