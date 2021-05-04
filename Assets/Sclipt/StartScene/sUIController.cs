using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class sUIController : MonoBehaviourPunCallbacks
{
    string roomname;

    string connectway;
    [SerializeField]InputField nicknameInput=null;
    [SerializeField] InputField roomnameCreateVer=null;
    [SerializeField] InputField roomnameEnterVer=null;
    [SerializeField] Text roomNameList=null;
    sNetworkGameManager networkGameManager;
 
    [SerializeField] GameObject connectingFilter=null;
    [SerializeField] GameObject connectingText;
    [SerializeField] GameObject enterInputField = null;
    [SerializeField] GameObject createInputField = null;
    [SerializeField] GameObject selectPanel=null;
    [SerializeField] GameObject startPanel=null;

    [SerializeField] Image createbutton=null;
    [SerializeField] Image enterbutton=null;
    [SerializeField] Image randombutton=null;

    [SerializeField] SoundController soundController = null;
    bool start= true;


    private void Start()
    {
        networkGameManager = GetComponent<sNetworkGameManager>();
    }


    //ルーム一覧
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        if (roomList == null)
            return;

        roomNameList.text = "";
        for (int i = 0; i < roomList.Count; i++)
        {
            roomNameList.text += roomList[i].Name+"\n";
           
        }

    }

    private void Update()
    {
        if (start==true)
        {
            if(Input.GetMouseButton(0))
            {
                selectPanel.SetActive(true);
                startPanel.SetActive(false);
                start = false;
            }
        }
    }

    public void CreateButtonOn()
    {
        soundController.ClickSE();
        createInputField.SetActive(true);
        enterInputField.SetActive(false);
        createbutton.color = new Color(142.0f / 255.0f, 142.0f / 255.0f, 142.0f / 255.0f);
        enterbutton.color = new Color(1.0f, 1.0f, 1.0f);
        randombutton.color = new Color(1.0f, 1.0f, 1.0f);
        connectway = "create";
        networkGameManager.Connect("1.0");
    }

    public void EnterButtonOn()
    {
        soundController.ClickSE();
        createInputField.SetActive(false);
        enterInputField.SetActive(true);
        createbutton.color = new Color(1.0f, 1.0f, 1.0f);
        enterbutton.color = new Color(142.0f / 255.0f, 142.0f / 255.0f, 142.0f / 255.0f);
        randombutton.color = new Color(1.0f, 1.0f, 1.0f);
        connectway = "enter";
        networkGameManager.Connect("1.0");
    }
    public void RandomEnterButtonOn()
    {
        soundController.ClickSE();
        createInputField.SetActive(false);
        enterInputField.SetActive(false);
        createbutton.color = new Color(1.0f, 1.0f, 1.0f);
        enterbutton.color = new Color(1.0f, 1.0f, 1.0f);
        randombutton.color = new Color(142.0f / 255.0f, 142.0f / 255.0f, 142.0f / 255.0f);
        connectway = "random";
        networkGameManager.Connect("1.0");
    }

    public void ConnectStartButtonOn()
    {
        soundController.ClickSE();
        connectingFilter.SetActive(true);
        connectingText = GameObject.Find("Text_Connecting");
        connectingText.GetComponent<Text>().text = "接続中…";  
        networkGameManager.SetMyNickName(nicknameInput.text);
        RoomConect();
        // Photonに接続       
    }

    public void RoomConect()
    {
        if (string.IsNullOrWhiteSpace(nicknameInput.text))
        {
            connectingText.GetComponent<Text>().text = "入力をやり直してください";
            return;
        }

        if (connectway == "create")
        {
            if (string.IsNullOrWhiteSpace(roomnameCreateVer.text))
            {
                connectingText.GetComponent<Text>().text = "入力をやり直してください";
                return;
            }
            roomname = roomnameCreateVer.text;
            networkGameManager.CreateAndJoinRoom(roomname);
            
        }

        else if (connectway == "enter")
        {
            if (string.IsNullOrWhiteSpace(roomnameEnterVer.text))
            {
                connectingText.GetComponent<Text>().text = "入力をやり直してください";
                return;
            }
            roomname = roomnameEnterVer.text;
            networkGameManager.JoinRoom(roomname);
            
        }

        else if (connectway == "random")
        {
                networkGameManager.JoinRandomRoom();
        }

        else
        {
            connectingText.GetComponent<Text>().text = "入力をやり直してください";
        }
    }


    public void TextChange()
    {
        connectingText.GetComponent<Text>().text = "失敗しました";
    }

    public void BackButtonOn()
    {
        soundController.Click1SE();
        connectingFilter.SetActive(false);
        connectway = "";
        createbutton.color = new Color(1.0f, 1.0f, 1.0f);
        enterbutton.color = new Color(1.0f, 1.0f, 1.0f);
        randombutton.color = new Color(1.0f, 1.0f, 1.0f);
        createInputField.SetActive(false);
        enterInputField.SetActive(false);
        PhotonNetwork.Disconnect();
    }

}
