using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TRGames.ProMiner.Gameplay
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] bool isFreeMode;
        [SerializeField] GroundBuilder gb;
        [SerializeField] PickAxe axe;
        [SerializeField] LoadingManager loading;

        private static List<GroundData> savedData = new List<GroundData>();
        private static string path1;

        private volatile bool isSaving = false;

        private void Start()
        {
            if (!isFreeMode)
            {
                axe.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                loading.OffLoading();
                return;
            }

            path1 = Path.Combine(Application.persistentDataPath, "xml_grounds.xml");
            Load();
        }

        private void Load()
        {
            if (File.Exists(path1))
            {
                using (FileStream fs = new FileStream(path1, FileMode.OpenOrCreate))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(List<GroundData>));
                    savedData = (List<GroundData>)xml.Deserialize(fs);
                    Debug.LogAssertion("Successfuly deserializated");
                }

                StartCoroutine(gb.Create(savedData));
            }
            else
                gb.CreateEmpty();
        }

        private void XMLSave()
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<GroundData>));

            using (FileStream fs = new FileStream(path1, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                savedData.Clear();
                savedData = new List<GroundData>();

                foreach (var g in gb.Grounds)
                    savedData.Add(new GroundData(g.Key, g.Value));

                ser.Serialize(fs, savedData);
                Debug.LogAssertion("Successfuly xml serializated");
            }
        }
        
        private void OnApplicationQuit()
        {
            if (!isFreeMode)
                return;

            XMLSave();
            PlayerPrefs.SetString("lastPositionX", axe.transform.position.x.ToString());
            PlayerPrefs.SetString("lastPositionY", axe.transform.position.y.ToString());
            PlayerPrefs.Save();
        }
    }

    [Serializable]
    public struct GroundData
    {
        public SerializableVector position;
        public SerializableColor color;

        public GroundData(Ground g, Vector3 pos)
        {
            this.position = SerializeUtility.ToGroundVector(pos);
            this.color = SerializeUtility.ToGroundColor(g.Color);
        }
    }

    public class SerializeUtility
    {
        public static SerializableColor ToGroundColor(Color c)
        {
            return new SerializableColor(c);
        }

        public static Color ToColor(SerializableColor c)
        {
            return new Color(c.r, c.g, c.b, 1);
        }

        public static SerializableVector ToGroundVector(Vector2 c)
        {
            return new SerializableVector(c);
        }

        public static Vector3 ToVector(SerializableVector c)
        {
            return new Vector2(Convert.ToSingle(c.x), Convert.ToSingle(c.y));
        }
    }

    [Serializable]
    public class SerializableColor
    {
        public float r, g, b;

        public SerializableColor()
        {

        }

        public SerializableColor(Color c)
        {
            this.r = c.r;
            this.g = c.g;
            this.b = c.b;
        }
    }

    [Serializable]
    public class SerializableVector
    {
        public float x, y;

        public SerializableVector()
        {

        }

        public SerializableVector(Vector2 v)
        {
            this.x = Convert.ToSingle((double)v.x);
            this.y = Convert.ToSingle((double)v.y);
        }
    }
}
