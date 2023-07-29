using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerOnlineInfoSO", menuName = ("ScriptableObject/Character/PlayerOnlineInfoSO"))]
public class PlayerOnlineInfoSO : ScriptableObject
{
    [field: SerializeField] public string  playerName { get; set; }
    
}
