using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public List<Image> medalDisplays = new List<Image>();
    public List<Sprite> medalSprites;

    public CanvasGroup promptGroup;
    public CanvasGroup buttonGroup;
    public bool hasPrompted = false;
    public GameObject hiddenCircuitButton;

    private void Start()
    {
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
                    return;
                }

                if (score >= 15)
                {
                    medalDisplays[i].sprite = medalSprites[2];
                    return;
                }

                if (score >= 13)
                {
                    medalDisplays[i].sprite = medalSprites[1];
                    return;
                }

                if (score >= 9)
                {
                    medalDisplays[i].sprite = medalSprites[0];
                    return;
                }
            }
            else if (i < 3)
            {
                showHidden = false;
            }

        }

        hiddenCircuitButton.SetActive(showHidden);
    }

    public void SetPromptAlpha(float alpha)
    {
        promptGroup.alpha = alpha;
    }

    public void MoveButtonX(float xVal)
    {
        buttonGroup.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(xVal, -654, 0);
    }

    public void Update()
    {
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
        LeanTween.value(this.gameObject, MoveButtonX, -450, 190, 0.8f);

    }

    public void LoadCircuit(int index)
    {
        SaveManager.Instance.forceCircuitPlay = true;
        SaveManager.Instance.circuitIndex = index;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


}
