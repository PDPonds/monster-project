using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    Button button;

    [HideInInspector]public Transform itemParent;

    [HideInInspector]public int x;
    [HideInInspector]public int y;
    [HideInInspector]public bool hasItem;

    private void Awake()
    {
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
            ItemObj itemObj = PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>();
            if (CanPress(PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>(), itemObj.itemObjData.isRotate))
            {
                itemObj.UnSelected(this);
            }
        }

    }

    public bool CanPress(ItemObj item, bool isRotate)
    {
        List<SlotUI> slots = new List<SlotUI>();
        if (!isRotate)
        {
            if (itemParent == PlayerUI.Instance.itemParent)
            {
                slots = PlayerManager.Instance.GetListInventorySlot(this, item.itemObjData.item.itemGridWidth, item.itemObjData.item.itemGridHeight, PlayerUI.Instance.slotParent.transform);
            }
            else if (itemParent == PlayerUI.Instance.storageParent)
            {
                slots = PlayerManager.Instance.GetListStorageSlot(this, item.itemObjData.item.itemGridWidth, item.itemObjData.item.itemGridHeight, PlayerUI.Instance.storageSlot.transform);
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (itemParent == PlayerUI.Instance.itemParent)
            {
                slots = PlayerManager.Instance.GetListInventorySlot(this, item.itemObjData.item.itemGridHeight, item.itemObjData.item.itemGridWidth, PlayerUI.Instance.slotParent.transform);
            }
            else if (itemParent == PlayerUI.Instance.storageParent)
            {
                slots = PlayerManager.Instance.GetListStorageSlot(this, item.itemObjData.item.itemGridHeight, item.itemObjData.item.itemGridWidth, PlayerUI.Instance.storageSlot.transform);
            }
            else
            {
                return false;
            }
        }

        if (slots.Count != item.itemObjData.item.GetSlotSize())
        {
            return false;
        }

        if (slots.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (itemParent == PlayerUI.Instance.itemParent)
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
                else if (itemParent == PlayerUI.Instance.storageParent)
                {
                    if (!PlayerManager.Instance.IsSlotNotOutOfStorageSlot(slots[i].x, slots[i].y))
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
                else
                {
                    return false;
                }
                
            }
        }
        return true;
    }

}
