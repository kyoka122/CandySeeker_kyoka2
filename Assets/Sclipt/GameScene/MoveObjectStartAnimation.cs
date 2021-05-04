using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveObjectStartAnimation : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject scripts=null;
    [SerializeField] SoundController soundController = null;
    PhotonView scriptsPhotonView;
   
    private void Awake()
    {
        scriptsPhotonView = scripts.GetComponent<PhotonView>();
    }


    public void MoveObjectTurnStartOff()
    {
        Debug.Log("MoveObjectTurnStartOff");
        if (PhotonNetwork.IsMasterClient)//どっちがいいんだろ？
        {
            scriptsPhotonView.RPC("MoveObject", RpcTarget.AllViaServer);
        }
       
    }
    public void MoveObjectStartSoundOn()
    {
        soundController.MoveTurnStartSE();
    }
 
}
