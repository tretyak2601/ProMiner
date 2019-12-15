using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TRGames.ProMiner.Gameplay;

public class LevelEditor : EditorWindow
{
    GroundType groundType;
    GameObject parentObject;
    Ground groundPrefab;
    TNTController tnt;
    bool addTNT;
    DimondsObject diamonds;
    PowerUpsScriptable powerUps;

    string width = default;
    string height = default;

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditor>("Level editor");
    }

    private void OnGUI()
    {
        width = EditorGUILayout.TextField("Width: ", width);
        height = EditorGUILayout.TextField("Height: ", height);
        groundType = (GroundType)EditorGUILayout.EnumPopup("Ground type", groundType);
        addTNT = EditorGUILayout.Toggle("Add TNT", addTNT);

        tnt = Resources.Load<TNTController>("tnt");
        groundPrefab = Resources.Load<Ground>("Ground");
        diamonds = Resources.Load<DimondsObject>("Diamonds");
        powerUps = Resources.Load<PowerUpsScriptable>("PowerUps");

        if (GUILayout.Button("Build"))
        {
            if (parentObject == null)
            {
                parentObject = new GameObject("PARENT");
                parentObject.AddComponent<GroundBuilder>().Grounds = new LinkedList<KeyValuePair<Ground, Vector3>>();

                parentObject.transform.position = Vector3.zero;
                Build();
            }
        }
    }

    private void Build()
    {
        int Width = int.Parse(width);
        int Height = int.Parse(height);

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                int random = Random.Range(0, 200);
                Vector3 pos = (Vector3.zero + Vector3.right * groundPrefab.GetComponent<SpriteRenderer>().size.x / 6 * j) + (Vector3.down * groundPrefab.GetComponent<SpriteRenderer>().size.y / 6 * i);

                if (random == 0 && addTNT)
                {
                    Instantiate(tnt, pos, Quaternion.identity, parentObject.transform);
                    continue;
                }
                else if (random == 1 || random == 14)
                {
                    Instantiate(diamonds.crystals[Random.Range(0, diamonds.crystals.Length)], pos, Quaternion.identity, parentObject.transform);
                    continue;
                }
                else if (random == 2 || random == 16)
                {
                    Instantiate(powerUps.PowerUps[Random.Range(0, powerUps.PowerUps.Length)], pos, Quaternion.identity, parentObject.transform);
                    continue;
                }
                else if (random == 3 || random == 4 || random == 5 ||
                    random == 6 || random == 7 || random == 8 || random == 9 ||
                    random == 10 || random == 11 || random == 12 || random == 13)
                {
                    if (!(i == 0 || j == 0 || i == Height - 1 || j == Width - 1))
                        continue;
                }


                var obj = Instantiate(groundPrefab, pos, Quaternion.identity, parentObject.transform);

                if (i == 0 || j == 0 || i == Height - 1 || j == Width - 1)
                    obj.Init(parentObject.GetComponent<GroundBuilder>(), groundType, true);
                else
                    obj.Init(parentObject.GetComponent<GroundBuilder>(), groundType);

                if (obj != null)
                    parentObject.GetComponent<GroundBuilder>().Grounds.AddLast(new KeyValuePair<Ground, Vector3>(obj, obj.transform.position));
            }
        }
    }
}
