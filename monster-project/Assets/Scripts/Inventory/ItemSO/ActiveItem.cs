using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UseableItem/ActiveItem")]
public class ActiveItem : UseableItem
{
    public ActiveItem()
    {
        usableItemType = UseableItemType.ActiveItem;
    }
}
