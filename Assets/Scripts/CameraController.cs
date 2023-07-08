using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CameraController : UnitySingleton<CameraController>
{
    public CinemachineVirtualCamera bowlerVCam;
    public CinemachineVirtualCamera playerVCam;
    public Vignette currentVignette;

    [Header("Player Cam Size")]
    public float defaultSize;
    public float zoomedSize;

    public enum CameraState {Player, Bowler};
    public CameraState currentCameraState;

    private void Start()
    {
        Camera.main.GetComponent<Volume>().profile.TryGet(out currentVignette);
    }

    public void SetCameraState(CameraState state)
    {
        if(currentCameraState == state)
        {
            return;
        }

        currentCameraState = state;

        switch (state) {
            case CameraState.Player:
                playerVCam.enabled = true;
                bowlerVCam.enabled = false;
                break;
            case CameraState.Bowler:
                playerVCam.enabled = false;
                bowlerVCam.enabled = true;
                break;

        }
    }

    public void SetBowlerCamTarget(Transform target)
    {
        if(bowlerVCam.Follow == null)
        {
            bowlerVCam.Follow = target;
        }
            
    }

    public void SetVignetteStrength(float value)
    {
        if (currentVignette)
        {
            currentVignette.intensity.value = value;
        }
    }

    public float GetVignetteStrength()
    {
        if (currentVignette)
        {
            return currentVignette.intensity.value;
        }

        return 0;
    }

    public void SetPlayerCamZoom(float value)
    {
        playerVCam.m_Lens.OrthographicSize = value;
    }

    public float GetPlayerCamZoom()
    {
        return playerVCam.m_Lens.OrthographicSize;
    }
}
