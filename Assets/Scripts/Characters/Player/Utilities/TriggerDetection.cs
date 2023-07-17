using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    public Action<Collider> OnCollisionEnter_Action;
    public Action<Collider> OnCollisionExit_Action;
    

    private void OnTriggerEnter(Collider other)
    {
        OnCollisionEnter_Action ?. Invoke(other);
    }
    
    private void OnTriggerExit(Collider other)
    {
        OnCollisionExit_Action ?. Invoke(other);
    }
}
