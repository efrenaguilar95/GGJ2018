using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediPod : MonoBehaviour {

    void OnTriggerStay2D(Collider2D collider)
    {
        GameObject hitObj = collider.gameObject;
        if (hitObj.tag == "Human")
        {
            Destroy(hitObj);
        }
    }
}
