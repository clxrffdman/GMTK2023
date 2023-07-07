using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int levelNumber;
    [SerializeField]
    public List<Wave> waves = new List<Wave>();
    // levels have: background? anything else?
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void InitLevel() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel() {
        InitLevel();
        StartCoroutine(StartWaves());
    }

    public IEnumerator EndLevel() {
        // do stuff when level ends
        Debug.Log("waves done!");
        yield return null;
    }

    public IEnumerator StartWaves() {
        // intro
        yield return new WaitForSeconds(1);

        // waves start
        foreach (Wave wave in waves) {
            yield return wave.StartWave();
        }

        // waves end
        yield return EndLevel();
    }
}
