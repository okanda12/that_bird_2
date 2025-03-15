using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class First_Get : MonoBehaviour
{
    //アイテムを初取得したときに出てくるオブジェクトです.
    [SerializeField] GameObject wood_prefab;//木のprefab
    [SerializeField] Transform wood_pivot;//これを中心に複数の木が回転します.
    [SerializeField] Sprite wood1;//木のスプライト1
    [SerializeField] Sprite wood2;//木のスプライト2
    [SerializeField] Image item_Image;//アイテムが入るオブジェクト
    [SerializeField] TextMeshProUGUI name_text;//名前が入るテキスト
    [SerializeField] TextMeshProUGUI intro_text;//紹介文が入るテキスト
    [SerializeField] float StartScale = 0.1f;//First_Getの最初の大きさ
    [SerializeField] float EndScale = 1.4f;//First_Getの最後の大きさ
    [SerializeField] int wood_num = 5;
    [SerializeField] float pivot_rotation_angle = 0.2f;//pivotが1フレームで何度回るか
    [SerializeField] float emerge_time = 0.1f;//UIが出てくる速度です.

    public bool order_breakup = false;//解散命令　これがtrueになるとUI自体が削除される.



    // Start is called before the first frame update
    void Start()
    {

        this.transform.localScale = StartScale * Vector3.one;

        


        

    }

    //外部からの命令(ScriptableObject)の指定があって初めて作動する.
    public IEnumerator First_Get_Anim(ItemData itemData)
    {

        //木をpivotに配置


        Wood_Arrangement(itemData);
        GameManager.instance.PlayPororonSE();

        StartCoroutine(Rotate_Pivot());//pivotを回します
        StartCoroutine(Scale_Animation());//このUI自体の拡大，縮小です

        yield return null;
    }





    //このオブジェクトを拡大・縮小するアニメーションです
    public IEnumerator Scale_Animation()
    {
        float elapsedTime=0f;
        float t;



        //目的スケールの1.1倍へ膨らむ．→振動を再現
        float EndScale_after = EndScale * 1.1f;
        while (elapsedTime <emerge_time)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / emerge_time;


            //StartScaleからEndScaleへ変化
            this.transform.localScale = Vector3.Lerp(StartScale*Vector3.one, EndScale_after*Vector3.one, t);
            yield return null;
        
        }

        this.transform.localScale = EndScale_after * Vector3.one;

        


        //EndScaleへ収束します.
        elapsedTime = 0f;
        

        while (elapsedTime < 0.1f)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / emerge_time;


            //StartScaleからEndScaleへ変化
            this.transform.localScale = Vector3.Lerp(EndScale_after * Vector3.one, EndScale * Vector3.one, t);
            yield return null;

        }

        this.transform.localScale = EndScale * Vector3.one;



        //解散命令が出るまでまつ
        while (!order_breakup)
        {
            yield return null;
        }


        //解散
        elapsedTime = 0f;


        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / emerge_time;


            //StartScaleからEndScaleへ変化
            this.transform.localScale = Vector3.Lerp(EndScale* Vector3.one, StartScale * Vector3.one, t);
            yield return null;

        }

        this.transform.localScale = StartScale * Vector3.one;

        //破壊！！！！！！！！！！！！！！！！！！！！！！！
        Destroy(this.gameObject);



    }



    //解散命令　ボタンから呼び出します
    public void Order_Breakuo()
    {
        order_breakup = true;
    }




    //wood_pivotを回す関数です
    public IEnumerator Rotate_Pivot()
    {
        while (true)
        {
            //ずっと回転
            wood_pivot.transform.localRotation *= Quaternion.Euler(0, 0, pivot_rotation_angle);
            yield return null;
        }



    }

    //木を配置する関数です
    public void Wood_Arrangement(ItemData itemData)
    {


        //pivotの子オブジェクト(木)初期化
        foreach (Transform child in wood_pivot)
        {
            Destroy(child.gameObject);
        }

        //wood1とwood2を交互に配置する　
        for (int i=0; i<wood_num;i++)
        {
            float angle = i * (360f / (float)wood_num);//角度で指定
            Quaternion wood_Quaternion = Quaternion.Euler(0f, 0f, angle);
            
            //pivotの子オブジェクトとして生成
            GameObject wood = Instantiate(wood_prefab,wood_pivot.position,wood_Quaternion, wood_pivot);

            //交互にspriteを変える
            if (i%2 == 0)
            {
                
                wood.GetComponent<Image>().sprite = wood1;

            }
            else
            {
                wood.GetComponent<Image>().sprite = wood2;
            }

            //itemDataからテーマカラーを参照
            Color itemColor = itemData.itemColor;
            if (itemColor==null)//もしなかったら
            {
                itemColor=Color.white;
            }
            else
            {
                itemColor = itemData.itemColor;
                

            }
            wood.GetComponent<Image>().color = itemColor;//色を反映

            item_Image.sprite = itemData.itemSprite;//スプライトを入れる
            name_text.text = itemData.itemName_JP ;//名前を入れる
            intro_text.text = itemData.itemIntro;//紹介文を入れる
            //name_text.text = itemData.itemName;//効果を入れる




        }



    }











}
