using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEventReferences : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference BowlingAlleyMusic {get; private set;}
    [field: SerializeField] public EventReference JapaneseMusic {get; private set;}    
    [field: SerializeField] public EventReference HalloweenMusic {get; private set;}
    [field: SerializeField] public EventReference MainMenuMusic {get; private set;}        

    

    [field: Header("Menu/Game SFX")]
    [field: SerializeField] public EventReference Success {get; private set;}   
    [field: SerializeField] public EventReference Failure {get; private set;}
    [field: SerializeField] public EventReference MenuClick {get; private set;}

    [field: Header("Ball SFX")]  
    [field: SerializeField] public EventReference BallRolling {get; private set;}
    [field: SerializeField] public EventReference BallBumper {get; private set;} 
    [field: SerializeField] public EventReference BallPit {get; private set;}   
    [field: SerializeField] public EventReference BallOnBall {get; private set;}          

    [field: Header("Player SFX")]  
    [field: SerializeField] public EventReference ChargingSound {get; private set;}
    [field: SerializeField] public EventReference JumpSound {get; private set;}

    [field: Header("Non-Player Pin SFX")]  
    [field: SerializeField] public EventReference BallCollision {get; private set;} 
    [field: SerializeField] public EventReference PinPlaced {get; private set;}     

    [field: Header("Thrower SFX")]   
    [field: SerializeField] public EventReference SumoStomp {get; private set;}   
    [field: SerializeField] public EventReference SamuraiSlowdown {get; private set;}    
    [field: SerializeField] public EventReference SamuraiSlash {get; private set;}  
    [field: SerializeField] public EventReference Dialogue {get; private set;}          

    [field: Header("Thrower SFX")]   
    [field: SerializeField] public EventReference SamuraiSlowSnapshot {get; private set;}   
    [field: SerializeField] public EventReference PauseMenu {get; private set;} 


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
