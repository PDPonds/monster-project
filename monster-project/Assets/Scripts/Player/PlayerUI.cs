using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : Singleton<PlayerUI>
{
    [Header("===== Interact =====")]
    [SerializeField] GameObject interactCondition;

    [Header("===== Inventory =====")]
    public GameObject slotObj;
    public GameObject itemObjPrefab;

    [Header("- Toggle Show and Hide inventory")]
    public Transform inventoryTab;
    [Header("- Inventory UI")]
    public Transform slotParent;
    [Header("- Item Obj")]
    public Transform itemParent;

    [Header("Test")]
    public ItemSO testItem;
    [HideInInspector] public GameObject curItemObjSelected;

    public SlotNode[,] InitSlot(int width, int height, Transform parent, Transform itemParent)
    {
        SlotNode[,] slots = new SlotNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = Instantiate(slotObj);
                obj.transform.SetParent(parent, false);
                RectTransform rect = obj.GetComponent<RectTransform>();
                Vector2 size = new Vector2(rect.rect.width, rect.rect.height);
                float originX = ((float)(-size.x * (float)width) / 2f) + size.x / 2f;
                float originY = ((float)(-size.y * (float)height) / 2f) + size.y / 2f;
                Vector2 origin = new Vector2(originX, originY);
                rect.anchoredPosition = new Vector3(x, y, 0) * size + origin;
                SlotUI slotUi = obj.GetComponent<SlotUI>();
                SlotNode slot = new SlotNode(slotUi, x, y);
                slots[x, y] = slot;
            }
        }

        return slots;
    }

    public void ClearSlot(Transform parent, Transform itemParent)
    {
        if (parent.childCount > 0) for (int x = 0; x < parent.childCount; x++) Destroy(parent.GetChild(x).gameObject);
        if (itemParent.childCount > 0) for (int x = 0; x < itemParent.childCount; x++) Destroy(itemParent.GetChild(x).gameObject);
    }

    #region Inventory

    public void ToggleInventory()
    {
        if (inventoryTab.gameObject.activeSelf)
        {
            inventoryTab.gameObject.SetActive(false);
            PlayerManager.Instance.SwitchPhase(PlayerPhase.Normal);
        }
        else
        {
            inventoryTab.gameObject.SetActive(true);
            PlayerManager.Instance.SwitchPhase(PlayerPhase.UIShow);
        }
    }

    public GameObject InitItemObj(ItemSO item, int amount)
    {
        GameObject obj = Instantiate(itemObjPrefab, itemParent);

        ItemObj itemObj = obj.GetComponent<ItemObj>();
        itemObj.item = item;
        itemObj.amount = amount;
        itemObj.UpdateAmountText();
        itemObj.visual.sprite = item.itemSprite;

        return obj;
    }

    public bool TryAddItem(ItemSO item, int amount)
    {
        GameObject obj = InitItemObj(item, amount);
        ItemObj itemObj = obj.GetComponent<ItemObj>();

        for (int x = 0; x < PlayerManager.Instance.inventoryWidth; x++)
        {
            for (int y = 0; y < PlayerManager.Instance.inventoryHeight; y++)
            {
                SlotUI pressPos = PlayerManager.Instance.GetSlot(x, y, slotParent.transform);
                if (pressPos.CanPress(itemObj, itemObj.isRotate))
                {
                    itemObj.UnSelected(pressPos);
                    return true;
                }
            }
        }

        itemObj.ToggleRotate();

        for (int x = 0; x < PlayerManager.Instance.inventoryWidth; x++)
        {
            for (int y = 0; y < PlayerManager.Instance.inventoryHeight; y++)
            {
                SlotUI pressPos = PlayerManager.Instance.GetSlot(x, y, slotParent.transform);
                if (pressPos.CanPress(itemObj, itemObj.isRotate))
                {
                    itemObj.UnSelected(pressPos);
                    return true;
                }
            }
        }

        Destroy(obj);
        return false;

    }

    public bool HasItemObjSelected()
    {
        return curItemObjSelected != null;
    }

    public void RotateSelectedObject()
    {
        if (HasItemObjSelected() && inventoryTab.gameObject.activeSelf)
        {
            ItemObj itemObj = curItemObjSelected.GetComponent<ItemObj>();
            itemObj.ToggleRotate();
        }
    }

    #endregion

    #region Interact

    public void ShowInteractCondition()
    {
        interactCondition.SetActive(true);
    }

    public void HideInteractCondition()
    {
        interactCondition.SetActive(false);
    }

    #endregion


}
