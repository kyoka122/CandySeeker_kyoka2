using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject[] PlayerNum;

    //Pun監視つける
    private void Start()
    {
        for (int i = 0; i < 8; i++)
            PlayerNum[i].SetActive(true);





        //プレイヤーリスト
    }

    
    
    public void MyScore(int score)
    {



        AllScore();
    }

    //Pun監視つける
    public void AllScore()
    {

    }




}
