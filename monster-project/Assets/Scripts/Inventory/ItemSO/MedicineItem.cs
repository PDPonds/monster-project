using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/UseableItem/Medicine")]
public class MedicineItem : UseableItem
{
    public MedicineItem()
    {
        usableItemType = UseableItemType.Medicine;
    }
}
