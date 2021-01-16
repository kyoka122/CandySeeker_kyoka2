using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    UIController uI;
    int score=0;
    private void Start()
    {
        uI = GetComponent<UIController>();
    }
    public void ScoreUpdate(string job_DesObj)
    {
        if (job_DesObj == "king")
        {
            score += 3;
        }
        else if (job_DesObj == "citizen")
        {
            score += 1;
        }

        uI.MyScore(score);


    }


}
