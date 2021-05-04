using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnStartAnimation : MonoBehaviour
{
    [SerializeField] GameObject scripts=null;
    [SerializeField] SoundController soundController = null;
    bool myTurn;
    bool nextTurn;
    bool pieceExtinct;

    AnimationControllerscript animationControllerscript;

    private void Awake()
    {
        animationControllerscript = scripts.GetComponent<AnimationControllerscript>();
    }

    public void MyTurnBool(bool myTurnStart,bool nextTurnStart,bool myPieceExtinct)
    {
        myTurn = myTurnStart;
        nextTurn = nextTurnStart;
        pieceExtinct = myPieceExtinct;
    }

    public void TurnStartAnimationOff()
    {
        Debug.Log("first!TurnStartAnimationOff:" + "1" + myTurn + "2" + nextTurn);
        animationControllerscript.TurnStartAnimationOn(false, myTurn, nextTurn, pieceExtinct);
    }

  public void TurnStartSEOn()
    {
        soundController.MoveTurnStartSE();
    }
}
