using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TRGames.ProMiner.Gameplay;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] GroundSprite backgroundSprites;
    [SerializeField] Material waterMaterial;
    [SerializeField] SpriteRenderer background;

    public static LevelInitializer Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public void Init(Color color, GroundType type)
    {
        background.sprite = backgroundSprites.GetSprite(type);
        waterMaterial.SetColor("_Color", color);
        waterMaterial.SetColor("_StrokeColor", color);
    }
}
