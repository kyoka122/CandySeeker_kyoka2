using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class gNetworkGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject turnController2Obj = null;

    gUIController gUIController;
    Gamemanager gamemanager;
    gTurnController2 turnController2;

    private void Awake()
    {
        gUIController = GetComponent<gUIController>();
        gamemanager = GetComponent<Gamemanager>();
        turnController2 = turnController2Obj.GetComponent<gTurnController2>();
    }
    public override void OnLeftRoom()
    {
        gUIController.LeftRoom();
        Debug.Log("OnLeftRoom");
    }


    public void FinishRoomSettings()
    {
        PhotonNetwork.CurrentRoom.IsVisible = true;
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var roomproperties = PhotonNetwork.CurrentRoom.CustomProperties;//Roomは大文字
            if (otherPlayer.CustomProperties["num"]is int num)
            {
                if (turnController2Obj.activeSelf==true&&num ==(gamemanager.movePlayerJudge - 1) % gamemanager.startPlayerCount)
                {
                    turnController2.StartJudge2Judge();
                }
                roomproperties[num.ToString()] = null;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

                //いなくなったプレイヤーのpiece編集
                Debug.Log("PlayerLeft:");
                var playerproperties = new ExitGames.Client.Photon.Hashtable();
                playerproperties["piece"] = 0;
                otherPlayer.SetCustomProperties(playerproperties);

                //Text??



            }
            else
            {
                Debug.Log("CustomPropatiesDestroyMissed");
            }



        }
    }
        



    /*public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)//
    {
        gUIController.ScoreChange(propertiesThatChanged);//途中退室ぐらい
    }*/


    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)



    /*public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        gUIController.ScoreChange(targetPlayer, changedProps);
    }*/


}
