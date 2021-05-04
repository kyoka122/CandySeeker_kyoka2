using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

public class gUIController : MonoBehaviourPunCallbacks
{
    [SerializeField] Slider TimerFillSlider = null;//タイマーの赤い部分
    [SerializeField] Text timeText = null;//残り時間の表示テキスト

    [SerializeField] GameObject[] playerList=new GameObject[8];
    [SerializeField] Image[] playerNumColor = new Image[8];
    [SerializeField] Text[] playerNameText=new Text[8];
    [SerializeField] GameObject scoreList=null;

    [SerializeField] GameObject[] scoreTextObj = new GameObject[8];
    [SerializeField] GameObject[] pieceNumTextObj = new GameObject[8];
    [SerializeField] GameObject[] turnLimitTextObj = new GameObject[8];
    [SerializeField] GameObject clickControllerObj = null;
    [SerializeField] Text nextMyTurn=null;
    [SerializeField] GameObject Canvas_Text = null;
    [SerializeField] Text myScoreText = null;

    //[SerializeField] GameObject scoreFilterObj = null;

    Text[] scoreText=new Text[8];
    Text[] pieceNumText = new Text[8];
    Text[] turnLimitText = new Text[8];

    [SerializeField] Text turnCountText=null;

    [SerializeField] GameObject text_score=null;//"得点"Text
    [SerializeField] GameObject text_other=null;//それ以外
    [SerializeField] GameObject myTurnObject = null;
    [SerializeField] GameObject turnController1Obj = null;
    [SerializeField] GameObject turnController2Obj = null;
    [SerializeField] GameObject mobileButtonObj = null;
    [SerializeField] GameObject cameraPCObj = null;
    private int maxPlayer = 8;
    int nextMyTurnCount = 0;
    private int timeLimit=0;
    [SerializeField] GameObject DisConnectedFilter=null;
    //[SerializeField] GameObject turnchangefilter=null;

    ColorList colorList;
    Gamemanager gamemanager;
    gTurnController1 turnController1;
    gTurnController2 turnController2;
    //[SerializeField] Text 


    private void Awake()
    {
        colorList = GetComponent<ColorList>();
        gamemanager = GetComponent<Gamemanager>();
        turnController1 = turnController1Obj.GetComponent<gTurnController1>();
        turnController2 = turnController2Obj.GetComponent<gTurnController2>();
        for (int i=0; i< maxPlayer; i++) 
        {
            pieceNumText[i] = pieceNumTextObj[i].GetComponent<Text>();
            scoreText[i] = scoreTextObj[i].GetComponent<Text>();            
            turnLimitText[i] = turnLimitTextObj[i].GetComponent<Text>();
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties["Time"] is int time)
        {
            timeLimit = time;
        }

    }


    //Pun監視つける

    public void PlayerList()//Awakeから呼び出し
    {
        int playercount = PhotonNetwork.CurrentRoom.PlayerCount;
        for (int i = 0; i < playercount; i++)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties[i.ToString()] is Player customPlayer)
            {
                playerList[i].SetActive(true);
                playerNameText[i].text = customPlayer.NickName;

                if (customPlayer.CustomProperties["color"] is string customColor) 
                {
                    playerNumColor[i].color = colorList.NameListColor(customColor);
                }
            }      
        }
        /*for (int i=playercount; i<maxPlayer;i++)
        {
            playerList[i].SetActive(false);
        }*/

        if (pieceNumText[0].text == 0.ToString())//大丈夫？？
        {

            if (PhotonNetwork.CurrentRoom.CustomProperties["Piece"] is int pieceNum)
            {
                for (int i = 0; i < gamemanager.startPlayerCount; i++)
                {
                    pieceNumText[i].text = pieceNum.ToString();
                }
            }
            if (PhotonNetwork.CurrentRoom.CustomProperties["Turn"] is int turnLimitNum)
            {
                for (int i = 0; i < gamemanager.startPlayerCount; i++)
                {
                    turnLimitText[i].text = turnLimitNum.ToString();
                }
            }
        }
    }
    

    public override void OnPlayerPropertiesUpdate(Player player, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (player.CustomProperties["num"]is int num) 
        {
            if (player.CustomProperties["score"] is int score)
            {
                Debug.Log("ScoreCustomPropertiesUpdate!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                scoreText[num].text = score.ToString();
                if (player.IsLocal)
                {
                    myScoreText.text = score.ToString();
                }
                
            }
            if (player.CustomProperties["piece"] is int piece)
            {
                Debug.Log("gUIPieceNum:"+piece);
                pieceNumText[num].text = piece.ToString();

                if (player==PhotonNetwork.LocalPlayer)
                {
                    if(turnController2Obj.activeSelf == true)
                    {
                        turnController2.PieceLimitJudge();
                    }
                }
           
            }
            if (player.CustomProperties["turn"] is int turn)
            {
                turnLimitText[num].text = turn.ToString();

                if (player == PhotonNetwork.LocalPlayer)
                {
                    if(turnController2Obj.activeSelf == true)
                    {
                        turnController2.TurnOverJudge();
                    }
                }
            }

        }
    }
    public void TurnCountTextChange()//アニメーションから呼び出し
    {
        int turnCount = (gamemanager.movePlayerJudge - 1) / gamemanager.startPlayerCount + 1;
        Debug.Log("turnCount:"+ turnCount);
        turnCountText.text = "ターン" + turnCount;
    }

    [PunRPC]
    public void NextMyTurnSet()
    {
        nextMyTurnCount = gamemanager.playerNum-(gamemanager.movePlayerJudge - 1) % gamemanager.startPlayerCount;
        if (nextMyTurnCount<0)
        {
            nextMyTurnCount = gamemanager.startPlayerCount + nextMyTurnCount;
        }
        nextMyTurn.text = "あなたの番まであと"+ nextMyTurnCount+ "ターン";
    }

    public void MyTurnObject(int movePlayerNum)
    {
        myTurnObject.transform.localPosition = new Vector2(-160, 120 - 40 * movePlayerNum);
    }

    [PunRPC]
    public void TimeReset()
    {
        TimerFillSlider.value = timeLimit;
        timeText.text = "next";
    }


    public void ScoreListPosition()
    {
        scoreList.transform.SetParent(Canvas_Text.transform);
        scoreList.transform.SetAsLastSibling();
        scoreList.transform.localPosition = new Vector2(0, 0);
        text_score.GetComponent<Text>().text = "最終pt";
        text_score.transform.Translate(-10.0f, 0, 0);
        text_other.SetActive(false);
        myTurnObject.SetActive(false);
        for (int i = 0; i < gamemanager.startPlayerCount; i++) 
        {
            pieceNumTextObj[i].SetActive(false);
            turnLimitTextObj[i].SetActive(false);
        }
    }


    public void LeftRoom()
    {
        DisConnectedFilter.SetActive(true);
    }

    public void BacktoStartButton()
    {
        SceneManager.LoadScene("GameStartScene");
    }
    public void TurnSkipButton()
    {
        if (clickControllerObj.activeSelf)
        {
            if (turnController1Obj.activeSelf)
            {
                turnController1.OnTurnTimeEnds();
            }
            else
            {
                turnController2.OnTurnTimeEnds();
            }
        }
    }

    public void MobileChangeButtonOn()
    {
        if (cameraPCObj.activeSelf)
        {
            mobileButtonObj.SetActive(true);
            cameraPCObj.SetActive(false);
        }
        else
        {
            mobileButtonObj.SetActive(false);
            cameraPCObj.SetActive(true);
        }
        
        
    }

}







