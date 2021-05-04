using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAnimation : MonoBehaviour
{
    Gamemanager gamemanager;
    //ScoreSortScript scoreSortScript;

    [SerializeField] GameObject scripts = null;
    [SerializeField] SoundController soundController = null;

    private void Awake()
    {
        gamemanager = scripts.GetComponent<Gamemanager>();
        //scoreSortScript = scoreSortScriptObj.GetComponent<ScoreSortScript>();
    }
    public void FinishSE()
    {
        soundController.GameFinishSE();
    }

    public void GameFinishAnimationOff()
    {
        gamemanager.ScoreBoard();
    }


}
