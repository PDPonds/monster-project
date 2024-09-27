using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : Singleton<PlayerUI>
{
    [Header("===== Interact =====")]
    [SerializeField] GameObject interactCondition;

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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            GameObject item = InitItemObj(testItem, 1);
            SpawnItem(item.GetComponent<ItemObj>(), PlayerManager.Instance.GetSlot(0, 1));
        }
    }

}
