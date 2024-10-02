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

    [Header("===== Storage =====")]
    [Header("- Toggle Show and Hide storage")]
    public Transform storageTab;
    [Header("- Item Obj")]
    public Transform storageParent;
    [Header("- Storage UI")]
    public Transform storageItemParent;

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
                Vector2 origin = new Vector2(-size.x * width, -size.y * height) / 2;
                rect.anchoredPosition = new Vector3(x, y, 0) * size + origin;

                SlotUI slotUi = obj.GetComponent<SlotUI>();
                SlotNode slot = new SlotNode(slotUi, x, y);
                slots[x, y] = slot;
            }
        }

        return slots;
    }

    #region Inventory

    public void ToggleInventory()
    {
        if (inventoryTab.gameObject.activeSelf)
        {
            inventoryTab.gameObject.SetActive(false);
            storageTab.gameObject.SetActive(false);
        }
        else
        {
            inventoryTab.gameObject.SetActive(true);
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

    public void SpawnItem(ItemObj item, SlotUI pressPos)
    {
        if (pressPos.CanPress(item))
        {
            item.UnSelected(pressPos);
        }
        else
        {
            Destroy(item.gameObject);
        }
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
            itemObj.RotateOnSelected();
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



    #endregion

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            GameObject item = InitItemObj(testItem, 1);
            SpawnItem(item.GetComponent<ItemObj>(), PlayerManager.Instance.GetSlot(0, 1, slotParent.transform));
        }
    }

}
