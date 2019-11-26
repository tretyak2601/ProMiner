using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TRGames.ProMiner.Gameplay;

public class LevelEditor : EditorWindow
{
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
        color = EditorGUILayout.ColorField("Color", color);
        groundPrefab = Resources.Load<Ground>("Ground");

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

        if (GUILayout.Button("Colorize selectable"))
        {
            foreach (var obj in Selection.gameObjects)
                obj.GetComponent<Ground>()?.SetColor(color);
        }
    }

    Color color;

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
                obj.Init(parentObject.GetComponent<GroundBuilder>());

                if (obj != null)
                    parentObject.GetComponent<GroundBuilder>().Grounds.AddLast(new KeyValuePair<Ground, Vector3>(obj, obj.transform.position));
            }
        }
    }
}
