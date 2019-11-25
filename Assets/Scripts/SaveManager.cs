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
        [SerializeField] GroundBuilder gb;
        [SerializeField] PickAxe axe;

        private static List<GroundData> savedData = new List<GroundData>();
        private static string path1;

        Queue<Task> tasks = new Queue<Task>();

        private volatile bool isSaving = false;

        private void Start()
        {
            axe.OnCubeDestroyed += OnCubeDestroyeHandler;
            gb.OnSaveXML += XMLSave;
            gb.OnGroundAdded += AddNode;

            path1 = Path.Combine(Application.persistentDataPath, "xml_grounds.xml");
            Load();
        }

        private void OnCubeDestroyeHandler(GroundType arg1, Vector2 position)
        {
            Task t = new Task(() => RemoveNode(position));

            if (!isSaving)
                t.Start();
            else
                tasks.Enqueue(t);
        }

        private void Update()
        {
            if (!isSaving && tasks.Count > 0)
                tasks.Dequeue().Start();
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
                    savedData.Add(new GroundData(g.Value.Item1, g.Value.Item2));

                ser.Serialize(fs, savedData);
                Debug.LogAssertion("Successfuly xml serializated");
            }
        }

        private void RemoveNode(Vector2 position)
        {
            isSaving = true;
            XmlDocument doc = new XmlDocument();
            doc.Load(path1);

            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("//GroundData");

            foreach (XmlNode node in nodes)
            {

                if (System.Math.Round(float.Parse(node["position"]["x"].InnerText.Replace('.', ',')), 3) == System.Math.Round(Convert.ToSingle((double)position.x), 3) &&
                    System.Math.Round(float.Parse(node["position"]["y"].InnerText.Replace('.', ',')), 3) == System.Math.Round(Convert.ToSingle((double)position.y), 3))
                {
                    node.ParentNode.RemoveChild(node);
                    doc.Save(path1);
                    Debug.LogAssertion("REMOVED");
                }
            }

            isSaving = false;
        }

        private void AddNode(List<KeyValuePair<int, (Ground, Vector3)>> list)
        {
            if (!File.Exists(path1))
                return;

            if (!isSaving)
                Add(list);
            else
            {
                Task t = new Task(() => Add(list));
                tasks.Enqueue(t);
            }
        }

        private void Add(List<KeyValuePair<int, (Ground, Vector3)>> list)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path1);

            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("//ArrayOfGroundData");

            foreach (var pair in list)
            {
                var gd = doc.CreateElement("GroundData");

                var pos = doc.CreateElement("position");

                var x = doc.CreateElement("x");
                x.InnerText = pair.Value.Item2.x.ToString().Replace(',', '.');
                var y = doc.CreateElement("y");
                y.InnerText = pair.Value.Item2.y.ToString().Replace(',', '.');

                pos.AppendChild(x);
                pos.AppendChild(y);

                var col = doc.CreateElement("color");

                var r = doc.CreateElement("r");
                r.InnerText = pair.Value.Item1.Color.r.ToString().Replace(',', '.');
                var g = doc.CreateElement("g");
                g.InnerText = pair.Value.Item1.Color.g.ToString().Replace(',', '.');
                var b = doc.CreateElement("b");
                b.InnerText = pair.Value.Item1.Color.b.ToString().Replace(',', '.');

                col.AppendChild(r);
                col.AppendChild(g);
                col.AppendChild(b);

                gd.AppendChild(pos);
                gd.AppendChild(col);

                root.AppendChild(gd);
            }

            doc.Save(path1);
            Debug.LogAssertion("Successfuly added");
        }

        private void OnApplicationQuit()
        {
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
