using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    RectTransform rectTransform;
    Button button;

    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public bool hasItem;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        button.onClick.AddListener(TryPressObject);
    }

    public Vector2 GetCenterPosition()
    {
        return rectTransform.localPosition;
    }

    public Vector2 GetButtonLeftPosition()
    {
        Vector2 center = GetCenterPosition();
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        Vector2 buttonLeft = new Vector2(center.x - (width / 2), center.y - (height / 2));
        return buttonLeft;
    }

    public void TryPressObject()
    {
        if (PlayerUI.Instance.HasItemObjSelected())
        {
            ItemObj itemObj = PlayerUI.Instance.curItemObjSelected.GetComponent<ItemObj>();
            itemObj.UnSelected(GetButtonLeftPosition());
        }
    }


}
