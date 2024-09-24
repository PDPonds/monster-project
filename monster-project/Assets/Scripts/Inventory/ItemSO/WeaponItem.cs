using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Gun , Melee
}

public class WeaponItem : UseableItem
{
    public WeaponType weaponType;

    public WeaponItem()
    {
        usableItemType = UseableItemType.Weapon;
    }
}
