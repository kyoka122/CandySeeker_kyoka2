using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreController : MonoBehaviour
{
    int score=0;
    //int piece;

   //[SerializeField] GameObject turnController1Obj=null;
   [SerializeField] GameObject destroyCheckObj = null;
    //Gamemanager gamemanager;
    List<Player> destroyPlayers=new List<Player>();
    List<int> destroynum = new List<int>();
    DestroyCheck destroyCheck;
    private bool secondDestroy=true;

    private void Awake()
    {
        //gamemanager = GetComponent<Gamemanager>();
        destroyCheck = destroyCheckObj.GetComponent<DestroyCheck>();
    }
    /*public void ScoreSet()
    {
        piece = gamemanager.startPlayerPieceNum;
    }*/

    public void ScoreUpdate(string job_DesObj)
    {
        Debug.Log(job_DesObj);
        if (job_DesObj == "king")
        {
            score += 3;
        }
        else 
        {
            score += 1;
        }
        Debug.Log("ScoreCustomPropertiesGo!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        var playerproperties = new ExitGames.Client.Photon.Hashtable();
        playerproperties["score"] = score;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);

    }
    public void OtherPieceUpdate(Player destroyPlayer)
    {
        Debug.Log("destroyPlayer" + destroyPlayer);
        for (int i=0; i<destroyPlayers.Count;i++)
        {
            if(destroyPlayers[i]==destroyPlayer)
            {
                int prevnum = destroynum[i];
                prevnum++;
                destroynum[i] = prevnum;
                secondDestroy = false;
                Debug.Log("destroyPlayers[]?????:" + i +":"+ destroynum[i]);
                break;
            }
            
        }
        if (secondDestroy)
        {
            destroyPlayers.Add(destroyPlayer);
            destroynum.Add(1);
          
        }
        for (int i = 0; i < destroyPlayers.Count; i++)
        {
            Debug.Log("destroynum[]" + i + ":" + destroynum[i]);
            Debug.Log("destroyPlayers[]:" + i + ":" + destroyPlayers[i]);
        }
            Debug.Log("OtherPieceUpdateOff");
        secondDestroy = true;
        destroyCheck.destroyFinish = true;
    }

    public void PieceSet()
    {
        for (int i = 0; i < destroyPlayers.Count; i++)
        {
            Debug.Log("destroyPlayers.Count:" + destroyPlayers.Count);
            Debug.Log("destroynum[i]!!!!!!!!!!!:" + destroynum[i]);
            Debug.Log("destroyPlayers[i]!!!!!!!!!!!:" + destroyPlayers[i]);

            if (destroyPlayers[i].CustomProperties["piece"] is int piece)
            {
                piece -= destroynum[i];
                Debug.Log("ScoreControllerPiece:" + piece);
                
                var playerproperties = new ExitGames.Client.Photon.Hashtable();
                playerproperties["piece"] = piece;
                destroyPlayers[i].SetCustomProperties(playerproperties);
            }
        }
        destroyPlayers.Clear();
        destroynum.Clear();
    }

}
