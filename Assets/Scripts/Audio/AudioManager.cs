using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class AudioManager : MonoBehaviour
{
   private List<EventInstance> eventInstances;

    public static AudioManager instance {get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Attempted to create second Audio Manager instance.");
        }
        instance = this;
        eventInstances = new List<EventInstance>();
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }
    private void OnDesstroy()
    {
        CleanUp();
    }
}
