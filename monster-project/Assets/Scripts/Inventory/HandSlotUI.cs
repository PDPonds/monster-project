using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandSlotUI : MonoBehaviour
{
    public RectTransform rectTransform;
    Button button;

    [HideInInspector] public bool hasItem;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TryPressObject);
    }

    public Vector2 GetCenterPosition()
    {
        return rectTransform.anchoredPosition;
    }

    public Vector2 GetButtonLeftPosition()
    {
        Vector2 center = GetCenterPosition();
        float width = rectTransform.rect.size.x;
        float height = rectTransform.rect.size.y;
        Vector2 buttonLeft = new Vector2(center.x - (width / 2), center.y - (height / 2));
        return buttonLeft;
    }

    public void TryPressObject()
    {
        if (PlayerUI.Instance.HasItemObjSelected())
        {
            ItemObj itemObj = PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>();
            if (CanPress())
            {
                itemObj.UnSelected(this);
            }
        }

    }

    public bool CanPress()
    {
        return hasItem == false;
    }

}
