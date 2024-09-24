using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnUseableItemType
{
    InGameItem, CollectItem, WeaponGear
}

public class UnUseableItem : ItemSO
{
    public UnUseableItemType unUseableItemType;

    public UnUseableItem()
    {
        itemType = ItemType.UnUseableItem;
    }
}
