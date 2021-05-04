using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Net.Http.Headers;

public class ScoreSortScript : MonoBehaviour
{
    [SerializeField] GameObject scripts = null;
    Gamemanager gamemanager;
    [SerializeField] Text[] playerNameText = new Text[8];
    [SerializeField] Text[] playerNumText = new Text[8];
    [SerializeField] Image[] playerColorImage = new Image[8];
    [SerializeField] Text[] scoreText = new Text[8];


    private int startPlayer;

    private int tmp1;
    private string tmp2;
    private Color tmp3;
    private int rank;

    private void Awake()
    {
        gamemanager = scripts.GetComponent<Gamemanager>();
        startPlayer = gamemanager.startPlayerCount;
    }

    public void PlayerScoreSort(int[] scoreSort)
    {
        scripts.SetActive(false);
        for (int i = 0; i < gamemanager.startPlayerCount; i++)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties[i.ToString()] is Player player)
            {
                WinnerSet(player, null);
            }
        }

        {
            Debug.Log("Sort!!!!!!!!!!" + startPlayer);
            int maxScore = 0;

            for (int i = 0; i < startPlayer; i++)
            {
                Debug.Log("scoreSortaaaaaaaaaaaaaaaaaaaaaa:" + i + scoreSort[i]);
            }
            for (int i = 0; i < startPlayer; i++)//Text代入のためCount-1なし
            {
                Debug.Log("Sort:" + i);
                for (int j = i + 1; j < startPlayer; j++)
                {
                    if (scoreSort[i] < scoreSort[j])
                    {
                        tmp1 = scoreSort[j];
                        tmp2 = playerNameText[j].text;
                        tmp3 = playerColorImage[j].color;

                        scoreSort[j] = scoreSort[i];
                        playerNameText[j].text = playerNameText[i].text;
                        playerColorImage[j].color = playerColorImage[i].color;

                        scoreSort[i] = tmp1;
                        playerNameText[i].text = tmp2;
                        playerColorImage[i].color = tmp3;

                    }
                    //scoreTextObj[i].SetActive(false);
                    Debug.Log("scoreSort :i: " + scoreSort[i]);

                }

                Debug.Log("scoreSort1: " + scoreSort[i]);
                Debug.Log("scoreText1:" + scoreText[i].text);
                Debug.Log("Set:" + i);

                Debug.Log("scoreSort2: " + scoreSort[i]);
                Debug.Log("scoreText2:" + scoreText[i].text);
                if (scoreSort[i] < maxScore || i == 0)
                {
                    Debug.Log("MaxScoreUpdate");/////////////////////////////////////////////////////////
                    maxScore = scoreSort[i];
                    rank = i + 1;
                }

                if (scoreSort[i] == maxScore)///////////////////////////////////////////////////////////else
                {
                    playerNumText[i].text = rank.ToString();

                    if (rank == 1 && playerNameText[i].text.ToString() == PhotonNetwork.LocalPlayer.NickName)
                    {
                        Debug.Log("WinnerSet");
                        WinnerSet(PhotonNetwork.LocalPlayer, "win");
                    }
                }

                Debug.Log("scoreSort3:" + scoreSort[i]);
                Debug.Log("scoreText3:" + scoreText[i].text);
                scoreText[i].text = scoreSort[i].ToString();
                Debug.Log("scoreSort4:" + scoreSort[i]);
                Debug.Log("scoreText4:" + scoreText[i].text);

            }

        }      

    }
    private void WinnerSet(Player player, string result)
    {
        var playerproperties = new ExitGames.Client.Photon.Hashtable();
        playerproperties["win"] = result;
        player.SetCustomProperties(playerproperties);
    }
}
