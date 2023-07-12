using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
    [Header("Current Game State")]
    public bool isPaused = false;
    public bool hasLost = false;
    public bool canPause = true;
    public float baseSlowMoDuration = 0;
    public float slowMoDuration = 0;
    [HideInInspector] public float toTimescale = 1f;

    public SpriteRenderer bgSr;
    public List<Sprite> bgList = new List<Sprite>();

    public Stack<GameObject> pauseUIPanels = new Stack<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        bgSr.sprite = bgList[SaveManager.Instance.circuitIndex];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isPaused)
        {
            UpdateSlowMotion();
        }
         

    }

    public void StartSlowMotion(float duration, float to=1f) {
        slowMoDuration = duration;
        baseSlowMoDuration = slowMoDuration;
        toTimescale = to;
    }

    public void UpdateSlowMotion() {
        if (slowMoDuration <= 0) return;
        slowMoDuration = Mathf.Max(slowMoDuration-Time.unscaledDeltaTime, 0f);
        Time.timeScale = Mathf.Max(toTimescale-(slowMoDuration/baseSlowMoDuration), 0.2f);
        Time.fixedDeltaTime = 0.01666667f * Time.timeScale;

        if (Time.timeScale >= 1f)
        {
            toTimescale = 1f;
            Time.fixedDeltaTime = 0.01666667f;
        } 
    }

    public void TogglePanel(GameObject panel, bool state)
    {
        panel.SetActive(state);
    }

    public void TogglePause(bool shouldPause)
    {
        if (hasLost || !canPause)
        {
            return;
        }

        if (shouldPause)
        {
            Time.timeScale = 0;
            isPaused = true;


            if (pauseUIPanels.Count == 0)
            {
                pauseUIPanels.Push(GameplayUIManager.Instance.pausePanel);
                TogglePanel(GameplayUIManager.Instance.pausePanel, true);
            }
            else
            {

                TogglePanel(pauseUIPanels.Peek(), true);
            }

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isPaused", 1.0f);
            //ambianceEventInstance.setPaused(true);
        }
        else
        {

            if (pauseUIPanels.Count > 0)
            {
                TogglePanel(pauseUIPanels.Pop(), false);
            }

            if (pauseUIPanels.Count >= 1)
            {
                Debug.Log("Panels: " + pauseUIPanels.Peek().name);
                return;
            }

            isPaused = false;
            Time.timeScale = 1;

            //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isPaused", 0.0f);
        }

    }


}
