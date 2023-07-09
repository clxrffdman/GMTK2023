using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMODUnity;

public class VolumeSliderController : MonoBehaviour
{
    public Slider volumeSlider;

    private void Awake()
    {
        if(volumeSlider == null)
        {
            volumeSlider = GetComponent<Slider>();
        }
    }

    private void Start()
    {
        float vol;
        RuntimeManager.GetBus("bus:/").getVolume(out vol);
        volumeSlider.value = vol;
    }

    public void UpdateVolume(float val)
    {
        RuntimeManager.GetBus("bus:/").setVolume(val);
    }
}
