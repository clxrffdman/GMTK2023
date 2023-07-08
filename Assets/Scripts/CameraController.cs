using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : UnitySingleton<CameraController>
{
    public CinemachineVirtualCamera bowlerVCam;
    public CinemachineVirtualCamera playerVCam;

    public enum CameraState {Player, Bowler};
    public CameraState currentCameraState;

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
}
