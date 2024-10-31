using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AllItem")]
public class AllItem : ScriptableObject
{
    public List<ItemSO> items = new List<ItemSO>();

    public List<ItemSO> GetCanCraftItem(ItemSO item)
    {
        List<ItemSO> canCraftItems = new List<ItemSO>();

        for (int i = 0; i < items.Count; i++)
        {
            ItemSO curItem = items[i];
            if (curItem.craftingComponents.Count > 0)
            {
                for (int j = 0; j < curItem.craftingComponents.Count; j++)
                {
                    ItemSO curComponent = curItem.craftingComponents[j].item;
                    if (item == curComponent)
                    {
                        canCraftItems.Add(curItem);
                        break;
                    }
                }
            }
        }
        return canCraftItems;
    }


}
