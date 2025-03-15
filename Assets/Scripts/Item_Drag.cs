using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item_Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //スロット上で使うドラッグできるアイテムです
    [SerializeField] GameObject item_slot;//この親オブジェクトのスロットです
    [SerializeField] Image item_Image;//子オブジェクトの画像です

   
    private RectTransform rectTransform;//このオブジェクトの
    private CanvasGroup canvasGroup;//このオブジェクトの
    private Canvas canvas;





    //ハイライトを切り替えるため使う
    private GuruGuru GuruGuru;

    //ぐるぐるの位置を記録します
    private GuruGuru currentGuruGuru;


    private Vector3 originalPosition;//元の位置の座標
    public ItemData itemData;//item_slotから取ってきます





    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        
    }

    private void Start()
    {
        //スロットのitemDataと同じ画像に初期化
        itemData = item_slot.GetComponent<ItemSlot>().itemData;
        item_Image.sprite = itemData.itemSprite;
        item_Image.enabled = false;//普段は何も画像なし
        GuruGuru = GameObject.Find("GuruGuru").GetComponent<GuruGuru>();

    }


    /// <summary>
    /// GuruGuruでOnTriggerExit2Dをしたときに呼び出されます
    /// </summary>
    /// <param name="guru"></param>
    public void ClearGuruGuru()
    {
        currentGuruGuru = null;

    }



    /// <summary>
    /// アイテムの使用場所　ぐるぐるを設定します
    /// </summary>
    /// <param name="guru"></param>
    public void SetGuruGuru(GuruGuru guru)
    {
        currentGuruGuru = guru;

    }



    /// <summary>
    /// ドラッグ初め
    /// </summary>

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.instance.PlayWoodSE();//ドラッグしたときの音　木
         
        item_Image.enabled = true;//画像あり

        //ドラッグしている最中他のスロットの下に隠れてしまうため
        //親のスロットを親の親の中で一番上にします.
        item_slot.transform.SetAsLastSibling();

        originalPosition = rectTransform.position;//元の位置を保存
        canvasGroup.blocksRaycasts = false; //このレイキャストを無効→後ろのオブジェクトを通す
        //Debug.Log("on begin Drag");
    }


    /// <summary>
    /// //ドラッグ途中
    /// </summary>
    /// <param name="eventData"></param>

    public void OnDrag(PointerEventData eventData)
    {

        GuruGuru.Frame_Highlight();

        // マウスの移動量 (eventData.delta) を考慮してドラッグ
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        //Debug.Log("on Drag");
    }


    /// <summary>
    /// //ドラッグ終わり
    /// </summary>
    /// <param name="eventData"></param>

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;//レイキャストを有効に戻す

        //Debug.Log("endDrag");

        GuruGuru.Clear_Frame_Highlight();

        // **事前に登録された GuruGuru の処理を実行**
        if (currentGuruGuru != null)
        {
            currentGuruGuru.UseItem();
        }


        item_Image.enabled = false;//画像なし
        rectTransform.position = originalPosition;//元の位置へ

    }


   
   
}
