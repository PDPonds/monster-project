using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftItemObj : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image itemSprite;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] Image holdingFill;

    [SerializeField] Transform itemGridParent;
    [SerializeField] GameObject itemGridChild;

    bool canCraft;

    ItemSO item;

    bool isHold;
    float curHoldTime;

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerUI.Instance.GenerateComponentItem(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerUI.Instance.HideCraftingComponent();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHold = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHold = false;
        curHoldTime = 0;
        holdingFill.fillAmount = 0;
    }

    private void Update()
    {
        if (isHold && canCraft)
        {
            curHoldTime += Time.deltaTime;
            float percent = curHoldTime / 2f;
            holdingFill.fillAmount = percent;
            if (curHoldTime >= 2f)
            {
                TryCraft();
                curHoldTime = 0;
                isHold = false;
            }
        }
    }

    void TryCraft()
    {
        if (item != null && item.craftingComponents.Count > 0 && PlayerUI.Instance.curItemObjSelected == null)
        {
            bool addItemSuccess = PlayerUI.Instance.TryAddItemToInventory(item, 1);
            if (addItemSuccess)
            {
                for (int i = 0; i < item.craftingComponents.Count; i++)
                {
                    ItemSO curComponent = item.craftingComponents[i].item;
                    int componentAmount = item.craftingComponents[i].amount;
                    PlayerUI.Instance.RemoveItem(curComponent, componentAmount);
                }
            }
            PlayerUI.Instance.GenerateCraftingItemObj();

        }

    }

    public void Setup(ItemSO item)
    {
        this.item = item;
        Image img = GetComponent<Image>();
        if (CanCraft(item))
        {
            canCraft = true;
            img.color = Color.white;
        }
        else
        {
            canCraft = false;
            img.color = Color.gray;
        }

        float rectWidth = 1f;
        float rectHeight = 1f;
        if (item.itemGridWidth >= item.itemGridHeight)
        {
            rectWidth = (float)item.itemGridWidth / (float)item.itemGridHeight;
            rectHeight = (float)item.itemGridHeight / (float)item.itemGridHeight;
        }
        else
        {
            rectWidth = (float)item.itemGridWidth / (float)item.itemGridWidth;
            rectHeight = (float)item.itemGridHeight / (float)item.itemGridWidth;
        }
        itemSprite.rectTransform.sizeDelta = new Vector2(rectWidth * 50f, rectHeight * 50f);
        itemSprite.sprite = item.itemSprite;

        itemName.text = item.name;
        GenerateGridItem(item);
    }

    void GenerateGridItem(ItemSO item)
    {
        int width = item.itemGridWidth;
        int height = item.itemGridHeight;

        GridLayoutGroup gridLayoutGroup = itemGridParent.GetComponent<GridLayoutGroup>();
        RectTransform gridRect = itemGridParent.GetComponent<RectTransform>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = width;
        gridLayoutGroup.cellSize = Vector2.one * gridRect.rect.width / width;
        int count = width * height;
        for (int i = 0; i < count; i++)
        {
            Instantiate(itemGridChild, itemGridParent);
        }
    }

    bool CanCraft(ItemSO item)
    {
        List<CraftSlot> craftComponents = item.craftingComponents;
        int hasCount = 0;
        if (craftComponents.Count > 0)
        {
            for (int j = 0; j < craftComponents.Count; j++)
            {
                ItemSO componentItem = craftComponents[j].item;
                int componentCount = craftComponents[j].amount;
                if (PlayerUI.Instance.HasItem(componentItem, out List<ItemObj> itemObj, out int count) && count >= componentCount)
                {
                    hasCount++;
                }
            }
        }

        return hasCount == craftComponents.Count && hasCount != 0;
    }

}
