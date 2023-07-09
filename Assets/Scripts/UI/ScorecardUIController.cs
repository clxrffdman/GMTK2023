using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorecardUIController : MonoBehaviour
{
    public GameObject scorecardElementPrefab;
    public Transform scorecardElementParent;

    public List<ScorecardElement> scorecardElements = new List<ScorecardElement>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int index, bool isWin)
    {
        scorecardElements[index].SetScore(isWin);
    }

    public void SetSelected(int index)
    {
        for(int i = 0; i < scorecardElements.Count; i++)
        {
            if (i == index)
            {
                scorecardElements[i].SetSelected(true);
            }
            else
            {
                scorecardElements[i].SetSelected(false);
            }
        }
        
    }

    public void ClearElements()
    {
        for(int i = scorecardElements.Count-1; i >= 0; i--)
        {
            Destroy(scorecardElements[i].gameObject);
        }

        scorecardElements.Clear();
    }

    public void InitScorecard(Level level)
    {
        for(int i = 0; i < level.waves.Count; i++)
        {
            var newCardElement = Instantiate(scorecardElementPrefab, scorecardElementParent);
            scorecardElements.Add(newCardElement.GetComponent<ScorecardElement>());
            newCardElement.GetComponent<ScorecardElement>().indexText.text = (i + 1).ToString();
        }
    }
}
