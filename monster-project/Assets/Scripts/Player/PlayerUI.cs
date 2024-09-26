using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : Singleton<PlayerUI>
{

    [Header("===== Inventory =====")]
    [Header("- Toggle Show and Hide inventory")]
    public Transform inventoryTab;

    [Header("- Inventory UI")]
    public Transform slotParent;
    public GameObject inventorySlotObj;

    [Header("- Item Obj")]
    public Transform itemParent;
    public GameObject itemObjPrefab;

    [Header("Test")]
    public ItemSO testItem;
    [HideInInspector] public GameObject curItemObjSelected;

    #region Inventory

    public void ToggleInventory()
    {
        if (inventoryTab.gameObject.activeSelf)
        {
            inventoryTab.gameObject.SetActive(false);
        }
        else
        {
            inventoryTab.gameObject.SetActive(true);
        }
    }

    public GameObject InitItemObj(ItemSO item)
    {
        GameObject obj = Instantiate(itemObjPrefab, itemParent);

        ItemObj itemObj = obj.GetComponent<ItemObj>();
        itemObj.item = item;

        RectTransform rect = itemObj.visual.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(item.itemGridWidth, item.itemGridHeight) * 100 / 2;
        rect.sizeDelta = new Vector2(item.itemGridWidth * 100, item.itemGridHeight * 100);

        itemObj.visual.sprite = item.itemSprite;
        return obj;
    }

    public bool HasItemObjSelected()
    {
        return curItemObjSelected != null;
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            InitItemObj(testItem);
        }
    }

}
