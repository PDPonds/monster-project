using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    GameInput input;

    private void OnEnable()
    {
        if (input == null)
        {
            input = new GameInput();
            input.action.PlayerMoveInput.performed += i => PlayerManager.Instance.moveInput = i.ReadValue<Vector2>();
            input.action.MousePos.performed += i => PlayerManager.Instance.mousePos = i.ReadValue<Vector2>();
            input.action.Aim.performed += i => PlayerManager.Instance.ToggleAim();
            input.action.ToggleInventory.performed += i => PlayerUI.Instance.ToggleInventory();
            input.action.RotateOnSelect.performed += i => PlayerUI.Instance.RotateSelectedObject();
            input.action.Interact.performed += i => PlayerManager.Instance.InteractClick();

            input.action.HandSlot1.performed += i => PlayerUI.Instance.SelectItemInHand(1);
            input.action.HandSlot2.performed += i => PlayerUI.Instance.SelectItemInHand(2);
            input.action.HandSlot3.performed += i => PlayerUI.Instance.SelectItemInHand(3);

        }

        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

}
