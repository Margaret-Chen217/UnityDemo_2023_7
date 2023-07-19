using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [Header("State Group Parameter Names")]
    [SerializeField] private string groundParameterName = "Grounded";
    [SerializeField] private string movingParameterName = "Moving";
    [SerializeField] private string stoppingParameterName = "Stopping";
    [SerializeField] private string landingParameterName = "Landing";
    [SerializeField] private string airborneParameterName = "Airborne";

    [Header("Grounded Parameter Names")]
    [SerializeField] private string idleParameterName = "isIdling";
    [SerializeField] private string dashParameterName = "isDashing";
    [SerializeField] private string walkParameterName = "isWalking";
    [SerializeField] private string runParameterName = "isRunning";
    [SerializeField] private string sprintParameterName = "isSprinting";
    [SerializeField] private string mediumStopParameterName = "isMediumStopping";
    [SerializeField] private string hardStopParameterName = "isHardStopping";
    [SerializeField] private string rollParameterName = "isRolling";
    [SerializeField] private string hardLandParameterName = "isHardLanding";

    [Header("Airborne Parameter Names")] 
    [SerializeField] private string fallParameterName = "isFalling";
    
    public int GroundParameterHash { get; private set; }
    public int MovingParameterHash { get; private set; }
    public int StoppingParameterHash { get; private set; }
    public int LandingParameterHash { get; private set; }
    public int AirborneParameterHash { get; private set; }
    
    
    public int IdleParameterHash { get; private set; }
    public int DashParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int SprintParameterHash { get; private set; }
    public int MediumStopParameterHash { get; private set; }
    public int HardStopParameterHash { get; private set; }
    public int RollParameterHash { get; private set; }
    public int HardLandParameterHash { get; private set; }
    public int FallParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        MovingParameterHash = Animator.StringToHash(movingParameterName);
        StoppingParameterHash = Animator.StringToHash(stoppingParameterName);
        LandingParameterHash = Animator.StringToHash(landingParameterName);
        AirborneParameterHash = Animator.StringToHash(airborneParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        DashParameterHash = Animator.StringToHash(dashParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        SprintParameterHash = Animator.StringToHash(sprintParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        MediumStopParameterHash = Animator.StringToHash(mediumStopParameterName);
        HardStopParameterHash = Animator.StringToHash(hardStopParameterName);
        RollParameterHash = Animator.StringToHash(rollParameterName);
        HardLandParameterHash = Animator.StringToHash(hardLandParameterName);
        FallParameterHash = Animator.StringToHash(fallParameterName); 
        
        Debug.Log($"GroundParameterHash : {groundParameterName}");
        Debug.Log($"MovingParameterHash : {movingParameterName}");
        Debug.Log($"StoppingParameterHash : {stoppingParameterName}");
        Debug.Log($"LandingParameterHash : {landingParameterName}");
        // Debug.Log($"GroundParameterHash : {groundParameterName}");
        // Debug.Log($"GroundParameterHash : {groundParameterName}");
        // Debug.Log($"GroundParameterHash : {groundParameterName}");
        // Debug.Log($"GroundParameterHash : {groundParameterName}");
        // Debug.Log($"GroundParameterHash : {groundParameterName}");
        // Debug.Log($"GroundParameterHash : {groundParameterName}");
        // Debug.Log($"GroundParameterHash : {groundParameterName}");
        // Debug.Log($"GroundParameterHash : {groundParameterName}");
    }
    
}