using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeList :MonoBehaviour
{
    [System.NonSerialized] public string color1;
    [System.NonSerialized] public string color2;
    [System.NonSerialized] public string job;

    GameObject scripts;
    //GameObject piece;

    Gamemanager gamemanager;
    gInstantiateController gInstantiateController;

    private void Awake()
    {
        scripts = GameObject.Find("scripts");
        gInstantiateController = scripts.GetComponent<gInstantiateController>();
        gamemanager = scripts.GetComponent<Gamemanager>();
        //piece = gameObject;
        gameObject.name = gameObject.GetComponent<PhotonView>().Owner.NickName+(" : ") + gameObject.GetComponent<PhotonView>().ViewID;
        gInstantiateController.WorldObjectSettings(gameObject);
    }

    public void ColorSettings1(string localColor)
    {
        color1 = localColor;
        color2 = localColor;
    }

    public void ColorSettings2(string localColor)
    {
        color2 = localColor;
    }

    public void JobSettings(string localJob)
    {
        job = localJob;
    }


}
