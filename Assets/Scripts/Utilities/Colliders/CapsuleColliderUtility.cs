using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CapsuleColliderUtility
{
    public CapsuleColliderData CapsuleColliderData { get; private set; }
    [field: SerializeField] public DefaultColliderData DefaultColliderData { get; private set; }
    [field: SerializeField] public SlopeData SlopeData { get; set; }

    public void Initialize(GameObject gameObject)
    {
        if (CapsuleColliderData != null)
        {
            return;
        }
        CapsuleColliderData = new CapsuleColliderData();
        CapsuleColliderData.Initialize(gameObject);
    }

    public void CalculateCapsuleColliderDimensions()
    {
        SetCapsuleColliderRadius(DefaultColliderData.Radius);
        SetCapsuleColliderHeight(DefaultColliderData.Height * (1f - SlopeData.StepHeightPercentage));
        RecalculateColliderCenter();

        //防止当半高小于半径， 整个胶囊碰撞体上移
        float halfColliderHeight = CapsuleColliderData.Collider.height / 2;
        if (halfColliderHeight < CapsuleColliderData.Collider.radius)
        {
            SetCapsuleColliderRadius(halfColliderHeight);
        }
        
        CapsuleColliderData.UpdateColliderData();
    }

    private void RecalculateColliderCenter()
    {
        float colliderHeightDifference = DefaultColliderData.Height - CapsuleColliderData.Collider.height;
        Vector3 newColliderCenter = new Vector3(0f, DefaultColliderData.CenterY + (colliderHeightDifference / 2), 0f);
        CapsuleColliderData.Collider.center = newColliderCenter;
    }

    private void SetCapsuleColliderRadius(float radius)
    {
        CapsuleColliderData.Collider.radius = radius;
    }

    private void SetCapsuleColliderHeight(float height)
    {
        CapsuleColliderData.Collider.height = height;
    }
}