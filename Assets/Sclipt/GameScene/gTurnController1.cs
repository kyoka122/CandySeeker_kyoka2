using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class gTurnController1 : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject clickController = null;
    [SerializeField] GameObject scripts = null;
    [SerializeField] GameObject myTurnStartAnimationObj = null;
    [SerializeField] GameObject clickedMarkObj = null;
    //[SerializeField] GameObject timeCounter = null;
    [SerializeField] SoundController soundController = null;

    gInstantiateController gInstantiateController;
    Gamemanager gamemanager;
    AnimationControllerscript animationControllerscript;
    PhotonView scriptsPhotonView;
    gUIController gUIController;

    private int movePlayerNum;

    bool turnStart;
    bool myTurnStart;
    bool nextTurnStart;
    private void Awake()
    {
        gamemanager = scripts.GetComponent<Gamemanager>();
        //photonView = scripts.GetComponent<PhotonView>();
        gInstantiateController = scripts.GetComponent<gInstantiateController>();
        animationControllerscript = scripts.GetComponent<AnimationControllerscript>();
        scriptsPhotonView = scripts.GetComponent<PhotonView>();
        gUIController = scripts.GetComponent<gUIController>();
    }

   

    [PunRPC]
    public void StartTurn1()
    {
        gamemanager.movePlayerJudge++;
        Debug.Log("movePlayerJudge" + gamemanager.movePlayerJudge);
        movePlayerNum = (gamemanager.movePlayerJudge-1) % gamemanager.startPlayerCount;

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

        if (PhotonNetwork.IsMasterClient)
        {
            if (!(PhotonNetwork.CurrentRoom.CustomProperties[movePlayerNum.ToString()] is Player))//これでいける？？
            {
                nextTurnStart = true;
            }

        }

        if (movePlayerNum == gamemanager.playerNum)//playerNumは各自で違う
        {
            myTurnStart = true;     
        }
        Debug.Log("!!!!!!!!!TurnStartAnimation:"+ "1"+turnStart+ "2"+myTurnStart +"3"+ nextTurnStart);
        animationControllerscript.TurnStartAnimationOn(turnStart, myTurnStart, nextTurnStart,false);

    }

    public void NextTurnStartJudge1()//１人だけ
    {
        scriptsPhotonView.RPC("NextMyTurnSet", RpcTarget.AllViaServer);
        Debug.Log("!!!!!!!!!!!!!!!!!!!nextTurnStart:" + nextTurnStart);   
        if (nextTurnStart)
        {
            photonView.RPC("StartJudge1", RpcTarget.AllViaServer);
        }
        if (myTurnStart)
        {
            OnPlayerMove();
        }

    }

    private void OnPlayerMove()
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!OnPlayerMove");
        Debug.Log("ClickControllerObj!!!!!true");
        clickController.SetActive(true);
        soundController.TimerSEOn();
        scriptsPhotonView.RPC("TimeCounterOn", RpcTarget.AllViaServer);//GameManager
    }


    public void OnTurnTimeEnds()//出番の人が通常モードでよびだす
    {
        if (clickController.activeSelf)
        {
            clickController.SetActive(false);
            clickedMarkObj.SetActive(false);
            Debug.Log("ClickControllerObj!!!!!false");
            if (PhotonNetwork.LocalPlayer.CustomProperties["piece"]is int pieceNum) 
            {
                pieceNum--;
                var playerproperties = new ExitGames.Client.Photon.Hashtable();
                playerproperties["piece"] = pieceNum;
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);
            }

            gInstantiateController.TimeOverDestroy1();

            StartJudge1();
        }
    }

    public void InstantiateObject(GameObject firstClickedObject,GameObject secondClickedObject)
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!Instantiate");
        clickController.SetActive(false);
        Debug.Log("ClickControllerObj!!!!!false");
        gInstantiateController.WorldObjectInstantiate(firstClickedObject, secondClickedObject);
        StartJudge1();
    }

    public void StartJudge1()//駒関係
    {
        scriptsPhotonView.RPC("TimeCounterOff", RpcTarget.AllViaServer);
        myTurnStartAnimationObj.SetActive(false);
        if (gamemanager.movePlayerJudge == gamemanager.startAllPieceNum)
        {
            scriptsPhotonView.RPC("MoveObjectTurnStartOn", RpcTarget.AllViaServer);
        }
        else
        {
            Debug.Log("else");
            photonView.RPC("StartTurn1", RpcTarget.AllViaServer);
        }
    }


}
