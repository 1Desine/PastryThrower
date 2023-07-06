using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {

    [SerializeField] private GameInput gameInput;
    [SerializeField] private ThrowableObjectParent throwableObjectParent;

    [SerializeField] private float mouseSensitivity = 0.1f;


    private InteractionState interactionState;
    private enum InteractionState {
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
        UpdateInteractionState();

        Debug.Log(interactionState);
        Debug.Log(throwPower);


        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }




    private void HandleLook() {
        Vector2 lookInput = gameInput.GetLookDelta();

        if(interactionState == InteractionState.FreeLook) {
            Vector3 lookRoation = new Vector3(lookInput.y, lookInput.x, 0f);
            transform.eulerAngles += lookRoation * mouseSensitivity;
            throwPower = 0;
        } else if(interactionState == InteractionState.Aiming) {
            throwPower -= lookInput.y / Screen.height;
            throwPower = Mathf.Abs(throwPower);

            throwDirection = transform.forward * throwPower + transform.up * throwPower;
            Debug.DrawRay(transform.position, throwDirection);
        }
    }

    private void UpdateInteractionState() {
        interactionState = gameInput.GetAimBool() == false ? 
            InteractionState.FreeLook : InteractionState.Aiming;
    }





}
