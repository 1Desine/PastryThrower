using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class GameInput : MonoBehaviour {

    private PlayerInputActions playerInputActions;

    public event EventHandler OnThrowCanceled;
    public event EventHandler OnSpawnPastry;
    public event EventHandler OnJump;



    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Player.ThrowCancel.performed += ThrowCancel_performed;
        playerInputActions.Player.Jump.performed += Jump_performed;
    }



    public bool IsThrowButtonDown() {
        return playerInputActions.Player.Throw.IsPressed();
    }
    private void ThrowCancel_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnThrowCanceled?.Invoke(this, EventArgs.Empty);
    }
    private void SpawnPastry_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSpawnPastry?.Invoke(this, EventArgs.Empty);
    }
    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetLookDelta() {
        return playerInputActions.Player.Look.ReadValue<Vector2>();
    }
    public Vector2 GetMovementVectorNormalized() {
        return playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
    public float GetScrollWheel() {
        // Scroll wheel.y returns {-120;120} per rotation, so we find --> {-120 or 120} % 119 = {-1 of 1}
        return playerInputActions.Player.ThrowSpeed.ReadValue<Vector2>().y % 119;
    }




}
