using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    //中心3本の木についているスクリプトです.

    float Bulu_time = 0.2f;//ブルブル震える時間
    float Bulu_magnitiude = 0.04f;//震えの強さ

    Vector3 originalPos;//初期位置

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
    }


    //クリックすると木の音がなります．
    public void OnMouseDown()
    {
        GameManager.instance.PlayZunSE();
        StartCoroutine(Bulu());
    }


    //クリックされるとブルブル震えるモーションです.
    private IEnumerator Bulu()
    {
        float elapsedTime = 0f;


        while (elapsedTime < Bulu_time)
        {
            elapsedTime += Time.deltaTime;

            float offsetX = Random.Range(-1f, 1f) * Bulu_magnitiude;
            float offsetY = Random.Range(-1f, 1f) * Bulu_magnitiude;

            //オブジェクトの位置を更新
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);


            yield return null;
        }

        //震え終わったら元の位置に戻す
        transform.localPosition = originalPos;

    }



    
    // Update is called once per frame
    void Update()
    {
        
    }
}
