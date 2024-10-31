using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour, IInteracable
{
    public void Interact()
    {
        PlayerUI.Instance.ShowCraftingPanel();
    }

    public string InteractText()
    {
        return "[E] to Craft";
    }
}
