using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerNetworkMover : MonoBehaviourPunCallbacks
{
    public GameObject NetworkDisconnect;

    public override void OnPlayerLeftRoom(Player otherplayer)
    {
        NetworkDisconnect.SetActive(true);

    }
    public void BackButtonOn()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }

    public void CancelButtonOn()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("StartScene");
    }



}
