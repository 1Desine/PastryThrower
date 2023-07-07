using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadVisual : MonoBehaviour {

    [SerializeField] private GameObject[] Eyes;
    Material[] materials = new Material[2];

    Color transitionColor = Color.red;

    private float stateComplition;


    private void Awake() {
        materials[0] = Eyes[0].GetComponent<MeshRenderer>().material;
        materials[1] = Eyes[1].GetComponent<MeshRenderer>().material;


    }


    private void Update() {
        float transitionSpeed = 0.01f;

        materials[0].color = Color.Lerp(materials[0].color, transitionColor, transitionSpeed);
        materials[1].color = Color.Lerp(materials[1].color, transitionColor, transitionSpeed);


        stateComplition += Time.deltaTime;
        if(stateComplition > 1) {
            stateComplition = 0;

            if(transitionColor == Color.red) {
                transitionColor = Color.green;
            } else
            if(transitionColor == Color.green) {
                transitionColor = Color.blue;
            } else {
                transitionColor = Color.red;
            }
        }
    }



}
