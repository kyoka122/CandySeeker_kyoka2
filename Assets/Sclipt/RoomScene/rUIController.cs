using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class rUIController : MonoBehaviourPunCallbacks
{

    //2つ以外全同期
    private int maxPlayer;
    //private string masterclientname=null;
    private bool gamesystemjudge = false;
    private bool colorjudge=false;

    string color;
    [SerializeField] GameObject rNetworkGameManagerObj=null;
    [SerializeField] SoundController soundController = null;
    [SerializeField] Text[] NumText = new Text[5];//０はPieceNumText、１はKingNumText、２はSpyNumText、３はSpyNumText、４はTurnLimitNumText、５はTimeLimitNumText
  

    rNetworkGameManager rNetworkGameManager;
    AnimationControllerscript animationControllerscript;
    //PhotonView photonView;
    rGameboard rGameboard;
    ColorList colorList;

    private void Start()
    {
        //photonView = GetComponent<PhotonView>();

        rNetworkGameManager = rNetworkGameManagerObj.GetComponent<rNetworkGameManager>();
        rGameboard = GetComponent<rGameboard>();     
        animationControllerscript = GetComponent<AnimationControllerscript>();
        colorList = GetComponent<ColorList>();
        maxPlayer = rNetworkGameManager.maxPlayer;
        currentRoomName.text = PhotonNetwork.CurrentRoom.Name;

        var roomproperties = new ExitGames.Client.Photon.Hashtable();
        roomproperties["Set"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

        var playerproperties = new ExitGames.Client.Photon.Hashtable();
        playerproperties["set"] = false;
        playerproperties["setN"] = false;
        playerproperties["setS"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);

        GameSettingListUpdate();//ゲームからルームに戻ってきてた時いる
        rGameboard.FirstGameboardSet();


    }

    private int pieceNum;
    private int kingNum;
    private int spyNum;
    private int timeLimitNum;
    private int turnLimitNum;



    [PunRPC]
    public void GameSettingListUpdate()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties["Piece"] is int customPieceNum)
        {
            pieceNum = customPieceNum;
            NumText[0].text = pieceNum.ToString();
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties["King"] is int customKingNum)
        {
            kingNum = customKingNum;
            NumText[1].text = kingNum.ToString();
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties["Spy"] is int customSpyNum)
        {
            spyNum = customSpyNum;
            NumText[2].text = spyNum.ToString();
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties["Turn"] is int customTurnNum)
        {
            turnLimitNum = customTurnNum;
            NumText[3].text = turnLimitNum.ToString();
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties["Time"] is int customTimeNum)
        {
            timeLimitNum = customTimeNum;
            NumText[4].text = timeLimitNum.ToString();
        }
        Debug.Log("GameSettingListUpdate");

    }

        


    //PlayerList関連////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private GameObject[] playerObject = new GameObject[8];
    private GameObject[] playerText = new GameObject[8];
    [SerializeField] Object playerPrefab = null;
    [SerializeField] GameObject　masterclient=null ;
    [SerializeField] GameObject[] winnerMark = new GameObject[8];
    //[SerializeField] GameObject playerListObj = null;
    //GameObject instancewinner = null;

    public void ConnectUpdateList(Player player)
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            Debug.Log("ConectUpdateList");
            Debug.Log(player.NickName);
            if (playerObject[i] == null)
            {         
                playerObject[i] = (GameObject)Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
                playerObject[i].transform.SetParent(GameObject.Find("Canvas").transform, false);
                playerObject[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(150, 60 - 25 * i);
                Transform playerpos = playerObject[i].transform.Find("PlayerText");

                if (player.CustomProperties["color"] is string customColor)
                {
                    Debug.Log("ColorSet");//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    playerObject[i].GetComponent<Image>().color = colorList.NameListColor(customColor);
                    if (player.IsLocal)
                    {
                        color = customColor;
                    }
                }

                playerText[i] = playerpos.gameObject;
                playerText[i].GetComponent<Text>().text = player.NickName;
                
                Debug.Log(player.NickName + " Instance");
                //masterclientname = player.NickName;
                if (playerText[i].GetComponent<Text>().text == PhotonNetwork.MasterClient.NickName)
                {
                    masterclient.transform.localPosition = new Vector2(-165, 125 - 50 * i);
                }

                if (player.CustomProperties["win"] is string)
                {
                    Debug.Log("winner:true");
                    winnerMark[i].SetActive(true);
                }
                else
                {
                    Debug.Log("winner:false");
                    winnerMark[i].SetActive(false);
                }
                //GameStartJudge1();
                //GameStartJudge2();
                break;
            }
        }
    }

    public void DisConnectUpdateList(Player player)
    {
        int currentplayernum = PhotonNetwork.CurrentRoom.PlayerCount;
        for (int i = 0; i < maxPlayer; i++)
        {
            if (playerText[i].GetComponent<Text>().text == player.NickName)
            {
                Destroy(playerObject[i]);
                for (int j = i; j < currentplayernum; j++)//CurrentPlayerは動的に
                {             
                    playerObject[j] = playerObject[j + 1];

                    //playerText[j].GetComponent<Text>().text = playerText[j+1].GetComponent<Text>().text;
                    playerText[j] = playerText[j + 1];

                    playerObject[j].GetComponent<RectTransform>().anchoredPosition = new Vector2(150, 60 - 25 * j); //////////////////////////////////////
                    
                    if(playerText[j].GetComponent<Text>().text== PhotonNetwork.MasterClient.NickName)
                    {
                        masterclient.transform.localPosition = new Vector2(-165, 125 - 50 * j);/////////////////////////////////////////j,i
                    }

                    if(winnerMark[j].activeSelf)
                    {
                        Debug.Log("winner:true");
                        winnerMark[j].SetActive(true);
                        if (j<7) 
                        {
                            winnerMark[j + 1].SetActive(false);
                        }
                    }
                    if (j+1 == currentplayernum)////////////////
                    {
                        playerObject[j + 1] = null;
                        playerText[j + 1] = null;
                    }
                }
                Debug.Log(player.NickName + " Destroy");
                break;
            }
        }
    }

    //システムボタンと通信切断////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] GameObject disConnectedFilter = null;
    public void LeftRoom()
    {
        soundController.ClickSE();
        disConnectedFilter.SetActive(true);
    }

    public void BacktoStartButton()
    {
        soundController.ClickSE();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("GameStartScene");
    }


