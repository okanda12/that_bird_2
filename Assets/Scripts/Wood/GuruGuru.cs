using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;



public class GuruGuru : MonoBehaviour
{
    //中央の木についている回転体(ぐるぐる)
    //アイテムスロットからアイテムをドラッグアンドドロップすると
    //アイテムを使用することが出来ます.


    [SerializeField] Transform wing;//子オブジェクトの羽部分　回すため
    [SerializeField] float rotation_speed_nor = 0.2f;//安静時の回転速度
    [SerializeField] GameObject score_Add_Canvas;//スコアを加算するときに飛び散るテキストのcanvas



    [SerializeField] SpriteRenderer frame;//アイテムをドラッグするとき光る
    [SerializeField] SpriteRenderer hane_highlight;//アイテムを入れると光る
    [SerializeField] Sprite frame_Sprite_highlight;//ハイライト時
    [SerializeField] Sprite frame_Sprite_norm;//通常時
    [SerializeField] Wood1 wood1;//オーブを発光させるwood1オブジェクト
    [SerializeField] Score score;//スコアを表示させるオブジェクト


    private Item_Drag draggedItem;//ドラッグされたアイテムから取得します
    private Item_Drag currentItem;//現在触れているアイテム
    Color hane_highlight_Color;//highlight用の羽根についている色
    private float rotation_speed;//羽根の回転速度

    // Start is called before the first frame update
    void Start()
    {
        rotation_speed = rotation_speed_nor;
        //回します
        StartCoroutine(Rotatewing());

        
        //羽根のハイライト　最初は透明
        Color hane_highlight_Color =hane_highlight.color;
        hane_highlight_Color.a = 0f;
        hane_highlight.color = hane_highlight_Color; // 変更を適用


    }


    /// <summary>
    /// ハイライトをつけます
    /// </summary>
    public void Frame_Highlight()
    {

        frame.sprite = frame_Sprite_highlight;
    }



    /// <summary>
    /// ハイライトを消します
    /// </summary>
    public void Clear_Frame_Highlight()
    {
        frame.sprite = frame_Sprite_norm;
        
    }

    /// <summary>
    /// //ドラッグしたアイテムががグルグルと重なっている時,
    /// </summary>
    /// <param name="collision"></param>

    private void OnTriggerEnter2D(Collider2D collision)
    {
        draggedItem = collision.GetComponent<Item_Drag>();
        //Debug.Log("Something entered: " + collision.name); // ここでUIが入っているか確認

        if (draggedItem != null)
        {
            draggedItem.SetGuruGuru(this);//アイテム側にぐるぐるを渡す
            currentItem = draggedItem;
        }




    }

    
    /// <summary>
    /// アイテムがGuruGuru外に出たとき
    /// </summary>
    /// <param name="collision"></param>
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        Item_Drag draggedItem = collision.GetComponent<Item_Drag>();

        if (draggedItem != null && draggedItem == currentItem)
        {
            draggedItem.ClearGuruGuru(); // GuruGuru を解除
            currentItem = null;
        }
    }





    /// <summary>
    /// アイテムを使用 Item_Drag側から呼び出します
    /// </summary>
    /// 
    public void UseItem()
    {
        if (currentItem != null) {


            if (draggedItem == null)
            {
                Debug.Log("draggedIte=null " );


                return;
            }
            else
            {
                //ドラッグしたアイテムのデータ
                ItemData receivedItem = draggedItem.itemData;
                Debug.Log("Received item: " + receivedItem.name);

                //GameManagerの辞書を持ってくる.3要素目がスコア
                var item_dic = GameManager.instance.dic[receivedItem.itemName];


                Debug.Log(item_dic);


                //もしアイテムがあるなら使用する
                if (item_dic[0] > 0)
                {
                    item_dic[0] -= 1;//カウントを一つ減らす
                    GameManager.instance.throwCount += 1;
                    int throwCount = GameManager.instance.throwCount;



                    GameManager.instance.PlayDonkiraSE();//カポッ                          
                    Add_Rotate_Score(item_dic);//スコア加算

                    wood1.Lighten(throwCount);
                  

                    if (throwCount >= 4)//4回投げると初期化
                    {

                        wood1.AllDelight();//光を消す



                        //辞書から0となっているイベントを持ってくる
                        var NextEvent = GameManager.instance.Eventdic.FirstOrDefault(e => e.Value[1] == 0);
                        int score_now = GameManager.instance.rotate_score;//現在のスコア
                        int score_next = NextEvent.Value[0];//次のスコア

                        //もし達成したら
                        if (score_now >= score_next)
                        {

                            //Debug.Log($"you complete {NextEvent.Value[0]}");

                            GameManager.instance.Eventdic[NextEvent.Key][1] = 1;//達成済みとする
                            StartCoroutine(score.EventStart(NextEvent.Key));

                        }
                        GameManager.instance.throwCount =0;
                        GameManager.instance.rotate_score = 0;
                    }
                    









                    currentItem = null; // 使用後にリセット





                }
                else
                {
                    Debug.Log("you dont have this item!!!!!!!!!!");
                }
            }

        }

    }











    /// <summary>
    /// //全体的なスコアを加算します
    /// </summary>
    /// <param name="item_dic"></param>

    public void Add_Rotate_Score(List<int> item_dic)
    {
        //スコア
        int score = item_dic[2];

        GameManager.instance.rotate_score += score;

        //飛び散るテキストを配置
        GameObject score_add_canvas = Instantiate(score_Add_Canvas,this.transform.position, Quaternion.identity, this.transform);
        Spark_text spark_text = score_add_canvas.GetComponent<Spark_text>();//スクリプトを取得
        StartCoroutine(spark_text.Score_Add(score));//飛び散る演出
        
        
        //羽根にハイライトを付けます
        StartCoroutine(Rotatewing2());

        //ブルブル震えるモーション
        Wood wood_script = GetComponentInParent<Wood>();
        //wood_script.OnMouseDown(); 



        Debug.Log($"score + {score}");
    }




    /// <summary>
    /// ///常時羽部分が回転
    /// </summary>
    /// <returns></returns>

    public IEnumerator Rotatewing()
    {
        while (true)
        {
            //ずっと回転
            wing.transform.localRotation *= Quaternion.Euler(0, 0, rotation_speed);
            yield return null;


        }


    }

    /// <summary>
    /// アイテムを入れると羽根回転
    /// </summary>
    /// <returns></returns>
    public IEnumerator Rotatewing2()
    {
        float Duration = 1f;//開店時間
        float t;
        float elapsedTime = 0f;

        hane_highlight_Color = Color.white;

        while (elapsedTime<Duration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / Duration;

            hane_highlight_Color.a = 1f - t;
            
           //Debug.Log(hane_highlight_Color.a);

            hane_highlight.color = hane_highlight_Color; // 変更を適用

            yield return null;
        }


    }


    /// <summary>
    /// アイテムを入れると羽根回転
    /// </summary>
    /// <returns></returns>
    public IEnumerator Rotatewing3(int score)
    {
        float Duration = 1f;//開店時間
        float t;
        float elapsedTime = 0f;

        hane_highlight_Color = Color.white;

        //行き
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / Duration;

            rotation_speed += (float)score * (t);

            
            yield return null;
        }

        elapsedTime = 0f;
        //帰り

        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / Duration;

            rotation_speed -= (float)score * (t);


            yield return null;
        }




    }






}
