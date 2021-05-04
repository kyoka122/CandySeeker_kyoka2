using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class Gamemanager :MonoBehaviourPunCallbacks
{
    gUIController gUIController;
    gNetworkGameManager gNetworkGameManager;
    gGameboardList gGamebordList;
    gAnimationController gAnimationController;
    gInstantiateController gInstantiateController;
    WaitSetProperties waitSetProperties;
    gTurnController1 turnController1;
    gTurnController2 turnController2;
    DestroyJudge destroyJudge;
    //ScoreController scoreController;
    ScoreSortScript scoreSortScript;

    public int startPlayerCount;
    public int movePlayerJudge = 0;//増えてく
    public int playerNum;//playerNumは各自で違う
    public int startPlayerPieceNum;
    public int startAllPieceNum;
    int[] scoreSort;

    [SerializeField] GameObject turnController1Obj = null;
    [SerializeField] GameObject turnController2Obj = null;
    [SerializeField] GameObject gScoreFilterClickObj = null;
    [SerializeField] GameObject timeCounterObj = null;
    [SerializeField] GameObject gameStartAnimationObj = null;
    [SerializeField] GameObject waitSetPropertiesObj = null;
    [SerializeField] GameObject putObjectStartAnimationObj = null;
    [SerializeField] GameObject finishAnimationObj = null;
    [SerializeField] GameObject clickControllerObj = null;
    [SerializeField] GameObject scoreSortScriptObj = null;
    [SerializeField] SoundController soundController = null;

    private void Awake()
    {
        startPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        scoreSortScriptObj.SetActive(true);
        gNetworkGameManager = GetComponent<gNetworkGameManager>();
        gUIController = GetComponent<gUIController>();
        gGamebordList = GetComponent<gGameboardList>();
        gAnimationController = GetComponent<gAnimationController>();
        gInstantiateController = GetComponent<gInstantiateController>();
        waitSetProperties = waitSetPropertiesObj.GetComponent<WaitSetProperties>();
        turnController1 = turnController1Obj.GetComponent<gTurnController1>();
        turnController2 = turnController2Obj.GetComponent<gTurnController2>();
        //photonViewTurn1 = turnController1Obj.GetComponent<PhotonView>();
        destroyJudge = GetComponent<DestroyJudge>();
        //scoreController = GetComponent<ScoreController>();
        scoreSortScript = scoreSortScriptObj.GetComponent<ScoreSortScript>();
        //photonViewTurn2 = turnController2Obj.GetComponent<PhotonView>();

    }
    private void Start()
    {
        waitSetProperties.PropertiesJudge();
    }
    public void SetGameScene()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["num"] is int num)
        {
            playerNum = num;
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties["Piece"] is int piecenum)
        {
            startPlayerPieceNum = piecenum;
            //scoreController.ScoreSet();
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties["Stage"] is string ID)
        {
            Debug.Log("gameboardjudge!!!!!"+ID);
            gGamebordList.Gameboard(ID);
            destroyJudge.ColliderSizeSet(ID);
        }
        else
        {
            Debug.Log("gameboardID NotFound!!!!!");
        }

        startAllPieceNum = startPlayerPieceNum * startPlayerCount;
        gUIController.PlayerList();
        gInstantiateController.LocalObjectInstantiate();

        var playerproperties = new ExitGames.Client.Photon.Hashtable();
        playerproperties["set"] = true;
        playerproperties["set1"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);
        //PhotonNetwork.IsMessageQueueRunning=true;

    }



    [PunRPC]
    private void GameStartAnimation()
    {
        Debug.Log("GameStartAnimation");
        waitSetPropertiesObj.SetActive(false);
        gAnimationController.GameStartAnimationOn();
        
    }

    public void PutObject()
    {
        gameStartAnimationObj.SetActive(false);
        Debug.Log("PutStart!!!!");
        turnController1.StartTurn1();
    }

    [PunRPC]
    public void MoveObject()
    {
        putObjectStartAnimationObj.SetActive(false);
        turnController2Obj.SetActive(true);
        turnController1Obj.SetActive(false);//いる
        turnController2.StartTurn2();
    }


    [PunRPC]
    public void TimeCounterOn()
    {
        timeCounterObj.SetActive(true);
    }

    [PunRPC]
    public void TimeCounterOff()
    {
        timeCounterObj.SetActive(false);
        soundController.TimerSEOff();
        photonView.RPC("TimeReset", RpcTarget.AllViaServer);
        
    }



    [PunRPC]
    public void Gamefinish()
    {
        turnController1Obj.SetActive(false);//いらない
        turnController2Obj.SetActive(false);
        Debug.Log("ClickControllerObj!!!!!false");
        clickControllerObj.SetActive(false);
        timeCounterObj.SetActive(false);//RPCで遅れてくる
        soundController.TimerSEOff();


        if (PhotonNetwork.IsMasterClient)
        {
            gNetworkGameManager.FinishRoomSettings();      
        }

        gAnimationController.GameFinishAnimationOn();

    }

    public void ScoreBoard()
    {
        //Playerのnumと同じ配列の要素番号にscoreの値を入れる
        finishAnimationObj.SetActive(false);
        scoreSort = new int[startPlayerCount];
        for (int i = 0; i < startPlayerCount; i++)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties[i.ToString()] is Player player)//ifでいい。
            {
                if (player.CustomProperties["score"] is int score)
                {
                    if (player.CustomProperties["piece"] is int pieceNum)
                    {
                        scoreSort[i] = score + pieceNum;
                        Debug.Log(player.NickName+":[score]:" + score);
                        Debug.Log(player.NickName + ":[piece]:" + pieceNum);
                    }
                   
                }
            }
            else
            {
                scoreSort[i] = 0;
                Debug.Log("scoreSort[i] .else");
            }

            Debug.Log("scoreSort[i] " + scoreSort[i]);
        }
        //scoreSortScriptObj.SetActive(true);
        gUIController.ScoreListPosition();
        gScoreFilterClickObj.SetActive(true);
        scoreSortScript.PlayerScoreSort(scoreSort);

    }













    //いるけどいらない

    [PunRPC]
    public void GameSettingListUpdate()
    {
        return;
    }


    [PunRPC]
    public void GameboardListUpdate()
    {
        return;
    }
}



    


