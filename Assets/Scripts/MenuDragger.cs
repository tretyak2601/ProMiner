using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TRGames.ProMiner.Gameplay;

public class MenuDragger : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform canvas;
    [SerializeField] RectTransform menu;
    [SerializeField] Image image;
    [SerializeField] PickAxe axe;

    private Vector2 endPos;
    private Vector2 startPosition;

    State state;

    private void Start()
    {
        state = State.Hidden;
        endPos = new Vector2(image.rectTransform.sizeDelta.x, image.rectTransform.sizeDelta.y);
        startPosition = menu.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        menu.anchoredPosition += new Vector2(0, eventData.delta.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (state == State.Hidden)
        {
            axe.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            menu.DOAnchorPos(Vector2.zero - Vector2.up * image.rectTransform.sizeDelta.y, 0.4f).OnComplete(() => state = State.Showed);
        }
        else
        {
            axe.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            menu.DOAnchorPos(new Vector2(0, -canvas.sizeDelta.y), 0.4f).OnComplete(() => state = State.Hidden);
        }
    }

    enum State
    {
        Hidden,
        Showed
    }
}
