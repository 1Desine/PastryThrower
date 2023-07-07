using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    [SerializeField] private GameObject visual;
    [SerializeField] private bool setRandomColor;

    [SerializeField] private Pastry.HitTargetCallBackArgs.TargetType targetType;



    private void Awake() {
        SetColor();
    }



    public Pastry.HitTargetCallBackArgs.TargetType GetTargetType() {
        return targetType;
    }





    private void SetColor() {
        if(setRandomColor == false) return;
        Material material = visual.GetComponent<MeshRenderer>().material;

        Color color =  Color.green;

        material.color = color;
    }


}
