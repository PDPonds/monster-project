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

    [Header("- Toggle Show and Hide Storage")]
    public Transform storageTab;
    [Header("- Inventory UI")]
    public Transform storageSlot;
    [Header("- Item Obj")]
    public Transform storageParent;

    [Header("Test")]
    public ItemSO testItem;
    [HideInInspector] public GameObject curItemObjSelected;

    public SlotNode[,] InitInventorySlot(int width, int height)
    {
        SlotNode[,] slots = new SlotNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = Instantiate(slotObj);
                obj.transform.SetParent(slotParent, false);
                RectTransform rect = obj.GetComponent<RectTransform>();
                Vector2 size = new Vector2(rect.rect.width, rect.rect.height);
                float originX = ((float)(-size.x * (float)width) / 2f) + size.x / 2f;
                float originY = ((float)(-size.y * (float)height) / 2f) + size.y / 2f;
                Vector2 origin = new Vector2(originX, originY);
                rect.anchoredPosition = new Vector3(x, y, 0) * size + origin;
                SlotUI slotUi = obj.GetComponent<SlotUI>();
                SlotNode slot = new SlotNode(slotUi, x, y, itemParent);
                slots[x, y] = slot;
            }
        }

        return slots;
    }

    public SlotNode[,] InitStorageSlot(int width, int height)
    {
        SlotNode[,] slots = new SlotNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = Instantiate(slotObj);
                obj.transform.SetParent(storageSlot);
                RectTransform rect = obj.GetComponent<RectTransform>();
                Vector2 size = new Vector2(rect.rect.width, rect.rect.height);
                float originX = ((float)(-size.x * (float)width)) + size.x / 2;
                float originY = ((float)(-size.y * (float)height) / 2f) + size.y / 2f;
                Vector2 origin = new Vector2(originX, originY);
                rect.anchoredPosition = new Vector3(x, y, 0) * size + origin;
                SlotUI slotUi = obj.GetComponent<SlotUI>();
                SlotNode slot = new SlotNode(slotUi, x, y, storageParent);
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
            if (curItemObjSelected == null)
            {
                if(PlayerManager.Instance.curStorage != null) PlayerManager.Instance.curStorage.UpdateStorageData();

                inventoryTab.gameObject.SetActive(false);
                HideStorage();
                PlayerManager.Instance.SwitchPhase(PlayerPhase.Normal);
            }
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
        itemObj.itemObjData.item = item;
        itemObj.itemObjData.amount = amount;
        itemObj.UpdateAmountText();
        itemObj.visual.sprite = item.itemSprite;

        return obj;
    }

    public bool TryAddItemToInventory(ItemSO item, int amount)
    {
        GameObject obj = InitItemObj(item, amount);
        ItemObj itemObj = obj.GetComponent<ItemObj>();

        for (int x = 0; x < PlayerManager.Instance.inventoryWidth; x++)
        {
            for (int y = 0; y < PlayerManager.Instance.inventoryHeight; y++)
            {
                SlotUI pressPos = PlayerManager.Instance.GetInventorySlot(x, y, slotParent.transform);
                if (pressPos.CanPress(itemObj, itemObj.itemObjData.isRotate))
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
                SlotUI pressPos = PlayerManager.Instance.GetInventorySlot(x, y, slotParent.transform);
                if (pressPos.CanPress(itemObj, itemObj.itemObjData.isRotate))
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

    #region Storage

    public void ShowStorage(int width, int height)
    {
        inventoryTab.gameObject.SetActive(true);
        storageTab.gameObject.SetActive(true);
        PlayerManager.Instance.storageSlots = InitStorageSlot(width, height);
        PlayerManager.Instance.SwitchPhase(PlayerPhase.UIShow);
    }

    public void HideStorage()
    {
        storageTab.gameObject.SetActive(false);
        ClearSlot(storageParent, storageSlot);
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TryAddItemToInventory(testItem, 1);
        }
    }

}
