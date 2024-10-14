using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UseableItem/Weapon/Melee")]
public class MeleeItem : WeaponItem
{
    [Header("===== Melee =====")]
    public float knockbackForce;
    public AnimatorOverrideController attackOverride;

    public MeleeItem()
    {
        weaponType = WeaponType.Melee;
    }

}
