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

    GameObject wood2;
    private int score_now;//現在のスコア
    private int score_next;//次のスコア
    // Start is called before the first frame update
    void Start()
    {
        
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


        //もし達成したら
        if (score_now >= score_next)
        {
            
            Debug.Log($"you complete {NextEvent.Value[0]}");
            GameManager.instance.Eventdic[NextEvent.Key][1] = 1;//達成済みとする

            GameObject canvas = GameObject.Find("Canvas");//シーン内から探す
            //NEWSを配置
            GameObject NEWS_object = Instantiate(NEWS, Vector3.zero, Quaternion.identity, canvas.transform);



        }


    }
}
