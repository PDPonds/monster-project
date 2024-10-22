using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UseableItem/Weapon/Gun")]
public class GunItem : WeaponItem
{
    public float fireRate;
    public ItemSO ammoType;
    public int maxAmmoInMag;
    public float reloadDuration;

    public GunItem() 
    {
        weaponType = WeaponType.Gun;
    }

}
