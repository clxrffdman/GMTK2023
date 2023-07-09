using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public List<Image> medalDisplays = new List<Image>();
    public List<Sprite> medalSprites;

    public GameObject hiddenCircuitButton;

    private void Start()
    {
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
            else if (i != 3)
            {
                showHidden = false;
            }

        }

        hiddenCircuitButton.SetActive(showHidden);
    }

    public void LoadCircuit(int index)
    {
        SaveManager.Instance.forceCircuitPlay = true;
        SaveManager.Instance.circuitIndex = index;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


}
