using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkGameManager : MonoBehaviourPunCallbacks
{
    Text inputfield;
    public GameObject ConnectingPanel;
    //ゲームバージョン指定（設定しないと警告が出る）
    string GameVersion = "Ver1.0";

    //ルームオプションのプロパティー
    static RoomOptions RoomOPS = new RoomOptions()
    {
        MaxPlayers = 2, //0だと人数制限なし
        IsOpen = true, //部屋に参加できるか
        IsVisible = true, //この部屋がロビーにリストされるか
    };

    public TextController textController;
    public static bool startGame=false;
    public Text ownerNameText;
    public Text otherNameText;
    public string ownerName;
    public string otherName;
    private PhotonView PV;

    // Use this for initialization
    void Start()
    {
        PV = GetComponent<PhotonView>();
        
        //PhotonCloudに接続
        Debug.Log("PhotonLoing");
        //ゲームバージョン設定
        PhotonNetwork.GameVersion = GameVersion;
        //PhotonServerSettingsファイルで構成されたPhotonに接続。
        PhotonNetwork.ConnectUsingSettings();
        
    }

    //クライアントがマスターサーバーに接続されたときに呼び出される。
    public override void OnConnectedToMaster()
    {

        //ルームへの参加　or 新規作成
        PhotonNetwork.JoinOrCreateRoom("Photonroom", RoomOPS, null); //("ルームの名前",ルームオプションの変数,新規ルームを一覧したいロビー。nullで無視)

    }

    //ルーム作成して入室に成功したときに呼び出される。
    public override void OnJoinedRoom()
    {
        //Room型とPlayer型の指定。
        Room myroom = PhotonNetwork.CurrentRoom;　//myroom変数にPhotonnetworkの部屋の現在状況を入れる。
        Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;　//playerをphotonnetworkのローカルプレイヤーとする
        Debug.Log("ルーム名:" + myroom.Name);
        Debug.Log("PlayerNo" + player.ActorNumber);
        Debug.Log("プレイヤーID" + player.UserId);

        


        if (player.ActorNumber == 1)
        {
            Debug.Log("プレイヤー1");
            Debug.Log("ルームマスター" + player.IsMasterClient);
            Debug.Log("名前：" + player.NickName);
            //ルームマスターならTrur。最初に部屋を作成した場合は、基本的にルームマスターなはず。
            ownerName = player.NickName;
            ownerNameText.text = ownerName;
        }


        if (player.ActorNumber == 2)
        {
            Debug.Log("プレイヤー2");
            Debug.Log("名前：" + player.NickName);
            otherName = player.NickName;
            otherNameText.text = otherName;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            ownerNameText.text = PhotonNetwork.MasterClient.NickName;
            ConnectingPanel.SetActive(false);//conecttingを消す
            Debug.Log("Connect");
            PhotonNetwork.CurrentRoom.IsOpen = false;
            startGame = true;
            textController.StartAnimation();
        }
        else
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            ConnectingPanel.SetActive(true);
            startGame = false;
        }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "が参加しました");
        otherNameText.text = newPlayer.NickName;
        ConnectingPanel.SetActive(false);//conecttingを消す
        Debug.Log("Connect");
        PhotonNetwork.CurrentRoom.IsOpen = false;
        startGame = true;
        textController.StartAnimation();
    }

    public void CancelButtonOn()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }


    public void GameSign()
    {
        BlockGenerator_2 BlockGene = GetComponent<BlockGenerator_2>();
        //BlockGene.GameStart();

    }

    //入室失敗したときに呼び出される動作。
    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        Debug.Log("入室失敗");
        //ルームを作成する。
        PhotonNetwork.CreateRoom(null, RoomOPS); //JoinOrCreateroomと同じ引数が使用可能。nullはルーム名を作成したくない場合roomNameを勝手に割り当てる。
    }

    //ルーム作成失敗したときの動作。
    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        Debug.Log("作成失敗");

    }

    [PunRPC]
    public void ShowPlayerName(string name)
    {
        if (PV.IsMine)
        {
            otherNameText.text = name;
        }
        else
        {
            ownerNameText.text = name;
        }
        
    }
}

