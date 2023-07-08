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
    public CinemachineVirtualCamera ballVCam;
    public Vignette currentVignette;

    [Header("Player Cam Size")]
    public float defaultSize;
    public float zoomedSize;

    public enum CameraState {Player, Bowler, Ball};
    public CameraState currentCameraState;
    public BallController trackedBall = null;

    private void Start()
    {
        Camera.main.GetComponent<Volume>().profile.TryGet(out currentVignette);
    }

    public void SetCameraState(CameraState state, BallController ball=null)
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
                ballVCam.enabled = false;
                trackedBall = null;
                break;
            case CameraState.Bowler:
                playerVCam.enabled = false;
                bowlerVCam.enabled = true;
                ballVCam.enabled = false;
                trackedBall = null;
                break;
            case CameraState.Ball:
                if (ball == null || trackedBall != null) break;
                trackedBall = ball;
                playerVCam.enabled = false;
                bowlerVCam.enabled = false;
                ballVCam.enabled = true;
                break;
        }
    }

    public void Update() {
        if (currentCameraState == CameraState.Ball && (trackedBall != null)) {
            ballVCam.transform.position = new Vector3(ballVCam.transform.position.x, trackedBall.transform.position.y, ballVCam.transform.position.z);
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
