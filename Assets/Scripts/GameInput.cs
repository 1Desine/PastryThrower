using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInput : MonoBehaviour {

    private PlayerInputActions playerInputActions;

    public event EventHandler OnThrowPastry;



    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Player.ThrowPastry.performed += ThrowPastry_performed;
    }



    private void ThrowPastry_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnThrowPastry?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetLookDelta() {
        return playerInputActions.Player.Look.ReadValue<Vector2>();
    }
    public bool GetAimBool() {
        return playerInputActions.Player.Aim.IsPressed();
    }
    

}
