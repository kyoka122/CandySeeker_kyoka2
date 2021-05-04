using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class gAnimationController : MonoBehaviour
{
    [SerializeField]GameObject gameStartAnimationObj=null;
    [SerializeField]GameObject gameFinishAnimationObj=null;
    [SerializeField] GameObject moveObjectStartObj=null;

    Gamemanager gamemanager;
    
    private void Awake()
    {
        gamemanager = GetComponent<Gamemanager>();
    }



    public void GameStartAnimationOn()
    {
        Debug.Log("GameStartAnimationOn");
        gameStartAnimationObj.SetActive(true);
        Debug.Log("gameStartAnimationObj:True");
    }

    [PunRPC]
    public void MoveObjectTurnStartOn()
    {
        Debug.Log("MoveObjectTurnStartOn");
        moveObjectStartObj.SetActive(true);
    }

    public void GameFinishAnimationOn()
    {
        gameFinishAnimationObj.SetActive(true);
    }

}
