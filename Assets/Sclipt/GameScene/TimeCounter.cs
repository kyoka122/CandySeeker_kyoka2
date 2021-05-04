using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] Slider TimerFillSlider=null;//タイマーの赤い部分
    [SerializeField] Text timeText=null;//残り時間の表示テキスト
    [SerializeField] GameObject gTurnController1Obj=null;
    [SerializeField] GameObject gTurnController2Obj=null;
    //[SerializeField] GameObject scripts=null;

    gTurnController1 gTurnController1;
    gTurnController2 gTurnController2;

    float countdown;
    float timeLimit;
    bool first=true;
    private void Awake()
    {
        gTurnController1=gTurnController1Obj.GetComponent<gTurnController1>();
        gTurnController2 = gTurnController2Obj.GetComponent<gTurnController2>();

        if (PhotonNetwork.CurrentRoom.CustomProperties["Time"] is int time) 
        {
            timeLimit = time;
        }
        TimerFillSlider.maxValue = timeLimit;
    }
    private void OnEnable()
    {
        countdown = timeLimit;
        first = true;
    }


    // Update is called once per frame
    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown>=0) 
        {
            timeText.text = countdown.ToString("f1") + "秒";
            TimerFillSlider.value = countdown;
        }
        if (countdown<0&& first)
        {

            Debug.Log("OnTurnTimeEndsJudge");
            if (gTurnController1Obj.activeSelf)
            {
                Debug.Log("OnTurnTimeEnds");
                gTurnController1.OnTurnTimeEnds();
            }
            else
            {
                Debug.Log("OnTurnTimeEnds");
                gTurnController2.OnTurnTimeEnds();
            }
            first = false;
        }


    }

}