using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_inField : MonoBehaviour
{
    //フィールド上に出現するアイテムについているスクリプトです.
    //実態はspriteも何もないprefabですが,itemDataの情報を反映する
    //ことでStartで特性を持ちます．



    [SerializeField] public ItemData itemData;//何かしら元となるオブジェクトから指定されるScriptableObjectです
    [SerializeField] SpriteRenderer sprite_renderer;//このprefabについているrendererです



    
    PolygonCollider2D Polygon_Colider;//葉っぱの当たり判定
    float Colider_enable_Y=3.7f;//どこからcoliderを有効化させるかの基準です.これ以下で有効化


    // Start is called before the first frame update
    void Start()
    {
        //出現の初期はcoliderを外しておきます.

        Polygon_Colider =this.GetComponent<PolygonCollider2D>();
        Polygon_Colider.enabled=false;

        //itemDataで設定されているspriteにします
        sprite_renderer.sprite = itemData.itemSprite;
        




    }

    //かざした時ハイライトが付くように
    private void OnMouseEnter()
    {
        sprite_renderer.sprite = itemData.itemSprite_Highlight;
    }
    private void OnMouseExit()
    {
        sprite_renderer.sprite = itemData.itemSprite;
    }
    /// <summary>
    ///  //クリックされた時
    /// </summary>

    public void OnMouseDown()
    {

        if (this==null)
        {

        }
        else
        {
            //GameManagerでアイテムの値を管理しています.
            GameManager.instance.Item_Get(itemData.itemName);

            Destroy(gameObject);//これを破壊
        }
        







    }




    // Update is called once per frame
    void Update()
    {
        //範囲内にあるときだけColiderをオンにする
        if (this.transform.position.y < Colider_enable_Y)
        {
            Polygon_Colider.enabled = true;
        }
    }




}
