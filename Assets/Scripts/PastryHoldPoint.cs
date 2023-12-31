using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Pastry;

public class PastryHoldPoint : MonoBehaviour {

    [SerializeField] private ThrowableObjectsSOList throwableObjectsSOList;

    [SerializeField] private Transform mapPastryHolder;

    Pastry pastry;



    private void Update() {
        KeepPastryInfront();
    }

    public bool SpawnPastry(HitTargetCallBack callBack) {
        if(HasPastry()) {
            return false;
        }

        Pastry randomPastry = throwableObjectsSOList.GetRandomPastry();
        pastry = Instantiate(randomPastry, mapPastryHolder);
        pastry.hitTargetCallBack = callBack;

        pastry.transform.position = this.transform.position;

        Rigidbody pastryBody = pastry.GetComponent<Rigidbody>();
        pastryBody.useGravity = false;

        return true;
    }

    public void ThrowPastry(Vector3 direction) {
        if(!HasPastry()) {
            return;
        }

        Rigidbody pastryBody = pastry.GetComponent<Rigidbody>();
        pastryBody.useGravity = true;

        pastry.SetIsBeingCarried_False();

        pastryBody.AddForce(direction, ForceMode.Impulse);

        pastry = null;
    }

    public bool HasPastry() {
        return pastry != null ? true : false;
    }

    private void KeepPastryInfront() {
        if(HasPastry()) {
            if(pastry.IsBeingCarried()) {
                float straighteningSpeed = 60f * Time.deltaTime;
                pastry.transform.position = Vector3.Slerp(pastry.transform.position, this.transform.position, straighteningSpeed);
            }
        }
    }




}
