using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemObjVisual : MonoBehaviour, IPointerClickHandler
{
    ItemObj parentObject;

    public void SetupItemVisual(ItemObj itemObj)
    {
        parentObject = itemObj;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!PlayerUI.Instance.HasItemObjSelected())
            {
                int half = parentObject.GetHalfAmount();
                if (half > 0)
                {
                    GameObject obj = PlayerUI.Instance.InitItemObj(parentObject.itemObjData.item, half);
                    obj.transform.SetParent(parentObject.transform.parent);

                    ItemObj itemObj = obj.GetComponent<ItemObj>();
                    itemObj.Rotate(parentObject.itemObjData.isRotate);

                    itemObj.SelectThisItem();

                    parentObject.RemoveItemAmount(half);

                }
                else
                {
                    if (!PlayerUI.Instance.HasItemObjSelected())
                    {
                        parentObject.SelectThisItem();
                    }

                    if (PlayerManager.Instance.curStorage != null)
                    {
                        PlayerManager.Instance.curStorage.UpdateStorageData();
                    }
                }
            }
            else
            {
                ItemObj handObj = PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>();
                if (handObj.itemObjData.item == parentObject.itemObjData.item)
                {
                    parentObject.AddItemAmount();
                    handObj.RemoveItemAmount();
                }
            }
        }
    }

}
