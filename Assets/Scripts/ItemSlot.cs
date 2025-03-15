using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;




public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    //アイテムスロットについているスクリプトです



    [SerializeField] Image itemKage;//itemの後ろにある影です.
    [SerializeField] public ItemData itemData;//対応するアイテムのScriptableObjectを入れてください
    [SerializeField] Image itemImage;//このスロット下のもの(常に表示されている)
    [SerializeField] GameObject item_Drag;//このスロット下のドラッグできる(ドラッグする時だけ可視化される)
    [SerializeField] TextMeshProUGUI text;//このスロット下のもの
    


    //First_Get関連
    //奥からぶわっと木が出る感じです
    [SerializeField] GameObject first_Get;//初めて取得したときにでるUIのprefabです



    




    string itemName;//アイテムの名前,enumから選べます．

    //きゅぽんアニメーションで使います
    //それぞれ初めの拡大比率と終わりの拡大比率です.
    float kyupon_originalScale =3f;
    float kyupon_endScale = 2.4f;


    int pre_item_num = 0;//過去のアイテムの数を保持します．

    // Start is called before the first frame update
    void Start()
    {
        
         

         itemName = itemData.itemName;//GameManagerの辞書から持ってくるため
         itemImage.sprite = itemData.itemSprite;//スプライトを設定
         itemKage.sprite = itemData.itemSprite_Kage;//スプライトの影の設定

          //最初は何も写さない
         itemImage.enabled=false;
         text.enabled=false;
         itemKage.enabled = false;


        //アイテムのドラッグもできない
        item_Drag.SetActive(false);

    }
    //かざした時ハイライトが付くように
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemImage.sprite = itemData.itemSprite_Highlight;
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        itemImage.sprite = itemData.itemSprite;
    }


    /// <summary>
    /// //現在のアイテム数をテキストに表示します．
    /// </summary>

    // Update is called once per frame
    void Update()
    {


        //GameManagerの辞書から現在の取得数を持ってくる.0要素が取得数
        var Item_num = GameManager.instance.dic[itemName];
        


        
        //過去の値と比べて変化しているならアニメーションを開始します
        if (Item_num[0] != pre_item_num)
        {
            StartCoroutine(Kyupon());
        }


        




        pre_item_num = Item_num[0];//過去の値を保持します.
        text.text = Item_num[0].ToString();//テキストに反映
        


        //初取得か判断する.なるべく計算量少なく
        
        if (Item_num[1] == 0)//未取得=0,既取得=1
        {
            
            if (Item_num[0] > 0)//初めて取得するアイテムです
            {
                itemImage.enabled = true;//写す
                itemKage.enabled = true;//写す
                text.enabled = true;//写す
                

                Debug.Log($"You got {itemName} for firsttime!!");
                GameManager.instance.dic[itemName][1]= 1; //二度とここを通らないように0から1へ変更


                //first_Getを生成します.
                GameObject canvas = GameObject.Find("Canvas");//シーン内から探す
                GameObject first_get = Instantiate(first_Get, Vector3.zero, Quaternion.identity, canvas.transform);
                First_Get first_get_script= first_get.GetComponent<First_Get>();//スクリプトを取得
                StartCoroutine(first_get_script.First_Get_Anim(itemData));//アニメーション開始．データを渡す





            }

        }
        else//既取得
        {


            //アイテムの数が1以上ならドラッグ出来るようにします
            if (Item_num[0] > 0)
            {
                item_Drag.SetActive(true);
                itemImage.enabled = true;//見えるように
            }
            else
            {
                item_Drag.SetActive(false);
                itemImage.enabled = false;//見えないように
            }


            
            text.enabled = true;//写す

        }


    }









    /// <summary>
    /// //アイテムを取得したときにスロットの中できゅぽんと拡大して縮小するやつです
    /// </summary>
    /// <returns></returns>

    private IEnumerator Kyupon()
    {
        Vector3 scale = itemImage.transform.localScale;

        float elapsedTime = 0f;
        float Duration = 0.05f;
        float t = 0f;

        //Debug.Log($"initialscale={transform.localScale}");
        
        ///////////////////////
        //膨らむ
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / Duration;
            scale.x = Mathf.Lerp(kyupon_originalScale,kyupon_endScale,t);
            itemImage.transform.localScale = scale; // **更新を適用**
            yield return null;

            //Debug.Log(transform.localScale);
        }

        scale.x = kyupon_endScale ;
        

        elapsedTime = 0f;
        
        ///////////////
        //縮む
        while (elapsedTime <Duration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / Duration;
            scale.x = Mathf.Lerp(kyupon_endScale, kyupon_originalScale, t);
            itemImage.transform.localScale = scale; // **更新を適用**
            yield return null;

            //Debug.Log(transform.localScale);
        }


        scale.x = kyupon_originalScale;//元に戻る
        





    }


























}
