using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UseableItem/Weapon/Gun")]
public class GunItem : WeaponItem
{
    [Header("===== Fire =====")]
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float fireRate;

    [Header("===== Ammo =====")]
    public ItemSO ammoType;
    public int maxAmmoInMag;
    public float reloadDuration;
    public float bulletDuration;

    public GunItem() 
    {
        weaponType = WeaponType.Gun;
    }

}
