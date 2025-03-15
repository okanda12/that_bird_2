using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kusa : MonoBehaviour
{
    //画面上部の草についているスクリプトです．


    //葉っぱ生成関連

    [SerializeField] ItemData happaData;//草のScriptableオブジェクトを入れてください
    [SerializeField] GameObject itemPrefab_inField;//itemの元になるオブジェクトです
    [SerializeField] Vector3 itemSpawnOffset = Vector3.zero; //アイテムは中心から出てきます
    [SerializeField] float kusa_happa_spawn_Luck = 0.5f;//クリックして葉っぱが出てくる確立
    [SerializeField] float kusa_eda_spawn_Luck = 0.30f;//その中でも枝が出てくる確率
    [SerializeField] ItemData edaData;//枝のScriptableオブジェクトを入れてください

    //3.86


    //ブルブル関連
    float Bulu_time = 0.3f;//ブルブル震える時間
    float Bulu_magnitiude = 0.1f;//震えの強さ
    Vector3 originalPos;//初期位置



    //そよかぜ関連
    float Soyo_Luck = 0.10f;//そよ風でこの確率で勝手に草木が揺れます.
    float Soyo_magnitude = 0.1f;//そよ風で移動する





    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;//初期位置を記憶しておく

        StartCoroutine(SoyoCoroutine());//一秒ごとに抽選するそよ風用のコルーチン
    }



    /// <summary>
    /// //マウスで草木をクリックすると以下を実行します
    /// </summary>

    public void OnMouseDown()
    {
        GameManager.instance.PlayShigeSE();//SE鳴らす
        StartCoroutine(Bulu());//ブルブル震える
        Spawn_Happa();//葉っぱを生成する(確率)
    }


    /// <summary>
    ///  //ブルブル震えるモーションです.
    /// </summary>
    /// <returns></returns>


    private IEnumerator Bulu()
    {
        float elapsedTime = 0f;


        while (elapsedTime < Bulu_time)
        {
            elapsedTime += Time.deltaTime;

            float offsetX = Random.Range(-1f, 1f) * Bulu_magnitiude;
            float offsetY = Random.Range(-1f, 1f) * Bulu_magnitiude;

            //オブジェクトの位置を更新
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);


            yield return null;
        }

        //震え終わったら元の位置に戻す
        transform.localPosition = originalPos;

    }





    /// <summary>
    /// //アイテムの葉っぱを生成する関数です
    /// </summary>

    public void Spawn_Happa()
    {
        if (Random.value < kusa_happa_spawn_Luck)
        {
            GameManager.instance.PlayHowaSE();//ほわっという音
            //生成位置は草のワールド座標にオフセットを加えた位置
            Vector3 spawnPosition = transform.position + itemSpawnOffset;

            //アイテムの素体を生成します.
            GameObject item_object = Instantiate(itemPrefab_inField, spawnPosition, Quaternion.identity);
            //重なるとクリックできないのでz座標を手前に
            item_object.transform.position = new Vector3(item_object.transform.position.x, item_object.transform.position.y, -0.1f);

            Item_inField item_script = item_object.GetComponent<Item_inField>();

            //ここで初めて葉っぱとして生じます
            if (Random.value<kusa_eda_spawn_Luck)//枝の場合
            {
                item_script.itemData = edaData;
            }
            else
            {
                item_script.itemData = happaData;
                
            }
            
            
            
            

        
        }


    }


    /// <summary>
    /// //一秒ごとに以下の抽選を行い，草木を揺らします．
    /// </summary>
    /// <returns></returns>


    private IEnumerator SoyoCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);//一秒待機
            if (Random.value < Soyo_Luck) 
            {
                StartCoroutine(SoyoShake());
            }


        }
    }
    private IEnumerator SoyoShake()
    {
        float duration = 0.8f;//そよ風の揺れの時間
        float elapsedTime = 0f;

        //ランダムなオフセットを算出
        Vector3 offset = new Vector3(Random.Range(-1f, 1f) * Soyo_magnitude ,Random.Range(-1f, 1f) * Soyo_magnitude, 0f);


        while (elapsedTime<duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            //サインカーブを使ってスムーズな動きを実現(最初は0、中央で最大、最後は0に戻る）
            float factor = Mathf.Sin(t * Mathf.PI);
            transform.localPosition = originalPos + offset * factor;
            yield return null;


        }
        //元の位置に戻す
        transform.localPosition = originalPos;



    }

   




}
