using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameInput input;

    PlayerManager playerManager;
    PlayerUI playerUI;

    private void OnEnable()
    {
        if (input == null)
        {
            input = new GameInput();
            playerManager = GetComponent<PlayerManager>();
            playerUI = GetComponent<PlayerUI>();

            input.action.PlayerMoveInput.performed += i => playerManager.moveInput = i.ReadValue<Vector2>();
            input.action.MousePos.performed += i => playerManager.mousePos = i.ReadValue<Vector2>();
            input.action.Aim.performed += i => playerManager.ToggleAim();
            input.action.ToggleInventory.performed += i => playerUI.ToggleInventory();
        }

        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

}
