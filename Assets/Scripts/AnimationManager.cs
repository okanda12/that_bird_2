using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{


    public static AnimationManager instance = null;//ƒVƒ“ƒOƒ‹ƒgƒ“



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
    
    
}
