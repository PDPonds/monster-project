using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : Singleton<PlayerManager>
{
    Rigidbody rb;
    CapsuleCollider col;
    Animator anim;
    PlayerUI playerUI;

    #region Movement

    [Header("===== Controller =====")]
    [HideInInspector] public bool isAim;
    [Header("- Mouse")]
    [HideInInspector] public Vector2 mousePos;
    [SerializeField] LayerMask mousePosMask;
    [Header("- Move")]
    [HideInInspector] public Vector2 moveInput;
    Vector3 moveDir;
    [SerializeField] float moveSpeed;
    [Header("- Rotation")]
    [SerializeField] float rotationSpeed;

    #endregion

    #region Inventory
    [Header("===== Inventory =====")]
    public int inventoryWidth;
    public int inventoryHeight;
    SlotNode[,] slots;
    #endregion

    #region Interact

    [Header("===== Interact =====")]
    [SerializeField] float interactForwardOffset;
    [SerializeField] float interactSize;
    [SerializeField] LayerMask interactMask;
    IInteracable curIInteractObject;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = rb.GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        playerUI = GetComponent<PlayerUI>();
    }

    private void Start()
    {
        InitInventory();
    }

    private void Update()
    {
        MoveHandle();
        RotationHandle();
        AnimHandle();
        InteractHandle();
    }

    #region Mouse
    public Vector3 GetWorldPosFormMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Vector3 worldPos = transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, mousePosMask))
        {
            worldPos = hit.point;
        }

        return worldPos;
    }

    public Vector3 GetDirToMouse()
    {
        Vector3 mouseWorldPos = GetWorldPosFormMouse();
        Vector3 dir = mouseWorldPos - transform.position;
        dir.Normalize();
        dir.y = 0;
        return dir;
    }

    public Vector3 GetInvertDirFormMouse()
    {
        return GetDirToMouse() * -1f;
    }

    #endregion

    #region Aim
    public void ToggleAim()
    {
        isAim = !isAim;
        if (isAim)
        {
            anim.SetFloat("isAim", 1);
        }
        else
        {
            anim.SetFloat("isAim", 0);
        }
    }
    #endregion

    #region Controller

    void MoveHandle()
    {
        moveDir = Camera.main.transform.forward * moveInput.y;
        moveDir = moveDir + Camera.main.transform.right * moveInput.x;
        moveDir.Normalize();
        moveDir.y = 0;
        moveDir = moveDir * moveSpeed;

        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

    }

    void RotationHandle()
    {
        if (isAim)
        {
            Vector3 targetDir = GetDirToMouse();

            if (targetDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

                transform.rotation = playerRot;
            }
        }
        else
        {
            if (moveDir != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, (rotationSpeed * 100) * Time.deltaTime);

                var rotatedVector = transform.rotation * Vector3.forward;
            }

        }
    }

    void AnimHandle()
    {
        if (isAim)
        {
            float forwardBackwardsMagnitude = 0;
            float rightLeftMagnitude = 0;
            if (moveInput.magnitude > 0)
            {
                Vector3 lookingDir = GetDirToMouse();
                if (lookingDir == Vector3.zero) lookingDir = transform.forward;
                forwardBackwardsMagnitude = Mathf.Clamp(Vector3.Dot(new Vector3(moveInput.x, 0, moveInput.y), lookingDir), -1, 1);
                Vector3 perpendicularLookingAt = new Vector3(lookingDir.z, 0, -lookingDir.x);
                rightLeftMagnitude = Mathf.Clamp(Vector3.Dot(new Vector3(moveInput.x, 0, moveInput.y), perpendicularLookingAt), -1, 1);
            }
            anim.SetFloat("z", forwardBackwardsMagnitude);
            anim.SetFloat("x", rightLeftMagnitude);
        }
        else
        {
            if (moveInput.magnitude > 0)
            {
                anim.SetFloat("NoAimWalk", 1);
            }
            else
            {
                anim.SetFloat("NoAimWalk", 0);
            }
        }
    }
    #endregion

    #region Inventory

    void InitInventory()
    {
        slots = new SlotNode[inventoryWidth, inventoryHeight];
        GridLayoutGroup gridLayout = playerUI.slotParent.GetComponent<GridLayoutGroup>();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = inventoryWidth;
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                GameObject slotObj = Instantiate(playerUI.inventorySlotObj, playerUI.slotParent);
                SlotUI slotUi = slotObj.GetComponent<SlotUI>();
                slotUi.itemParent = PlayerUI.Instance.itemParent;
                SlotNode slot = new SlotNode(slotUi, x, y);
                slots[x, y] = slot;
            }
        }
    }

    public SlotUI GetSlot(int x, int y)
    {
        if (IsSlotNotOutOfInventorySlot(x, y))
        {
            for (int i = 0; i < PlayerUI.Instance.slotParent.childCount; i++)
            {
                GameObject obj = PlayerUI.Instance.slotParent.GetChild(i).gameObject;
                SlotUI slotUi = obj.GetComponent<SlotUI>();
                if (slotUi.x == x && slotUi.y == y)
                {
                    return slotUi;
                }
            }
        }

        return null;
    }

    public List<SlotUI> GetSlot(SlotUI firstSlot, int width, int height)
    {
        List<SlotUI> slots = new List<SlotUI>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int xGrid = firstSlot.x + x;
                int yGrid = firstSlot.y - y;
                if (IsSlotNotOutOfInventorySlot(xGrid, yGrid))
                {
                    SlotUI near = GetSlot(firstSlot.x + x, firstSlot.y - y);
                    slots.Add(near);
                }
            }
        }

        return slots;
    }

    public bool IsSlotNotOutOfInventorySlot(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < inventoryWidth && y < inventoryHeight);
    }

    public List<Slot> GetItemInInventory()
    {
        List<Slot> slots = new List<Slot>();

        if (PlayerUI.Instance.itemParent.childCount > 0)
        {
            for (int i = 0; i < PlayerUI.Instance.itemParent.childCount; i++)
            {
                GameObject itemUI = PlayerUI.Instance.itemParent.GetChild(i).gameObject;
                ItemObj itemObj = itemUI.GetComponent<ItemObj>();
                Slot slot = new Slot();
                slot.item = itemObj.item;
                slot.amount = itemObj.amount;
                slots.Add(slot);
            }

        }
        return slots;
    }

    #endregion

    #region Interact

    void InteractHandle()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward * interactForwardOffset, interactSize, interactMask);
        if (cols.Length > 0)
        {
            if (cols[0].TryGetComponent<IInteracable>(out IInteracable interacable))
            {
                if (curIInteractObject == null)
                {
                    PlayerUI.Instance.ShowInteractCondition();
                    curIInteractObject = interacable;
                }
            }
            else
            {
                PlayerUI.Instance.HideInteractCondition();
                curIInteractObject = null;
            }
        }
        else
        {
            PlayerUI.Instance.HideInteractCondition();
            curIInteractObject = null;
        }
    }

    public void InteractClick()
    {
        if (curIInteractObject != null)
        {
            curIInteractObject.Interact();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward * interactForwardOffset, interactSize);
    }

    #endregion

}


[Serializable]
public class SlotNode
{
    public SlotUI slotUI;
    public SlotNode(SlotUI slotUI, int x, int y)
    {
        this.slotUI = slotUI;
        this.slotUI.x = x;
        this.slotUI.y = y;
    }
}

[Serializable]
public class Slot
{
    public ItemSO item;
    public int amount;

}
