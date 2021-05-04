using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gScoreFilterClick : MonoBehaviour
{
    [SerializeField] GameObject turnController1Obj=null;
    [SerializeField] GameObject turnController2Obj=null;
    [SerializeField] GameObject clickControllerObj=null;
    [SerializeField] GameObject timeCounterObj=null;
    [SerializeField] SoundController soundController = null;
    private void Update()
    {
        if (turnController2Obj.activeSelf)
        {
            turnController2Obj.SetActive(false);
            soundController.TimerSEOff();
        }
        if (clickControllerObj.activeSelf)
        {
            clickControllerObj.SetActive(false);
        }
        if (turnController1Obj.activeSelf)
        {
            turnController1Obj.SetActive(false);
        }
        if (timeCounterObj.activeSelf)
        {
            timeCounterObj.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("RoomScene");
        }

    }
}
