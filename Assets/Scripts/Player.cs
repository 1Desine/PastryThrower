using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using static Pastry;

public class Player : MonoBehaviour {

    [SerializeField] private GameInput gameInput;
    [SerializeField] private PastryHoldPoint pastryHoldPoint;

    [SerializeField] private float mouseSensitivity = 0.1f;

    public event EventHandler<OnScoreChangedEventArgs> OnScoreChanged;
    public class OnScoreChangedEventArgs : EventArgs {
        public int score;
    }
    public event EventHandler<OnThrowPowerChangedEventArgs> OnThrowPowerChanged;
    public class OnThrowPowerChangedEventArgs : EventArgs {
        public float throwPower;
    }



    private Vector3 throwDirection;
    private float throwPower = 0.5f;
    private float throwPower_PerModify = 0.1f;


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
        pastryHoldPoint.ThrowPastry(throwDirection);
    }

    private void LateUpdate() {
        // Preparing UI
        OnThrowPowerChanged?.Invoke(this, new OnThrowPowerChangedEventArgs(){
            throwPower = throwPower
        });
        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
            score = playerScore
        });
    }

    private void Update() {
        HandleLook();
        HandleThrowPower();


        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }




    private void HandleLook() {
        Vector2 lookInput = gameInput.GetLookDelta();
        Vector3 lookRoation = new Vector3(lookInput.y, lookInput.x, 0);

        throwDirection = transform.forward * throwPower + transform.up * throwPower;
        transform.eulerAngles += lookRoation * mouseSensitivity;
        Debug.DrawRay(transform.position, throwDirection);
    }
    private void HandleThrowPower() {
        switch(gameInput.GetScrollWheel()) {
            case 1:
                if(throwPower < 1) {
                    throwPower += throwPower_PerModify;

                    OnThrowPowerChanged?.Invoke(this, new OnThrowPowerChangedEventArgs {
                        throwPower = throwPower,
                    });
                }
                break;
            case -1:
                if(throwPower > 0) {
                    throwPower -= throwPower_PerModify;

                    OnThrowPowerChanged?.Invoke(this, new OnThrowPowerChangedEventArgs {
                        throwPower = throwPower,
                    });
                }
                break;
        }
        if(throwPower < 0) throwPower = 0;
        if(throwPower > 1) throwPower = 1;
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
                playerScore += (int)hitTargetCallBackArgs.distance; // change later
                break;
        }

        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
            score = playerScore
        });
    }



}
