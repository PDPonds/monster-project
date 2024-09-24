using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    PlayerManager playerManager;

    [Header("===== Inventory =====")]
    public Transform inventoryParent;
    public GameObject inventorySlotObj;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    #region Inventory

    public void ToggleInventory()
    {
        if (inventoryParent.gameObject.activeSelf)
        {
            inventoryParent.gameObject.SetActive(false);
        }
        else
        {
            inventoryParent.gameObject.SetActive(true);
        }
    }


    #endregion

}
