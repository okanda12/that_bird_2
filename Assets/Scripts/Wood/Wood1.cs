using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood1 : MonoBehaviour
{

    [SerializeField] List<FourOrb> FourOrbs;


   
    void Start()
    {
        
    }


    /// <summary>
    /// 対応するオーブを発光させます
    /// </summary>
    /// <param name="CountNum"></param>
    public void Lighten(int CountNum)
    {

        FourOrb Orb = FourOrbs[CountNum-1]; //要素を取得し,

        Orb.LightAnim();//発行


    }


    /// <summary>
    /// 全てのオーブの光を消します
    /// </summary>
    public void AllDelight()
    {
        foreach(FourOrb Orb in FourOrbs){


            Orb.DeLightAnim();

        }


    }




}
