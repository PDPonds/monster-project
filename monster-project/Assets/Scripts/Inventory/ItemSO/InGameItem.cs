using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UnUseableItem/InGameItem")]
public class InGameItem : UnUseableItem
{
    public InGameItem()
    {
        unUseableItemType = UnUseableItemType.InGameItem;
    }
}
