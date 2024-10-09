using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipmentSlotType
{
    Weapon, Head, Body, Leg, Shoe
}

public class EquipmentSlotUI : MonoBehaviour
{
    public RectTransform rectTransform;

    public EquipmentSlotType equipmentSlotType;

    Button button;

    [HideInInspector] public bool hasItem;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TryPressObject);
    }

    public Vector2 GetCenterPosition()
    {
        return Vector2.zero;
    }

    public Vector2 GetButtonLeftPosition()
    {
        Vector2 center = GetCenterPosition();
        float width = rectTransform.rect.size.x;
        float height = rectTransform.rect.size.y;
        Vector2 buttonLeft = new Vector2(center.x - (width / 2), center.y - (height / 2));
        return buttonLeft;
    }

    public void TryPressObject()
    {
        if (PlayerUI.Instance.HasItemObjSelected())
        {
            ItemObj itemObj = PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>();
            if (CanPress(itemObj))
            {
                itemObj.UnSelected(this);
            }
        }

    }

    public bool CanPress(ItemObj itemObj)
    {
        bool isCollectItemType = false;
        switch (equipmentSlotType)
        {
            case EquipmentSlotType.Weapon:
                if (itemObj.itemObjData.item is WeaponItem)
                {
                    isCollectItemType = true;
                }
                break;
            case EquipmentSlotType.Head:
                if (itemObj.itemObjData.item is EquipmentItem head)
                {
                    if (head.equipmentType == EquipmentType.Head)
                        isCollectItemType = true;
                }
                break;
            case EquipmentSlotType.Body:
                if (itemObj.itemObjData.item is EquipmentItem body)
                {
                    if (body.equipmentType == EquipmentType.Body)
                        isCollectItemType = true;
                }
                break;
            case EquipmentSlotType.Leg:
                if (itemObj.itemObjData.item is EquipmentItem leg)
                {
                    if (leg.equipmentType == EquipmentType.Leg)
                        isCollectItemType = true;
                }
                break;
            case EquipmentSlotType.Shoe:
                if (itemObj.itemObjData.item is EquipmentItem shoe)
                {
                    if (shoe.equipmentType == EquipmentType.Shoe)
                        isCollectItemType = true;
                }
                break;
        }

        return !hasItem && isCollectItemType;
    }

}
