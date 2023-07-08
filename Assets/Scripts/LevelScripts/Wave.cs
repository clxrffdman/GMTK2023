using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
3 waves:
set of dudes come on
each of them bowl at different positions / angles with different modifiers
so, throwerWave would contain the info for that INDIVIDUAL thrower
and each wave would have a variable amount of thrower waves: usually one i guess? idfk
*/
[Serializable]
public class Wave
{
    public BowlerProfile profile;
    public List<ThrowerWave> throwerWaves = new List<ThrowerWave>();
    public int numPins;
    public float pinPosOffset = 0.5f;

    // init the thrower
    public IEnumerator StartWave() {
        LevelManager.Instance.hasFailedCurrentWave = false;
        GameplayUIManager.Instance.portraitController.LoadProfile(profile);
        yield return PlayerController.Instance.SpawnAnim();
        PlayerController.Instance.locked = true;
        CourseController.Instance.PlaceRandomPins(numPins, pinPosOffset);
        yield return new WaitForSeconds(1.3f);

        CameraController.Instance.SetCameraState(CameraController.CameraState.Bowler);
    
        yield return new WaitForSeconds(0.8f);

        foreach (ThrowerWave wave in throwerWaves) {
            Thrower thrower = CourseController.Instance.InitThrower(wave.thrower);
            thrower.InitBowl(wave);
            CourseController.Instance.ThrowBalls(thrower, wave);
            //yield return thrower.ThrowBall(wave.ball, wave.ballMods);
        }
        yield return new WaitUntil(DoneThrowing);
        PlayerController.Instance.locked = false;
        yield return new WaitUntil(() => CourseController.Instance.currentBalls.Count <= 0);
        Debug.Log("done with this wave");
        yield return new WaitForSeconds(1f);
        yield return CourseController.Instance.ClearInstances();
        // end wave in 1
        yield return new WaitForSeconds(2f);
        EndWave();
    }

    public void EndWave() {
        Debug.Log("wave over");
        PlayerController.Instance.anim.SetBool("Hit", false);
        
        GameplayUIManager.Instance.scorecardUIController.SetScore(LevelManager.Instance.currentWaveIndex, LevelManager.Instance.hasFailedCurrentWave);
        LevelManager.Instance.currentLevelWinCount += LevelManager.Instance.hasFailedCurrentWave ? 0 : 1;
    }
    public bool DoneThrowing() {
        foreach (Thrower thrower in CourseController.Instance.currentThrowers) {
            if (!thrower.doneThrowing) return false;
        }
        return true;
    }
}


[Serializable]
public class ThrowerWave {
    public GameObject thrower;
    public List<GameObject> balls;
    public BallModifier throwerMod;
    public float consecBallOffset = 0.35f;
    public float xOffset;
    public float bowlAngle;
    public List<BallModifier> ballMods;

    public List<BallModifier> ApplyThrowerMod() {
        List<BallModifier> newModList = new List<BallModifier>(ballMods);
        BallModifier modClone = throwerMod.Clone();
        if(modClone is ThrowBase)
        {
            ((ThrowBase)modClone).throwAngleRandomDelta = bowlAngle;
        }
        newModList.Add(modClone);
        
        return newModList;
    }
}

/*

*/

