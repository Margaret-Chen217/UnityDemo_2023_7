using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IKTest : MonoBehaviour
{
    public Animator animator;

    private GameObject LeftToe;
    private GameObject RightToe;
    private GameObject LeftHandTarget;

    //public GameObject target;

    private Vector3 leftToeIKPos, rightToeIKPos;

    [field: Range(0f, 1f)] public float weight = 1;
    [field: Range(0f, 5f)] public float rayDistance = 1;

    private int groundLayerMask = 6;

    private Vector3 leftFootTarget;

    private bool isOnGround;

    void Start()
    {
        LeftToe = GameObject.Find("Left ankle");
        RightToe = GameObject.Find("Right ankle");
        LeftHandTarget = GameObject.Find("LeftHandTarget");

        leftFootTarget = LeftToe.transform.position;
        Debug.Log($"LeftFoot Target: {leftFootTarget}");
    }

    void FixedUpdate()
    {
        Ray leftToeRay = new Ray(leftFootTarget, Vector3.forward);

        RaycastHit leftToeHit, rightToeHit;
        //leftFootTarget = LeftToe.transform.position;
        if (Physics.Raycast(leftToeRay, out leftToeHit, 2))
        {
            
            bool leftToeRayBool = Physics.Raycast(leftToeRay, out leftToeHit, 1);
            if (!leftToeRayBool)
            {
                //向前移动
                leftFootTarget.z += 1f;
                Debug.Log("Forward");
                //Debug.Log($"Left Toe: {leftToeRay}");
            }
            else
            {
                //向上移动
                leftFootTarget.y += 1f;
                Debug.Log("Up");
            }
        }
        else
        {
            leftFootTarget = LeftToe.transform.position;
        }
    }

    public void OnAnimatorIK(int layerIndex)
    {
        //Debug.Log($"LeftFoot Target: {leftFootTarget}");

        //animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftToeIKPos);
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget);
        animator.SetIKRotation(AvatarIKGoal.LeftFoot, transform.rotation);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight);
        
        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget.transform.position);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}