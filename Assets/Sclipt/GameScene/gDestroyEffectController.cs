using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gDestroyEffectController : MonoBehaviour
{

    //GameObject gTurnController2Obj=null;

    ParticleSystem particle;
    //gTurnController2 gTurnController2;

    private void Awake()
    {
        //var main= GetComponent<ParticleSystem>().main;
        //main.stopAction = ParticleSystemStopAction.Callback;

        particle = GetComponent<ParticleSystem>();

        //gTurnController2 = gTurnController2Obj.GetComponent<gTurnController2>();
    }

    public void DestroyObjectEffect()
    {
        particle.Play();
    }

    /*private void OnParticleSystemStopped()
    {
        gTurnController2.Score();
    }*/
}
