using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Pastry.HitTargetCallBackArgs;

public class Pastry : MonoBehaviour {

    [SerializeField] private GameObject Visual;


    public delegate void HitTargetCallBack(HitTargetCallBackArgs hitTargetCallBackArgs);
    public HitTargetCallBack hitTargetCallBack;
    public class HitTargetCallBackArgs {
        public enum TargetType {
            Static,
            Dynamic,
        }

        public TargetType targetType;
        public float distance;
    }

    private Rigidbody body;
    private new Collider collider;

    private bool isBeingCarried;
    private Vector3 startPosition;
    private bool hitSomething;


    private void Awake() {
        body = gameObject.GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;

        body.drag = 1f;
        isBeingCarried = true;

        SetColor();
    }


    private void Update() {
        if(isBeingCarried) {
            startPosition = transform.position;
        }
    }



    public bool IsBeingCarried() {
        return isBeingCarried;
    }

    public void SetBeingCarried_False() {
        isBeingCarried = false;
        hitSomething = false;
        collider.enabled = true;
    }


    private void OnTriggerEnter(Collider trigger) {
        Callback(trigger.gameObject);
        hitSomething = true;
    }
    private void OnCollisionEnter(Collision collision) {
        Callback(collision.gameObject);
        hitSomething = true;
    }

    private void Callback(GameObject interactGameObject) {
        if(hitSomething == false) {
            if(interactGameObject.TryGetComponent(out Target target)) {
                float distance = (transform.position - startPosition).magnitude;

                hitTargetCallBack(new HitTargetCallBackArgs() {
                    targetType = target.GetTargetType(),
                    distance = distance,
                });
                hitSomething = true;
            }
        }
    }





    private void SetColor() {
        Material material =  Visual.GetComponent<MeshRenderer>().material;

        Color color = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f), 1f);

        material.color = color;
    }


}
