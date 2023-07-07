using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    [SerializeField] private Pastry.HitTargetCallBackArgs.TargetType targetType;




    public Pastry.HitTargetCallBackArgs.TargetType GetTargetType() {
        return targetType;
    }




}
