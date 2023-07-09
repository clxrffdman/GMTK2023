using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEventReferences : MonoBehaviour
{
    [field: Header("Music References")]
    [field: SerializeField] public EventReference BowlingAlleyMusic {get; private set;}

    [field: Header("Ball SFX")]  
    [field: SerializeField] public EventReference BallRolling {get; private set;}
    [field: SerializeField] public EventReference BallBumper {get; private set;} 
    [field: SerializeField] public EventReference BallPit {get; private set;}       

    [field: Header("Player SFX")]  
    [field: SerializeField] public EventReference ChargingSound {get; private set;}
    [field: SerializeField] public EventReference JumpSound {get; private set;}

    [field: Header("Non-Player Pin SFX")]  
    [field: SerializeField] public EventReference BallCollision {get; private set;} 

    public static FMODEventReferences instance {get; private set;}
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Attempted to create second FMODEventReferences instance.");
        }
        instance = this;
    }
}
