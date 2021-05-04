using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class gInstantiateController : MonoBehaviourPunCallbacks
{
    GameObject[] localPieceObj;
    GameObject[] worldPieceObj;

    GameObject gameboardObj;
 
    ScoreController scoreController;
    [SerializeField] SoundController soundController = null;
    //GameObject firstClickedObject;

    [SerializeField] GameObject PieceObj = null;
    [SerializeField] GameObject particleObj=null;
    private Vector2 localPieceObjSize;
    private Vector2 worldPieceObjSize;
    private float worldPieceObjColliderSize;

    private Sprite pieceSprite;
    private Sprite kingSprite;

    private string worldcolor1;
    private string worldcolor2;
    private string worldjob;
    private Vector2 worldpos;

    gPieceList gPieceList;

    int citizennum;
    int localObjnum = 0;
    int worldObjnum = 0;

    private void Awake()
    {
        gPieceList = GetComponent<gPieceList>();
        scoreController = GetComponent<ScoreController>();
        localPieceObjSize = new Vector2(4, 4);
    }

    public void GameboardObjSet(GameObject activeGameboardObj)
    {
        gameboardObj = activeGameboardObj;
    }



    public void LocalObjectInstantiate()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties["Piece"] is int pieceNum)
        {
            citizennum = pieceNum;
            localPieceObj = new GameObject[pieceNum];
            worldPieceObj = new GameObject[pieceNum];
            Debug.Log("StartPiece" + pieceNum);
        }




        if (PhotonNetwork.LocalPlayer.CustomProperties["color"] is string localColor1)
        {
            pieceSprite = gPieceList.PieceSprite(localColor1);

            if (PhotonNetwork.CurrentRoom.CustomProperties["Spy"] is int spyNum)
            {
                Debug.Log("SpyInstance");
                citizennum -= spyNum;
                for (int i = 0; i < spyNum; i++)
                {
                    Debug.Log("SpyInstance " + localObjnum);
                    localPieceObj[localObjnum] = Instantiate(PieceObj);
                    localPieceObj[localObjnum].transform.SetParent(GameObject.Find("Canvas").transform, false);
                    localPieceObj[localObjnum].transform.localPosition = new Vector3(270 + (50 * (localObjnum % 3)), -200 + 50 * (localObjnum / 3), 0);
                    localPieceObj[localObjnum].transform.localScale = localPieceObjSize;

                    localPieceObj[localObjnum].GetComponent<SpriteRenderer>().sprite = pieceSprite;
                    localPieceObj[localObjnum].GetComponent<LocalAttributeList>().JobSettings("spy");
                    localPieceObj[localObjnum].GetComponent<LocalAttributeList>().ColorSettings1(localColor1);

                    if (PhotonNetwork.LocalPlayer.CustomProperties["num"] is int localPlayerNum)
                    {
                        Debug.Log("localPlayerNum"+ localPlayerNum);
                        Debug.Log("spyColorNum"+ (localPlayerNum + 1 + i) % PhotonNetwork.CurrentRoom.PlayerCount);
                        string spyColornum = ((localPlayerNum + 1 + i) % PhotonNetwork.CurrentRoom.PlayerCount).ToString();
                        if (PhotonNetwork.CurrentRoom.CustomProperties[spyColornum] is Player spyColorPlayer)
                        {
                            Debug.Log(spyColorPlayer.NickName);
                            if (spyColorPlayer.CustomProperties["color"] is string localColor2)
                            {
                                Debug.Log(localColor2);
                                Transform pos = localPieceObj[localObjnum].transform.Find("Sprite2");
                                pos.gameObject.GetComponent<SpriteRenderer>().sprite = gPieceList.SpySprite(localColor2);

                                localPieceObj[localObjnum].GetComponent<LocalAttributeList>().ColorSettings2(localColor2);

                            }
                        }
                    }
                    localObjnum++;
                }
            }


            if (PhotonNetwork.CurrentRoom.CustomProperties["King"] is int kingNum)
            {

                kingSprite = gPieceList.KingSprite(localColor1);
                Debug.Log("KingInstance");
                citizennum -= kingNum;
                for (int i = 0; i < kingNum; i++)
                {
                    Debug.Log("KingInstance" + localObjnum);
                    localPieceObj[localObjnum] = Instantiate(PieceObj);
                    localPieceObj[localObjnum].transform.SetParent(GameObject.Find("Canvas").transform, false);
                    localPieceObj[localObjnum].transform.localPosition = new Vector3(270 + (50 * (localObjnum % 3)), -200 + 50 * (localObjnum / 3), 0);
                    localPieceObj[localObjnum].transform.localScale = localPieceObjSize;

                    localPieceObj[localObjnum].GetComponent<SpriteRenderer>().sprite = kingSprite;

                    localPieceObj[localObjnum].GetComponent<LocalAttributeList>().JobSettings("king");
                    localPieceObj[localObjnum].GetComponent<LocalAttributeList>().ColorSettings1(localColor1);
                    localPieceObj[localObjnum].GetComponent<LocalAttributeList>().ColorSettings2(localColor1);

                    localObjnum++;
                }
            }


            Debug.Log("PieceInstance");
            for (int i = 0; i < citizennum; i++)
            {
                Debug.Log("PieceInstance" + localObjnum);
                localPieceObj[localObjnum] = Instantiate(PieceObj);
                //localPieceObj[localObjnum] = Instantiate(PieceObj, new Vector2(0, 84 - 50 * localObjnum), new Quaternion(0, 90, 0, 0));

                localPieceObj[localObjnum].transform.SetParent(GameObject.Find("Canvas").transform, false);
                localPieceObj[localObjnum].transform.localPosition = new Vector3(270 + (50 * (localObjnum % 3)), -200 + 50 * (localObjnum / 3), 0);
                localPieceObj[localObjnum].transform.localScale = localPieceObjSize;

                localPieceObj[localObjnum].GetComponent<SpriteRenderer>().sprite = pieceSprite;

                localPieceObj[localObjnum].GetComponent<LocalAttributeList>().JobSettings("citizen");
                localPieceObj[localObjnum].GetComponent<LocalAttributeList>().ColorSettings1(localColor1);
                localPieceObj[localObjnum].GetComponent<LocalAttributeList>().ColorSettings2(localColor1);

                localObjnum++;
            }
            localObjnum--;



        }

    }

    public void TimeOverDestroy1()
    {
        for (; 0 <= localObjnum; localObjnum--)
        {
            if (localPieceObj[localObjnum].activeSelf)
            {
                localPieceObj[localObjnum].SetActive(false);
                break;
            }
        }
    }


    public void WorldObjectInstantiate(GameObject firstClicked, GameObject secondClicked)//RPCにしたほうがいい？
    {
        string job = firstClicked.GetComponent<LocalAttributeList>().job;
        string color1 = firstClicked.GetComponent<LocalAttributeList>().color1;
        string color2 = firstClicked.GetComponent<LocalAttributeList>().color2;
        float posx = secondClicked.transform.localPosition .x;
        float posy = secondClicked.transform.localPosition.y;

        firstClicked.SetActive(false);//最後

        photonView.RPC("WorldObjectSettings", RpcTarget.AllViaServer, color1, color2, job,posx,posy);

    }

    [PunRPC]
    public void WorldObjectSettings(string color1,string color2,string job,float posx,float posy)
    {
        worldcolor1 = color1;
        worldcolor2 = color2;
        worldjob = job;
        worldpos = new Vector2(posx,posy);
        if (PhotonNetwork.LocalPlayer.CustomProperties["color"]is string mycolor)
        {
            if (worldcolor1== mycolor) 
            {
                RPCInstantiate();
            }
        }
    }

    public void RPCInstantiate()
    {
        Debug.Log(worldObjnum+"worldPieceObjInstantiate!!!!!!!!!!!!");
        worldPieceObj[worldObjnum] = PhotonNetwork.Instantiate("WorldPieceObj", new Vector3(0, 0, 0), Quaternion.identity, 0);
        worldObjnum++;
    }

    public void WorldObjectSettings(GameObject piece)
    {
        Debug.Log(worldcolor1);
        Debug.Log(worldcolor2);
        Debug.Log(worldjob);
        Debug.Log(worldpos);
        Debug.Log(piece);
        Debug.Log(gameboardObj);

        piece.transform.SetParent(gameboardObj.transform, false);
        piece.transform.localPosition = worldpos;

        if (PhotonNetwork.CurrentRoom.CustomProperties["Stage"] is string ID)
        {
            worldPieceObjSize = gPieceList.WorldPieceSize(ID);
            worldPieceObjColliderSize = gPieceList.WorldPieceColliderSize(ID);
        }
        piece.transform.localScale = worldPieceObjSize;
        piece.GetComponent<CircleCollider2D>().radius = worldPieceObjColliderSize;
        if (worldjob == "king")
        {
            piece.GetComponent<SpriteRenderer>().sprite = gPieceList.KingSprite(worldcolor1);
        }
        else
        {
            piece.GetComponent<SpriteRenderer>().sprite = gPieceList.PieceSprite(worldcolor1);
        }

        if (worldjob == "spy")
        {
            Transform pos1 = piece.transform.Find("Sprite2");
            pos1.gameObject.GetComponent<SpriteRenderer>().sprite = gPieceList.SpySprite(worldcolor2);
        }

        piece.GetComponent<AttributeList>().JobSettings(worldjob);
        piece.GetComponent<AttributeList>().ColorSettings1(worldcolor1);
        piece.GetComponent<AttributeList>().ColorSettings2(worldcolor2);

    }

    public void TurnOverDestroy2()
    {
        soundController.TurnLimitOverSE();
        for (int i= worldObjnum-1; 0 <=i; i--)
        {
            if (worldPieceObj[i]!=null)
            {
                PhotonNetwork.Destroy(worldPieceObj[i]);
                soundController.DestroyEffectSE();
                particleObj.transform.localPosition = new Vector3(worldPieceObj[i].transform.localPosition.x, worldPieceObj[i].transform.localPosition.y, 120);
                particleObj.GetComponent<gDestroyEffectController>().DestroyObjectEffect();//Destroy()エフェクト

                Debug.Log("TurnOverDestroy");
                scoreController.OtherPieceUpdate(PhotonNetwork.LocalPlayer);
                scoreController.PieceSet();
                break;
            }
        }
        
    }

}
