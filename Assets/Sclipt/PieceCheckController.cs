using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCheckController : MonoBehaviour
{
    StatusColor statusColor;
    
    StatusJob statusJob;
    
    string color1;
    string color2;
    string job1;
    private void Start()
    {
        statusColor = GetComponent<StatusColor>();
        statusJob = GetComponent<StatusJob>();
    }
    public string ColorCheck1(string piece)
    {
        switch (piece)
        {
            case "player1_citizen":
                color1 = statusColor.Player1();
                break;
            case "player1_king":
                color1= statusColor.Player1();
                break;
            case "player1_spy1":
                color1 = statusColor.Player1();
                break;
            case "player1_spy2":
                color1 = statusColor.Player1();
                break;
            case "player2_citizen":
                color1 = statusColor.Player1();
                break;
            case "player2_king":
                color1 = statusColor.Player1();
                break;
            case "player2_spy1":
                color1 = statusColor.Player1();
                break;
            case "player2_spy2":
                color1 = statusColor.Player1();
                break;
            case "player3_citizen":
                color1 = statusColor.Player1();
                break;
            case "player3_king":
                color1 = statusColor.Player1();
                break;
            case "player3_spy1":
                color1 = statusColor.Player1();
                break;
            case "player3_spy2":
                color1 = statusColor.Player1();
                break;


            default:
                color1 = null;
                break;
        }
        return color1;
    }

    public string ColorCheck2(string piece)
    {
        switch (piece)
        {
            case "player1_citizen":
                color2 = statusColor.Player1();
                break;
            case "player1_king":
                color2 = statusColor.Player1();
                break;
            case "player1_spy1":
                color2 = statusColor.Player1();
                break;
            case "player1_spy2":
                color2 = statusColor.Player1();
                break;

            default:
                color2 = null;
                break;
        }

        return color2;

    }

    public string JobCheck(string piece)
    {
        switch (piece)
        {
            case "player1_citizen":
                job1 = statusJob.Job1();
                break;
            case "player1_king":
                job1 = statusJob.Job1();
                break;
            case "player1_spy1":
                job1 = statusJob.Job1();
                break;
            case "player1_spy2":
                job1 = statusJob.Job1();
                break;

             default:
                job1 = null;
                break;
        }

        return job1;
    }


}
