using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : UnitySingleton<SaveManager>
{
    public int circuitIndex = 0;
    public bool forceCircuitPlay = false;

    public Dictionary<int, int> scoreDictionary = new Dictionary<int, int>() { };

    public override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        base.Awake();

        DontDestroyOnLoad(this.gameObject);
    }

    

}
