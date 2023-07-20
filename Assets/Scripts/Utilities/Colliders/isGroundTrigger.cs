using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class isGroundTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.name == "LeftFootTrigger")
        {
            HandleFootGroundEnter(true);
        }
        else
        {
            HandleFootGroundEnter(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    bool HandleFootGroundEnter(bool isLeft)
    {
        return false;
    }
    
    bool HandleFootGroundExit(bool isLeft)
    {
        return false;
    }
    

}
