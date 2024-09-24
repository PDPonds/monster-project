using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameInput input;

    PlayerManager playerManager;

    private void OnEnable()
    {
        if (input == null)
        {
            input = new GameInput();
            playerManager = GetComponent<PlayerManager>();
            input.action.PlayerMoveInput.performed += i => playerManager.moveInput = i.ReadValue<Vector2>();
            input.action.MousePos.performed += i => playerManager.mousePos = i.ReadValue<Vector2>();
            input.action.Aim.performed += i => playerManager.ToggleAim();
        }

        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

}
