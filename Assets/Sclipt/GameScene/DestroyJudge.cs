using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyJudge : MonoBehaviourPunCallbacks//,IPunOwnershipCallbacks
{
    [SerializeField] GameObject clickControllerObj = null;
    [SerializeField] GameObject turnController2Obj = null;
    [SerializeField] GameObject destroyCheckObj=null;
    gTurnController2   turnController2;
    PhotonView photonViewTurn2;
    //PhotonView photonView;
    gPieceList gPieceList;
    ScoreController scoreController;
    DestroyCheck destroyCheck;
    float colliderSize;


    //bool destroy = false;
    int destroyObjOwnernum;
    int destroyObjID;
    Player destroyObjOwner;

    //for文の初期値iに直す
    private void Awake()
    {
        turnController2 = turnController2Obj .GetComponent<gTurnController2>();
        //photonView = GetComponent<PhotonView>();
        gPieceList = GetComponent<gPieceList>();
        photonViewTurn2 = turnController2Obj.GetComponent<PhotonView>();
        scoreController = GetComponent<ScoreController>();
        destroyCheck = destroyCheckObj.GetComponent<DestroyCheck>();
    }

    public void ColliderSizeSet(string ID)
    { 
        colliderSize = gPieceList.WorldPieceJudgeColliderSize(ID);
    }


    public void Judge(GameObject firstClickedObject, GameObject secondClickedObject)
    {
        clickControllerObj.SetActive(false);
        Debug.Log("DestroyJudge!!!");
        Debug.Log("ClickControllerObj!false");
        //destroy = false;
        turnController2.MyTurnLimitDown();//falseの後
        Debug.Log("firstfor");
        Collider2D[] colList = Physics2D.OverlapCircleAll(secondClickedObject.transform.position, colliderSize);//firstClickedObjectは間に合わない
        int colListlen = colList.Length;
        destroyCheckObj.SetActive(true);

        for (int i = 0; i < colListlen; i++)//動かした駒周りのコライダーについて
        {
            Collider2D col1 = colList[i];
            Debug.Log("col1:"+i +":"+col1) ;

            string piece1color1 = firstClickedObject.GetComponent<AttributeList>().color1;

            if (col1.gameObject.CompareTag("Piece") && (col1.gameObject != firstClickedObject))//自分は判定に入れない１//ここに当てはまらなかったら次のListの配列へ（隣り合う駒が0）
            {
                Collider2D[] edgecolList = Physics2D.OverlapCircleAll(col1.gameObject.transform.position, 30.0f);
                int edgecolListlen = edgecolList.Length;
                for(int o=0; o<edgecolListlen; o++)
                {
                    Debug.Log(edgecolList[o].gameObject);
                    if (edgecolList[o].gameObject.CompareTag("Edge"))//追加
                    {
                        Debug.Log("+CompareTag(Edge).col1" + col1);

                        Debug.Log("edgecolList.Length" + edgecolList.Length);

                        if (ColorJudge(col1, piece1color1))
                        {
                            Collider2D[] colList2 = Physics2D.OverlapCircleAll(col1.gameObject.transform.position, colliderSize);//挟まれる駒と接触するコライダー
                            EdgeSecondJudge(col1, piece1color1, colList2);
                        }

                    }
                }
                
                //FieldもEdgeも両方判定！

                    Debug.Log("piece1color1" + firstClickedObject.name);

                    if (ColorJudge(col1, piece1color1))
                    {
                        Collider2D[] colList2 = Physics2D.OverlapCircleAll(col1.gameObject.transform.position, colliderSize);//挟まれる駒と接触するコライダー
                        FieldSecondJudge(col1, colList, colListlen, piece1color1, colList2);
                    }

                
               
            }
            
        }
        destroyCheck.destroyJudgeFinish = true;
       
        Debug.Log("################################################");

    }

    private bool ColorJudge(Collider2D col1,string piece1color1)
    {
        string piece2color1 = col1.gameObject.GetComponent<AttributeList>().color1;
        //Debug.Log("piece1color1"+ piece1color1);
        Debug.Log("piece2color1"+ col1.name);
        if (piece1color1 != piece2color1)//隣合う駒の色が違ったら//ここに当てはまらなかったら次のListの配列へ（隣り合う色違いの駒が0）
        {
            //Debug.Log("true!!");
            return true;
        }
        else
        {
            //Debug.Log("false");
            return false;
        }
    }



    
    //むり
    private void EdgeSecondJudge(Collider2D col1, string piece1color1, Collider2D[] colList2)
    {
        int collist2len = colList2.Length;

        for (int j = 0; j < collist2len; j++)//挟まれる駒それぞれに対して挟む駒の判定をしていく
        {
            Collider2D col2 = colList2[j];

            //挟む駒判定//////////////////////////////////
            if (col2 != col1 && col2.gameObject.CompareTag("Piece"))//自分は判定に入れない２
            {
                Debug.Log("EdgeSecondJudge");
                //色判定２//////////////////////////////
                string piece3color1 = col2.gameObject.GetComponent<AttributeList>().color1;
                string piece3color2 = col2.gameObject.GetComponent<AttributeList>().color2;
                if (piece1color1 == piece3color1 || piece1color1 == piece3color2)//挟む色が同じだったら(スパイ含む)
                {
                    Debug.Log("EdgeSecondJudge(color)");
                    Debug.Log("col1:"+col1);
                    Debug.Log("col2:"+col2);
                    //int viewID = col1.gameObject.GetComponent<PhotonView>().ViewID;
                    string destroyObjJob = col1.gameObject.GetComponent<AttributeList>().job;
                    float posx = col1.transform.position.x;
                    float posy = col1.transform.position.y;
                    destroyObjOwner = col1.GetComponent<PhotonView>().Owner;
                    if (destroyObjOwner.CustomProperties["num"] is int num)
                    {
                        destroyObjOwnernum = num;
                    }
                    destroyObjID = col1.GetComponent<PhotonView>().ViewID;
                    Debug.Log("EdgeDestroy");
                    photonView.RPC("DestroyReq", RpcTarget.AllViaServer, destroyObjOwnernum, destroyObjID, destroyObjJob,posx,posy);
                    destroyCheck.destroyFinish = false;
                    //destroy = true;
                    //return;////////////////////////////////////////////////////////////////////////////////////////
                }
            }
        }
    }






    private void FieldSecondJudge(Collider2D col1, Collider2D[] colList, int colListlen,string piece1color1,Collider2D[] colList2)
    {
        int collist2len = colList2.Length;

        for (int j = 0; j < collist2len; j++)//挟まれる駒"それぞれ"に対して挟む駒の判定をしていく=2重for文
        {
            if (colList2[j].gameObject != col1.gameObject) //自分は判定に入れない２
            {
                Debug.Log("FieldSecondJudge");
                //Lineを見つける//////////////////////////////////////////
                for (int k = 0; k < colListlen; k++)
                {
                    //Debug.Log("colList[j]" +"j="+j +colList2[j]);
                    //Debug.Log("colList[k]" + "k="+k+colList[k]);
      
                    //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!colList[k+1]" + colList[k+1]);

                    if (colList[k].gameObject.CompareTag("Line") && colList2[j].gameObject.CompareTag("Line"))
                    {
                        if (colList[k] == colList2[j])
                        {
                            Debug.Log("LineJudge");
                            Collider2D linecol = colList[k];

                            FieldThirdJudge(col1, colList2, piece1color1, linecol, collist2len);
                            return;/////////////////////////////////////////////////////////////////////////////////////////////////
                        }
                    }
                }
            }
        }

    }


    private void FieldThirdJudge(Collider2D col1, Collider2D[] colList2, string piece1color1, Collider2D linecol,int collist2len)
    {
        //挟む駒判定//////////////////////////////////
        for (int l = 0; l < collist2len; l++)
        {
            Debug.Log("ThirdJudge");
            Collider2D col2 = colList2[l];
            if (col2.gameObject.CompareTag("Piece")&&col1!=col2)
            {
                //色判定２//////////////////////////////
                string piece3color1 = col2.gameObject.GetComponent<AttributeList>().color1;
                string piece3color2 = col2.gameObject.GetComponent<AttributeList>().color2;
                if (piece1color1 == piece3color1 || piece1color1 == piece3color2)//挟む色が同じだったら(2色目含む)
                {
                    Debug.Log("ThirdJudge(color)");
                    Collider2D[] colList3 = Physics2D.OverlapCircleAll(col2.gameObject.transform.position, 30.0f);//挟む駒のコライダー
                    int collist3len = colList3.Length;

                    Debug.Log("!!!linecol"+ linecol);
                    Debug.Log("collist3len" + collist3len);
                    for (int m = 0; m < collist3len; m++)
                    {
                        Debug.Log("colList3[m].gameObject" + colList3[m].gameObject);
                        if (colList3[m].gameObject.CompareTag("Line")&& colList3[m] == linecol)//これで十分
                        {
                            destroyCheck.destroyFinish = false;
                            Debug.Log("colList3[m].gameObject.CompareTag(Line)==== linecol"+ colList3[m].gameObject);
                            string destroyObjJob = col1.gameObject.GetComponent<AttributeList>().job;
                            float posx = col1.transform.localPosition.x;
                            float posy = col1.transform.localPosition.y;////////////////////////////////////////////////local

                            destroyObjOwner=col1.GetComponent<PhotonView>().Owner;
                            Debug.Log("scoreController.OtherPieceUpdate");
                            
                            if (destroyObjOwner.CustomProperties["num"]is int num)
                            {
                                destroyObjOwnernum = num;
                            }
                            destroyObjID = col1.GetComponent<PhotonView>().ViewID;

                            Debug.Log("PieceDestroy");
                            photonView.RPC("DestroyReq", RpcTarget.AllViaServer, destroyObjOwnernum, destroyObjID, destroyObjJob, posx, posy);
                          
                            
                           
                            //destroy = true;
                            //col1.GetComponent<PhotonView>().RequestOwnership();

                            return;////////////////////////////////////////////////////////////////////////////////////////
                        }
                    }
                }
            }
        }

    }

    [PunRPC]
    public void DestroyReq(int destroyObjOwnernum,int destroyObjID, string destroyObjJob, float posx,float posy)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties[destroyObjOwnernum.ToString()]is Player player)
        {
            Debug.Log("aa");
            if (player.IsLocal)//消される人
            {
                Debug.Log("Destroyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy");
                Debug.Log("destroyObjID" + destroyObjID);
                
                PhotonNetwork.Destroy(PhotonView.Find(destroyObjID));
                photonViewTurn2.RPC("DestroyObject", RpcTarget.AllViaServer, destroyObjJob, posx, posy, destroyObjOwnernum);
            }
        }
       
    }


    /*void IPunOwnershipCallbacks.OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        return;
    }
    void IPunOwnershipCallbacks.OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (previousOwner.IsLocal)
        {
            scoreController.MyPieceUpdate();
        }
        if (targetView.IsMine)
        {
            PhotonNetwork.Destroy(targetView.gameObject);
            photonViewTurn2.RPC("DestroyObject", RpcTarget.AllViaServer, destroyObjJob, posx, posy);
        }
    }*/


}
