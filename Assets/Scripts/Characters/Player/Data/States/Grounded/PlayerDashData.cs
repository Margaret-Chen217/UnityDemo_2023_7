using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDashData
{
    [field: SerializeField]
    [field: Range(1f, 3f)]
    public float SpeedModifier { get; private set; } = 2f;
    
    [field: SerializeField]
    public PlayerRotationData RotationData { get; private set; }

    //连续冲刺时间
    [field: SerializeField]
    [field: Range(0f, 2f)]
    public float TimeToBeConsideredConsecutive { get; private set; } = 2f;

    //每轮dash次数
    [field: SerializeField]
    [field: Range(1, 10)]
    public float ConsecutiveDashesLimitAmount { get; private set; } = 2;

    //连续dash后的冷却时间
    [field: SerializeField]
    [field: Range(0f, 5f)]
    public float DashLimitReachedCooldown { get; private set; } = 1.75f;
    
}