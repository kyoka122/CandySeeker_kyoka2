using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class rNetworkGameManager : MonoBehaviourPunCallbacks
{
    public int maxPlayer = 8;//rUIのPlayerObjectも変更
    [SerializeField] GameObject scripts=null;

    rUIController rUIController;
    Player[] player;


    //Roomに初めて入った時とGameが終わって部屋に戻ってきたときの処理
    private void Start()
    {
        rUIController = scripts.GetComponent<rUIController>();

        player = PhotonNetwork.PlayerList;

        int currentnum = PhotonNetwork.CurrentRoom.PlayerCount;

        for (int i = 0; i < currentnum; i++)
        {
            if (i< PhotonNetwork.CurrentRoom.PlayerCount - 1)
            {
                Debug.Log(player[i].NickName+"&"+ player[currentnum - 1].NickName);
                if (player[i].NickName == player[currentnum - 1].NickName)
                {
                    Debug.Log("new");
                    player[currentnum - 1].NickName = player[currentnum - 1].NickName + "2";
                } 
            }
            rUIController.ConnectUpdateList(player[i]);
        }

        

    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable i_propertiesThatChanged)
    {
        Debug.Log("Update!");
        photonView.RPC("GameSettingListUpdate", RpcTarget.AllViaServer);
        photonView.RPC("GameboardListUpdate", RpcTarget.AllViaServer);
        Debug.Log("OnPhotonCustomRoomPropertiesChanged");
     }




    // 他のプレイヤーが入室してきた時
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            player = PhotonNetwork.PlayerList;
            if (i < PhotonNetwork.CurrentRoom.PlayerCount - 1)
            {
                if (player[i].NickName == newPlayer.NickName)
                {
                    Debug.Log("pulasu");
                    newPlayer.NickName = newPlayer.NickName + "2";
                }
            }

        }
        rUIController.ConnectUpdateList(newPlayer);
        Debug.Log(newPlayer.NickName + " is joined.");
    }


    // 他のプレイヤーが退室した時
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        rUIController.DisConnectUpdateList(otherPlayer);
        Debug.Log(otherPlayer.NickName + " is left.");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (newMasterClient.IsLocal)
        {
            rUIController.AllListUpdate();
        }
    }

    /*public override void OnMasterClientSwitched(Player newMasterClient)
    {
        masterclientname = newMasterClient;


        Debug.Log("masterrrrrrrr");
    }

    public Player MasterClient()
    {
        return masterclientname;
    }*/



    //自分が退出した時
    public override void OnLeftRoom()
    {
        rUIController.LeftRoom();
        Debug.Log("OnLeftRoom");
    }

   


}
