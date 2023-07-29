using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShootingDetect : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    public GameObject obj;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
                Debug.Log($"Hit Point: {hit.point}");
                Instantiate(obj, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}