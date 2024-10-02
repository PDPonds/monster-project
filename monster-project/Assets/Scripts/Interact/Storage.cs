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
        PlayerUI.Instance.inventoryTab.gameObject.SetActive(true);
        PlayerUI.Instance.storageTab.gameObject.SetActive(true);

    }


}
