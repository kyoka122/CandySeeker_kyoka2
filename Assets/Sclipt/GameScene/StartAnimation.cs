using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    [SerializeField] GameObject scripts=null;
    [SerializeField] SoundController soundController = null;
    Gamemanager gamemanager;
    private void Awake()
    {
        gamemanager = scripts.GetComponent<Gamemanager>();
    }
    public void GameStartAnimationOff()
    {
        Debug.Log("GameStartAnimationOff");
        gamemanager.PutObject();
        
    }
    public void GameStartSEOn()
    {
        soundController.GameStartSE();
    }
}
