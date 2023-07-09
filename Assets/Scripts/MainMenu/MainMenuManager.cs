using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{


    public void LoadCircuit(int index)
    {
        SaveManager.Instance.forceCircuitPlay = true;
        SaveManager.Instance.circuitIndex = index;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


}
