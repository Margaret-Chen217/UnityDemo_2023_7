using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSprintData
{
    [field: SerializeField]
    [field: Range(1f, 3f)]
    public float SpeedModifier { get; private set; } = 1.7f;

    //经过多长时间从Sprint转换到Run
    [field: SerializeField]
    [field: Range(1f, 3f)]
    public float SprintToRunTime { get; private set; } = 1f;
    
    //walk->sprint->walk中，sprint->walk过渡时间
    [field: SerializeField]
    [field: Range(0f, 2f)]
    public float RunToWalkTime { get; private set; } = 0.5f;
}