using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UseableItem/Weapon/Gun")]
public class GunItem : WeaponItem
{
    public GunItem() 
    {
        weaponType = WeaponType.Gun;
    }

}
