using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameInput input;

    PlayerUI playerUI;

    private void OnEnable()
    {
        if (input == null)
        {
            input = new GameInput();
            playerUI = GetComponent<PlayerUI>();

            input.action.PlayerMoveInput.performed += i => PlayerManager.Instance.moveInput = i.ReadValue<Vector2>();
            input.action.MousePos.performed += i => PlayerManager.Instance.mousePos = i.ReadValue<Vector2>();
            input.action.Aim.performed += i => PlayerManager.Instance.ToggleAim();
            input.action.ToggleInventory.performed += i => playerUI.ToggleInventory();
        }

        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

}
