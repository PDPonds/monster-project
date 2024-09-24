using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UnUseableItem/WeaponGear")]
public class WeaponGearItem : UnUseableItem
{
    public WeaponGearItem()
    {
        unUseableItemType = UnUseableItemType.WeaponGear;
    }
}
