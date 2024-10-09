using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/UnUseableItem/EquipmentItem/Shoe")]
public class ShoeEquipmentItem : EquipmentItem
{
    public ShoeEquipmentItem()
    {
        equipmentType = EquipmentType.Shoe;
    }
}
