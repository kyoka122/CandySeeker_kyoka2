using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public class gTurnController2 : MonoBehaviourPunCallbacks
{
    Gamemanager gamemanager;
    ScoreController scoreController;
    //PhotonView photonView;
    AnimationControllerscript animationControllerscript;
    gInstantiateController gInstantiateController;
    PhotonView scriptsPhotonView;
    gUIController gUIController;
    gGameboardList gGameboardList;

    [SerializeField] GameObject clickController = null;
    [SerializeField] GameObject scripts = null;
    [SerializeField] GameObject myTurnStartAnimationObj = null;
    [SerializeField] GameObject particleObj = null;
    [SerializeField] GameObject clickedMarkObj = null;
    [SerializeField] GameObject destroyCheckObj = null;
    [SerializeField] SoundController soundController = null;

    //[SerializeField] Text TurnText;//ターン数の表示テキスト

    private int movePlayerNum;

    private bool turnStart;
    private bool myTurnStart;
    private bool nextTurnStart;
    private bool myPieceExtinct;
    private int turnController2MovePlayerNum;
    Player[] playerList;
    bool startJudge2Judge = true;//多分いらない
    private int particleNum=0;
    private List<GameObject> particleObjList=new List<GameObject>();
    string turncontroller2GameboardID;
    public void Awake()
    {
        scoreController = scripts.GetComponent<ScoreController>();
        gamemanager = scripts.GetComponent<Gamemanager>();
        //photonView = scripts.GetComponent<PhotonView>();//scriptsにphotonviewを付けておくのを忘れずに。
        animationControllerscript = scripts.GetComponent<AnimationControllerscript>();
        gInstantiateController = scripts.GetComponent<gInstantiateController>();
        scriptsPhotonView = scripts.GetComponent<PhotonView>();
        gUIController = scripts.GetComponent<gUIController>();
        gGameboardList = scripts.GetComponent<gGameboardList>();
        if (PhotonNetwork.CurrentRoom.CustomProperties["Stage"]is string ID)
        {
            Debug.Log("turncontroller2.GameboardID");
            turncontroller2GameboardID = ID;
        }
    }


    [PunRPC]
    public void StartTurn2()
    {
        gamemanager.movePlayerJudge++;
        Debug.Log("movePlayerJudge" + gamemanager.movePlayerJudge);
        movePlayerNum = (gamemanager.movePlayerJudge - 1) % gamemanager.startPlayerCount;
        gUIController.MyTurnObject(movePlayerNum);

        if (movePlayerNum == 0)
        {
            turnStart = true;
        }
        else
        {
            turnStart = false;
        }


        myTurnStart = false;
        nextTurnStart = false;
        myPieceExtinct = false;
        //2つのifの内両方には入らない
        if (PhotonNetwork.IsMasterClient)
        {
            if (!(PhotonNetwork.CurrentRoom.CustomProperties[movePlayerNum.ToString()] is Player))//これでいける？？
            {
                nextTurnStart = true;
            }
        }
        //2つめ
        if (movePlayerNum == gamemanager.playerNum)//playerNumは各自で違うb
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties["piece"] is int piecenum)
            {
                
                if (piecenum != 0)
                {
                    myTurnStart = true;
                }
                else
                {
                    myPieceExtinct = true;
                }
 
            }
        }
        Debug.Log("myPieceExtinct:" + myPieceExtinct);
        animationControllerscript.TurnStartAnimationOn(turnStart, myTurnStart, nextTurnStart, myPieceExtinct);
        startJudge2Judge = true;//多分いらない
        Debug.Log("movePlayerNum1:" + movePlayerNum);
        Debug.Log(" gamemanager.playerNum1:" + gamemanager.playerNum);
    }

    public void NextTurnStartJudge2()
    {
        scriptsPhotonView.RPC("NextMyTurnSet", RpcTarget.AllViaServer);
        Debug.Log("myPieceExtinct"+ myPieceExtinct);
        Debug.Log("!!!!!!!!!!!!!!!!!!!nextTurnStart:" + nextTurnStart);
        if (nextTurnStart)
        {
            Debug.Log("nextTurnStart");
            StartJudge2Judge();
        }
        else if (myPieceExtinct)
        {
            Debug.Log("MyPieceNone.NextTurnStart");
            StartJudge2Judge();
        }
        else if (myTurnStart)
        {
            Debug.Log("myTurnStart");
            OnPlayerMove();
        }
      

    }

    private void OnPlayerMove()
    {
        turnController2MovePlayerNum = movePlayerNum;
        clickController.SetActive(true);
        particleNum = 0;
        Debug.Log("ClickControllerObj!!!!!true");
        soundController.TimerSEOn();
        scriptsPhotonView.RPC("TimeCounterOn", RpcTarget.AllViaServer);//GameManager
    }



    public void OnTurnTimeEnds()//出番の人が通常モードでよびだす
    {
        if (clickController.activeSelf)
        {
            Debug.Log("ClickControllerObj!!!!!false");
            clickController.SetActive(false);
            clickedMarkObj.SetActive(false);
            MyTurnLimitDown();
            StartJudge2Judge();
        }
        else
        {
            Debug.Log("clickController.activeSelf==false");
        }
    }

    public void MyTurnLimitDown()
    {
        clickController.SetActive(false);
        Debug.Log("ClickControllerObj!!!!!false");
        var playerproperties = new ExitGames.Client.Photon.Hashtable();
        if (PhotonNetwork.LocalPlayer.CustomProperties["turn"] is int turn)
        {
            turn--;
            playerproperties["turn"] = turn;
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);

    }

    [PunRPC]
    public void DestroyObject(string job, float posx, float posy,int destroyObjOwnernum)
    {
        //scoreController.ScoreUpdate(job);//StartJudge2()はDestroyJudgeで。
        if (particleObjList.Count>=particleNum)
        {
            Debug.Log("particleNum"+ particleNum);
            Debug.Log("particleObjList.Count"+ particleObjList.Count);
            particleObjList.Add(Instantiate(particleObj));
            gGameboardList.ParticleSetParent(turncontroller2GameboardID,particleObjList[particleNum]);

        }
        particleObjList[particleNum].transform.localPosition = new Vector3(posx, posy, 120);

        particleObjList[particleNum].GetComponent<gDestroyEffectController>().DestroyObjectEffect();//Destroy()エフェクト？
        particleNum++;
        soundController.DestroyEffectSE();
        Debug.Log("movePlayerNum2:"+ movePlayerNum);
        Debug.Log("gamemanager.playerNum2:" + gamemanager.playerNum);
        Debug.Log("turnController2MovePlayerNum: " + turnController2MovePlayerNum);
        if (movePlayerNum == gamemanager.playerNum)
        {
            Debug.Log("PieceUpdate!!!!!!!!!!!!!!!");
            
            var playerproperties = new ExitGames.Client.Photon.Hashtable();
            if (PhotonNetwork.CurrentRoom.CustomProperties["Turn"] is int turn)
            {
                playerproperties["turn"] = turn;
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);

            if (PhotonNetwork.CurrentRoom.CustomProperties[destroyObjOwnernum.ToString()]is Player player)
            {
                scoreController.OtherPieceUpdate(player);
            }



            scoreController.ScoreUpdate(job);//StartJudge2()はDestroyJudgeで。/////////////////////////////////////////////////////
            //StartJudge2Judge();
        }


    }

    public void StartJudge2Judge()//多分いらない
    {
        destroyCheckObj.SetActive(false);
        Debug.Log("StartJudge2JudgeOnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");//1回
        if (startJudge2Judge)
        {
            Debug.Log("startJudge2Judge=true");
            startJudge2Judge = false;
            StartJudge2();
        }     
    }

    public void StartJudge2()//そのターンの人しか通らない
    {
     
        Debug.Log("StartJudge22222222222222222222222222");
        destroyCheckObj.SetActive(false);
        scriptsPhotonView.RPC("TimeCounterOff", RpcTarget.AllViaServer);
        clickController.SetActive(false);
        Debug.Log("ClickControllerObj!!false");
        myTurnStartAnimationObj.SetActive(false);
 
        Debug.Log("StartTurn2On");
        photonView.RPC("StartTurn2", RpcTarget.AllViaServer);
        Debug.Log("StartJudgeJudgeJudge");
        scoreController.PieceSet();
    }


    public void TurnOverJudge()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["turn"] is int myTurn)
        {
            if (myTurn == -1)
            {
                var playerproperties = new ExitGames.Client.Photon.Hashtable();
                if (PhotonNetwork.CurrentRoom.CustomProperties["Turn"] is int Turn)
                {
                    playerproperties["turn"] = Turn;
                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);
                gInstantiateController.TurnOverDestroy2();
            }
        }
    }
    public void PieceLimitJudge()
    {
        // 駒が各種１個以下になったら
        bool piececheck = true;
        bool secondpiececheck = true;

        playerList = PhotonNetwork.PlayerList;
        for (int i = 0; i <PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (playerList[i].CustomProperties["piece"] is int pieceNum)
            {
                if (1 < pieceNum && piececheck)
                {
                    piececheck = false;

                }
                else if (0 < pieceNum && secondpiececheck)
                {
                    secondpiececheck = false;
                }

                if (i == gamemanager.startPlayerCount - 1)//…
                {
                    if (piececheck || secondpiececheck) //両方falseを通った場合入らない                       
                    {
                        Debug.Log("GamefinishOn");
                        scriptsPhotonView.RPC("Gamefinish", RpcTarget.AllViaServer);
                        return;
                    }
                }



            }



        }
    }
}
