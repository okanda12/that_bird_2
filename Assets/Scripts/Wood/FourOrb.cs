using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourOrb : MonoBehaviour
{
    
    /// <summary>
    ///　wood1の子です.ぐるぐるにアイテムを使用すると光り,４個使用すると消えます．
    /// </summary>


    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void LightAnim()//発光します
    {
        animator.SetTrigger("activate");
    }

    public void DeLightAnim()//光を消します
    {
        animator.SetTrigger("destroy");
    }




  

}
