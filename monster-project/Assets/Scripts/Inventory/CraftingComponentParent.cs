using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingComponentParent : MonoBehaviour
{
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 pos = Input.mousePosition;
        float pivotX = pos.x / Screen.width;
        float pivotY = pos.y / Screen.height;

        rect.pivot = new Vector2(pivotX, pivotY);
        transform.position = pos;
    }
}
