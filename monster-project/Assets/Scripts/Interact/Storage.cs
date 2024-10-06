using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public class Storage : MonoBehaviour, IInteracable
{
    public int storageWidth;
    public int storageHeight;
    /*[hideininspector]*/
    public List<ItemObjData> storageDatas = new List<ItemObjData>();

    public void Interact()
    {
        if (PlayerManager.Instance.IsPhase(PlayerPhase.Normal))
        {
            PlayerManager.Instance.curStorage = this;
            PlayerUI.Instance.ShowStorage(storageWidth, storageHeight);
            InitItemInStorage();
        }
    }

    public string InteractText()
    {
        return $"[E] to Open Chest";
    }

    public void InitItemInStorage()
    {
        if (storageDatas.Count > 0)
        {
            for (int i = 0; i < storageDatas.Count; i++)
            {
                ItemObjData data = storageDatas[i];
                InitItemObj(data);
            }
        }
    }

    void InitItemObj(ItemObjData data)
    {
        GameObject obj = PlayerUI.Instance.InitItemObj(data.item, data.amount);
        ItemObj itemObj = obj.GetComponent<ItemObj>();

        itemObj.itemObjData = data;

        SlotUI pressSlot = PlayerManager.Instance.GetStorageSlot(data.pressSlotsXY[0].x, data.pressSlotsXY[0].y, PlayerUI.Instance.storageSlot.transform);
        itemObj.transform.SetParent(pressSlot.itemParent);

        itemObj.rectTransform.anchoredPosition = pressSlot.GetButtonLeftPosition();
        itemObj.visual.rectTransform.sizeDelta = new Vector2(data.item.itemGridWidth, data.item.itemGridHeight) * 100;

        itemObj.Rotate(data.isRotate);

        if (!data.isRotate)
        {
            itemObj.visual.rectTransform.anchoredPosition = new Vector2(data.item.itemGridWidth, data.item.itemGridHeight) * 100 / 2;
        }
        else
        {
            itemObj.visual.rectTransform.anchoredPosition = new Vector2(data.item.itemGridHeight, data.item.itemGridWidth) * 100 / 2;
        }

        for (int i = 0; i < data.pressSlotsXY.Count; i++)
        {
            SlotUI slot = PlayerManager.Instance.GetStorageSlot(data.pressSlotsXY[i].x, data.pressSlotsXY[i].y, PlayerUI.Instance.storageSlot);
            slot.hasItem = true;
        }

    }

    public bool TryAddItemToStorage(ItemSO item, int amount)
    {
        GameObject obj = PlayerUI.Instance.InitItemObj(item, amount);
        ItemObj itemObj = obj.GetComponent<ItemObj>();

        for (int x = 0; x < storageWidth; x++)
        {
            for (int y = 0; y < storageHeight; y++)
            {
                SlotUI pressPos = PlayerManager.Instance.GetStorageSlot(x, y, PlayerUI.Instance.storageSlot.transform);
                if (pressPos.CanPress(itemObj, itemObj.itemObjData.isRotate))
                {
                    itemObj.UnSelected(pressPos);
                    return true;
                }
            }
        }

        itemObj.ToggleRotate();

        for (int x = 0; x < storageWidth; x++)
        {
            for (int y = 0; y < storageHeight; y++)
            {
                SlotUI pressPos = PlayerManager.Instance.GetStorageSlot(x, y, PlayerUI.Instance.storageSlot.transform);
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

    List<ItemObj> GetItem()
    {
        List<ItemObj> itemObjs = new List<ItemObj>();

        if (PlayerUI.Instance.storageParent.childCount > 0)
        {
            for (int x = 0; x < PlayerUI.Instance.storageParent.childCount; x++)
            {
                ItemObj obj = PlayerUI.Instance.storageParent.GetChild(x).GetComponent<ItemObj>();
                itemObjs.Add(obj);
            }
        }

        return itemObjs;
    }

    public void UpdateStorageData()
    {
        storageDatas.Clear();
        List<ItemObj> itemObjs = GetItem();
        if (itemObjs.Count > 0)
        {
            for (int x = 0; x < itemObjs.Count; x++)
            {
                ItemObjData data = itemObjs[x].itemObjData;
                storageDatas.Add(data);
            }
        }
    }

}
