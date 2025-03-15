using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{

    //一定時間経過すると確率で風が吹くようになります.
    //風は右から左に流れ,それにともなって草木が揺れ,
    //自動的に草をクリックしてくれます

    [SerializeField] float moveSpeed = 1f; // 風の移動速度
    [SerializeField] List<Transform> kusakis;  // 草木オブジェクトのリスト

    [SerializeField] float haji_right_x = 11f;//画面右端のTransformのy座標です
    [SerializeField] float haji_left_x = -11f;//画面左端のTransformのy座標です
    // Start is called before the first frame update




    void Start()
    {
        StartCoroutine(Wind_Rises_Pre());
    }


    //風を起こす抽選です
    public IEnumerator Wind_Rises_Pre()
    {
        

        while (true)
        {
            //風の抽選間隔
            float wind_interval = GameManager.instance.wind_interval;
            //風が吹く確率
            float wind_luck = GameManager.instance.wind_luck;


            yield return new WaitForSeconds(wind_interval);

          

            if (Random.Range(0f, 1f) < wind_luck)
            {
                StartCoroutine(Wind_Rises());
                //Debug.Log("wind rises!!!!!!");
            }
            else
            {
                //Debug.Log("wind failed....");
            }

           

        }
        
    }


    /// <summary>
    /// 風アニメーションです
    /// </summary>
    /// <returns></returns>

    public IEnumerator Wind_Rises()
    {
        //初期設定
        List<Transform> kusakis_Use = new List<Transform>(kusakis);



       //初期位置(右端)に設定,x,zはそのまま
        transform.position = new Vector3( haji_right_x, transform.position.y,transform.position.z);


        float elapsedTime = 0f;
        float t;
        float Duration = 3f;//風の吹く時間


        //WindManagerの初めの位置と終わりの位置
        Vector3 StartPosition= new Vector3(haji_right_x, transform.position.y , transform.position.z);
        Vector3 EndPosition= new Vector3(haji_left_x, transform.position.y,  transform.position.z);






        while (elapsedTime<Duration)
        {
            t = elapsedTime / Duration;



            // 後ろから順にループしながら削除
            for (int i = kusakis_Use.Count - 1; i >= 0; i--)
            {
                Transform kusaki = kusakis_Use[i];

                //もし,草木を通ったら
                if (transform.position.x < kusaki.position.x)
                {
                    Kusa kusa_script = kusaki.GetComponent<Kusa>();
                    kusa_script.OnMouseDown();//草木をクリックしたときと同じ挙動
                    kusakis_Use.RemoveAt(i); // 安全に削除
                }
            }
            
            

            //右から左へ移動
            transform.position = Vector3.Lerp(StartPosition, EndPosition ,t);



            elapsedTime += Time.deltaTime;
            yield return null;
           



        }
        transform.position = EndPosition;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
