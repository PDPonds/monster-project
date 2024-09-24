using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UseableItemType
{
    Weapon , Food , Water , Medicine , ActiveItem , ThrowingItem
}

public class UseableItem : ItemSO
{
    public UseableItemType usableItemType;

    public UseableItem()
    {
        itemType = ItemType.UseableItem;
    }

}
