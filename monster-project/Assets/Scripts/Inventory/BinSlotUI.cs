using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinSlotUI : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ShowConfirmDestroyItem);
    }

    void ShowConfirmDestroyItem()
    {
        if (PlayerUI.Instance.HasItemObjSelected())
        {
            ItemObj itemObj = PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>();
            PlayerUI.Instance.ShowConfirmToDestroy(itemObj);
        }
    }

}
