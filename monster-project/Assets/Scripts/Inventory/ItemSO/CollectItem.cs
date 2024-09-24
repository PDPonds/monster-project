using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UnUseableItem/CollectItem")]
public class CollectItem : UnUseableItem
{
    public CollectItem()
    {
        unUseableItemType = UnUseableItemType.CollectItem;
    }
}
