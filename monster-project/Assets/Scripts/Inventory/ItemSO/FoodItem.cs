using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UseableItem/Food")]

public class FoodItem : UseableItem
{
    public FoodItem()
    {
        usableItemType = UseableItemType.Food;
    }
}
