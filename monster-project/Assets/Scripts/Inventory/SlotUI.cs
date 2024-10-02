using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    RectTransform rectTransform;
    Button button;

    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public bool hasItem;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        button.onClick.AddListener(TryPressObject);
    }

    private void Update()
    {
        if (hasItem)
        {
            Image img = GetComponent<Image>();
            img.color = Color.red;
        }
        else
        {
            Image img = GetComponent<Image>();
            img.color = Color.gray;
        }
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
            if (CanPress(PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>()))
            {
                ItemObj itemObj = PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>();
                itemObj.UnSelected(this);
            }
        }

    }

    public bool CanPress(ItemObj item)
    {
        List<SlotUI> slots = new List<SlotUI>();
        if (!item.isRotate) slots = PlayerManager.Instance.GetSlot(this, item.item.itemGridWidth, item.item.itemGridHeight, PlayerUI.Instance.slotParent.transform);
        else slots = PlayerManager.Instance.GetSlot(this, item.item.itemGridHeight, item.item.itemGridWidth, PlayerUI.Instance.slotParent.transform);

        if (slots.Count != item.item.GetSlotSize()) return false;

        if (slots.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (!PlayerManager.Instance.IsSlotNotOutOfInventorySlot(slots[i].x, slots[i].y))
                {
                    return false;
                }
                else
                {
                    if (slots[i].hasItem)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

}
