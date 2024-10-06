using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    public ItemObjData itemObjData = new ItemObjData();

    Button button;
    public RectTransform rectTransform;
    public Image visual;
    public TextMeshProUGUI amountText;

    bool isSelected;


    private void Awake()
    {
        button = visual.GetComponent<Button>();

        button.onClick.AddListener(OnClickThisItem);
    }

    private void Update()
    {
        if (isSelected)
        {
            transform.position = PlayerManager.Instance.mousePos;
        }
    }

    public void OnClickThisItem()
    {
        if (PlayerUI.Instance.HasItemObjSelected())
        {
            ItemObj itemObj = PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>();
            if (itemObjData.item == itemObj.itemObjData.item)
            {
                if (itemObjData.amount < itemObjData.item.maxStack)
                {
                    AddItemAmount();
                    itemObj.RemoveItemAmount();
                }
            }
        }
        else
        {
            SelectThisItem();
        }

        if (PlayerManager.Instance.curStorage != null)
        {
            PlayerManager.Instance.curStorage.UpdateStorageData();
        }

    }

    void SelectThisItem()
    {
        if (!PlayerUI.Instance.HasItemObjSelected())
        {
            visual.raycastTarget = false;
            amountText.raycastTarget = false;
            isSelected = true;

            if (itemObjData.handSlot != null) Rotate(false);

            visual.rectTransform.sizeDelta = new Vector2(itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight) * 100;
            visual.rectTransform.anchoredPosition = new Vector2(itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight) * 100 / 2;

            if (transform.parent == PlayerUI.Instance.itemParent)
            {
                for (int i = 0; i < itemObjData.pressSlotsXY.Count; i++)
                {
                    SlotUI slot = PlayerManager.Instance.GetInventorySlot(itemObjData.pressSlotsXY[i].x, itemObjData.pressSlotsXY[i].y, PlayerUI.Instance.slotParent);
                    slot.hasItem = false;
                }
            }
            else if (transform.parent == PlayerUI.Instance.storageParent)
            {
                for (int i = 0; i < itemObjData.pressSlotsXY.Count; i++)
                {
                    SlotUI slot = PlayerManager.Instance.GetStorageSlot(itemObjData.pressSlotsXY[i].x, itemObjData.pressSlotsXY[i].y, PlayerUI.Instance.storageSlot);
                    slot.hasItem = false;
                }
            }
            else
            {
                return;
            }

            itemObjData.pressSlotsXY.Clear();

            if (itemObjData.handSlot != null)
            {
                itemObjData.handSlot.hasItem = false;
                itemObjData.handSlot = null;
            }

            PlayerUI.Instance.curItemObjSelected = gameObject;
        }
    }

    public void UnSelected(SlotUI pressSlot)
    {
        if (!itemObjData.isRotate)
        {
            if (pressSlot.itemParent == PlayerUI.Instance.itemParent)
            {
                visual.rectTransform.anchoredPosition = new Vector2(itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight) * 100 / 2;
                itemObjData.pressSlotsXY = PlayerManager.Instance.GetInventorySlot(pressSlot, itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight, PlayerUI.Instance.slotParent.transform);
            }
            else if (pressSlot.itemParent == PlayerUI.Instance.storageParent)
            {
                visual.rectTransform.anchoredPosition = new Vector2(itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight) * 100 / 2;
                itemObjData.pressSlotsXY = PlayerManager.Instance.GetStorageSlot(pressSlot, itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight, PlayerUI.Instance.storageSlot.transform);
            }
            else
            {
                return;
            }
        }
        else
        {
            if (pressSlot.itemParent == PlayerUI.Instance.itemParent)
            {
                visual.rectTransform.anchoredPosition = new Vector2(itemObjData.item.itemGridHeight, itemObjData.item.itemGridWidth) * 100 / 2;
                itemObjData.pressSlotsXY = PlayerManager.Instance.GetInventorySlot(pressSlot, itemObjData.item.itemGridHeight, itemObjData.item.itemGridWidth, PlayerUI.Instance.slotParent.transform);
            }
            else if (pressSlot.itemParent == PlayerUI.Instance.storageParent)
            {
                visual.rectTransform.anchoredPosition = new Vector2(itemObjData.item.itemGridHeight, itemObjData.item.itemGridWidth) * 100 / 2;
                itemObjData.pressSlotsXY = PlayerManager.Instance.GetStorageSlot(pressSlot, itemObjData.item.itemGridHeight, itemObjData.item.itemGridWidth, PlayerUI.Instance.storageSlot.transform);
            }
            else
            {
                return;
            }
        }

        visual.raycastTarget = true;
        amountText.raycastTarget = true;
        isSelected = false;
        transform.SetParent(pressSlot.itemParent);
        rectTransform.anchoredPosition = pressSlot.GetButtonLeftPosition();
        visual.rectTransform.sizeDelta = new Vector2(itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight) * 100;

        if (pressSlot.itemParent == PlayerUI.Instance.itemParent)
        {
            for (int i = 0; i < itemObjData.pressSlotsXY.Count; i++)
            {
                SlotUI slot = PlayerManager.Instance.GetInventorySlot(itemObjData.pressSlotsXY[i].x, itemObjData.pressSlotsXY[i].y, PlayerUI.Instance.slotParent);
                slot.hasItem = true;
            }
        }
        else if (pressSlot.itemParent == PlayerUI.Instance.storageParent)
        {
            for (int i = 0; i < itemObjData.pressSlotsXY.Count; i++)
            {
                SlotUI slot = PlayerManager.Instance.GetStorageSlot(itemObjData.pressSlotsXY[i].x, itemObjData.pressSlotsXY[i].y, PlayerUI.Instance.storageSlot);
                slot.hasItem = true;
            }
        }
        else
        {
            return;
        }

        PlayerUI.Instance.curItemObjSelected = null;

        if (PlayerManager.Instance.curStorage != null)
        {
            PlayerManager.Instance.curStorage.UpdateStorageData();
        }

    }

    public void UnSelected(HandSlotUI handSlot)
    {
        visual.raycastTarget = true;
        amountText.raycastTarget = true;
        isSelected = false;
        Rotate(false);
        transform.SetParent(handSlot.rectTransform);
        rectTransform.anchoredPosition = handSlot.GetButtonLeftPosition();
        visual.rectTransform.sizeDelta = new Vector2(handSlot.rectTransform.rect.width, handSlot.rectTransform.rect.height);
        visual.rectTransform.anchoredPosition = new Vector2(handSlot.rectTransform.rect.width, handSlot.rectTransform.rect.height) / 2;
        handSlot.hasItem = true;
        itemObjData.handSlot = handSlot;
        PlayerUI.Instance.curItemObjSelected = null;
        if (PlayerManager.Instance.curStorage != null)
        {
            PlayerManager.Instance.curStorage.UpdateStorageData();
        }
    }

    public void ToggleRotate()
    {
        itemObjData.isRotate = !itemObjData.isRotate;
        if (itemObjData.isRotate)
        {
            visual.rectTransform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            visual.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

    }

    public void Rotate(bool isRotate)
    {
        itemObjData.isRotate = isRotate;
        if (itemObjData.isRotate)
        {
            visual.rectTransform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            visual.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

    }

    public void AddItemAmount()
    {
        itemObjData.amount++;
        UpdateAmountText();
    }

    public void AddItemAmount(int count)
    {
        itemObjData.amount += count;
        if (itemObjData.amount > itemObjData.item.maxStack)
        {
            itemObjData.amount = itemObjData.item.maxStack;
        }
        UpdateAmountText();
    }

    public void RemoveItemAmount()
    {
        itemObjData.amount--;
        UpdateAmountText();
        if (itemObjData.amount <= 0)
        {
            DestroyItem();
        }
    }

    public void DestroyItem()
    {
        PlayerUI.Instance.curItemObjSelected = null;
        Destroy(gameObject);
    }

    public void UpdateAmountText()
    {
        amountText.text = itemObjData.amount.ToString();
    }

}
