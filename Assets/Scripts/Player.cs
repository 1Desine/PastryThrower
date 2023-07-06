using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {

    [SerializeField] private GameInput gameInput;
    [SerializeField] private PastryHoldPoint throwableObjectParent;

    [SerializeField] private float mouseSensitivity = 0.1f;


    private AimState aimState;
    private enum AimState {
        FreeLook,
        Aiming,
    }



    private float throwPower;
    private Vector3 throwDirection;


    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameInput.OnThrowPastry += GameInput_OnThrowPastry;
    }

    private void GameInput_OnThrowPastry(object sender, System.EventArgs e) {
        throwableObjectParent.ThrowPastry(throwDirection);
    }

    private void Update() {
        HandleLook();
        UpdateAimState();


        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }




    private void HandleLook() {
        Vector2 lookInput = gameInput.GetLookDelta();

        if(aimState == AimState.FreeLook) {
            Vector3 lookRoation = new Vector3(lookInput.y, lookInput.x, 0f);
            transform.eulerAngles += lookRoation * mouseSensitivity;
            throwPower = 0;
            throwDirection = Vector3.zero;

        } else if(aimState == AimState.Aiming) {
            throwPower -= lookInput.y / Screen.height;
            throwPower = Mathf.Abs(throwPower);

            throwDirection = transform.forward * throwPower + transform.up * throwPower;
            Debug.DrawRay(transform.position, throwDirection);
        }
    }

    private void UpdateAimState() {
        aimState = gameInput.GetAimBool() == false ?
            AimState.FreeLook : AimState.Aiming;
    }





}
