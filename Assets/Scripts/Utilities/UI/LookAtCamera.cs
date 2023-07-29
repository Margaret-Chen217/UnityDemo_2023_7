using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LookAtCamera : MonoBehaviour
{
    
    public GameObject obj;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        obj.transform.LookAt(mainCamera.transform);
    }
}
