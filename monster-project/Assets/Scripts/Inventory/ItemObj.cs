using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    [HideInInspector] public ItemSO item;
    [HideInInspector] public int amount;

    Button button;
    RectTransform rectTransform;
    public Image visual;

    bool isSelected;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = visual.GetComponent<Button>();

        button.onClick.AddListener(SelectThisItem);
    }

    private void Update()
    {
        if (isSelected)
        {
            transform.position = PlayerManager.Instance.mousePos;
        }
    }

    public void SelectThisItem()
    {
        if (!PlayerUI.Instance.HasItemObjSelected())
        {
            visual.raycastTarget = false;
            isSelected = true;
            PlayerUI.Instance.curItemObjSelected = gameObject;
        }
    }

    public void UnSelected(Vector2 dropPos)
    {
        visual.raycastTarget = true;
        isSelected = false;
        rectTransform.anchoredPosition = dropPos;
        PlayerUI.Instance.curItemObjSelected = null;
    }

}
