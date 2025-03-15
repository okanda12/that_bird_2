using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Spark_text : MonoBehaviour
{
    //ぐるぐるにアイテムが入れられて得点が入るときの演出です

    [SerializeField] TextMeshProUGUI text;//子オブジェクトのtext




    //Vector3 StartPosition = Vector3.zero;//開始位置 
    Vector3 StartPosition = new Vector3(0.188f,0.09f,0);//終了位置
    Vector3 EndPosition = new Vector3(0.2f, 0.14f, 0);//終了位置




    // Start is called before the first frame update
    void Start()
    {
        //キャンバスグループがアタッチされているか確認
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        // **Nullチェック**
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがアタッチされていません！");
            return;
        }
    }




    //GuruGuruからInstantiateされて呼び出されます.
    public IEnumerator Score_Add(int score)
    {
        //初期位置設定
        this.transform.localPosition = StartPosition;

       


        //透明化するための奴です
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        float duration = 1f;
        float t;


        while (elapsedTime <duration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / duration;

            //テキスト反映
            text.text = "+" + score.ToString();
            
            //座標位置更新
            this.transform.localPosition = Vector3.Lerp(StartPosition,EndPosition,t);
            
            

            yield return null;

        }

       
        

        //破壊!!!!!!!!!!!!!!!!!!!!!!!!
        Destroy(gameObject);



    }






    



  
}
