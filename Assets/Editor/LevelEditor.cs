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

    string width = default;
    string height = default;

    const float groundWidth = 0.438f;
    const float groundHeight = 0.425f;

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditor>("Level editor");
    }

    private void OnGUI()
    {
        width = EditorGUILayout.TextField("Width: ", width);
        height = EditorGUILayout.TextField("Height: ", height);
        groundPrefab = Resources.Load<Ground>("Ground");
        groundType = (GroundType)EditorGUILayout.EnumPopup("Ground type", groundType);

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
                Vector3 pos = (Vector3.zero + Vector3.right * groundWidth * j) + (Vector3.down * groundHeight * i);
                var obj = Instantiate(groundPrefab, pos, Quaternion.identity, parentObject.transform);

                if (i == 0 || j == 0 || i == Height - 1 || j == Width - 1)
                { // BORDERS
                    Debug.LogAssertion("NONE");
                    obj.Init(parentObject.GetComponent<GroundBuilder>(), GroundType.None);
                }
                else
                    obj.Init(parentObject.GetComponent<GroundBuilder>(), groundType);

                if (obj != null)
                    parentObject.GetComponent<GroundBuilder>().Grounds.AddLast(new KeyValuePair<Ground, Vector3>(obj, obj.transform.position));
            }
        }
    }
}
