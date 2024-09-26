using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    UseableItem , UnUseableItem
}

public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public int itemGridWidth;
    public int itemGridHeight;
    public ItemType itemType;
}
