using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreResultUIController : MonoBehaviour
{
    [SerializeField] private Image medalImg;
    [SerializeField] private TextMeshProUGUI medalText;
    [SerializeField] private GameObject nextCircuitButton;

    public List<Sprite> medalSprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableNextCircuitButton()
    {
        nextCircuitButton.SetActive(false);
    }

    public void SetScore(int score)
    {
        medalText.text = score + "/" + LevelManager.Instance.currentCircuit.GetMaxFrames();

        if(score >= 18)
        {
            medalImg.sprite = medalSprites[3];
            return;
        }

        if (score >= 15)
        {
            medalImg.sprite = medalSprites[2];
            return;
        }

        if (score >= 13)
        {
            medalImg.sprite = medalSprites[1];
            return;
        }

        if (score >= 9)
        {
            medalImg.sprite = medalSprites[0];
            return;
        }
    }

    public void ContinueToNextCircuit()
    {
        SaveManager.Instance.circuitIndex++;
        SaveManager.Instance.forceCircuitPlay = true;
        StartCoroutine(ToScene(1));
        //SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void RestartCircuit()
    {
        SaveManager.Instance.forceCircuitPlay = true;
        StartCoroutine(ToScene(1));
        //SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        StartCoroutine(ToScene(0));
    }
    public IEnumerator ToScene(int scene) {
        yield return (GameplayUIManager.Instance.transitionPanelController.FadeToBlack(1.5f, 0.23f, false));
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
