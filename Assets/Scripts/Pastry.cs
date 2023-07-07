using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Pastry.HitTargetCallBackArgs;

public class Pastry : MonoBehaviour {


    public delegate void HitTargetCallBack(HitTargetCallBackArgs hitTargetCallBackArgs);
    public HitTargetCallBack hitTargetCallBack;
    public class HitTargetCallBackArgs {
        public enum TargetType {
            Static,
        }

        public TargetType targetType;
        public float distance;
    }


    private bool isBeingCarried;



    private Vector3 startPosition;


    private void Awake() {
        isBeingCarried = true;
    }


    private void Update() {
        if (isBeingCarried) {
            startPosition = transform.position;
        }
    }


    public bool IsBeingCarried() {
        return isBeingCarried;
    }

    public void SetBeingCarried_False() {
        isBeingCarried = false;
    }


    private void OnTriggerEnter(Collider trigger) {
        if(trigger.gameObject.TryGetComponent(out Target target)) {
            float distance = (transform.position - startPosition).magnitude;

            hitTargetCallBack(new HitTargetCallBackArgs() {
                targetType = target.GetTargetType(),
                distance = distance,
            });
        }
    }



}
