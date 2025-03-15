using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class SoundSlider : MonoBehaviour
{
    //SoundSliderPrefabにくっついているものです.
    //BGMとSEの音量を別々に設定することが出来ます.


    //AudioMixerを入れる
    [SerializeField] AudioMixer audioMixer;

    //それぞれのスライダーを入れる
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;

    // Start is called before the first frame update
    void Start()
    {
        //ミキサーのvolumeにスライダーのvolumeを入れる.

        //BGM
        audioMixer.GetFloat("BGMparam", out float bgmVolume);
        BGMSlider.value = bgmVolume;

        //SE
        audioMixer.GetFloat("SEparam", out float seVolume);
        SESlider.value = seVolume;


        
    }


    
    //ミキサーの値を変えている
    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGMparam",volume);
    }

    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SEparam", volume);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
