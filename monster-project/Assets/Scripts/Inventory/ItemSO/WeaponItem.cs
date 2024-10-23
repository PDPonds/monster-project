using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Gun , Melee
}

public class WeaponItem : UseableItem
{
    public int damage;

    public WeaponType weaponType;

    public WeaponItem()
    {
        usableItemType = UseableItemType.Weapon;
    }
}
