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
    [HideInInspector] public Thrower leadBowler;
    public List<ThrowerWave> throwerWaves = new List<ThrowerWave>();
    public int numPins;
    public float pinPosOffset = 0.5f;
    [HideInInspector] public bool waveDone = false;

    // init the thrower
    public IEnumerator StartWave() {
        waveDone = false;
        leadBowler = throwerWaves[0].thrower.GetComponent<Thrower>();
        GameManager.Instance.canPause = false;
        LevelManager.Instance.hasFailedCurrentWave = false;
        LevelManager.Instance.currentCourseState = CourseState.PreppingThrow;
        GameplayUIManager.Instance.scorecardUIController.SetSelected(LevelManager.Instance.currentWaveIndex);
        GameplayUIManager.Instance.portraitController.LoadProfile(leadBowler);
        GameplayUIManager.Instance.portraitController.LoadBallType(throwerWaves[0].balls[0].GetComponent<BallController>());
        Debug.Log("portraiting");
        GameplayUIManager.Instance.portraitController.RequestPortraitQuip();
        Debug.Log("spawn anim about");
        yield return PlayerController.Instance.SpawnAnim();
        PlayerController.Instance.locked = true;
        Debug.Log("begin placing pins");
        CourseController.Instance.PlaceRandomPins(numPins, pinPosOffset);
        yield return new WaitForSeconds(1.3f);

        foreach (ThrowerWave wave in throwerWaves) {
            Thrower thrower = CourseController.Instance.InitThrower(wave.thrower);
            thrower.InitBowl(wave);
            CourseController.Instance.ThrowBalls(thrower, wave);
            //yield return thrower.ThrowBall(wave.ball, wave.ballMods);
        }
        CameraController.Instance.SetCameraState(CameraController.CameraState.Bowler);

        //yield return new WaitUntil(DoneThrowing);
        yield return new WaitUntil(() => CameraController.Instance.currentCameraState == CameraController.CameraState.Player);
        GameManager.Instance.StartSlowMotion(2f);
        LevelManager.Instance.currentCourseState = CourseState.Throwing;
        GameplayUIManager.Instance.portraitController.RequestPortraitQuip();
        //CameraController.Instance.SetCameraState(CameraController.CameraState.Player);
        PlayerController.Instance.locked = false;
        yield return new WaitUntil(() => CourseController.Instance.currentBalls.Count <= 0);
        Debug.Log("done with this wave");
        waveDone = true;
        yield return new WaitForSeconds(1f);
        yield return CourseController.Instance.ClearInstances();
        // end wave in 1
        yield return new WaitForSeconds(0.7f);
        LevelManager.Instance.currentCourseState = LevelManager.Instance.hasFailedCurrentWave ? CourseState.RoundEndFail : CourseState.RoundEndSuccess;
        GameplayUIManager.Instance.portraitController.RequestPortraitQuip();
        GameManager.Instance.canPause = true;
        yield return new WaitForSeconds(2f);

        EndWave();
    }

    public void EndWave() {
        Debug.Log("wave over");
        PlayerController.Instance.anim.SetBool("Hit", false);
        
        GameplayUIManager.Instance.scorecardUIController.SetScore(LevelManager.Instance.currentWaveIndex, !LevelManager.Instance.hasFailedCurrentWave);
        LevelManager.Instance.currentLevelWinCount += LevelManager.Instance.hasFailedCurrentWave ? 0 : 1;
        LevelManager.Instance.currentCircuitWinCount += LevelManager.Instance.hasFailedCurrentWave ? 0 : 1;
        PlayerController.Instance.EndCharge();
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

    public List<BallModifier> ApplyThrowerMod(Thrower thrower) {
        List<BallModifier> newModList = new List<BallModifier>(ballMods);
        BallModifier modClone = throwerMod.Clone();
        if(modClone is ThrowBase)
        {
            ((ThrowBase)modClone).throwAngleRandomDelta = bowlAngle;
            if (((ThrowBase)modClone).applyToThrower) {
                thrower.currThrowMods.Add(((ThrowBase)modClone));
                //return newModList;
            }
        }
        newModList.Add(modClone);
        
        return newModList;
    }
}

/*

*/

