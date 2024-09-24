using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/UseableItem/ThrowingItem")]
public class ThrowingItem : UseableItem
{
    public ThrowingItem()
    {
        usableItemType = UseableItemType.ThrowingItem;
    }
}
