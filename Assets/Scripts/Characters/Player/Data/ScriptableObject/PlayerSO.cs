using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = ("ScriptableObject/Character/PlayerSO"))]
public class PlayerSO : ScriptableObject
{
    [field:SerializeField] public PlayerGroundedData GroundedData { get; private set; }
    
    [field:SerializeField] public PlayerAirborneData AirborneData { get; private set; }
}