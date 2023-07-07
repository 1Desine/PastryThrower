using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Pastry;

public class Player : MonoBehaviour {

    [SerializeField] private GameInput gameInput;
    [SerializeField] private PastryHoldPoint pastryHoldPoint;

    [SerializeField] private float mouseSensitivity = 0.1f;

    public event EventHandler<OnScoreChangedEventArgs> OnScoreChanged;
    public class OnScoreChangedEventArgs : EventArgs {
        public int score;
    }


    private AimState aimState;
    private enum AimState {
        FreeLook,
        Aiming,
    }

    private float throwPower;
    private Vector3 throwDirection;


    private int playerScore;



    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameInput.OnThrowPastry += GameInput_OnThrowPastry;
        gameInput.OnSpawnPastry += GameInput_OnSpawnPastry;
    }

    private void GameInput_OnSpawnPastry(object sender, System.EventArgs e) {
        pastryHoldPoint.SpawnPastry(PastryHitTargetCallback);
    }

    private void GameInput_OnThrowPastry(object sender, System.EventArgs e) {
        if(aimState != AimState.Aiming) return;
        pastryHoldPoint.ThrowPastry(throwDirection);
    }

    private void Update() {
        HandleLook();
        UpdateAimState();


        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }




    private void HandleLook() {
        Vector2 lookInput = gameInput.GetLookDelta();
        Vector3 lookRoation = Vector3.zero;

        if(aimState == AimState.FreeLook) {
            lookRoation += new Vector3(lookInput.y, 0, 0);
            throwPower = 0;
            throwDirection = Vector3.zero;

        } else if(aimState == AimState.Aiming) {
            throwPower -= lookInput.y / Screen.height;
            throwPower = Mathf.Abs(throwPower);

            throwDirection = transform.forward * throwPower + transform.up * throwPower;
            Debug.DrawRay(transform.position, throwDirection);
        }
        lookRoation += new Vector3(0, lookInput.x, 0);
        transform.eulerAngles += lookRoation * mouseSensitivity;
    }

    private void UpdateAimState() {
        aimState = gameInput.GetAimBool() == false ?
            AimState.FreeLook : AimState.Aiming;
    }

    private void PastryHitTargetCallback(HitTargetCallBackArgs hitTargetCallBackArgs) {
        switch(hitTargetCallBackArgs.targetType) {
            default: Debug.LogError("hitTargetCallBackArgs.targetType = null"); break;
            case HitTargetCallBackArgs.TargetType.Static:
                Debug.Log("Hit static");
                playerScore += (int)hitTargetCallBackArgs.distance;
                break;
            case HitTargetCallBackArgs.TargetType.Dynamic:
                Debug.Log("Hit dynamic");
                playerScore += (int)hitTargetCallBackArgs.distance;
                break;

        }

        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
            score = playerScore
        });
    }



}
