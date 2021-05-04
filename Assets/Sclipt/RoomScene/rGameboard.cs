using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.PlayerLoop;

public class rGameboard : MonoBehaviourPunCallbacks
{
    rUIController rUIController;
    //PhotonView photonView;
   
    [SerializeField] Image selectgameboard=null;
 
    [SerializeField] Sprite[] selectedgameboardAver = new Sprite[2];//リストにする？
    [SerializeField] Sprite[] selectedgameboardBver = new Sprite[3];
    [SerializeField] SoundController soundController = null;
    [SerializeField] Text gameboardtext=null;
    public int gameboardMaxPiece;

    [System.NonSerialized]public string gameboardID;

    public void FirstGameboardSet()
    {
        rUIController = GetComponent<rUIController>();
        //photonView = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties["Stage"] is string ID) //応急処置
            {
                Debug.Log("Rgameboardjudge!!!!!" + ID);
                switch (ID)
                {
                    case "A1":
                        GameboardA1ButtonOn();
                        break;
                    case "A2":
                        GameboardA2ButtonOn();
                        break;
                    case "B1":
                        GameboardB1ButtonOn();
                        break;
                    case "B2":
                        GameboardB2ButtonOn();
                        break;
                    case "B3":
                        GameboardB3ButtonOn();
                        break;
                    default:
                        GameboardA1ButtonOn();
                        break;
                }

            }
            else
            {
                Debug.Log("rGameboard:Stage NotFound");
                GameboardA1ButtonOn();
            }
        }
        

        GameboardListUpdate();
    }



    //ゲームボード切り替え//////////////////////////////
    //少人数用

    public void GameboardA1ButtonOn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            soundController.ClickSE();
            gameboardID = "A1";
            gameboardMaxPiece = 33;
            SetGameboardProperties();
            rUIController.GameStartJudge1();
        }

    }



    public void GameboardA2ButtonOn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            soundController.ClickSE();
            gameboardID = "A2";
            gameboardMaxPiece = 32;
            SetGameboardProperties();
            rUIController.GameStartJudge1();
        }

    }


    //大人数用
    public void GameboardB1ButtonOn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            soundController.ClickSE();
            gameboardID = "B1";
            gameboardMaxPiece = 45;
            SetGameboardProperties();
            rUIController.GameStartJudge1();
        }
    }

    public void GameboardB2ButtonOn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            soundController.ClickSE();
            gameboardID = "B2";
            gameboardMaxPiece = 49;
            SetGameboardProperties();
            rUIController.GameStartJudge1();
        }

    }

    public void GameboardB3ButtonOn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            soundController.ClickSE();
            gameboardID = "B3";
            gameboardMaxPiece = 46;
            SetGameboardProperties();
            rUIController.GameStartJudge1();
        }

    }


    private void SetGameboardProperties()
    {
        var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
        roomproperties["Stage"] = gameboardID;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);
    }




    //RPCで全員に適応
    [PunRPC]
    public void GameboardListUpdate()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties["Stage"] is string ID)
        {
            switch (ID)
            {
                case "A1":
                    GameboardImageChangeA(0, "終点");
                    break;
                case "A2":
                    GameboardImageChangeA(1, "20RD");
                    break;
                case "B1":
                    GameboardImageChangeB(0, "大戦場");
                    break;
                case "B2":
                    GameboardImageChangeB(1, "ダイヤ");
                    break;
                case "B3":
                    GameboardImageChangeB(2, "star");
                    break;
                default:
                    Debug.Log("Stage NotFound");
                    break;
            }
            Debug.Log("GameboardListUpdate");
        }
    }


    public void GameboardImageChangeA(int gameboardnum,string name)
    {
        Debug.Log(name.ToString());
        Debug.Log(gameboardnum);

        selectgameboard.sprite = selectedgameboardAver[gameboardnum];
        gameboardtext.text = name;

    }

    public void GameboardImageChangeB(int gameboardnum, string name)
    {
        selectgameboard.sprite = selectedgameboardBver[gameboardnum];
        gameboardtext.text = name;

    }

    


   
}
