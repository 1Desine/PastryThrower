using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ThrowableObjectsSOList : ScriptableObject {

    public List<ThrowableObjectSO> throwableObjectsSOList;

    public GameObject GetRandomPastry() {
        GameObject randomPastry = throwableObjectsSOList[Random.Range(0, throwableObjectsSOList.Count)].prefab;
        return randomPastry;
    }

}
