using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinokoManager : MonoBehaviour
{
    //一定間隔で抽選が行われ，キノコマンがキノコを落とします

    [SerializeField] Transform Kinoko_man;//子オブジェクトのキノコマン
    [SerializeField] GameObject item_inField;//itemの元となるprefab
    [SerializeField] ItemData KinokoData;//キノコScriptableObject
    [SerializeField] ItemData RareData;//レアキノコのScriptableObject


    [SerializeField] float haji_right_x = 5.6f;//キノコマンが消える場所です
    [SerializeField] float haji_left_x = -11f;//画面左端のTransformのx座標です

    [SerializeField] float top_y =0.56f;//画面中部のy座標です


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Kinoko_pre());
    }



    /// <summary>
    /// キノコが出現する確率
    /// </summary>
    /// <returns></returns>
    public IEnumerator Kinoko_pre()
    {
        while (true)
        {
            //キノコの抽選間隔
            float kinoko_interval = GameManager.instance.kinoko_interval;
            //キノコが出る確率
            float kinoko_luck = GameManager.instance.kinoko_luck;




            yield return new WaitForSeconds(kinoko_interval);

            if (Random.Range(0f, 1f) < kinoko_luck)
            {
                StartCoroutine(Kinoko_Spawns());
                Debug.Log("kinoko rises!!!!!!");
            }
            else
            {
                Debug.Log("kinoko failed....");
            }







        }



    }




    public IEnumerator Kinoko_Spawns()
    {

        float start_x = haji_left_x;//初期位置設定
        float end_x=haji_right_x;//最終位置




        //初期位置
        Vector3 startPos = new Vector3(start_x, top_y, transform.position.z);
        //終点位置
        Vector3 endPos = new Vector3(end_x, top_y, transform.position.z);


        float elapsedTime = 0f;
        float duration = 6f;



        float elapsedTime2 = 0f;
        float duration2 = 1f;
        float t;


        while (elapsedTime<duration)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime2 += Time.deltaTime;
            t = elapsedTime / duration;
            Kinoko_man.position = Vector3.Lerp(startPos, endPos, t);

            //キノコ生成
            if (elapsedTime2>duration2)
            {
                Spawn_Kinoko();
                elapsedTime2 = 0f;
            }

            yield return null;



        }

        




    }




    public void Spawn_Kinoko()
    {   //ここで初めてキノコかレアキノコが生じます

        GameManager.instance.PlayHowaSE();//ほわっという音



        GameObject item_object = Instantiate(item_inField, Kinoko_man.position, Quaternion.identity);

        //重なるとクリックできないのでz座標を手前に
        item_object.transform.position = new Vector3(item_object.transform.position.x, item_object.transform.position.y, -0.1f);

        Item_inField item_script = item_object.GetComponent<Item_inField>();



        if (Random.value < GameManager.instance.rarekinoko_luck)//レアキノコの確率
        {
            item_script.itemData = RareData;
        }
        else
        {
            item_script.itemData =  KinokoData;

        }



        //ランダムな方向に力を加える
        Rigidbody2D rb =item_object.GetComponent<Rigidbody2D>();
        // ランダムな方向と力を設定
        float forceMagnitude = 1f; // 力の大きさ（数値を調整）
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // ランダムな方向
        rb.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);

    }


}
