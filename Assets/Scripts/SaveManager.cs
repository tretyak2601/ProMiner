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

        private static volatile List<GroundData> savedData = new List<GroundData>();
        private static string path;
        private static string path1;

        private void Start()
        {
            axe.OnXmlChange += AddNode;
            path = Path.Combine(Application.persistentDataPath, "grounds.dat");
            path1 = Path.Combine(Application.persistentDataPath, "xml_grounds.xml");
            Load();
            Thread auto = new Thread(AutoSave);
            auto.Start();
        }

        void AutoSave()
        {
            Thread.Sleep(5000);
            Save();
        }

        private void Save()
        {
            savedData.Clear();

            foreach (var g in gb.Grounds)
                savedData.Add(new GroundData(g.Value.Item1, g.Value.Item2));

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                formatter.Serialize(fs, savedData);
                Debug.LogAssertion("Successfuly serializated");
            }

            XMLSave();
        }

        private void Load()
        {
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    savedData = (List<GroundData>)formatter.Deserialize(fs);
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
                ser.Serialize(fs, savedData);
                Debug.LogAssertion("Successfuly xml serializated");
            }
        }

        private void RemoveNode(Vector2 pos)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path1);

            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("//ArrayOfGroundData");
            foreach (XmlNode node in nodes)
            {
                if (node["position"]["x"].InnerText == pos.x.ToString() &&
                    node["position"]["y"].InnerText == pos.y.ToString())
                    node.RemoveAll();
            }
        }

        private void AddNode(KeyValuePair<int, (Ground, Vector3)> pair)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path1);

            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("//ArrayOfGroundData");

            var gd = doc.CreateElement("GroundData");
            var type = doc.CreateElement("type");
            type.InnerText = pair.Value.Item1.GroundType.ToString();

            var pos = doc.CreateElement("position");

            var x = doc.CreateElement("x");
            x.InnerText = pair.Value.Item2.x.ToString();
            var y = doc.CreateElement("y");
            y.InnerText = pair.Value.Item2.y.ToString();
            var z = doc.CreateElement("z");
            z.InnerText = pair.Value.Item2.z.ToString();

            pos.AppendChild(x);
            pos.AppendChild(y);
            pos.AppendChild(z);

            var col = doc.CreateElement("color");

            var r = doc.CreateElement("r");
            r.InnerText = pair.Value.Item1.Color.r.ToString();
            var g = doc.CreateElement("g");
            g.InnerText = pair.Value.Item1.Color.g.ToString();
            var b = doc.CreateElement("b");
            b.InnerText = pair.Value.Item1.Color.b.ToString();
            var a = doc.CreateElement("a");
            a.InnerText = "1";

            col.AppendChild(r);
            col.AppendChild(g);
            col.AppendChild(b);

            var indexI = doc.CreateElement("indexI");
            indexI.InnerText = pair.Value.Item1.listIndex.Key.ToString();
            var indexJ = doc.CreateElement("indexJ");
            indexJ.InnerText = pair.Value.Item1.DownPos.ToString();

            gd.AppendChild(type);
            gd.AppendChild(pos);
            gd.AppendChild(col);
            gd.AppendChild(indexI);
            gd.AppendChild(indexJ);
            
            root.AppendChild(gd);
            doc.Save(Console.Out);
            Debug.LogAssertion("Successfuly added");
        }
    }

    [Serializable]
    public struct GroundData
    {
        public GroundType type;
        public SerializableVector position;
        public SerializableColor color;
        public int indexI;
        public int indexJ;

        public GroundData(Ground g, Vector3 pos)
        {
            this.type = g.GroundType;
            this.position = SerializeUtility.ToGroundVector(pos);
            this.color = SerializeUtility.ToGroundColor(g.Color);
            this.indexJ = g.listIndex.Key;
            this.indexI = g.DownPos;
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

        public static SerializableVector ToGroundVector(Vector3 c)
        {
            return new SerializableVector(c);
        }

        public static Vector3 ToVector(SerializableVector c)
        {
            return new Vector3(c.x, c.y, c.z);
        }
    }

    [Serializable]
    public class SerializableColor
    {
        public float r, g, b, a;

        public SerializableColor()
        {

        }

        public SerializableColor(Color c)
        {
            this.r = c.r;
            this.g = c.g;
            this.b = c.b;
            this.a = c.a;
        }
    }

    [Serializable]
    public class SerializableVector
    {
        public float x, y, z;

        public SerializableVector()
        {

        }

        public SerializableVector(Vector3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }
    }
}
