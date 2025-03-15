using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    
    
    public static GameManager instance = null;//シングルトン



   //フェード関係のフラッグ，###  現在使っていない #######
    public bool fade_Close = false;//フェードが入って閉じたらシーンを切り替える
    public bool fade_End = false;//フェード終了
    


    [SerializeField] private GameObject Fadeprefab;//フェードのprefabを入れてください



    
    //アイテムの取得数と初めて取得したかどうかを管理する辞書
    public Dictionary<string, List<int>> dic = new Dictionary<string, List<int>>()
    {
        // "アイテムの名前"　<取得数>　<初取得の場合0>　<グルグルに入れたときのスコア>　という組み合わせ
        {"happa" , new List<int>{ 0, 0 ,1} },
        {"eda" , new List<int>{ 0, 0 , 10} },

        {"wing" , new List<int>{ 0, 0, 2} },
        {"mimizu" , new List<int>{ 0, 0,20} },

        {"kinoko" , new List<int>{ 0, 0, 5} },
        {"kinoko_rare" , new List<int>{ 0, 0,50} },

        {"zenmai" , new List<int>{ 0, 0,10} },
        {"zenmai_rare" , new List<int>{ 0, 0,100} },

        {"orb" , new List<int>{ 0, 0,100} },
        {"orb_rare" , new List<int>{ 0, 0, 200} },

    };





    //イベントに関するdictionallyです
    public Dictionary<string, List<int>> Eventdic = new Dictionary<string, List<int>>()
    {
        // "Eventの名前"　<イベントを発動させるためのスコア>　　<既に発動したかどうか>　という組み合わせ
        {"Event1" , new List<int>{ 5, 0 } }, //鳥が来るようになったぞ！
        {"Event2" , new List<int>{ 30, 0 } },　//キノコが発生するようになったぞ！
        {"Event3" , new List<int>{ 100, 0 } },　//鬱鳥出現
        {"Event4" , new List<int>{ 1000, 0 } },　//ゼンマイが生えるようになったぞ！
        {"Event5" , new List<int>{ 10000, 0 } },　//川が出現するようになったぞ！

    };


    public int rotate_score = 0;//全体的な回転スコア

    public float wind_interval = 25f;//この間隔で風の抽選を行う
    public float wind_luck = 0.5f;//この確率で風がふく


    public float bird_interval = 5f;//この間隔で鳥の抽選を行う
    public float bird_luck = 0.3f;//この確率で鳥が現れる
    public float mimizu_luck = 0.1f;//この確率でミミズが出る























    private void Awake()
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
        gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        if (!Fadeprefab)
        {
            Debug.Log("fadeprefabが設定されていません！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    /// <summary>
    /// //Forestシーンを呼ぶ
    /// </summary>


    public void LoadForestScene()
    {
        fade_Close = false;
        fade_End = false;


      

        GameObject fadeobject = Instantiate(Fadeprefab);
        FadePrefab fadePrefab=fadeobject.GetComponent<FadePrefab>();

        fadePrefab.FadeStartEvoker("Forest");



    }


    /// <summary>
    /// //StartSceneを呼ぶ
    /// </summary>

    public void LoadStartScene()
    {
        fade_Close = false;
        fade_End = false;
       

       
        GameObject fadeobject = Instantiate(Fadeprefab);
        FadePrefab fadePrefab = fadeobject.GetComponent<FadePrefab>();

        fadePrefab.FadeStartEvoker("StartScene");

    }






    /// <summary>
    /// //各アイテムをゲットしたときの挙動
    /// </summary>


    public void Item_Get(string itemName)
    {
        PlayPyuiSE();//ピュイ効果音
        dic[itemName][0] += 1;//辞書に上書き
    }









    //以下はボタンから音を呼び出すやつです．
    //SoundManagerにPlayWoodSEを書いても良かったのですが,
    //SoundManagerやSoundSourceが非有効化され,音が鳴りませんでした.解決するまでここから呼び出します

    //カチ音を鳴らす
    public void PlayWoodSE()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.Kachi);
    }


    public void PlayFadeSE()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.Ween);
    }

    public void PlayZunSE()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.Zun);
    }

    public void PlayShigeSE()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.Shige);
    }

    public void PlayHowaSE()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.Howa);
    }
    public void PlayPyuiSE()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.Pyui);
    }
    public void PlayKapoSE()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.Kapo);
    }
    public void PlayDonkiraSE()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.Donkira);
    }
    public void PlayPororonSE()
    {
        SoundManager.instance.PlaySE(SoundManager.instance.Pororon);
    }



}
