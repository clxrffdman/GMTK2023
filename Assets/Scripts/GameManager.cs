using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
    public float baseSlowMoDuration = 0;
    public float slowMoDuration = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlowMotion();    
    }

    public void StartSlowMotion(float duration) {
        slowMoDuration = duration;
        baseSlowMoDuration = slowMoDuration;
    }

    public void UpdateSlowMotion() {
        if (slowMoDuration <= 0) return;
        slowMoDuration = Mathf.Max(slowMoDuration-Time.unscaledDeltaTime, 0f);
        Time.timeScale = Mathf.Max(1f-(slowMoDuration/baseSlowMoDuration), 0.2f);
    }

    
}
