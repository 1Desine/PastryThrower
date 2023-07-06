using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pastry : MonoBehaviour {


    public delegate void HitTargetCallBack();
    public HitTargetCallBack hitTargetCallBack;



    private bool isBeingCarried;



    private void Awake() {
        isBeingCarried = true;
    }


    public bool IsBeingCarried() {
        return isBeingCarried;
    }

    public void SetBeingCarried_False() {
        isBeingCarried = false;
    }


    private void OnTriggerEnter(Collider trigger) {
        Debug.Log(trigger.transform.position);
        if(trigger.gameObject.GetComponent<Target>() != null) {
            hitTargetCallBack();
        }
    }



}
