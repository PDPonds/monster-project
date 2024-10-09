using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UnUseableItem/EquipmentItem/Body")]
public class BodyEquipmentItem : EquipmentItem
{
    public BodyEquipmentItem()
    {
        equipmentType = EquipmentType.Body;
    }
}
