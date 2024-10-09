using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerPhase
{
    Normal, UIShow
}

public class PlayerManager : Singleton<PlayerManager>
{
    [HideInInspector] public Rigidbody rb;
    CapsuleCollider col;
    Animator anim;

    #region Phase
    PlayerPhase phase;
    #endregion

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
    [HideInInspector] public SlotNode[,] inventorySlots;
    #endregion

    #region Interact

    [Header("===== Interact =====")]
    [SerializeField] Vector3 interactOffset;
    [SerializeField] float interactSize;
    [SerializeField] LayerMask interactMask;
    IInteracable curIInteractObject;

    #endregion

    #region Storage
    [Header("===== Storage =====")]
    [HideInInspector] public Storage curStorage;
    [HideInInspector] public SlotNode[,] storageSlots;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = rb.GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        inventorySlots = PlayerUI.Instance.InitInventorySlot(inventoryWidth, inventoryHeight);
        PlayerUI.Instance.SetupEquipmentAndHandSlot();
    }

    private void Update()
    {
        MoveHandle();
        RotationHandle();
        AnimHandle();
        InteractHandle();
        UpdatePhase();
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
        if (IsPhase(PlayerPhase.Normal))
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
    }
    #endregion

    #region Controller

    void MoveHandle()
    {
        if (CanMove())
        {
            moveDir = Camera.main.transform.forward * moveInput.y;
            moveDir = moveDir + Camera.main.transform.right * moveInput.x;
            moveDir.Normalize();
            moveDir.y = 0;
            moveDir = moveDir * moveSpeed;
        }
        else
        {
            moveDir = Vector3.zero;
        }

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
        if (CanMove())
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
        else
        {
            anim.SetFloat("z", 0);
            anim.SetFloat("x", 0);
            anim.SetFloat("NoAimWalk", 0);
        }
    }

    bool CanMove()
    {
        return !IsPhase(PlayerPhase.UIShow);
    }

    #endregion

    #region Inventory

    public SlotUI GetInventorySlot(int x, int y, Transform slot)
    {
        if (IsSlotNotOutOfInventorySlot(x, y))
        {
            for (int i = 0; i < slot.childCount; i++)
            {
                GameObject obj = slot.GetChild(i).gameObject;
                SlotUI slotUi = obj.GetComponent<SlotUI>();
                if (slotUi.x == x && slotUi.y == y)
                {
                    return slotUi;
                }
            }
        }

        return null;
    }

    public List<Vector2Int> GetInventorySlot(SlotUI firstSlot, int width, int height, Transform slot)
    {
        List<Vector2Int> slots = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int xGrid = firstSlot.x + x;
                int yGrid = firstSlot.y + y;
                if (IsSlotNotOutOfInventorySlot(xGrid, yGrid))
                {
                    Vector2Int near = new Vector2Int(firstSlot.x + x, firstSlot.y + y);
                    slots.Add(near);
                }
            }
        }

        return slots;
    }

    public List<SlotUI> GetListInventorySlot(SlotUI firstSlot, int width, int height, Transform slot)
    {
        List<SlotUI> slots = new List<SlotUI>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int xGrid = firstSlot.x + x;
                int yGrid = firstSlot.y + y;
                if (IsSlotNotOutOfInventorySlot(xGrid, yGrid))
                {
                    SlotUI near = GetInventorySlot(firstSlot.x + x, firstSlot.y + y, slot);
                    slots.Add(near);
                }
            }
        }

        return slots;
    }

    public SlotUI GetStorageSlot(int x, int y, Transform slot)
    {
        if (IsSlotNotOutOfStorageSlot(x, y))
        {
            for (int i = 0; i < slot.childCount; i++)
            {
                GameObject obj = slot.GetChild(i).gameObject;
                SlotUI slotUi = obj.GetComponent<SlotUI>();
                if (slotUi.x == x && slotUi.y == y)
                {
                    return slotUi;
                }
            }
        }

        return null;
    }

    public List<Vector2Int> GetStorageSlot(SlotUI firstSlot, int width, int height, Transform slot)
    {
        List<Vector2Int> slots = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int xGrid = firstSlot.x + x;
                int yGrid = firstSlot.y + y;
                if (IsSlotNotOutOfStorageSlot(xGrid, yGrid))
                {
                    Vector2Int near = new Vector2Int(firstSlot.x + x, firstSlot.y + y);
                    slots.Add(near);
                }
            }
        }

        return slots;
    }

    public List<SlotUI> GetListStorageSlot(SlotUI firstSlot, int width, int height, Transform slot)
    {
        List<SlotUI> slots = new List<SlotUI>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int xGrid = firstSlot.x + x;
                int yGrid = firstSlot.y + y;
                if (IsSlotNotOutOfStorageSlot(xGrid, yGrid))
                {
                    SlotUI near = GetStorageSlot(firstSlot.x + x, firstSlot.y + y, slot);
                    slots.Add(near);
                }
            }
        }

        return slots;
    }

    public bool IsSlotNotOutOfStorageSlot(int x, int y)
    {
        if (curStorage != null)
        {
            return (x >= 0 && y >= 0 && x < curStorage.storageWidth && y < curStorage.storageHeight);
        }
        else
        {
            return false;
        }
    }

    public bool IsSlotNotOutOfInventorySlot(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < inventoryWidth && y < inventoryHeight);
    }


    #endregion

    #region Interact

    void InteractHandle()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + interactOffset, interactSize, interactMask);
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
        Gizmos.DrawWireSphere(transform.position + interactOffset, interactSize);
    }

    #endregion

    #region Phase

    public void SwitchPhase(PlayerPhase phase)
    {
        this.phase = phase;
        switch (phase)
        {
            case PlayerPhase.Normal:
                break;
            case PlayerPhase.UIShow:
                break;
        }
    }

    void UpdatePhase()
    {
        switch (phase)
        {
            case PlayerPhase.Normal:
                break;
            case PlayerPhase.UIShow:
                break;
        }
    }

    public bool IsPhase(PlayerPhase phase)
    {
        return this.phase == phase;
    }

    #endregion

}


[Serializable]
public class SlotNode
{
    public SlotUI slotUI;

    public SlotNode(SlotUI slotUI, int x, int y, Transform itemParent)
    {
        this.slotUI = slotUI;
        this.slotUI.x = x;
        this.slotUI.y = y;
        this.slotUI.itemParent = itemParent;
    }
}

[Serializable]
public class ItemObjData
{
    public ItemSO item;
    public int amount;

    public List<Vector2Int> pressSlotsXY = new List<Vector2Int>();
    public EquipmentSlotUI handSlot;

    public bool isRotate;


}
