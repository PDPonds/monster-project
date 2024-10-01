using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour, IInteracable
{
    [Header("===== Inventory =====")]
    public int inventoryWidth;
    public int inventoryHeight;

    public void Interact()
    {
        Debug.Log("Open Storage");
    }

    
}
