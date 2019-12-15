using System.Collections;
using System.Collections.Generic;
using TRGames.ProMiner.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpMenu : MonoBehaviour
{
    [SerializeField] Text countText;

    [SerializeField] PoweUpElement sizeElement;
    [SerializeField] PoweUpElement speedElement;
    [SerializeField] PoweUpElement rageElement;

    private int totalCount;
    public int TotalCount
    {
        get
        {
            return totalCount;
        }
        set
        {
            countText.text = value.ToString();
            totalCount = value;
        }
    }

    private void Awake()
    {
        sizeElement.OnIncrement += () => PickAxe.Instance.transform.localScale += new Vector3(.05f, .05f, 0);
        sizeElement.OnDecrement += () => PickAxe.Instance.transform.localScale -= new Vector3(.05f, .05f, 0);

        TotalCount = 5;

        sizeElement.Init(this, 0);
        speedElement.Init(this, 0);
        rageElement.Init(this, 0);
    }

    private void OnEnable()
    {
        PickAxe.Instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnDisable()
    {
        PickAxe.Instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }
}
