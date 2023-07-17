using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DefaultColliderData
{
    [field: SerializeField] public float Height { get; set; } = 1.64f;
    [field: SerializeField] public float CenterY { get; set; } = 0.82f;
    [field: SerializeField] public float Radius { get; set; } = 0.2f;
}
