using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Head, Body, Leg, Shoe
}

public class EquipmentItem : UnUseableItem
{
    public EquipmentType equipmentType;

    public EquipmentItem()
    {
        unUseableItemType = UnUseableItemType.InGameItem;
    }
}
