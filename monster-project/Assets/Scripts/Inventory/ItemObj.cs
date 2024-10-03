using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    [HideInInspector] public ItemSO item;
    [HideInInspector] public int amount;

    [HideInInspector] public List<SlotUI> pressSlots;

    Button button;
    [SerializeField] RectTransform rectTransform;
    public Image visual;
    public TextMeshProUGUI amountText;

    bool isSelected;

    [HideInInspector] public bool isRotate;

    private void Awake()
    {
        button = visual.GetComponent<Button>();

        button.onClick.AddListener(OnClickThisItem);
    }

    private void Update()
    {
        if (isSelected)
        {
            transform.position = PlayerManager.Instance.mousePos;
        }
    }

    public void OnClickThisItem()
    {
        if (PlayerUI.Instance.HasItemObjSelected())
        {
            ItemObj itemObj = PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>();
            if (item == itemObj.item)
            {
                if (amount < item.maxStack)
                {
                    AddItemAmount();
                    itemObj.RemoveItemAmount();
                }
            }
        }
        else
        {
            SelectThisItem();
        }
    }

    public void SelectThisItem()
    {
        if (!PlayerUI.Instance.HasItemObjSelected())
        {
            visual.raycastTarget = false;
            amountText.raycastTarget = false;
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
        amountText.raycastTarget = true;
        isSelected = false;
        rectTransform.anchoredPosition = pressSlot.GetButtonLeftPosition();
        visual.rectTransform.sizeDelta = new Vector2(item.itemGridWidth, item.itemGridHeight) * 100;
        if (!isRotate)
        {
            visual.rectTransform.anchoredPosition = new Vector2(item.itemGridWidth, item.itemGridHeight) * 100 / 2;
            pressSlots = PlayerManager.Instance.GetSlot(pressSlot, item.itemGridWidth, item.itemGridHeight, PlayerUI.Instance.slotParent.transform);
        }
        else
        {
            visual.rectTransform.anchoredPosition = new Vector2(item.itemGridHeight, item.itemGridWidth) * 100 / 2;
            pressSlots = PlayerManager.Instance.GetSlot(pressSlot, item.itemGridHeight, item.itemGridWidth, PlayerUI.Instance.slotParent.transform);
        }

        for (int i = 0; i < pressSlots.Count; i++)
        {
            pressSlots[i].hasItem = true;
        }
        PlayerUI.Instance.curItemObjSelected = null;
    }

    public void RotateOnSelected()
    {
        isRotate = !isRotate;
        if (isRotate)
        {
            visual.rectTransform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            visual.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

    }

    public void AddItemAmount()
    {
        amount++;
        UpdateAmountText();
    }

    public void RemoveItemAmount()
    {
        amount--;
        UpdateAmountText();
        if (amount <= 0)
        {
            DestroyItem();
        }
    }

    public void DestroyItem()
    {
        PlayerUI.Instance.curItemObjSelected = null;
        Destroy(gameObject);
    }

    public void UpdateAmountText()
    {
        amountText.text = amount.ToString();
    }

}
