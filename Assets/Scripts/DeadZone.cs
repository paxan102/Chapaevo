using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeadZone : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Checker checker = other.gameObject.GetComponent<Checker>();
        
        if (checker)
        {
            checker.Disable();
        }
        
    }
}
