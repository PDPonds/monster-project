using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/UseableItem/Weapon/Melee")]
public class MeleeItem : WeaponItem
{
    public MeleeItem()
    {
        weaponType = WeaponType.Melee;
    }
}
