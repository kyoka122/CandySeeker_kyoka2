using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class ClickController : MonoBehaviour
{
    GameObject firstClickedObject=null;
    GameObject secondClickedObject=null;

    [SerializeField] GameObject turnController1Obj = null;
    [SerializeField] GameObject turnController2Obj = null;
    [SerializeField] GameObject scripts=null;
    [SerializeField] GameObject clickedMark = null;
    [SerializeField] GameObject clickedMark_Canvas = null;
    [SerializeField] SoundController soundController = null;
    [SerializeField] GameObject canvas = null;

    DestroyJudge destroyJudge;
    gTurnController1 turnController1;
    gTurnController2 turnController2;
    gPieceList gPieceList;
    PhotonView photonView;

    bool piececlick = false;
    private bool firstclick = false;
    string hitObjColor;
    float colliderSize;

    Player moveObjOwner;
    int moveObjOwnernum;
    int moveObjID;
    private float posx;
    private float posy;
    
    bool aa=true;
    private void Awake()
    {  
        destroyJudge = scripts.GetComponent<DestroyJudge>();
        turnController1= turnController1Obj.GetComponent<gTurnController1>();
        turnController2= turnController2Obj.GetComponent<gTurnController2>();
        photonView = GetComponent<PhotonView>();
        gPieceList = scripts.GetComponent<gPieceList>();
        if (PhotonNetwork.CurrentRoom.CustomProperties["Stage"]is string ID) 
        {
            colliderSize = gPieceList.WorldPieceJudgeColliderSize(ID);
            Debug.Log("colliderSize:"+ colliderSize);
        }
    }
 

    private void OnEnable()
    {
        piececlick = false;
        firstclick = false;
        firstClickedObject = null;
        secondClickedObject = null;
        hitObjColor = null;
        aa = true;
    }



    //ローカルで処理（SetActiveだから多分大丈夫）
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);//指定する？？
            RaycastHit2D[] hit2d = Physics2D.RaycastAll(ray.origin, ray.direction);
            int hit2Dlen = hit2d.Length;
            Debug.Log("hit2Dlen:"+hit2Dlen);
            //無理そうだったらレイヤーで
            for (int i = 0; i < hit2Dlen; i++)
            {
                Debug.Log("hit2d:" + hit2d[i]);
                if (hit2d[i].collider != null && hit2d[i].transform.gameObject.CompareTag("Piece"))
                {
                    aa = false;
                    if (PhotonNetwork.LocalPlayer.CustomProperties["color"] is string mycolor)
                    {

                        if (turnController1Obj.activeSelf&&hit2d[i].collider.gameObject.transform.parent.gameObject==canvas)
                        {
                            if (hit2d[i].transform.gameObject.GetComponent<LocalAttributeList>())
                            {
                                hitObjColor = hit2d[i].transform.gameObject.GetComponent<LocalAttributeList>().color1;
                                firstclick = true;
                            }

                        }
                        else if(turnController2Obj.activeSelf)
                        {
                            if (hit2d[i].transform.gameObject.GetComponent<AttributeList>())
                            {
                                hitObjColor = hit2d[i].transform.gameObject.GetComponent<AttributeList>().color1;
                                firstclick = true;
                            }
                        }
                        Debug.Log(mycolor);
                       
                        if (hitObjColor == mycolor&& firstclick == true)
                        {
                            firstClickedObject = hit2d[i].transform.gameObject;//////////////////////////////////////////////////////////////////
                            Debug.Log("firstClickedObject" + firstClickedObject);
                            if (turnController1Obj.activeSelf)
                            {
                                clickedMark_Canvas.SetActive(true);
                                clickedMark_Canvas.transform.localPosition = firstClickedObject.transform.localPosition;
                                soundController.Click1SE();
                            }
                            else
                            {
                                clickedMark.SetActive(true);
                                clickedMark.transform.localPosition = firstClickedObject.transform.localPosition;
                                soundController.Click1SE();
                            }
                            //clickedMark.transform.localPosition = firstClickedObject.transform.localPosition;
                            piececlick = true;         
                        }
                        firstclick = false;

                    }

                }
            }
            for (int i = 0; i < hit2Dlen; i++)
            {
                if (aa && (hit2d[i].transform.gameObject.CompareTag("Field") || hit2d[i].transform.gameObject.CompareTag("Edge"))) // 駒の次にクリックしたのがフィールド上(+Edge)のマスだったら
                {
                    secondClickedObject = hit2d[i].transform.gameObject;
                    Debug.Log("secondClickedObject" + secondClickedObject.transform.localPosition);
                    if (piececlick) //事前に駒をクリックしていたら
                    {
                        if (turnController1Obj.activeSelf)//turnController1
                        {
                            piececlick = false;
                            clickedMark_Canvas.SetActive(false);
                            soundController.PutCandySE();
                            turnController1.InstantiateObject(firstClickedObject, secondClickedObject);/////////////////////////////////////
                        }
                        else//turnController2
                        {
                            Debug.Log(secondClickedObject);
                            piececlick = false;

                            Collider2D[] colList = Physics2D.OverlapCircleAll(firstClickedObject.transform.position, colliderSize);//y軸の値調整、物体の基準を下に合わせる?//z軸関係ない…よね？
                            int colListlen = colList.Length;
                            Debug.Log("colListlen" + colListlen);



                            for (int j = 0; j < colListlen; j++)
                            {
                                Debug.Log("CickcontrollerSecondjudge");
                                Debug.Log("colList:" + colList[j].gameObject.name);

                                if (secondClickedObject == colList[j].gameObject)
                                {
                                    moveObjID = firstClickedObject.GetComponent<PhotonView>().ViewID;
                                    posx = secondClickedObject.transform.localPosition.x;
                                    posy = secondClickedObject.transform.localPosition.y;//local

                                    photonView.RPC("MoveReq", RpcTarget.AllViaServer, moveObjID, posx, posy);
                                    Debug.Log("DestroyJudgeGo!!");
                                    clickedMark.SetActive(false);
                                    soundController.PutCandySE();
                                    destroyJudge.Judge(firstClickedObject, secondClickedObject);/////////////////////////////////////
                                }
                            }

                        }

                    }
                }



            }
            aa = true;

        }
    }

    [PunRPC]
    public void MoveReq(int moveObjID,float posx,float posy)
    {
        Debug.Log(moveObjID);
         PhotonView.Find(moveObjID).transform.localPosition = new Vector2(posx,posy);//駒移動


    }


}
