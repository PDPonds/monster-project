using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    [HideInInspector] public ItemSO item;
    [HideInInspector] public int amount;

    [HideInInspector] public List<SlotUI> pressSlots;

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
            for (int i = 0; i < pressSlots.Count; i++)
            {
                pressSlots[i].hasItem = false;
            }
            PlayerUI.Instance.curItemObjSelected = gameObject;
        }
    }

    public void UnSelected(SlotUI pressSlot)
    {
        visual.raycastTarget = true;
        isSelected = false;
        rectTransform.anchoredPosition = pressSlot.GetButtonLeftPosition();
        visual.rectTransform.sizeDelta = new Vector2(item.itemGridWidth, item.itemGridHeight) * 100;
        visual.rectTransform.anchoredPosition = new Vector2(item.itemGridWidth, item.itemGridHeight) * 100 / 2;
        pressSlots = PlayerManager.Instance.GetSlot(pressSlot, item.itemGridWidth, item.itemGridHeight);
        for (int i = 0; i < pressSlots.Count; i++)
        {
            pressSlots[i].hasItem = true;
        }
        PlayerUI.Instance.curItemObjSelected = null;
    }

}
