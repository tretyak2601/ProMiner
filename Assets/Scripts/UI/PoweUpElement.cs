using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoweUpElement : MonoBehaviour
{
    [SerializeField] Image[] risks;

    private int maxCount = 7;
    private int powerUpCount = 0;

    PowerUpMenu menu;

    public Action OnIncrement;
    public Action OnDecrement;

    public void Init(PowerUpMenu menu, int count)
    {
        this.menu = menu;
        powerUpCount = count;

        for (int i = 0; i < powerUpCount; i++)
            risks[i].color = Color.green;
    }

    public void Add()
    {
        if (menu.TotalCount == 0)
            return;
        else
            menu.TotalCount--;

        for (int i = 0; i < powerUpCount + 1; i++)
            if (i < maxCount)
                risks[i].color = Color.green;

        if (powerUpCount < maxCount)
            powerUpCount++;

        OnIncrement?.Invoke();
    }

    public void Remove()
    {
        if (menu.TotalCount == maxCount || powerUpCount == 0)
            return;
        else
            menu.TotalCount++;

        foreach (var r in risks)
            r.color = Color.gray;

        for (int i = 0; i < powerUpCount - 1; i++)
            if (i < maxCount)
                risks[i].color = Color.green;

        if (powerUpCount > 0)
            powerUpCount--;

        OnDecrement?.Invoke();
    }
}
