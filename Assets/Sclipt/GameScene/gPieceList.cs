using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gPieceList : MonoBehaviour
{

    public Vector2 WorldPieceSize(string ID)
    {
        switch (ID)
        {
            case "A1":
                return new Vector2(0.008f, 0.008f);
            case "A2":
                return new Vector2(0.007f, 0.007f);
            case "A3":
                return new Vector2(0.008f, 0.008f);
            case "B1":
                return new Vector2(0.008f, 0.008f);
            case "B2":
                return new Vector2(0.008f, 0.008f);
            default:
                Debug.Log("Stage NotFound");
                return new Vector2(0, 0);
        }

    }

    public float WorldPieceColliderSize(string ID)
    {
        switch (ID)
        {
            case "A1":
                return 6.5f;
            case "A2":
                return 8.57f;
            case "A3":
                return 7.6f;
            case "B1":
                return 6.5f;
            case "B2":
                return 6.5f;
            default:
                Debug.Log("Stage NotFound");
                return 0.0f;
        }

    }

    public float WorldPieceJudgeColliderSize(string ID)//inspector:info.extent(script:bounds.size??)
    {
        switch (ID)
        {
            case "A1":
                return 78.0f;
            case "A2":
                return 91.2f;
            case "A3":
                return 78.0f;
            case "B1":
                return 78.0f;
            case "B2":
                return 90.0f;
            default:
                Debug.Log("Stage NotFound");
                return 0.0f;
        }

    }



    [SerializeField] Sprite[] sprite = new Sprite[11];
    [SerializeField] Sprite[] spriteK = new Sprite[11];
    [SerializeField] Sprite[] spriteS = new Sprite[11];


    public Sprite PieceSprite(string color)
    {
        switch (color)
        {
            case "red":
                return sprite[0];
            case "blue":
                Debug.Log("bluePieceSprite!");
                return sprite[1];
            case "yellow":
                Debug.Log("yellowPieceSprite!");
                return sprite[2];
            case "green":
                return sprite[3];
            case "brown":
                return sprite[4];
            case "purple":
                return sprite[5];
            case "lightblue":
                return sprite[6];
            case "black":
                return sprite[7];
            case "pink":
                return sprite[8];
            case "lightgreen":
                return sprite[9];
            case "bluegreen":
                return sprite[10];
            default:
                Debug.Log("Sprite NotFound");
                return null;
        }

    }


    public Sprite SpySprite(string color)
    {
        switch (color)
        {
            case "red":
                return spriteS[0];
            case "blue":
                return spriteS[1];
            case "yellow":
                return spriteS[2];
            case "green":
                return spriteS[3];
            case "brown":
                return spriteS[4];
            case "purple":
                return spriteS[5];
            case "lightblue":
                return spriteS[6];
            case "black":
                return spriteS[7];
            case "pink":
                return spriteS[8];
            case "lightgreen":
                return spriteS[9];
            case "bluegreen":
                return spriteS[10];
            default:
                Debug.Log("SpriteS NotFound");
                return null;
        }


    }


    public Sprite KingSprite(string color)
    {
        switch (color)
        {
            case "red":
                return spriteK[0];
            case "blue":
                return spriteK[1];
            case "yellow":
                return spriteK[2];
            case "green":
                return spriteK[3];
            case "brown":
                return spriteK[4];
            case "purple":
                return spriteK[5];
            case "lightblue":
                return spriteK[6];
            case "black":
                return spriteK[7];
            case "pink":
                return spriteK[8];
            case "lightgreen":
                return spriteK[9];
            case "bluegreen":
                return spriteK[10];
            default:
                Debug.Log("SpriteK NotFound");
                return null;
        }
    }



}
