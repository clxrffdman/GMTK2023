using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class BaseButtonClick : MonoBehaviour
{
    public EventReference buttonClickRef;

    public void OnPointerDown()
    {
        RuntimeManager.PlayOneShot(buttonClickRef);
    }
}
