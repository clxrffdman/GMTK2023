using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    // public List<Thrower> throwers = new List<Thrower>();
    // public List<Ball> balls = new List<Ball>();
    /*
    wave procedure:
        have a dude come up and throw the ball
        when the ball exits the area

    */
    [HideInInspector] public bool waveComplete = false;
    public IEnumerator StartWave() {
        /* 
        foreach (Thrower thrower in throwers) {
            
            balls.Add(thrower.ThrowBall());
        }
        yield return new WaitUntil(() => balls.Count <= 0)
        */
        Debug.Log("start wave");
        yield return new WaitForSeconds(3f);
        yield return EndWave();
    }

    public IEnumerator EndWave() {
        Debug.Log("wave over");
        yield return null;
    }
}
