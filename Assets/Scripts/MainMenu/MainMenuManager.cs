using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;
using TMPro;


public class MainMenuManager : MonoBehaviour
{

    public List<Image> medalDisplays = new List<Image>();
    public List<Sprite> medalSprites;
    public CanvasGroup transitionPanel;

    public EventInstance menuMusic;
    [SerializeField] private EventReference menuMusicReference;

    public CanvasGroup promptGroup;
    public CanvasGroup buttonGroup;
    public GameObject logo;
    public bool hasPrompted = false;
    public GameObject hiddenCircuitButton;

    private void Start()
    {
        menuMusic = RuntimeManager.CreateInstance(menuMusicReference);
        menuMusic.start();

        Time.timeScale = 1;
        promptGroup.alpha = 1;

        LeanTween.value(this.gameObject, SetPromptAlpha, 0, 1, 0.8f).setLoopPingPong();

        bool showHidden = true;

        for(int i = 0; i < medalDisplays.Count; i++)
        {
            medalDisplays[i].color = Color.clear;
            if (SaveManager.Instance.scoreDictionary.ContainsKey(i))
            {
                int score = SaveManager.Instance.scoreDictionary[i];

                medalDisplays[i].color = Color.white;

                if (score >= 18)
                {
                    medalDisplays[i].sprite = medalSprites[3];
                    continue;
                }

                if (score >= 15)
                {
                    medalDisplays[i].sprite = medalSprites[2];
                    continue;
                }

                if (score >= 13)
                {
                    medalDisplays[i].sprite = medalSprites[1];
                    continue;
                }

                if (score >= 9)
                {
                    medalDisplays[i].sprite = medalSprites[0];
                    continue;
                }
            }
            else if (i < 3)
            {
                showHidden = false;
            }

        }

        if (showHidden)
        {
            hiddenCircuitButton.GetComponent<Button>().interactable = true;
            hiddenCircuitButton.GetComponent<Image>().color = Color.white;
            hiddenCircuitButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        else
        {
            hiddenCircuitButton.GetComponent<Button>().interactable = false;
            hiddenCircuitButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            hiddenCircuitButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1,1,1,0.5f);
        }
    }

    public void SetPromptAlpha(float alpha)
    {
        promptGroup.alpha = alpha;
    }

    public void MoveButtonX(float xVal)
    {
        buttonGroup.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(xVal, -724, 0);
    }

    public void MoveLogoY(float yVal)
    {
        logo.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(530, yVal, 0);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            // endless mode baby
        }
        if (Input.anyKeyDown && !hasPrompted)
        {
            hasPrompted = true;
            LeanTween.cancel(this.gameObject);
            StartCoroutine(TweenRoutine());
        }
    }


    public IEnumerator TweenRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        LeanTween.alphaCanvas(promptGroup, 0, 0.2f);
        LeanTween.value(this.gameObject, MoveButtonX, -450, 190, 0.8f).setEaseOutElastic();
        LeanTween.value(this.gameObject, MoveLogoY, 260, -238, 0.8f).setEaseOutElastic();

        yield return new WaitForSeconds(0.85f);

        LeanTween.value(this.gameObject, MoveLogoY, -238, -251, 0.8f).setEaseInOutQuad().setLoopPingPong();

    }

    public void LoadCircuit(int index)
    {
        StartCoroutine(FadeToBlack(index));
        /*
        SaveManager.Instance.forceCircuitPlay = true;
        SaveManager.Instance.circuitIndex = index;
        menuMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        menuMusic.release();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        */
    }
    public IEnumerator FadeToBlack(int index) {
        transitionPanel.blocksRaycasts = true;
        LeanTween.alphaCanvas(transitionPanel, 1f, 1.4f);
        yield return new WaitForSeconds(1.6f);
        SaveManager.Instance.forceCircuitPlay = true;
        SaveManager.Instance.circuitIndex = index;
        menuMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        menuMusic.release();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        
    }

}
