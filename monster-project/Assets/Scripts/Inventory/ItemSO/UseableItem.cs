using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandSide 
{ 
    Left, Right 
}

public enum UseableItemType
{
    Weapon , Food , Water , Medicine , ActiveItem , ThrowingItem
}

public class UseableItem : ItemSO
{
    [Header("===== Select Item To Hand =====")]
    public HandSide holdingSide;
    public GameObject prefab;
    [Header("- Left Hand")]
    public Vector3 spawnLeftPos;
    public Vector3 rotationLeft;
    [Header("- Right Hand")]
    public Vector3 spawnRightPos;
    public Vector3 rotationRight;

    public UseableItemType usableItemType;

    public UseableItem()
    {
        itemType = ItemType.UseableItem;
    }

}
