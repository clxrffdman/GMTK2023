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

    public List<Sprite> medalSprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int score)
    {
        medalText.text = score + "/" + LevelManager.Instance.currentCircuit.GetMaxFrames();

        if(score >= 15)
        {
            medalImg.sprite = medalSprites[3];
            return;
        }

        if (score >= 13)
        {
            medalImg.sprite = medalSprites[2];
            return;
        }

        if (score >= 11)
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
        SaveManager.Instance.circuitIndex = SaveManager.Instance.circuitIndex++;
        SaveManager.Instance.forceCircuitPlay = true;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void RestartCircuit()
    {
        SaveManager.Instance.forceCircuitPlay = true;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