//ボード切り替え///////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] GameObject gameSystemPanel = null;
    [SerializeField] GameObject playerSettingsPanel = null;
    [SerializeField] GameObject gameboardPanel = null;
    [SerializeField] GameObject lackPlayerText;
    [SerializeField] Text currentRoomName = null;
    [SerializeField] GameObject gameboardPanel1 = null;
    [SerializeField] GameObject gameboardPanel2 = null;

    public void GameSystemButtonOn()
    {
        soundController.ClickSE();
        gameSystemPanel.transform.SetAsLastSibling();
    }

    public void PlayerSettingsButtonOn()
    {
        soundController.ClickSE();
        playerSettingsPanel.transform.SetAsLastSibling();
    }
    public void GameboardButtonOn()
    {
        soundController.ClickSE();
        gameboardPanel.transform.SetAsLastSibling();
    }

    public void Gameboard1ButtonOn()
    {
        soundController.ClickSE();
        gameboardPanel1.transform.SetAsLastSibling();
    }

    public void Gameboard2ButtonOn()
    {
        soundController.ClickSE();
        gameboardPanel2.transform.SetAsLastSibling();
    }



////////////////////////////////////////////////////////////////
///////ゲーム設定///////////////////////////////////////////////
////////////////////////////////////////////////////////////////

    //引数の値はNumTextのコメントアウト参照
    public void PieceNumUpbuttonOn()
    {
        soundController.ClickSE();
        if (pieceNum < 23&&PhotonNetwork.IsMasterClient)
        {
            pieceNum++;

            var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
            roomproperties["Piece"] = pieceNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);
            //マスターだけテキスト即反映にする？？
            GameStartJudge1();
        }
    }

    public void KingNumUpbuttonOn()
    {
        soundController.ClickSE();
        if (PhotonNetwork.IsMasterClient)
        {
            kingNum++;

            var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
            roomproperties["King"] = kingNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

            GameStartJudge1();
        }
    }

    public void SpyNumUpbuttonOn()
    {
        soundController.ClickSE();
        if (PhotonNetwork.IsMasterClient)
        {
            spyNum++;

            var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
            roomproperties["Spy"] = spyNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

            GameStartJudge1();
        }
    }

    public void TurnLimitUpbuttonOn()
    {
        soundController.ClickSE();
        if (PhotonNetwork.IsMasterClient)
        {
            turnLimitNum++;

            var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
            roomproperties["Turn"] = turnLimitNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

            GameStartJudge1();
        }
    }

    public void TimeLimitUpbuttonOn()
    {
        soundController.ClickSE();
        if (PhotonNetwork.IsMasterClient)
        {
            timeLimitNum += 5;
            Debug.Log("aaaaaTimeLimitNum2 "+timeLimitNum);
            var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
            roomproperties["Time"] = timeLimitNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

            GameStartJudge1();
        }
    }


    public void PieceNumDownButtonOn()
    {
        soundController.ClickSE();
        if (pieceNum > 2&&PhotonNetwork.IsMasterClient)
        {
            pieceNum--;

            var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
            roomproperties["Piece"] = pieceNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

            GameStartJudge1();
        }
    }

    public void KingNumDownButtonOn()
    {
        soundController.ClickSE();
        if (kingNum > 0 && PhotonNetwork.IsMasterClient)
        {
            kingNum--;

            var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
            roomproperties["King"] = kingNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

            GameStartJudge1();
        }
    }

    public void SpyNumDownButtonOn()
    {
        soundController.ClickSE();
        if (spyNum > 0 && PhotonNetwork.IsMasterClient)
        {
            spyNum--;

            var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
            roomproperties["Spy"] = spyNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

            GameStartJudge1();
        }
    }

    public void TurnLimitDownButtonOn()
    {
        soundController.ClickSE();
        if (turnLimitNum > 0 && PhotonNetwork.IsMasterClient)
        {
            turnLimitNum--;

            var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
            roomproperties["Turn"] = turnLimitNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

            GameStartJudge1();
        }
    }

    public void TimeLimitDownButtonOn()
    {
        soundController.ClickSE();

        if (timeLimitNum>5 && PhotonNetwork.IsMasterClient)
        {
            timeLimitNum-=5;

            var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
            roomproperties["Time"] = timeLimitNum;           
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

            GameStartJudge1();
        }
    }



    ////////////////////////////////////////////////////////////////
    ///////プレイヤー設定///////////////////////////////////////////
    ////////////////////////////////////////////////////////////////


    //名前/////
    public void Namechange(string newName)
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (playerText[i] != null)
            {
                if (playerText[i].GetComponent<Text>().text == newName)
                {
                    return;
                }
            }
        }
        string changePlayerName = PhotonNetwork.LocalPlayer.NickName;
        PhotonNetwork.LocalPlayer.NickName = newName;
        photonView.RPC("NameChange", RpcTarget.AllViaServer,  newName, changePlayerName);
        Debug.Log("LocalPlayer.NickName: " + PhotonNetwork.LocalPlayer.NickName);
       
    }

    [PunRPC]
    public void NameChange(string newName,string changePlayerName)
    {
        int membercount = PhotonNetwork.CurrentRoom.PlayerCount;
        for (int i = 0; i <membercount; i++)
        {
            if (playerText[i].GetComponent<Text>().text == changePlayerName)
            {               
                playerText[i].GetComponent<Text>().text = newName;
                Debug.Log("NameChange");
                break;
            }
        }
    }


    //プレイヤーのカラーチェンジ/////


    public void RedButtonOn()
    {
        soundController.ClickSE();
        if (true==ColorChangeJudge(255.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f))
        {
            color = "red";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 255.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f);
        }
        
    }
    public void BlueButtonOn()
    {
        soundController.ClickSE();
        if (true == ColorChangeJudge(0.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f))
        {
            color = "blue";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 0.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f);
        }
    }
    public void YellowButtonOn()
    {
        soundController.ClickSE();
        if (true == ColorChangeJudge(255.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f))
        {
            color = "yellow";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 255.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f);
        }
    }
    public void GreenButtonOn()
    {
        soundController.ClickSE();
        if (true == ColorChangeJudge(0.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f))
        {
            color = "green";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 0.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f);
        }
    }
    public void BrownButtonOn()
    {
        soundController.ClickSE();
        if (true == ColorChangeJudge(222.0f / 255.0f, 156.0f / 255.0f, 52.0f / 255.0f))
        {
            color = "brown";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 222.0f / 255.0f, 156.0f / 255.0f, 52.0f / 255.0f);
        }
    }
    public void PurpleButtonOn()
    {
        soundController.ClickSE();
        if (true == ColorChangeJudge(144.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f))
        {
            color = "purple";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 144.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f);
        }
    }
    public void LightblueButtonOn()
    {
        soundController.ClickSE();
        if (true == ColorChangeJudge(0.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f))
        {
            color = "lightblue";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 0.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
        }
    }
    public void BlackButtonOn()
    {
        soundController.ClickSE();
        if (true == ColorChangeJudge(0.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f))
        {
            color = "black";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 0.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f);
        }
    }
    public void PinkButtonOn()
    {
        soundController.ClickSE();
        if (true == ColorChangeJudge(255.0f / 255.0f, 126.0f / 255.0f, 255.0f / 255.0f))
        {
            color = "pink";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 255.0f / 255.0f, 126.0f / 255.0f, 255.0f / 255.0f);
        }
    }
    public void LightgreenButtonOn()
    {
        soundController.ClickSE();
        if (true == ColorChangeJudge(209.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f))
        {
            color = "lightgreen";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 209.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f);
        }
    }
    public void BluegreenButtonOn()
    {
        soundController.ClickSE();
        if (true == ColorChangeJudge(0.0f / 255.0f, 255.0f / 255.0f, 145.0f / 255.0f))
        {
            color = "bluegreen";
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.NickName, 0.0f / 255.0f, 255.0f / 255.0f, 145.0f / 255.0f);
        }
    }


    public bool ColorChangeJudge(float r, float g, float b)
    {
        int membercount = PhotonNetwork.CurrentRoom.PlayerCount;
        for (int i = 0; i < membercount; i++)
        {
            if (playerObject[i].GetComponent<Image>().color == new Color(r, g, b,0.25f))
            {
                return false;
            }
        }
        return true;
    }


   
    [PunRPC]
    public void ColorChange(string myNickname, float r, float g, float b)
    {
        int membercount = PhotonNetwork.CurrentRoom.PlayerCount;
        for (int i = 0; i < membercount; i++)
        {
            if (playerText[i].GetComponent<Text>().text == myNickname)
            {
                playerObject[i].GetComponent<Image>().color = new Color(r, g, b, 0.25f);

                /*if (photonView.IsMine)
                {
                    var playerproperties = new ExitGames.Client.Photon.Hashtable();
                        playerproperties["color"] = new Color(r, g, b, 0.25f);
                   
                    PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);
                    Debug.Log("isMine");

                    
                }*/

                var playerproperties = new ExitGames.Client.Photon.Hashtable();
                playerproperties["color"] = color;
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);

                Debug.Log("ColorChange");
                break;
            }
            if (i == membercount)
            {
                Debug.Log("MyObject:NotFound");
            }
        }
        //GameStartJudge2();
    }

    //更新ボタン
    public void AllListUpdate()
    {
        soundController.ClickSE();
        photonView.RPC("GameSettingListUpdate", RpcTarget.AllViaServer);
        photonView.RPC("GameboardListUpdate", RpcTarget.AllViaServer);
        Debug.Log("AllListUpdate");
    }





    //プレイ可能かどうか設定のジャッジ/////////////////////////////////////////////////
    [SerializeField] GameObject pieceNumWaring=null;
    [SerializeField] GameObject kingNumWaring=null;
    [SerializeField] GameObject spyNumWaring=null;
    [SerializeField] GameObject colorWarning=null;
    [SerializeField] GameObject gameSystemWarning=null;

 
    public void GameStartJudge1()//ゲーム設定、ゲームボードのジャッジ
    {

        if ((pieceNum * PhotonNetwork.CurrentRoom.PlayerCount) + 1 > rGameboard.gameboardMaxPiece)//動かせる箇所の１
        {
            pieceNumWaring.SetActive(true);
        }
        else
        {
            pieceNumWaring.SetActive(false);
        }
        if (pieceNum < kingNum + spyNum || pieceNum < kingNum)
        {
            kingNumWaring.SetActive(true);
        }
        else
        {
            kingNumWaring.SetActive(false);
        }
        if (pieceNum < kingNum + spyNum || pieceNum < spyNum)
        {
            spyNumWaring.SetActive(true);
        }
        else
        {
            spyNumWaring.SetActive(false);
        }


        if (!pieceNumWaring.activeSelf && !kingNumWaring.activeSelf && !spyNumWaring.activeSelf)
        {
            gamesystemjudge = true;
            gameSystemWarning.SetActive(false);
        }
        else
        {
            gamesystemjudge = false;
            gameSystemWarning.SetActive(true);
        }

    }

    void GameStartJudge2()//プレイヤーカラーのジャッジ//カスタムプロパティ使えばよかったなー
    {
        for (int i = 0; i<PhotonNetwork.CurrentRoom.PlayerCount; i++) 
        {
            if (playerObject[i].GetComponent<Image>().color == new Color(255.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.25f)) 
            {
                colorWarning.SetActive(true);
                colorjudge = false;
                return;
            }
            
        }
        colorWarning.SetActive(false);
        colorjudge = true;

    }


    //StartButton///////////////////////////////////////
    public void GameStartButtonOn()
    {
        soundController.ClickSE();
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

            GameStartJudge1();
            GameStartJudge2();


            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                animationControllerscript.LackPlayerTextAnimation();
            }

            else if (gamesystemjudge && colorjudge)
            {
                rNetworkGameManagerObj.SetActive(false);
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;

                SetRoomProperty();
                SetNumProperty();

                photonView.RPC("SetPlayerProperty", RpcTarget.AllViaServer);
                Debug.Log("SetPlayerProperties");

                var roomproperties = new ExitGames.Client.Photon.Hashtable();
                roomproperties["Set"] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

                photonView.RPC("LoadGameScene", RpcTarget.AllViaServer);

            }
            else
            {
                animationControllerscript.MissSettingAnimation();
            }
        }


    }
    [PunRPC]
    private void LoadGameScene()
    {
        var playerproperties = new ExitGames.Client.Photon.Hashtable();
        playerproperties["set1"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);
        //PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene("GameScene");
    }


    //カスタムプロパティ系////////////////////////////////////////////////////////////////////////////////

    [PunRPC]
    public void SetRoomProperty()
    {
        
        /*ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable
           {
           {"Stage", rGameboardController.GamebordSet() },
           {"pieceNum", pieceNum},
           {"kingNum",kingNum },
           {"spyNum",spyNum },
           {"turnLimitNum",turnLimitNum },
           {"timeLimitNum",timeLimitNum },

           };
           roomOptions.CustomRoomProperties = customRoomProperties;*/
        var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字

        roomproperties["Stage"] = rGameboard.gameboardID;
        roomproperties["Piece"] = pieceNum;
        roomproperties["King"] = kingNum;
        roomproperties["Spy"] = spyNum;
        roomproperties["Turn"] = turnLimitNum;
        roomproperties["Time"] = timeLimitNum;
        Debug.Log("SetRoomProperty");

        /*var properties = new ExitGames.Client.Photon.Hashtable();*/

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);
        //roomOptions.CustomRoomProperties =roomproperties;



    }

    [PunRPC]
    public void SetNumProperty()
    {
        List<int> playernum = RandomPlayerList();
        Player[] playerList = PhotonNetwork.PlayerList;


        Debug.Log("playernum[0]" + playernum[0]);
        Debug.Log("playernum[1]" + playernum[1]);



        var roomproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            roomproperties[playernum[i].ToString()] = playerList[i];
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomproperties);

            var playerproperties = new ExitGames.Client.Photon.Hashtable();//全部Master
            playerproperties["num"] = playernum[i];
            playerList[i].SetCustomProperties(playerproperties);

        }

        /*var properties = new ExitGames.Client.Photon.Hashtable();*/

     

        Debug.Log("SetNumProperty");

    }



    private List<int> RandomPlayerList()
    {

        List<int> numbers = new List<int>();
        List<int> playernum = new List<int>();

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            numbers.Add(i);
        }

        for (int i = 0; numbers.Count > 0; i++)
        {

            int index = Random.Range(0, numbers.Count);
            Debug.Log("aaaaaaaaaaaaaaaaaaaaaaindex"+ index);
            playernum.Add(numbers[index]);

            numbers.RemoveAt(index);

        }

        return playernum;
    }




    [PunRPC]
    public void SetPlayerProperty()
    {
        var playerproperties = new ExitGames.Client.Photon.Hashtable();//Roomは大文字

        playerproperties["piece"] = pieceNum;
        playerproperties["score"] = 0;
        playerproperties["spy"] = spyNum;
        playerproperties["turn"] = turnLimitNum;
        playerproperties["color"] =color;
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerproperties);
        Debug.Log("SetPlayerProperty");

        //呼び出し
        //int jusin = (int)PhotonNetwork.CurrentRoom.CustomProperties["piece"];//プロパティがなかった場合エラーが出る
        //OnPhotonCustomRoomPropertiesChanged(PhotonNetwork.CurrentRoom.CustomProperties);//カスタムプロパティの変数がいる
        //OnPhotonCustomPlayerProperties();//カスタムプロパティの変数を渡す必要がない

    }

    /*private void UpdateRoomCustomProperties(string newStageName, string newStageDifficulty)
    {
        if (PhotonNetwork.InRoom)
        {
            // ステージと難易度を更新
            ExitGames.Client.Photon.Hashtable customRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            customRoomProperties["Stage"] = newStageName;
            customRoomProperties["Difficulty"] = newStageDifficulty;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
        }
    }*/


    

    /*public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable i_propertiesThatChanged)
    {
        object value = null;
        
        if (i_propertiesThatChanged.TryGetValue("Piece", out value))
        {
            Debug.Log((string)value);
        }

        if (i_propertiesThatChanged.TryGetValue("1", out value))
        {
            Debug.Log((string)value);
        }


    }*/


    //Debug
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (PhotonNetwork.IsMasterClient)
            {
              

            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PhotonNetwork.IsMasterClient) 
            {

                if (PhotonNetwork.CurrentRoom.CustomProperties["Piece"] is int a)
                {
                    Debug.Log("1.Piece " + a);
                }
                if (PhotonNetwork.CurrentRoom.CustomProperties["King"] is int b)
                {
                    Debug.Log("2.King " + b);
                }
                if (PhotonNetwork.CurrentRoom.CustomProperties["Spy"] is int c)
                {
                    Debug.Log("3.Spy " + c);
                }
                if (PhotonNetwork.CurrentRoom.CustomProperties["Turn"] is int d)
                {
                    Debug.Log("4.Turn " + d);
                }
                if (PhotonNetwork.CurrentRoom.CustomProperties["Time"] is int e)
                {
                    Debug.Log("5.Time " + e);
                }
                if (PhotonNetwork.CurrentRoom.CustomProperties["0"] is Player f)
                {
                    Debug.Log("6.Player1 " + f.NickName);
                }
                if (PhotonNetwork.CurrentRoom.CustomProperties["Stage"] is string g)
                {
                    Debug.Log("7.Stage " + g);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SetPlayerProperty();
                if (PhotonNetwork.LocalPlayer.CustomProperties["piece"] is int aa)
                {
                    Debug.Log("custom1.player "+aa);
                }
                if (PhotonNetwork.LocalPlayer.CustomProperties["score"] is int ab)
                {
                    Debug.Log("custom2.score "+ab);
                }
                if (PhotonNetwork.LocalPlayer.CustomProperties["spy"] is int ac)//??
                {
                    Debug.Log(ac);
                }
                if (PhotonNetwork.LocalPlayer.CustomProperties["turn"] is int ad)
                {
                    Debug.Log("custom3.turn " + ad);
                }
                if (PhotonNetwork.LocalPlayer.CustomProperties["color"] is string ae)
                {
                    Debug.Log("custom4.color " + ae);
                }
                if (PhotonNetwork.LocalPlayer.CustomProperties["num"] is int af)
                {
                    Debug.Log("custom5.num " + af);
                }

            }
        }

    }*/


}
