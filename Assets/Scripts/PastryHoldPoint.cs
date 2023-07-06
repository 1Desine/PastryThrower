using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastryHoldPoint : MonoBehaviour {

    [SerializeField] private ThrowableObjectsSOList throwableObjectsSOList;

    GameObject pastry;


    private void Start() {
        SpawnPastry();
    }


    public void SpawnPastry() {
        if(pastry != null) {
            Debug.LogError("Tried spawn Pastry - pastry != null");
            return;
        }

        GameObject randomPastry = throwableObjectsSOList.GetRandomPastry();
        pastry = Instantiate(randomPastry);

        Debug.LogError("Tried spawn Pastry - pasrty Spawned");
    }


    public void ThrowPastry(Vector3 direction) {
        if(pastry == null) {
            Debug.LogError("Tried throwind Pastry - no Pastry to throw");
            return;
        }

        Rigidbody pastryBody = pastry.GetComponent<Rigidbody>();
        float forceModifier = 10;
        pastryBody.AddForce(direction * forceModifier, ForceMode.Impulse);


        Debug.Log("ThrowPastry - direction: " + direction);
    }


    public bool HasPastry() {
        return pastry != null ? true : false;
    }





}
