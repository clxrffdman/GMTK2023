using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Wave
{
    public List<Thrower> throwers = new List<Thrower>();
    /*
    wave procedure:
        have a dude come up and throw the ball
        when the ball exits the area
    */
    [HideInInspector] public bool waveComplete = false;
    public IEnumerator StartWave() {
        foreach (Thrower thrower in throwers) {
            CourseController.Instance.currentThrowers.Add(thrower);
            thrower.ThrowBall();
        }
        // wait for all balls to be gone
        yield return new WaitUntil(() => CourseController.Instance.currentBalls.Count <= 0);
        // end way in 3
        yield return new WaitForSeconds(3f);
        yield return EndWave();
    }

    public IEnumerator EndWave() {
        Debug.Log("wave over");
        CourseController.Instance.ClearThrowers();
        
        yield return null;
    }
}
