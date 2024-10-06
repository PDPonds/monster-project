using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    public ItemObjData itemObjData = new ItemObjData();

    Button button;
    [SerializeField] RectTransform rectTransform;
    public Image visual;
    public TextMeshProUGUI amountText;

    bool isSelected;


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
            if (itemObjData.item == itemObj.itemObjData.item)
            {
                if (itemObjData.amount < itemObjData.item.maxStack)
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

            if (itemObjData.handSlot != null) Rotate(false);

            visual.rectTransform.sizeDelta = new Vector2(itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight) * 100;
            visual.rectTransform.anchoredPosition = new Vector2(itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight) * 100 / 2;

            if (itemObjData.pressSlots.Count > 0)
            {
                for (int i = 0; i < itemObjData.pressSlots.Count; i++)
                {
                    itemObjData.pressSlots[i].hasItem = false;
                }
            }
            itemObjData.pressSlots.Clear();
            if (itemObjData.handSlot != null)
            {
                itemObjData.handSlot.hasItem = false;
                itemObjData.handSlot = null;
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
        visual.rectTransform.sizeDelta = new Vector2(itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight) * 100;
        if (!itemObjData.isRotate)
        {
            visual.rectTransform.anchoredPosition = new Vector2(itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight) * 100 / 2;
            itemObjData.pressSlots = PlayerManager.Instance.GetSlot(pressSlot, itemObjData.item.itemGridWidth, itemObjData.item.itemGridHeight, PlayerUI.Instance.slotParent.transform);
        }
        else
        {
            visual.rectTransform.anchoredPosition = new Vector2(itemObjData.item.itemGridHeight, itemObjData.item.itemGridWidth) * 100 / 2;
            itemObjData.pressSlots = PlayerManager.Instance.GetSlot(pressSlot, itemObjData.item.itemGridHeight, itemObjData.item.itemGridWidth, PlayerUI.Instance.slotParent.transform);
        }

        for (int i = 0; i < itemObjData.pressSlots.Count; i++)
        {
            itemObjData.pressSlots[i].hasItem = true;
        }
        PlayerUI.Instance.curItemObjSelected = null;
    }

    public void UnSelected(HandSlotUI handSlot)
    {
        visual.raycastTarget = true;
        amountText.raycastTarget = true;
        isSelected = false;
        Rotate(false);
        rectTransform.anchoredPosition = handSlot.GetButtonLeftPosition();
        visual.rectTransform.sizeDelta = new Vector2(handSlot.rectTransform.rect.width, handSlot.rectTransform.rect.height);
        visual.rectTransform.anchoredPosition = new Vector2(handSlot.rectTransform.rect.width, handSlot.rectTransform.rect.height) / 2;
        handSlot.hasItem = true;
        itemObjData.handSlot = handSlot;
        PlayerUI.Instance.curItemObjSelected = null;
    }

    public void ToggleRotate()
    {
        itemObjData.isRotate = !itemObjData.isRotate;
        if (itemObjData.isRotate)
        {
            visual.rectTransform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            visual.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

    }

    public void Rotate(bool isRotate)
    {
        itemObjData.isRotate = isRotate;
        if (itemObjData.isRotate)
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
        itemObjData.amount++;
        UpdateAmountText();
    }

    public void RemoveItemAmount()
    {
        itemObjData.amount--;
        UpdateAmountText();
        if (itemObjData.amount <= 0)
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
        amountText.text = itemObjData.amount.ToString();
    }

}
