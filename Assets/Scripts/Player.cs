using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
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
        public float throwPowerNormalized;
    }
    public event EventHandler<OnAmmoChangedEventArgs> OnAmmoChanged;
    public class OnAmmoChangedEventArgs : EventArgs {
        public int ammo;
    }

    public event EventHandler<HitTargetCallBackArgs> OnHitTarget;


    private Vector3 throwDirection;
    private float throwPowerNormalized;
    private float throwPower_AddUpSpeed = 1f;

    private ThrowingState throwingState;
    private enum ThrowingState {
        Idle,
        AddingPower,
        Canceled,
    }

    private bool useGravity;
    private float airborneForTime;

    private float playerRadius = 0.15f;

    private int playerScore;


    private int ammo;
    private int ammoMax = 10;
    private float ammoCooldown;
    private float ammoCooldownMax = 2f;



    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        useGravity = true;
        ammo = ammoMax;
        throwingState = ThrowingState.Idle;
    }

    private void Start() {
        gameInput.OnThrowCanceled += GameInput_OnThrowCanceled;
        gameInput.OnJump += GameInput_OnJump;

        StartCoroutine(CorountineUI());
    }

    private IEnumerator CorountineUI() {
        yield return new WaitForSeconds(0.01f);
        OnThrowPowerChanged?.Invoke(this, new OnThrowPowerChangedEventArgs() {
            throwPowerNormalized = throwPowerNormalized
        });
        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs {
            score = playerScore
        });
        OnAmmoChanged?.Invoke(this, new OnAmmoChangedEventArgs {
            ammo = ammo
        });
    }


    private void Update() {
        HandleLook();
        HandleMovement();
        HandleThrowing();
        Gravity();
        PushindObjects();
        CoolDown();

        Debug.DrawRay(playerHead.transform.position, playerHead.transform.forward, Color.green);
    }



    private void SpawnPastry() {
        if(ammo <= 0) return;
        if(pastryHoldPoint.SpawnPastry(PastryHitTargetCallback)) {

            ammo--;
            OnAmmoChanged?.Invoke(this, new OnAmmoChangedEventArgs {
                ammo = ammo
            });
        }
    }

    private void CoolDown() {
        ammoCooldown -= Time.deltaTime;
        if(ammoCooldown < 0) {
            ammoCooldown = ammoCooldownMax;
            if(ammo < ammoMax) {
                ammo++;
                OnAmmoChanged?.Invoke(this, new OnAmmoChangedEventArgs {
                    ammo = ammo
                });
            }
        }
    }

    private void HandleThrowing() {
        bool throwButtonDown = gameInput.IsThrowButtonDown();
        float throwForceModifier = 20f;


        if(throwingState == ThrowingState.Idle) {
            if(throwButtonDown && ammo > 0) {
                SpawnPastry();
                throwingState = ThrowingState.AddingPower;
            }
        } else if(throwingState == ThrowingState.AddingPower) {
            if(throwPowerNormalized < 1) {
                throwPowerNormalized += throwPower_AddUpSpeed * Time.deltaTime;
            }
            if(throwButtonDown == false) {
                throwingState = ThrowingState.Idle;
                float minThrowingPowerNormalized = 0.1f;
                if(throwPowerNormalized > minThrowingPowerNormalized) {
                    pastryHoldPoint.ThrowPastry(throwDirection * throwPowerNormalized * throwForceModifier);
                }
                throwPowerNormalized = 0;
            }
            OnThrowPowerChanged?.Invoke(this, new OnThrowPowerChangedEventArgs() {
                throwPowerNormalized = throwPowerNormalized
            });
        } else if(throwingState == ThrowingState.Canceled) {
            throwPowerNormalized = 0;
            if(throwButtonDown == false) {
                throwingState = ThrowingState.Idle;
            }
            OnThrowPowerChanged?.Invoke(this, new OnThrowPowerChangedEventArgs() {
                throwPowerNormalized = throwPowerNormalized
            });
        }
    }
    private void GameInput_OnThrowCanceled(object sender, EventArgs e) {
        throwingState = ThrowingState.Canceled;
    }

    private void GameInput_OnJump(object sender, EventArgs e) {
        Jump();
    }


    private void HandleLook() {
        Vector2 lookInput = gameInput.GetLookDelta();
        Vector3 PlayerBodyRoationY = new Vector3(0, lookInput.x, 0);
        Vector3 playerHeadRoationX = new Vector3(lookInput.y, 0, 0);

        transform.eulerAngles += PlayerBodyRoationY * mouseSensitivity; // Rotate Player left/right
        playerHead.transform.eulerAngles += playerHeadRoationX * mouseSensitivity; // Tilt playerHead up/down
        
        throwDirection = playerHead.transform.forward + playerHead.transform.up / 2; // I dont know how to rotate Vector3, so i tilt it down a bit

        Debug.DrawRay(playerHead.transform.position, throwDirection);
    }
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = transform.forward * inputVector.y + transform.right * inputVector.x;
        moveDir.y = 0; // NO down/up movement
        moveDir.Normalize();

        bool canMove = true;
        float playerSpeed = 5f;
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
    private void Gravity() {
        if(useGravity == false) return;

        float fallingSpeedMax = 50f * Time.deltaTime;
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
            float gravity = 20f;
            float fallingSpeed = gravity * airborneForTime * airborneForTime / 2 * Time.deltaTime;
            if(fallingSpeed > fallingSpeedMax) {
                fallingSpeed = fallingSpeedMax;
            }
            transform.position += Vector3.down * fallingSpeed;
        }
    }
    private void PushindObjects() {
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

                float pushingForce = 50 * Time.deltaTime;
                Vector3 pushingDrection;
                if(moveDir != Vector3.zero) {
                    pushingDrection = moveDir;
                } else {
                    pushingDrection = vectorToHitPosition;
                    pushingForce /= 1.5f;
                }
                pushingDrection.Normalize();
                hitBody.AddForce(pushingDrection * pushingForce, ForceMode.VelocityChange);

                Debug.DrawRay(transform.position + Vector3.down * castDistance, pushingDrection, Color.red, 1f);
            }

        }
    }

    private void PastryHitTargetCallback(HitTargetCallBackArgs hitTargetCallBackArgs) {
        OnHitTarget?.Invoke(this, hitTargetCallBackArgs);

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

    public int GetAmmoMax() {
        return ammoMax;
    }

}
