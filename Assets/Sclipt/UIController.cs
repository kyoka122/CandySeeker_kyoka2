using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject SettingsPanel=null;
    [SerializeField] GameObject LeaveCheckFilter=null;
    [SerializeField] GameObject[] IntroductionImage = new GameObject[8];
    [SerializeField] GameObject introductionPanel=null;
    [SerializeField] SoundController soundController = null;
    private int IntroductionImageNum=0;

    public void SettingsButton()
    {
        soundController.ClickSE();
        if (SettingsPanel.activeSelf == true)
        {
            SettingsPanel.SetActive(false);
        }

        else
            SettingsPanel.SetActive(true);

    }

    public void FirstLeaveButton()
    {
        soundController.ClickSE();
        LeaveCheckFilter.SetActive(true);
        SettingsPanel.SetActive(false);

    }
    public void SecondLeaveButton()
    {
        soundController.Click1SE();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("GameStartScene");
    }
    public void BackButton()
    {
        soundController.Click1SE();
        LeaveCheckFilter.SetActive(false);
    }

    public void RightButtonOn()
    {
        soundController.ClickSE();
        IntroductionImageNum++;
        if (IntroductionImageNum == 8)
        {
            IntroductionImage[IntroductionImageNum - 1].SetActive(false);
            IntroductionImageNum = 0;
            IntroductionImage[IntroductionImageNum].SetActive(true);
        }
        else
        {
            IntroductionImage[IntroductionImageNum-1].SetActive(false);
            IntroductionImage[IntroductionImageNum].SetActive(true);
        }
       
    }

    public void LeftButtonOn()
    {
        soundController.ClickSE();
        IntroductionImageNum--;
        if (IntroductionImageNum == -1)
        {
            IntroductionImage[IntroductionImageNum + 1].SetActive(false);
            IntroductionImageNum = 7;
            IntroductionImage[IntroductionImageNum].SetActive(true);
        }
        else
        {
            IntroductionImage[IntroductionImageNum + 1].SetActive(false);
            IntroductionImage[IntroductionImageNum].SetActive(true);
        }
    }

    public void IntroductionButtonOn()
    {
        soundController.ClickSE();
        SettingsPanel.SetActive(false);
        introductionPanel.SetActive(true);
    }
    public void IntroductionButtonOff()
    {
        soundController.Click1SE();
        introductionPanel.SetActive(false);
    }
}
