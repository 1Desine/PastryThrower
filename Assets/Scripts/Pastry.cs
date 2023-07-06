using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pastry : MonoBehaviour {

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


}
