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
    public List<ThrowerWave> throwerWaves = new List<ThrowerWave>();
    public List<Transform> pinPositions = new List<Transform>();

    // init the thrower
    public IEnumerator StartWave() {
        LevelManager.Instance.hasFailedCurrentWave = false;
        CameraController.Instance.SetCameraState(CameraController.CameraState.Bowler);

        yield return new WaitForSeconds(0.8f);

        foreach (ThrowerWave wave in throwerWaves) {
            Thrower thrower = CourseController.Instance.InitThrower(wave.thrower);
            thrower.InitBowl(wave);
            CourseController.Instance.ThrowBalls(thrower, wave);
            //yield return thrower.ThrowBall(wave.ball, wave.ballMods);
        }

        yield return new WaitUntil(DoneThrowing);
        


        Debug.Log("done throwing baby");
        yield return new WaitUntil(() => CourseController.Instance.currentBalls.Count <= 0);
        Debug.Log("done with this wave");
        // end way in 3
        yield return new WaitForSeconds(1f);
        yield return EndWave();
    }

    public IEnumerator EndWave() {
        Debug.Log("wave over");
        CourseController.Instance.ClearThrowers();

        GameplayUIManager.Instance.scorecardUIController.SetScore(LevelManager.Instance.currentWaveIndex, LevelManager.Instance.hasFailedCurrentWave);

        yield return null;
    }
    public bool DoneThrowing() {
        foreach (Thrower thrower in  CourseController.Instance.currentThrowers) {
            if (!thrower.doneThrowing) return false;
        }
        return true;
    }
}


[Serializable]
public class ThrowerWave {
    public GameObject thrower;
    public GameObject ball;
    public BallModifier throwerMod;
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

