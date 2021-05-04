using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class gGameboardList : MonoBehaviour
{
    [SerializeField] GameObject[] selectedgameboardAver = new GameObject[2];
    [SerializeField] GameObject[] selectedgameboardBver = new GameObject[3];
    [SerializeField] GameObject clickedMark = null;
    [SerializeField] GameObject turnOverParticleObj=null;
    gInstantiateController gInstantiateController;
    private void Awake()
    {
        gInstantiateController = GetComponent<gInstantiateController>();
    }

    public void Gameboard(string ID)
    {
        Debug.Log("gameboardchange!!!!!");
        switch (ID)
        {
            case "A1":
                selectedgameboardAver[0].SetActive(true);
                turnOverParticleObj.transform.SetParent(selectedgameboardAver[0].transform);
                clickedMark.transform.SetParent(selectedgameboardAver[0].transform);
                clickedMark.transform.localScale = new Vector3(0.025f, 0.025f, 0.0f);
                gInstantiateController.GameboardObjSet(selectedgameboardAver[0]);
                break;
            case "A2":
                selectedgameboardAver[1].SetActive(true);
                turnOverParticleObj.transform.SetParent(selectedgameboardAver[1].transform);
                clickedMark.transform.SetParent(selectedgameboardAver[1].transform);
                clickedMark.transform.localScale = new Vector3(0.023f, 0.023f, 0.0f);
                gInstantiateController.GameboardObjSet(selectedgameboardAver[1]);
                break;
            case "B1":
                selectedgameboardBver[0].SetActive(true);
                turnOverParticleObj.transform.SetParent(selectedgameboardBver[0].transform);
                clickedMark.transform.SetParent(selectedgameboardBver[0].transform);
                clickedMark.transform.localScale = new Vector3(0.025f, 0.025f, 0.0f);
                gInstantiateController.GameboardObjSet(selectedgameboardBver[0]);
                break;
            case "B2":
                selectedgameboardBver[1].SetActive(true);
                turnOverParticleObj.transform.SetParent(selectedgameboardBver[1].transform);
                clickedMark.transform.SetParent(selectedgameboardBver[1].transform);
                clickedMark.transform.localScale = new Vector3(0.025f, 0.025f, 0.0f);
                gInstantiateController.GameboardObjSet(selectedgameboardBver[1]);
                break;
            case "B3":
                selectedgameboardBver[2].SetActive(true);
                turnOverParticleObj.transform.SetParent(selectedgameboardBver[2].transform);
                clickedMark.transform.SetParent(selectedgameboardBver[2].transform);
                clickedMark.transform.localScale = new Vector3(0.025f, 0.025f, 0.0f);
                gInstantiateController.GameboardObjSet(selectedgameboardBver[2]);
                break;
            default:
                Debug.Log("Stage NotFound");
                break;
        }
        PositionSet(turnOverParticleObj);
    }

    public void ParticleSetParent(string ID,GameObject particleObj)
    {
        switch (ID)
        {
            case "A1":
                particleObj.transform.SetParent(selectedgameboardAver[0].transform);
                break;
            case "A2":
                particleObj.transform.SetParent(selectedgameboardAver[1].transform);
                break;
            case "B1":
                particleObj.transform.SetParent(selectedgameboardBver[0].transform);
                break;
            case "B2":
                particleObj.transform.SetParent(selectedgameboardBver[1].transform);
                break;
            case "B3":
                particleObj.transform.SetParent(selectedgameboardBver[2].transform);
                break;
            default:
                Debug.Log("Stage NotFound");
                break;
        }
        PositionSet(particleObj);
    }

    private void PositionSet(GameObject particleObj)
    {
        particleObj.transform.localScale = new Vector3(4,4,0);
    }
}

