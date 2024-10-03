using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour, IInteracable
{
    [SerializeField] ItemSO item;
    [SerializeField] int count;

    public void Interact()
    {
        if (PlayerUI.Instance.TryAddItem(item, count))
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Can't Get");
        }
    }

}
