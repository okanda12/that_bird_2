using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    //シングルトン
    public static SoundManager instance;

    //直下にbgmソースとseソースがあり,そのクリップを差し替えることで
    //曲を出しています.audioMixerのグループにはBGMとSEグループがあります．
    
    //オーディオソース
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;


    //音源です,全てフリー音源から取ってきています．
    //bgm
    [SerializeField] public AudioClip Tokai;//都会 https://soundeffect-lab.info/sound/environment/
    [SerializeField] public AudioClip Bird;//鳥のさえずり https://vsq.co.jp/plus/sound/category_sub/nature/


    //SE
    [SerializeField] public AudioClip Kachi;//かちっ https://soundeffect-lab.info/sound/button/　ボタンクリック
    [SerializeField] public AudioClip Ween;//自動ドア https://soundeffect-lab.info/sound/various/　フェード
    [SerializeField] public AudioClip Zun;//ズン https://soundeffect-lab.info/sound/button/ 木のクリック音
    [SerializeField] public AudioClip Shige;//茂み 草木のクリック音　https://soundeffect-lab.info/sound/various/various3.html
    [SerializeField] public AudioClip Howa;//ほわっ 草の生成音　https://soundeffect-lab.info/sound/button/
    [SerializeField] public AudioClip Pyui;//ピュイ！　アイテム取得音　https://soundeffect-lab.info/sound/button/
    [SerializeField] public AudioClip Kapo;//カポ　　https://soundeffect-lab.info/sound/various/various2.html
    [SerializeField] public AudioClip Donkira;//ドンキら-ん　アイテム使用音　https://soundeffect-lab.info/sound/various/various2.html
    [SerializeField] public AudioClip Pororon;//ぽろろん　アイテム使用音　https://soundeffect-lab.info/sound/various/various2.html

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("SoundMaager Destroyed!");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //初期BGM
        PlayBGM(Bird);

        //PlayWoodSE();
    }

    // Update is called once per frame
    void Update()
    {
        
        

    }







   
    
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("BGMのクリップが設定されていません");
            return;
        }
        
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
        
        


    }



    public void PlaySE(AudioClip clip)
    {

        if (clip == null)
        {
            Debug.LogError("SEのクリップが設定されていません");
            return;
        }
        
        seSource.PlayOneShot(clip);



    }
}
