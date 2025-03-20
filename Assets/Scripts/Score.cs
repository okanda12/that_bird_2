using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.Linq;



public class Score : MonoBehaviour
{
    //真ん中の木の中心についているスコアを表示するやつです


    [SerializeField] TextMeshProUGUI text_now;//現在のスコア_n
    [SerializeField] TextMeshProUGUI text_next;//次の目標
    [SerializeField] GameObject NEWS;//NEWSのプレファブを入れます
    [SerializeField] GameObject MoyaMoya;//キリのエフェクトプレファブを入れます



    [SerializeField] GameObject BirdManager;//prefabより
    [SerializeField] GameObject KinokoManager;//prefabより

    Wood wood2;
    private int score_now;//現在のスコア
    private int score_next;//次のスコア
    GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        wood2 = GetComponentInParent<Wood>();
        canvas = GameObject.Find("Canvas");//シーン内から探す
    }




    // Update is called once per frame
    void Update()
    {
        score_now = GameManager.instance.rotate_score;//現在のスコア
        text_now.text = score_now.ToString();

        //辞書から0となっているイベントを持ってくる
        var NextEvent = GameManager.instance.Eventdic.FirstOrDefault(e => e.Value[1] == 0);

        score_next = NextEvent.Value[0];//次のスコア
        text_next.text = score_next.ToString();//次のイベントの値


        


    }




    public IEnumerator EventStart(string Key)
    {
        //親オブジェクトの木をブルブルさせる
        GameManager.instance.PlayPyurorioSE();
        yield return StartCoroutine(wood2.LongBulu());
        GameObject NEWS_object;
        NEWS NEWS_script;

        switch (Key)
        {
            case "Event1":

                MoyaMoya_bird();
                Instantiate(BirdManager, Vector3.zero, Quaternion.identity);
                //NEWSを配置
                NEWS_object = Instantiate(NEWS, Vector3.zero, Quaternion.identity, canvas.transform);
                NEWS_script = NEWS_object.GetComponent<NEWS>();
                NEWS_script.Initialize(Key);//初期化設定

                break;
            case "Event2":

                MoyaMoya_bird();
                Instantiate(KinokoManager, Vector3.zero, Quaternion.identity);
                //NEWSを配置
                NEWS_object = Instantiate(NEWS, Vector3.zero, Quaternion.identity, canvas.transform);
                NEWS_script = NEWS_object.GetComponent<NEWS>();
                NEWS_script.Initialize(Key);//初期化設定

                break;
            default:
                //処理Default
                Debug.Log("Default");
                //break文
            break;

        }

        

    }


    /// <summary>
    /// もやもやのアニメーションを配置します
    /// </summary>
    private void MoyaMoya_bird()
    {
        float topY = 4.83f;//もやもやを配置するY座標
        float leftX = -10f;//画面左端のx座標
        float allX = 20f;//xの画面いっぱいのsize
        int imax=5;

        float stepX = allX / (imax - 1); // 等間隔の幅を計算


        //等間隔に生成
        for (int i = 0; i < imax; i++)
        {
            float posX = leftX + (stepX * i); // X座標を計算
            Vector3 position = new Vector3(posX, topY, 0); // もやもやの配置位置
            Instantiate(MoyaMoya, position, Quaternion.identity); // 生成
        }



    }
}
