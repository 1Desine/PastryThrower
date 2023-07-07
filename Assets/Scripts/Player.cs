using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using static Pastry;

public class Player : MonoBehaviour {

    [SerializeField] private GameInput gameInput;
    [SerializeField] private PastryHoldPoint pastryHoldPoint;
    [SerializeField] private GameObject playerColliderObject;
    private Collider playerCollider;
    [SerializeField] private LayerMask gravityRaycastLayerMask;

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

    private int playerScore;



    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCollider = playerColliderObject.GetComponent<Collider>();
        useGravity = true;

        gameInput.OnThrowPastry += GameInput_OnThrowPastry;
        gameInput.OnSpawnPastry += GameInput_OnSpawnPastry;
    }

    private void GameInput_OnSpawnPastry(object sender, EventArgs e) {
        pastryHoldPoint.SpawnPastry(PastryHitTargetCallback);
    }

    private void GameInput_OnThrowPastry(object sender, EventArgs e) {
        pastryHoldPoint.ThrowPastry(throwDirection);
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
        Vector3 inputDir = new Vector3(inputVector.y, 0, inputVector.x);
        Vector3 moveDir = transform.forward * inputDir.x + transform.right * inputDir.z;
        moveDir.y = 0; // NO down/up movement
        moveDir.Normalize();

        bool canMove = true;
        float playerSpeed = 2f;
        float playerRadius = 0.1f;
        float moveDistance = playerSpeed * Time.deltaTime;
        if(Physics.CapsuleCast(transform.position, playerHead.transform.position, playerRadius, moveDir, out RaycastHit hit, moveDistance)) {
            canMove = false;
            if(hit.collider.gameObject.TryGetComponent(out Rigidbody rb)) {
                rb.AddForce(moveDir * Time.deltaTime);
                canMove = true;
            }
        }

        if(canMove) {
            transform.position += moveDir * moveDistance;
        }
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



    private void HandleGravity() {
        if(useGravity == false) return;

        float rayDistance = 0.6f; // change, to change player height
        float playerHoverHeight_PercentsFromRayDistance = 0.8f;
        if(Physics.Raycast(transform.position, -Vector3.up, out RaycastHit raycastHit, rayDistance, gravityRaycastLayerMask)) {
            if(raycastHit.distance < rayDistance * playerHoverHeight_PercentsFromRayDistance) {
                airborneForTime = 0;
                transform.position = raycastHit.point + Vector3.up * rayDistance * playerHoverHeight_PercentsFromRayDistance;
            } else {
                float magnitingSpeed = 30 * Time.deltaTime;
                transform.position = Vector3.Slerp(transform.position, raycastHit.point + Vector3.up * rayDistance * playerHoverHeight_PercentsFromRayDistance, magnitingSpeed);
            }
        } else {
            airborneForTime += Time.deltaTime;
            float fallingSpeed = 9 * Time.deltaTime;
            transform.position += Vector3.down * fallingSpeed * airborneForTime * airborneForTime / 2;
        }
    }



}
