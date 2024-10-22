using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    public ItemObjData itemObjData = new ItemObjData();

    Button button;
    public RectTransform rectTransform;
    public Image visual;
    public TextMeshProUGUI amountText;

    bool isSelected;

    public int curAmmoInMag;

    private void Awake()
    {
        button = visual.GetComponent<Button>();
        ItemObjVisual itemObjVisual = visual.GetComponent<ItemObjVisual>();
        itemObjVisual.SetupItemVisual(this);

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

    public void SelectThisItem()
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
                transform.SetParent(PlayerUI.Instance.itemParent);
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

    public void UnSelected(EquipmentSlotUI handSlot)
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
        if (itemObjData.amount > itemObjData.item.maxStack)
        {
            itemObjData.amount = itemObjData.item.maxStack;
        }
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
        if (itemObjData.amount <= 0)
        {
            DestroyItem();
        }
        else
        {
            UpdateAmountText();
        }
    }

    public void RemoveItemAmount(int count)
    {
        itemObjData.amount -= count;
        if (itemObjData.amount <= 0)
        {
            DestroyItem();
        }
        else
        {
            UpdateAmountText();
        }
    }

    public int GetHalfAmount()
    {
        if (itemObjData.amount > 1)
        {
            int half = itemObjData.amount / 2;
            return half;
        }

        return 0;
    }

    public void DestroyItem()
    {
        PlayerUI.Instance.curItemObjSelected = null;
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

        Destroy(gameObject);
    }

    public void UpdateAmountText()
    {
        amountText.text = $"{itemObjData.amount} / {itemObjData.item.maxStack}";
    }

    public bool isGun(out GunItem gun)
    {
        if (itemObjData.item is GunItem gunItem)
        {
            gun = gunItem;
            return true;
        }

        gun = null;
        return false;
    }

    public bool isGun()
    {
        return itemObjData.item is GunItem;
    }

    public bool HasAmmo(out int curAmmoCount)
    {
        if (isGun() && curAmmoInMag > 0)
        {
            curAmmoCount = curAmmoInMag;
            return true;
        }

        curAmmoCount = 0;
        return false;
    }

    public void UseAmmo()
    {
        if (curAmmoInMag <= 0 || !isGun()) return;

        curAmmoInMag--;
        if (curAmmoInMag <= 0)
        {
            curAmmoInMag = 0;
        }
    }

    public void ReloadAmmo(int amount)
    {
        if (isGun(out GunItem gun))
        {
            curAmmoInMag += amount;
            if (curAmmoInMag > gun.maxAmmoInMag)
            {
                curAmmoInMag = gun.maxAmmoInMag;
            }
        }
    }

}
