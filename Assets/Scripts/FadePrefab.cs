using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadePrefab: MonoBehaviour
{
    //ゲームシーンに移る時のフェードアニメーションです.
    //ゲーム開始ボタンを押したときにGameManagerからこのスクリプトをもった
    //Fadeオブジェクトが発生し,Fadeアニメーションとともにシーンを切り替えます．
    

    public static FadePrefab instance = null;//シングルトン


    //yama_rayのprefab
    //実態としては空のオブジェクトとyamaのprefabで構成されています.
    //yamaは中心からx軸がyama_range_ini分ずれたところに配置されており,
    //これをpivotにおいてx軸をいじることでアニメーションを構成しています．
    [SerializeField] GameObject yama_ray;

    
    [SerializeField] Transform yama_pivot;//山の回転中心
    [SerializeField] float yama_range_ini=12f;//山の初期半径
    [SerializeField] float yama_range_last = 5f;//山の最終半径
    [SerializeField] int yama_num = 5;//山の個数




    [SerializeField] Transform back_mask;//Fadesレイヤーの0をマスク　一番下
    [SerializeField] Transform yama_mask;//Fadesレイヤーの1をマスク　二番目に下

    



    [SerializeField] float fade_StartTime=0.7f;//フェードが発生してから閉じるまでの時間
    [SerializeField] float fade_waitTime = 1f;//フェードが途中で止まる時間


    private Vector3 StartScale_mask = new Vector3(24f,24f,24f);//マスクの初期拡大スケール
    private Vector3 EndScale_mask = new Vector3(0f, 0f, 0f);//マスクの終了拡大スケール


    private Vector3 StartScale_mask_yama = new Vector3(24f, 24f, 24f);//yamaマスクの初期拡大スケール
    private Vector3 EndScale_mask_yama = new Vector3(8f, 8f, 8f);//yamaマスクの終了拡大スケール




    private bool order_all_assembled = false;//画面に全てのオブジェクトが到着した時にtrue
    private bool order_breakup = false;//解散命令:次のシーンがロードされるとtrue



    // Start is called before the first frame update
    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }




    private void Start()
    {

        //StartCoroutine(FadeStart());
        
    }



    //FadeStartを外部から呼び出すための関数
    public void FadeStartEvoker(string NextScene)
    {
        StartCoroutine(FadeStart(NextScene));
    }





    //
    //FadeStart FadeWait FadeEndに分かれている.
    //FadeStart

    //FadeWait

    //FadeEnd




    /// <summary>
    /// ///フェード始まり
    /// </summary>
    /// <param name="NextScene"></param>
    /// <returns></returns>

    public IEnumerator FadeStart(string NextScene)
    {

        GameManager.instance.PlayFadeSE();//効果音



        StartCoroutine(Back_Shrink());//背景のマスクです
        
        StartCoroutine(Jimen_and_Yama());//yamaのマスクです


        //まだ皆集まってないなら止まる
        while (!order_all_assembled)
        {

            yield return null;

        }

        //次のシーンをロード
        SceneManager.LoadScene(NextScene);
        

        yield return new WaitForSeconds(fade_waitTime);


        order_breakup = true;//解散命令



       
    }


    /// <summary>
    /// back_maskのスケールが小さくなり,元に戻ります
    /// </summary>


    public IEnumerator Back_Shrink()
    {

        back_mask.localScale = StartScale_mask;//初期位置に設定


        float elapsedTime = 0f;
        float t = 0f;

        //////////////////////////////////////
        //背景を切り抜くマスクがどんどん縮小していきます
        while (elapsedTime < fade_StartTime)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / fade_StartTime;

            back_mask.localScale = Vector3.Lerp(StartScale_mask, EndScale_mask, t);

            yield return null;

        }

        back_mask.localScale = EndScale_mask;//目的の位置で停止



        //解散命令が出されない限り停止
        while (!order_breakup)
        {

            yield return null;

        }





        //////////
        //解散命令が出されると元の大きさに戻る

        elapsedTime = 0f;
        t = 0f;
        GameManager.instance.PlayFadeSE();//効果音
        while (elapsedTime < fade_StartTime)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / fade_StartTime;

            back_mask.localScale = Vector3.Lerp(EndScale_mask, StartScale_mask, t);

            yield return null;


        }
        back_mask.localScale = StartScale_mask;//元の位置に戻る

        Destroy(gameObject);//このfadeを破壊




    }



    /// <summary>
    /// //上の背景と同様にmaskを縮めて.元の大きさに戻します.
    /// </summary>
    /// <returns></returns>


    public IEnumerator Jimen_and_Yama()
    {

        yama_mask.localScale = StartScale_mask_yama;//初期のスケールに設定

        yield return new WaitForSeconds(0.1f);//少しdelayがあるとかっこよくなるような気がして


       

        StartCoroutine(Yama_Animation());//山のアニメーションは別で処理します


        //集合！！！！！
        //maskを縮めます
        float elapsedTime = 0f;
        float t;


        
        while (elapsedTime < fade_StartTime)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / fade_StartTime;

            yama_mask.localScale = Vector3.Lerp(StartScale_mask_yama, EndScale_mask_yama, t);

            yield return null;

        }
        yama_mask.localScale =  EndScale_mask_yama;

        
        
        //解散命令が出るまで以下を繰り返す
        while (!order_breakup)
        {
            yield return null;

        }


        //解散！！！！
        //元の大きさにもどします
        elapsedTime = 0f;
 
        while (elapsedTime < fade_StartTime)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / fade_StartTime;

            yama_mask.localScale = Vector3.Lerp(EndScale_mask_yama, StartScale_mask_yama, t);

            yield return null;

        }
        yama_mask.localScale = StartScale_mask_yama;





    }


    /// <summary>
    /// //山のアニメーションに関するコルーチンです
    /// </summary>
    /// <returns></returns>


    public IEnumerator Yama_Animation()
    {
        List<Transform> yama_children = new List<Transform>();


        //pivotを中心に山が配置されます.TransformのListが返ってきます
        yama_children = Yama_Arrangement();

        float elapsedTime = 0f;
        float t;
       

        //集合！
        //それぞれの山のx軸を小さくすることで山が中心に集まります
        while (elapsedTime < fade_StartTime)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / fade_StartTime;


            for (int i = 0; i < yama_num; i++)
            {
                yama_children[i].localPosition= Vector3.Lerp(yama_range_ini*Vector3.right, yama_range_last * Vector3.right, t);

            }
            yield return null;

        }

        //最終位置に設定
        for (int i = 0; i < yama_num; i++)
        {
            yama_children[i].localPosition= new Vector3(yama_range_last, 0f, 0f);

        }

        order_all_assembled = true;//全員集合できました．


        
        //解散命令が出るまで回転
        while (!order_breakup)
        {

            //ずっとpivotは回転
            yama_pivot.transform.localRotation *= Quaternion.Euler(0, 0, 1f);

            
            yield return null;

        }



        //解散！！
        //全ての山のx軸を元に戻します
        elapsedTime = 0f;
        

        while (elapsedTime < fade_StartTime)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / fade_StartTime;

            for (int i = 0; i < yama_num; i++)
            {
                yama_children[i].localPosition = Vector3.Lerp(yama_range_last * Vector3.right, yama_range_ini* Vector3.right, t);

            }
            yield return null;

        }

        //最終位置に設定
        for (int i = 0; i < yama_num; i++)
        {
            yama_children[i].localPosition = new Vector3(yama_range_ini, 0f, 0f);

        }

      




    }



    /// <summary>
    /// //山のarayを配置する関数です,Trasformのリストが返ってきます
    /// </summary>
    /// <returns></returns>


    private List<Transform> Yama_Arrangement()
    {

        List<Transform> yama_children=new List<Transform>();//Listを定義

        //設定した数までyama_arrayをpivot下に配置していく
        for (int i = 0; i < yama_num; i++)
        {
            float angle = i * (360f / (float)yama_num);//角度で指定

            Quaternion yama_Quaternion = Quaternion.Euler(0f, 0f, angle);


            GameObject yama_ray_Instantiated = Instantiate(yama_ray, yama_pivot.position, yama_Quaternion, yama_pivot);
            
            
            
            Transform yama_child = yama_ray_Instantiated.transform.GetChild(0);

            yama_child.localPosition = new Vector3(yama_range_ini, 0f, 0f);//初期位置に設定

            yama_children.Add(yama_child);

            //Debug.Log(yama_child);


        }

        return yama_children;


    }










}






