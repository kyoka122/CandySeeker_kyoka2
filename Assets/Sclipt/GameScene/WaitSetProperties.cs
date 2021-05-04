using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class WaitSetProperties : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject scripts=null;
    Gamemanager gamemanager;
    bool first=true;

    private void Awake()
    {
        gamemanager = scripts.GetComponent<Gamemanager>();
    }

    public override void OnPlayerPropertiesUpdate(Player player, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {

        PropertiesJudge();
  
        
        

        if (PhotonNetwork.IsMasterClient)
        {
            Player[] playerlist = PhotonNetwork.PlayerList;
            int playercount = playerlist.Length;
            Debug.Log("playerlist.Length"+playerlist.Length);

            for (int i = 0; i < playercount; i++)
            {
                if (playerlist[i].CustomProperties["set"] is bool playerset)
                {
                    Debug.Log("playerset" + i + playerset);
                    if (playerset== false)
                    {
                        return;
                    }
                    
                }
            }
            Debug.Log("MasterClientRPC");
            photonView.RPC("GameStartAnimation", RpcTarget.AllViaServer);

        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("RoomPropertiesUpdate!!!!!!!!!");        
       PropertiesJudge();
        
    }

    public void PropertiesJudge()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["set1"] is bool set1)
        {

            if (PhotonNetwork.CurrentRoom.CustomProperties["Set"] is bool Set)
            {
                Debug.Log("SET:" + "1" + set1 + "Room" + Set);
                if (first & set1 && Set)
                {
                    Debug.Log("SetGameScene!!!!!!!(Instantiate)");
                    first = false;
                    gamemanager.SetGameScene();

                }
            }

        }
               
    }
}
