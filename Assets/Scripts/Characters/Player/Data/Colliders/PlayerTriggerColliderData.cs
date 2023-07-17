using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerTriggerColliderData
{
    [field: SerializeField] public BoxCollider GroundCheckCollider { get; private set; }
}
