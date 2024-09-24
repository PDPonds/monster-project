using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerManager : MonoBehaviour
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
    public inventorySlot[,] slots;
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
            if(moveDir != Vector3.zero)
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
            if(moveInput.magnitude > 0)
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

    public void InitInventory()
    {
        slots = new inventorySlot[inventoryWidth, inventoryHeight];
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                GameObject slotUi = Instantiate(playerUI.inventorySlotObj, playerUI.inventoryParent);
                inventorySlot slot = new inventorySlot(slotUi);
                slots[x, y] = slot;
            }
        }
    }

    #endregion

}

[Serializable]
public class inventorySlot
{
    public GameObject slotUI;
    public inventorySlot(GameObject slotUI)
    {
        this.slotUI = slotUI;
    }
}
