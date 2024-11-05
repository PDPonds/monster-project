using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : Singleton<PlayerUI>
{
    [Header("===== Hand =====")]
    public Transform inGameHandVisualParent;
    [SerializeField] Image slot1Visual;
    [SerializeField] TextMeshProUGUI slot1AmountText;
    [SerializeField] GameObject slot1Border;
    [SerializeField] Image slot2Visual;
    [SerializeField] TextMeshProUGUI slot2AmountText;
    [SerializeField] GameObject slot2Border;
    [SerializeField] Image slot3Visual;
    [SerializeField] TextMeshProUGUI slot3AmountText;
    [SerializeField] GameObject slot3Border;

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

    [Header("===== Equipment =====")]
    [Header("- Hand Slot")]
    [SerializeField] Vector2 handSlotOffset;
    public List<Transform> handSlots = new List<Transform>();

    [Header("- Equipment Slot")]
    [SerializeField] Vector2 equipmentSlotOffset;
    public List<Transform> equipmentSlots = new List<Transform>();

    [Header("- In Hand Item")]
    [SerializeField] Transform leftHandParent;
    [SerializeField] Transform rightHandParent;

    [Header("===== Bin =====")]
    [SerializeField] Transform binSlot;
    public Transform confirmToDestroyBG;
    [SerializeField] Image confirmItemIcon;
    [SerializeField] TextMeshProUGUI confirmItemName;
    [SerializeField] Button confirmDestroyButton;
    [SerializeField] Button cancleDestroyButton;
    ItemObj curSelectDestroyItemObj;

    [HideInInspector] public GameObject curItemObjSelected;

    [Header("===== Reload =====")]
    public Image reloadImg;

    [Header("===== Craft =====")]
    [SerializeField] Transform craftingPanel;
    [SerializeField] Transform craftingItemParent;
    [SerializeField] GameObject craftingItemObj;
    [Header("- Crafting Component")]
    [SerializeField] Image componentParent;
    [SerializeField] GameObject componentUIObj;

    [Header("===== Test =====")]
    public ItemSO testItem;

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
            HideInventoryPanel();

        }
        else
        {
            ClearItemOnHand();
            PlayerManager.Instance.isAim = false;
            inventoryTab.gameObject.SetActive(true);
            storageParent.gameObject.SetActive(false);
            craftingPanel.gameObject.SetActive(false);
            inGameHandVisualParent.gameObject.SetActive(false);
            PlayerManager.Instance.SwitchPhase(PlayerPhase.UIShow);
        }
    }

    public GameObject InitItemObj(ItemSO item, int amount)
    {
        GameObject obj = Instantiate(itemObjPrefab);

        ItemObj itemObj = obj.GetComponent<ItemObj>();
        itemObj.itemObjData.item = item;
        itemObj.itemObjData.amount = amount;
        itemObj.UpdateAmountText();
        itemObj.visual.sprite = item.itemSprite;

        return obj;
    }

    public bool TryAddItemToInventory(ItemSO item, int amount)
    {
        if (HasNotFullAmountItem(item, out int hasAmount, out int itemIndex))
        {
            List<ItemObj> items = GetItemInInventory();
            items[itemIndex].AddItemAmount(amount);
            return true;
        }
        else
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

    }

    public void RemoveItem(ItemSO item, int amount)
    {
        if (!HasItemCountForRemove(item, amount)) return;

        List<ItemObj> allItems = GetItemInInventory();
        if (allItems.Count == 0 || allItems == null || allItems.Count == 0) return;

        int removeAmount = amount;

        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].itemObjData.item == item)
            {
                int slotsAmount = allItems[i].itemObjData.amount;
                if (removeAmount <= slotsAmount)
                {
                    allItems[i].RemoveItemAmount(removeAmount);
                    return;
                }
                else
                {
                    removeAmount = slotsAmount;
                    int remainingAmount = amount - slotsAmount;
                    allItems[i].RemoveItemAmount(removeAmount);
                    removeAmount = remainingAmount;
                }

            }
        }
    }

    public bool HasItemCountForRemove(ItemSO item, int count)
    {
        int allItemCount = GetItemCountInInventory(item);

        if (allItemCount == 0) return false;

        if (allItemCount >= count) return true;
        else return false;
    }

    public int GetItemCountInInventory(ItemSO item)
    {
        int count = 0;
        List<ItemObj> allItems = GetItemInInventory();

        if (allItems.Count > 0)
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].itemObjData.item == item)
                {
                    count += allItems[i].itemObjData.amount;
                }
            }
        }

        return count;
    }

    public bool HasItemObjSelected()
    {
        return curItemObjSelected != null;
    }

    public void RotateSelectedObject()
    {
        if (HasItemObjSelected() && inventoryTab.gameObject.activeSelf && PlayerManager.Instance.IsPhase(PlayerPhase.UIShow))
        {
            ItemObj itemObj = curItemObjSelected.GetComponent<ItemObj>();
            itemObj.ToggleRotate();
        }
    }

    public List<ItemObj> GetItemInInventory()
    {
        List<ItemObj> items = new List<ItemObj>();
        if (itemParent.childCount > 0)
        {
            for (int i = 0; i < itemParent.childCount; i++)
            {
                ItemObj itemObj = itemParent.GetChild(i).GetComponent<ItemObj>();
                items.Add(itemObj);
            }
        }

        for (int i = 0; i < handSlots.Count; i++)
        {
            Transform slot = handSlots[i].transform;
            if (slot.transform.childCount > 0)
            {
                ItemObj item = slot.GetChild(0).GetComponent<ItemObj>();
                items.Add(item);
            }
        }

        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            Transform slot = equipmentSlots[i].transform;
            if (slot.transform.childCount > 0)
            {
                ItemObj item = slot.GetChild(0).GetComponent<ItemObj>();
                items.Add(item);
            }
        }

        return items;
    }

    public bool HasItem(ItemSO item, out int itemIndex)
    {
        List<ItemObj> items = GetItemInInventory();
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                ItemObjData data = items[i].itemObjData;
                if (item == data.item)
                {
                    itemIndex = i;
                    return true;
                }
            }
        }
        itemIndex = -1;
        return false;
    }

    public bool HasItem(ItemSO item, out ItemObj itemObj)
    {
        if (HasItem(item, out int index))
        {
            List<ItemObj> itemObjs = GetItemInInventory();
            itemObj = itemObjs[index];
            return true;
        }

        itemObj = null;
        return false;
    }

    public bool HasItem(ItemSO item, out List<ItemObj> items, out int count)
    {
        items = new List<ItemObj>();
        int amount = 0;
        List<ItemObj> allItems = GetItemInInventory();
        if (allItems.Count > 0)
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                ItemObjData data = allItems[i].itemObjData;
                if (item == data.item)
                {
                    items.Add(allItems[i]);
                    amount += data.amount;
                }
            }
        }

        count = amount;
        return items.Count > 0 && amount > 0;
    }

    public bool HasItem(int handIndex, out ItemObj itemObj)
    {
        if (handIndex < 1 || handIndex > 3)
        {
            itemObj = null;
            return false;
        }

        Transform slot = handSlots[handIndex - 1].transform;
        if (slot.transform.childCount > 0)
        {
            ItemObj item = slot.GetChild(0).GetComponent<ItemObj>();
            itemObj = item;
            return true;
        }

        itemObj = null;
        return false;
    }

    public bool HasItem(EquipmentSlotType type, out ItemObj itemObj)
    {
        if (type == EquipmentSlotType.Weapon)
        {
            itemObj = null;
            return false;
        }

        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            Transform slot = equipmentSlots[i].transform;
            EquipmentSlotUI ui = slot.GetComponent<EquipmentSlotUI>();
            if (ui.equipmentSlotType == type)
            {
                if (ui.transform.childCount > 0)
                {
                    ItemObj item = ui.transform.GetChild(0).GetComponent<ItemObj>();
                    itemObj = item;
                    return true;
                }
            }
        }

        itemObj = null;
        return false;
    }

    public bool HasNotFullAmountItem(ItemSO item, out int hasAmount, out int itemIndex)
    {
        List<ItemObj> items = GetItemInInventory();
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                ItemObjData data = items[i].itemObjData;
                if (item == data.item && data.amount < item.maxStack)
                {
                    itemIndex = i;
                    hasAmount = data.amount;
                    return true;
                }
            }
        }
        itemIndex = -1;
        hasAmount = 0;
        return false;
    }

    public void SetupEquipmentAndHandSlot()
    {
        Vector3 slot0Pos = slotParent.GetChild(0).GetComponent<Transform>().localPosition;

        if (handSlots.Count > 0)
        {
            for (int i = 0; i < handSlots.Count; i++)
            {
                RectTransform rect = handSlots[i].GetComponent<RectTransform>();
                Vector3 pos = rect.localPosition + new Vector3(handSlotOffset.x, handSlotOffset.y + slot0Pos.y, 0);
                rect.anchoredPosition = pos;
            }
        }

        float offset = PlayerManager.Instance.inventoryWidth * 90;
        RectTransform binRect = binSlot.GetComponent<RectTransform>();
        Vector3 binPos = binRect.localPosition + new Vector3(offset + slot0Pos.x, slot0Pos.y, 0);
        binRect.anchoredPosition = binPos;

        if (equipmentSlots.Count > 0)
        {
            for (int i = 0; i < equipmentSlots.Count; i++)
            {
                RectTransform rect = equipmentSlots[i].GetComponent<RectTransform>();
                Vector3 pos = rect.localPosition + new Vector3(equipmentSlotOffset.x + slot0Pos.x, equipmentSlotOffset.y, 0);
                rect.anchoredPosition = pos;
            }
        }

    }

    public void UpdateInGameHandVisual()
    {
        if (HasItem(1, out ItemObj itemObj1))
        {
            slot1Visual.gameObject.SetActive(true);
            slot1AmountText.gameObject.SetActive(true);
            ItemSO item = itemObj1.itemObjData.item;
            if (item is GunItem gun)
            {
                slot1Visual.sprite = item.itemSprite;
                int max = gun.maxAmmoInMag;
                int cur = itemObj1.curAmmoInMag;
                slot1AmountText.text = $"{cur} / {max}";
            }
            else
            {
                int amount = itemObj1.itemObjData.amount;
                if (amount > 1)
                {
                    slot1Visual.sprite = item.itemSprite;
                    slot1AmountText.text = $"{amount}";
                }
                else
                {
                    slot1Visual.gameObject.SetActive(false);
                    slot1AmountText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            slot1Visual.gameObject.SetActive(false);
            slot1AmountText.gameObject.SetActive(false);
        }

        if (HasItem(2, out ItemObj itemObj2))
        {
            slot2Visual.gameObject.SetActive(true);
            slot2AmountText.gameObject.SetActive(true);
            ItemSO item = itemObj2.itemObjData.item;
            if (item is GunItem gun)
            {
                slot2Visual.sprite = item.itemSprite;
                int max = gun.maxAmmoInMag;
                int cur = itemObj2.curAmmoInMag;
                slot2AmountText.text = $"{cur} / {max}";
            }
            else
            {
                int amount = itemObj2.itemObjData.amount;
                if (amount > 1)
                {
                    slot2Visual.sprite = item.itemSprite;
                    slot2AmountText.text = $"{amount}";
                }
                else
                {
                    slot2Visual.gameObject.SetActive(false);
                    slot2AmountText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            slot2Visual.gameObject.SetActive(false);
            slot2AmountText.gameObject.SetActive(false);
        }

        if (HasItem(3, out ItemObj itemObj3))
        {
            slot3Visual.gameObject.SetActive(true);
            slot3AmountText.gameObject.SetActive(true);
            ItemSO item = itemObj3.itemObjData.item;
            if (item is GunItem gun)
            {
                slot3Visual.sprite = item.itemSprite;
                int max = gun.maxAmmoInMag;
                int cur = itemObj3.curAmmoInMag;
                slot3AmountText.text = $"{cur} / {max}";
            }
            else
            {
                int amount = itemObj3.itemObjData.amount;
                if (amount > 1)
                {
                    slot3Visual.sprite = item.itemSprite;
                    slot3AmountText.text = $"{amount}";
                }
                else
                {
                    slot3Visual.gameObject.SetActive(false);
                    slot3AmountText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            slot3Visual.gameObject.SetActive(false);
            slot3AmountText.gameObject.SetActive(false);
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

    #region DestroyItemInInventory
    public void ShowConfirmToDestroy(ItemObj itemObj)
    {
        confirmToDestroyBG.gameObject.SetActive(true);
        confirmItemName.text = itemObj.itemObjData.item.itemName;
        confirmItemIcon.sprite = itemObj.itemObjData.item.itemSprite;
        curSelectDestroyItemObj = itemObj;
    }

    public void HideConfirmToDestroy()
    {
        curSelectDestroyItemObj = null;
        confirmToDestroyBG.gameObject.SetActive(false);
    }
    #endregion

    void HideInventoryPanel()
    {
        if (curItemObjSelected == null)
        {
            if (PlayerManager.Instance.curStorage != null) PlayerManager.Instance.curStorage.UpdateStorageData();

            inventoryTab.gameObject.SetActive(false);
            HideStorage();
            HideCraftingPanel();

            inGameHandVisualParent.gameObject.SetActive(true);
            UpdateInGameHandVisual();
            SelectItemInHand(PlayerManager.Instance.curSelectSlotIndex);

            PlayerManager.Instance.SwitchPhase(PlayerPhase.Normal);
        }
    }

    public void SelectItemInHand(int index)
    {
        if (PlayerManager.Instance.IsPhase(PlayerPhase.UIShow)) return;

        if (index < 1 || index > 3) return;

        if (leftHandParent.childCount > 0) for (int i = 0; i < leftHandParent.childCount; i++) { Destroy(leftHandParent.GetChild(i).gameObject); }
        if (rightHandParent.childCount > 0) for (int i = 0; i < rightHandParent.childCount; i++) { Destroy(rightHandParent.GetChild(i).gameObject); }

        slot1Border.gameObject.SetActive(false);
        slot2Border.gameObject.SetActive(false);
        slot3Border.gameObject.SetActive(false);

        if (index == 1) slot1Border.gameObject.SetActive(true);
        else if (index == 2) slot2Border.gameObject.SetActive(true);
        else if (index == 3) slot3Border.gameObject.SetActive(true);

        EquipmentSlotUI slot = handSlots[index - 1].GetComponent<EquipmentSlotUI>();
        PlayerManager.Instance.curSelectedSlot = slot;
        PlayerManager.Instance.curSelectSlotIndex = index;

        if (HasItem(index, out ItemObj item))
        {
            UseableItem useableItem = item.itemObjData.item as UseableItem;
            InitItemOnHand(useableItem);
            if (item.itemObjData.item is GunItem gun && item.curAmmoInMag == 0)
            {
                PlayerManager.Instance.TryReload();
            }
        }

    }

    void InitItemOnHand(UseableItem item)
    {
        switch (item.holdingSide)
        {
            case HandSide.Left:
                GameObject objLeft = Instantiate(item.prefab, leftHandParent);
                objLeft.transform.localPosition = item.spawnLeftPos;
                objLeft.transform.localRotation = Quaternion.Euler(item.rotationLeft);
                break;
            case HandSide.Right:
                GameObject objRight = Instantiate(item.prefab, rightHandParent);
                objRight.transform.localPosition = item.spawnRightPos;
                objRight.transform.localRotation = Quaternion.Euler(item.rotationRight);
                break;
        }

        if (item is MeleeItem meleeItem)
        {
            PlayerManager.Instance.anim.runtimeAnimatorController = meleeItem.attackOverride;
        }

    }

    void ClearItemOnHand()
    {
        if (leftHandParent.childCount > 0)
        {
            for (int i = 0; i < leftHandParent.childCount; i++)
            {
                Destroy(leftHandParent.GetChild(i).gameObject);
            }
        }

        if (rightHandParent.childCount > 0)
        {
            for (int i = 0; i < rightHandParent.childCount; i++)
            {
                Destroy(rightHandParent.GetChild(i).gameObject);
            }
        }
    }

    #region EscapeButton

    public void EscapePerformed()
    {
        if (inventoryTab.gameObject.activeSelf)
        {
            HideInventoryPanel();
        }
        else
        {
            Debug.Log("Pause");
        }
    }

    #endregion

    #region Crafting

    public void ShowCraftingPanel()
    {
        inventoryTab.gameObject.SetActive(true);
        craftingPanel.gameObject.SetActive(true);
        GenerateCraftingItemObj();
        PlayerManager.Instance.SwitchPhase(PlayerPhase.UIShow);
    }

    public void HideCraftingPanel()
    {
        craftingPanel.gameObject.SetActive(false);
    }

    void ClearCraftingItemParent()
    {
        if (craftingItemParent.childCount > 0)
        {
            for (int i = 0; i < craftingItemParent.childCount; i++)
            {
                Destroy(craftingItemParent.GetChild(i).gameObject);
            }
        }
    }

    public void GenerateCraftingItemObj()
    {
        ClearCraftingItemParent();
        List<ItemSO> canCraftItem = GetCanCraftItem();
        if (canCraftItem.Count > 0)
        {
            for (int i = 0; i < canCraftItem.Count; i++)
            {
                ItemSO item = canCraftItem[i];
                GameObject obj = Instantiate(craftingItemObj, craftingItemParent);
                CraftItemObj craftItemObj = obj.GetComponent<CraftItemObj>();
                craftItemObj.Setup(item);
            }
        }
    }

    List<ItemSO> GetCanCraftItem()
    {
        List<ItemSO> canCraftItem = new List<ItemSO>();
        List<ItemObj> allItemInInventory = GetItemInInventory();
        if (allItemInInventory.Count > 0)
        {
            for (int i = 0; i < allItemInInventory.Count; i++)
            {
                ItemObj curItemObj = allItemInInventory[i];
                ItemSO curItem = curItemObj.itemObjData.item;
                List<ItemSO> craftableItem = GameManager.Instance.allItemInGame.GetCanCraftItem(curItem);
                if (craftableItem.Count > 0)
                {
                    for (int j = 0; j < craftableItem.Count; j++)
                    {
                        ItemSO curCraftItem = craftableItem[j];
                        if (!HasItemInList(curCraftItem, canCraftItem))
                        {
                            canCraftItem.Add(curCraftItem);
                        }
                    }
                }
            }
        }
        return canCraftItem;
    }

    bool HasItemInList(ItemSO item, List<ItemSO> allItems)
    {
        if (allItems.Count > 0)
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i] == item)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void GenerateComponentItem(ItemSO item)
    {
        componentParent.gameObject.SetActive(true);
        ClearComponentItemParent();
        if (item != null && item.craftingComponents.Count > 0)
        {
            for (int i = 0; i < item.craftingComponents.Count; i++)
            {
                ItemSO curComponent = item.craftingComponents[i].item;
                int curComponentAmount = item.craftingComponents[i].amount;
                int curItemInInventoryCount = 0;
                if (HasItem(curComponent, out List<ItemObj> itemObj, out int count)) curItemInInventoryCount = count;
                GameObject obj = Instantiate(componentUIObj, componentParent.transform);
                Image componentSprite = obj.transform.GetChild(0).GetComponent<Image>();
                TextMeshProUGUI componentName = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                float rectWidth = 1f;
                float rectHeight = 1f;
                if (curComponent.itemGridWidth >= curComponent.itemGridHeight)
                {
                    rectWidth = (float)curComponent.itemGridWidth / (float)curComponent.itemGridHeight;
                    rectHeight = (float)curComponent.itemGridHeight / (float)curComponent.itemGridHeight;
                }
                else
                {
                    rectWidth = (float)curComponent.itemGridWidth / (float)curComponent.itemGridWidth;
                    rectHeight = (float)curComponent.itemGridHeight / (float)curComponent.itemGridWidth;
                }
                componentSprite.rectTransform.sizeDelta = new Vector2(rectWidth * 30f, rectHeight * 30f);
                componentSprite.sprite = curComponent.itemSprite;
                string name = string.Empty;
                name += curComponent.itemName;
                name += "     ";
                name += $"{curItemInInventoryCount} / {curComponentAmount}";
                componentName.text = name;
                if (curItemInInventoryCount >= curComponentAmount) componentName.color = Color.green;
                else componentName.color = Color.red;
            }
        }

    }

    public void HideCraftingComponent()
    {
        componentParent.gameObject.SetActive(false);
    }

    void ClearComponentItemParent()
    {
        if (componentParent.transform.childCount > 0)
        {
            for (int i = 0; i < componentParent.transform.childCount; i++)
            {
                Destroy(componentParent.transform.GetChild(i).gameObject);
            }
        }
    }

    #endregion

    private void Awake()
    {
        confirmDestroyButton.onClick.AddListener(() =>
        {
            if (curSelectDestroyItemObj != null)
            {
                curSelectDestroyItemObj.DestroyItem();
                HideConfirmToDestroy();
            }
        });
        cancleDestroyButton.onClick.AddListener(HideConfirmToDestroy);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TryAddItemToInventory(testItem, 1);
        }

    }

}
