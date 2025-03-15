using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ライカ
//狼の少年です.
//自動でアイテムを回収してくれます.



public class RAIKA : MonoBehaviour
{



    [SerializeField] float find_up_Y=-0.1f;//ライカが探せるY上限
    [SerializeField] float find_down_Y=-2f;//ライカが探せるY下限
    [SerializeField] float foot_speed=1f;//ライカの脚の速さ
    [SerializeField] float gosogoso_speed = 1f;//アイテム取得の速さ(秒数)

    List<Item_inField> foundItems=new List<Item_inField>();

    bool isSearching = false;//アイテム探し途中か




    //フィールド上のItemを探します
    void SerchItem()
    {
        List<Item_inField> localfoundItems = new List<Item_inField>();
        // シーン内のすべての `Item_inField` を取得
        Item_inField[] allItems = FindObjectsOfType<Item_inField>();
        
        foreach (Item_inField item in allItems)
        {
            float itemY = item.transform.position.y;

            // Y座標が範囲内ならリストに追加
            if (itemY >= find_down_Y && itemY <= find_up_Y)
            {
                localfoundItems.Add(item);
            }
        }

        foundItems = localfoundItems;

        


    }




    IEnumerator RunandPickup()
    {
        isSearching = true;
        float closestDistance = Mathf.Infinity;
        Item_inField closestItem = new Item_inField();

        //最も近いアイテムを探す
        foreach (Item_inField item in foundItems)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);

            
            if (distance < closestDistance)
            {
                    closestDistance = distance;
                    closestItem = item;
            }
            
        }

        //最も近いアイテムをとりにいく
        yield return StartCoroutine(PickUpItem(closestItem));
        Debug.Log("StartPickuo");




        isSearching = false;
    }



    //走って取りに行く動き
    IEnumerator PickUpItem(Item_inField item)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = item.transform.position;
        startPos.z = 0f;
        targetPos.z = 0f;

        float elapsedTime = 0f;

        //走る
        while (elapsedTime < gosogoso_speed)
        {


            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / gosogoso_speed);
            elapsedTime += Time.deltaTime;

            
            if (item ==null)
            {
                Debug.Log("Rika's item banished!");
                break;
            }
            targetPos = item.transform.position;

            yield return null;
        }

        //transform.position = targetPos;



        //取る
        yield return new WaitForSeconds(gosogoso_speed);
        item.OnMouseDown();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }




    // Update is called once per frame
    void Update()
    {

        SerchItem();//範囲内のアイテムリストを更新


        //もし，アニメーション中でなく,アイテムがあるなら取得しに行く
        if (isSearching == false && foundItems.Count != 0)
        {
            StartCoroutine(RunandPickup());


            Debug.Log("StartRunandPickuo");
        }

        


    }




}
