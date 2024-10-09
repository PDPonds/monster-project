using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/UnUseableItem/EquipmentItem/Head")]
public class HeadEquipmentItem : EquipmentItem
{
    public HeadEquipmentItem()
    {
        equipmentType = EquipmentType.Head;
    }
}
