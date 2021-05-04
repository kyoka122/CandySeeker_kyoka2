using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationControllerscript : MonoBehaviour
{
    [SerializeField] Animator lackPlayerTextAnimator=null;
    [SerializeField] Animator missSettingAnimator=null;

    [SerializeField] GameObject turnStartAnimatorObj = null;
    [SerializeField] GameObject myTurnStartAnimatorObj = null;

    Animator turnStartAnimator;
    Animator myTurnStartAnimator;

    TurnStartAnimation turnStartAnimation;
    MyTurnStartAnimation myTurnStartAnimation;


    private void Awake()
    {
        if (turnStartAnimatorObj != null) 
        {
            turnStartAnimation = turnStartAnimatorObj.GetComponent<TurnStartAnimation>();
            myTurnStartAnimation = myTurnStartAnimatorObj.GetComponent<MyTurnStartAnimation>();
            turnStartAnimator = turnStartAnimatorObj.GetComponent<Animator>();
            myTurnStartAnimator = myTurnStartAnimatorObj.GetComponent<Animator>();
        }
    }


    //animation動かす？animator動かす？
    public void LackPlayerTextAnimation()
    {
        lackPlayerTextAnimator.SetTrigger("LackPlayerText");
        Debug.Log("Animation");
    }
    public void MissSettingAnimation()
    {
        missSettingAnimator.SetTrigger("MissSettingText");
        Debug.Log("Animation");
    }

    public void TurnStartAnimationOn(bool turnStart, bool myTurnStart,bool nextTurnStart,bool myPieceExtinct)
    {
        turnStartAnimation.MyTurnBool(myTurnStart, nextTurnStart, myPieceExtinct);

        if (turnStart)
        {
            Debug.Log("TurnStart");
            turnStartAnimator.SetTrigger("TurnText");

        }
        else if (myPieceExtinct)
        {
            myTurnStartAnimation.MyTurnStartAnimationOff();
        }
        else if (myTurnStart)
        {
            Debug.Log("MyTurnStart");
            myTurnStartAnimatorObj.SetActive(true);
            //myTurnStartAnimatorObj.transform.localPosition=new Vector2(0,0);
            myTurnStartAnimator.SetTrigger("MyTurn");
        }
        else if(PhotonNetwork.IsMasterClient&& nextTurnStart)
        {
            Debug.Log("MasterClientON");
            myTurnStartAnimation.MyTurnStartAnimationOff();
        }

    }





}
