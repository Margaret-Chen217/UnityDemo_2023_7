using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerJumpData
{
    [field: SerializeField] public PlayerRotationData RotationData { get; private set; }

    [field: SerializeField]
    [field: Range(0f, 5f)]
    public float JumpGroundRayDistance { get; private set; }

    [field: SerializeField] public AnimationCurve JumpForceModifierOnSlopUpwards { get; private set; }
    [field: SerializeField] public AnimationCurve JumpForceModifierOnSlopDownwards { get; private set; }
    [field: SerializeField] public Vector3 StationaryForce { get; private set; }
    [field: SerializeField] public Vector3 WeakForce { get; private set; }
    [field: SerializeField] public Vector3 MediumForce { get; private set; }
    [field: SerializeField] public Vector3 StrongForce { get; private set; }

    [field: SerializeField]
    [field: Range(0f, 10f)]
    public float DecelerationForce { get; private set; } = 1.5f;
}