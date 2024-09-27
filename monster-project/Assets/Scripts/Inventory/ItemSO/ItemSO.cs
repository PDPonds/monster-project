using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    UseableItem, UnUseableItem
}

public class ItemSO : ScriptableObject
{
    [Header("- Item Infomation")]
    public string itemName;
    public Sprite itemSprite;

    [Header("- Slot Size")]
    public int itemGridWidth;
    public int itemGridHeight;

    [Header("- Slot")]
    public int maxStack;

    public ItemType itemType;

    public int GetSlotSize()
    {
        return itemGridWidth * itemGridHeight;
    }

}
