using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using static Pastry;

public class Player : MonoBehaviour {

    [SerializeField] private GameInput gameInput;
    [SerializeField] private PastryHoldPoint pastryHoldPoint;

    [SerializeField] private Transform playerHead;

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

    private bool useGravity;
    private float airborneForTime;

    private float playerRadius = 0.15f;

    private int playerScore;




    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        useGravity = true;

        gameInput.OnThrowPastry += GameInput_OnThrowPastry;
        gameInput.OnSpawnPastry += GameInput_OnSpawnPastry;
        gameInput.OnJump += GameInput_OnJump; ;
    }

    private void GameInput_OnSpawnPastry(object sender, EventArgs e) {
        pastryHoldPoint.SpawnPastry(PastryHitTargetCallback);
    }
    private void GameInput_OnThrowPastry(object sender, EventArgs e) {
        pastryHoldPoint.ThrowPastry(throwDirection);
    }
    private void GameInput_OnJump(object sender, EventArgs e) {
        Jump();
    }

    private void LateUpdate() {
        // Preparing UI
        OnThrowPowerChanged?.Invoke(this, new OnThrowPowerChangedEventArgs() {
            throwPower = throwPower
        });
        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
            score = playerScore
        });
    }

    private void Update() {
        HandleLook();
        HandleMovement();
        HandleThrowPower();
        HandleGravity();
        HadlePushindObjects();

        Debug.DrawRay(playerHead.transform.position, playerHead.transform.forward, Color.green);
    }




    private void HandleLook() {
        Vector2 lookInput = gameInput.GetLookDelta();
        Vector3 PlayerBodyRoationY = new Vector3(0, lookInput.x, 0);
        Vector3 playerHeadRoationX = new Vector3(lookInput.y, 0, 0);

        transform.eulerAngles += PlayerBodyRoationY * mouseSensitivity; // Rotate player
        playerHead.transform.eulerAngles += playerHeadRoationX * mouseSensitivity; // Rotate playerHead

        throwDirection = playerHead.transform.forward * throwPower + playerHead.transform.up * throwPower;

        Debug.DrawRay(playerHead.transform.position, throwDirection);
    }
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = transform.forward * inputVector.y + transform.right * inputVector.x;
        moveDir.y = 0; // NO down/up movement
        moveDir.Normalize();

        bool canMove = true;
        float playerSpeed = 2f;
        float moveDistance = playerSpeed * Time.deltaTime;
        if(Physics.CapsuleCast(transform.position, playerHead.transform.position, playerRadius, moveDir, out RaycastHit hit, moveDistance)) {
            canMove = false;
        }

        if(canMove) {
            transform.position += moveDir * moveDistance;
        }
    }
    private void Jump() {
        float jumpHeight = 1f;
        transform.position += Vector3.up * jumpHeight;
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
    private void HandleGravity() {
        if(useGravity == false) return;

        float castDistance = 0.6f; // to change player height
        float playerHoverHeight_PercentsFromRayDistance = 0.8f;

        Vector3 capsuleTopposition = transform.position + Vector3.up * 0.2f;
        if(Physics.CapsuleCast(transform.position, capsuleTopposition, playerRadius, Vector3.down, out RaycastHit raycastHit, castDistance)) {
            airborneForTime = 0;
            if(raycastHit.distance < castDistance * playerHoverHeight_PercentsFromRayDistance) {
                // If Player is lower then should be
                if(raycastHit.transform.GetComponent<Rigidbody>() != null) {
                    // If object has Rigidbody
                } else {
                    // If object does not have Rigidbody
                    transform.position = transform.position + Vector3.down * raycastHit.distance + Vector3.up * castDistance * playerHoverHeight_PercentsFromRayDistance;
                }
            } else {
                // Magniting Player to hit
                float magnitingSpeed = 30 * Time.deltaTime;
                transform.position = Vector3.Slerp(transform.position, transform.position + Vector3.down * raycastHit.distance + Vector3.up * castDistance * playerHoverHeight_PercentsFromRayDistance, magnitingSpeed);
            }
        } else {
            airborneForTime += Time.deltaTime;
            float fallingSpeed = 20 * Time.deltaTime;
            transform.position += Vector3.down * fallingSpeed * airborneForTime * airborneForTime / 2;
        }
    }
    private void HadlePushindObjects() {
        float castDistance = 1f;
        float pushingRadius = playerRadius * 1.1f;
        Vector3 capsuleTopposition = transform.position + Vector3.up * 0.2f;
        RaycastHit[] raycastHits = Physics.CapsuleCastAll(transform.position, capsuleTopposition, pushingRadius, Vector3.down, castDistance);
        foreach(RaycastHit hit in raycastHits) {
            if(hit.transform.TryGetComponent(out Rigidbody hitBody)) {
                // If object has Rigidbody
                Vector2 inputVector = gameInput.GetMovementVectorNormalized();
                Vector3 moveDir = transform.forward * inputVector.y + transform.right * inputVector.x;

                Vector3 vectorToHitPosition = new Vector3(
                        hit.transform.position.x - transform.position.x,
                        0,
                        hit.transform.position.z - transform.position.z 
                        );

                float pushingForce = (1 - vectorToHitPosition.magnitude) * 30;
                Vector3 pushingDrection;
                if(moveDir != Vector3.zero) {
                    pushingDrection = moveDir;
                } else {
                    pushingDrection = vectorToHitPosition;
                    pushingForce /= 1.5f;
                }
                pushingDrection.Normalize();
                hitBody.AddForce(pushingDrection * pushingForce * Time.deltaTime, ForceMode.VelocityChange);

                Debug.DrawRay(transform.position + Vector3.down * castDistance, pushingDrection, Color.red, 1f);
            }

        }
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
