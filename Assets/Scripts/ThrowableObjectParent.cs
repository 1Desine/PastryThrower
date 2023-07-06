using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObjectParent : MonoBehaviour {

    [SerializeField] private List<ThrowableObjectSO> throwableObjectParentSOList;


    GameObject pastry;



    private void Start() {
        SpawnPastry();
    }


    public void SpawnPastry() {
        if(pastry != null) {
            Debug.LogError("Tried spawn Pastry - pastry != null");
            return;
        }

        GameObject randomPastry = throwableObjectParentSOList[Random.Range(0, throwableObjectParentSOList.Count)].prefab;
        pastry = Instantiate(randomPastry, null);

        if(pastry != null) {
            Debug.LogError("Tried spawn Pastry - pasrty Spawned");
        } else {
            Debug.LogError("Tried spawn Pastry - pasrty did not Spawn");
        }
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
