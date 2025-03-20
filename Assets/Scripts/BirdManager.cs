using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    //一定間隔で抽選が行われ,鳥が羽根を落とします．


    [SerializeField] Transform bird;//子オブジェクトの鳥
    [SerializeField] GameObject item_inField;//itemの元となるprefab
    [SerializeField] ItemData wingData;//羽根のScriptableObject
    [SerializeField] ItemData mimizuData;//ミミズのScriptableObject


    [SerializeField] float haji_right_x = 11f;//画面右端のTransformのx座標です
    [SerializeField] float haji_left_x = -11f;//画面左端のTransformのx座標です

    [SerializeField] float top_y = 5.6f;//画面上部のy座標です


    Vector3 startPos;
    Vector3 endPos;
    Vector3 centerPos;//円の中心
    private float radius;//円の半径


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Bird_Pre());
    }

    /// <summary>
    /// 鳥が出現する抽選
    /// </summary>
    public IEnumerator Bird_Pre()
    {
        while (true )
        {
            //鳥の抽選間隔
            float bird_interval = GameManager.instance.bird_interval;
            //鳥が出る確率
            float bird_luck = GameManager.instance.bird_luck;


            yield return new WaitForSeconds(bird_interval);

            if (Random.Range(0f, 1f) < bird_luck)
            {
                Bird_Rises();
                Debug.Log("bird rises!!!!!!");
            }
            else
            {
                Debug.Log("bird failed....");
            }



        }
    }

    public void Bird_Rises()
    {

        float start_x = Random.Range(haji_left_x, haji_right_x);//初期位置設定
        float end_x;//最終位置




        if (start_x > 0)
        {
            //画面中心より右
            end_x = start_x - 6f;
        }
        else
        {
            //画面中心より左
            end_x = start_x + 6f;
        }



        //初期位置
        startPos = new Vector3(start_x, top_y, transform.position.z);
        //終点位置
        endPos = new Vector3(end_x, top_y, transform.position.z);
        //中心点
        centerPos= (startPos + endPos) / 2;

        radius = Vector3.Distance(startPos, centerPos);


        ///円弧上に鳥が飛んでいく
        StartCoroutine(FlyInArc());






    }


    /// <summary>
    /// 鳥が飛んで羽根を落とします
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlyInArc()
    {
        float elapsedTime = 0f;
        float flightDuration = 1f;

        bool casted = false;//羽根を落としたかどうか

        while (elapsedTime < flightDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / flightDuration; // 0 → 1 の値

            // t を 180° (π) にマッピングして円弧を作成
            float angle = Mathf.Lerp(0, Mathf.PI, t);

            // Sin, Cos を使って円軌道を作成
            float x = centerPos.x + radius * Mathf.Cos(angle);
            float y = centerPos.y - radius * Mathf.Sin(angle);

            bird.transform.localPosition = new Vector3(x, y, transform.position.z);

            // 途中で羽根を落とす
            if (t>0.5f && casted==false)
            {
                casted = true;
                Debug.Log($"Spawn wing!!t={t}");
                Spawn_wing();
            }

            yield return null;

        } 
    }

    public void Spawn_wing()
    {   //ここで初めて羽根かミミズが生じます

        GameManager.instance.PlayHowaSE();//ほわっという音




        GameObject item_object = Instantiate(item_inField, bird.position , Quaternion.identity);

        //重なるとクリックできないのでz座標を手前に
        item_object.transform.position = new Vector3(item_object.transform.position.x, item_object.transform.position.y,-0.1f);

        Item_inField item_script = item_object.GetComponent<Item_inField>();


        
        if (Random.value < GameManager.instance.mimizu_luck)//ミミズ
        {
            item_script.itemData = mimizuData;
        }
        else
        {
            item_script.itemData = wingData;

        }



    }







}
