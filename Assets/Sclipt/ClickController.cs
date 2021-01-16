using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    GameObject firstClickedObject;
    GameObject secondClickedObject;

    bool piececlick = false;
    bool fieldclick = false;
    string piecename;

    Collider[] colList;
    Collider col1;
    Collider col2;
    Collider linecol;//同じ線上の駒か

    PieceCheckController pieceCheck;
    TurnController turnController;
    ScoreController scoreController;

    private void Start()
    {
        firstClickedObject = null;
        secondClickedObject = null;
        pieceCheck = GetComponent<PieceCheckController>();
        turnController = GetComponent<TurnController>();
        scoreController = GetComponent<ScoreController>();
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);


            if (hit2d)
            {
                if (piececlick == true)//事前に駒をクリックしていたら
                {
                    secondClickedObject = hit2d.transform.gameObject;
                    if (secondClickedObject.CompareTag("Field"))//駒の次にクリックしたのがフィールド上のマスだったら
                        //移動制限かかなきゃ…
                        fieldclick = true;

                    Debug.Log(secondClickedObject);
                }
                else//駒のクリック判定
                {
                    firstClickedObject = hit2d.transform.gameObject;
                    Collider[] colList = Physics.OverlapSphere(firstClickedObject.transform.position, 0.3f);//0.3は要変更（隣が重なるように）
                    if (firstClickedObject.CompareTag("Piece"))
                        piececlick = true;

                    Debug.Log(firstClickedObject);
                }
            }


            //駒をクリックした後マスをクリックしたら
            if (fieldclick == true)
            {
                for (int i = 0; colList[i] != null; i++)//動かした駒周りのコライダーについて
                {
                    col1 = colList[i];
                    if (colList[i].CompareTag("Piece")) //コライダーのタグ…？ゲームオブジェクトじゃなくて？//ここに当てはまらなかったら次のListの配列へ（隣り合う駒が0）
                    {
                        string piece1color1 = pieceCheck.ColorCheck1(firstClickedObject.name);
                        string piece2color1 = pieceCheck.ColorCheck1(col1.gameObject.name);
  
                        
                        if (piece1color1 != piece2color1)//隣合う駒の色が違ったら//ここに当てはまらなかったら次のListの配列へ（隣り合う色違いの駒が0）
                        {

                      //-----------------------------------------------------------------------------------------------------------------------------------

                            Collider[] colList2 = Physics.OverlapSphere(col1.gameObject.transform.position, 0.3f);//挟まれる駒と接触するコライダー//配列初期化できてる？

                            for (int j = 0; colList2[i] != null; j++)//挟まれる駒それぞれに対して挟む駒の判定をしていく//直し
                            {
                                for (int k = 0; colList[i] != null; k++)
                                {
                                    if (colList[j].CompareTag("Line") == colList2[k].CompareTag("Line"))
                                    {
                                        linecol = colList[j];
                                    }

                                }
                                if (linecol == null)
                                {
                                    Debug.Log("Lineオブジェクトがずれています");
                                }

                                //挟む駒の判定
                                for (int l = 0; colList2[l] != null; l++)
                                {
                                    col2 = colList2[l];
                                    if (col2.CompareTag("Piece"))
                                    {

                                        string piece3color1 = pieceCheck.ColorCheck1(col1.gameObject.name);
                                        string piece3color2 = pieceCheck.ColorCheck2(col1.gameObject.name);//gameobject必要？
                                        if (piece1color1 == piece3color1 || piece1color1 == piece3color2)//挟む色が同じだったら(スパイ含む)
                                        {
                                            Collider[] colList3 = Physics.OverlapSphere(col1.gameObject.transform.position, 0.3f);//挟む駒のコライダー

                                            for (int m = 0; colList3[m] != null; m++)
                                            {

                                                if (colList3[m].CompareTag("Line") == linecol)
                                                {
                                                    //Destroy()エフェクト？
                                                    scoreController.ScoreUpdate(pieceCheck.JobCheck(col2.name));//colliderの名前？？
                                                        //PUN監視つける
                                            }
                                            }

                                        }


                                    }
                                }
  
                      //-----------------------------------------------------------------------------------------------------------------------------------    

                            }



                        }

                       
                    }

                }

                fieldclick = false;
                piececlick = false;

                turnController.TurnChange();
            }

        }
    }

}
