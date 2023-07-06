using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Pastry;

public class PastryHoldPoint : MonoBehaviour {

    [SerializeField] private ThrowableObjectsSOList throwableObjectsSOList;

    Pastry pastry;



    private void Update() {
        KeepPastryInfront();
    }

    public void SpawnPastry(HitTargetCallBack callBack) {
        if(HasPastry()) {
            Debug.LogError("Tried spawn Pastry - pastry != null");
            return;
        }

        Pastry randomPastry = throwableObjectsSOList.GetRandomPastry();
        pastry = Instantiate(randomPastry);
        pastry.hitTargetCallBack = callBack;

        pastry.transform.position = this.transform.position;

        Rigidbody pastryBody = pastry.GetComponent<Rigidbody>();
        pastryBody.useGravity = false;

        Debug.Log("Pastry - pasrty Spawned");
    }

    public void ThrowPastry(Vector3 direction) {
        if(!HasPastry()) {
            Debug.LogError("Tried throwind Pastry - no Pastry to throw");
            return;
        }

        Rigidbody pastryBody = pastry.GetComponent<Rigidbody>();
        pastryBody.useGravity = true;

        pastry.SetBeingCarried_False();

        float forceModifier = 10;
        pastryBody.AddForce(direction * forceModifier, ForceMode.Impulse);

        pastry = null;
    }

    public bool HasPastry() {
        return pastry != null ? true : false;
    }

    private void KeepPastryInfront() {
        if(HasPastry()) {
            if(pastry.IsBeingCarried()) {
                float straighteningSpeed = 2f;
                pastry.transform.position = Vector3.Slerp(pastry.transform.position, this.transform.position, straighteningSpeed * Time.deltaTime);
            }
        }
    }




}
