using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCheck : MonoBehaviour
{
    [System.NonSerialized] public bool destroyFinish;
    [System.NonSerialized] public bool destroyJudgeFinish;

    [SerializeField] GameObject turnController2Obj=null;
    gTurnController2 turnController2;
    private void Awake()
    {
        turnController2 = turnController2Obj.GetComponent<gTurnController2>();
    }
    private void OnEnable()
    {
        destroyFinish = true;
        destroyJudgeFinish = false;
    }

    private void Update()
    {
        if  (destroyJudgeFinish&& destroyFinish)
        {
            Debug.Log("DestroyCheckOff");
            turnController2.StartJudge2Judge();
            gameObject.SetActive(false);
        }
        Debug.Log("destroyFinish" + destroyFinish);
        Debug.Log("destroyJudgeFinish" + destroyJudgeFinish);
    }
}
