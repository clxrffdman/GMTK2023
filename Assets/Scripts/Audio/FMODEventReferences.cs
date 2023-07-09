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
