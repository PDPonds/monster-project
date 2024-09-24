using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/UseableItem/Water")]
public class WaterItem : UseableItem
{
    public WaterItem()
    {
        usableItemType = UseableItemType.Water;
    }
}
