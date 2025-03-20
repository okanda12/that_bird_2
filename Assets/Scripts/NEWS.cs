using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NEWS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_Intro;//紹介文テキスト
    [SerializeField] Image NEWS_image;

    [SerializeField] private List<EventData> allEvents; // すべてのEventDataを格納
    

    private Dictionary<string, EventData> eventDictionary = new Dictionary<string, EventData>();

    Vector3 StartPosition = new Vector3(1000, 0, 0);
    Vector3 CenterPosition = new Vector3(0, 0, 0);
    Vector3 EndPosition = new Vector3(-1000, 0, 0);


    private void Awake()
    {
        InitializeDictionary();//Serializefieldから辞書を作成
    }

    private void Start()
    {
        
        StartCoroutine(Right_to_Center());//アニメーション開始
    }


    /// <summary>
    /// イベント名を入れると対応する紹介文とスプライトが反映されます
    /// </summary>
    public void Initialize(string EventName)
    {
        EventData eventData = GetEventData(EventName);

        text_Intro.text = eventData.EventIntro;
        NEWS_image.sprite = eventData.EventSprite;


    }


    /// <summary>
    /// NEWS入りです
    /// </summary>
    /// <returns></returns>
    IEnumerator Right_to_Center()
    {
        GameManager.instance.PlayPunchSE();
        float elapsedTime = 0f;
        float duration = 0.3f;
        float t;

        while (elapsedTime <duration)
        {
            t = elapsedTime / duration;


            transform.localPosition = Vector3.Lerp(StartPosition, CenterPosition, t*t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = CenterPosition;


    }



    /// <summary>
    /// NEWS解散
    /// </summary>
    IEnumerator Center_to_Left()
    {
        GameManager.instance.PlayPunchSE();
        float elapsedTime = 0f;
        float duration = 0.3f;
        float t;

        while (elapsedTime < duration)
        {
            t = elapsedTime / duration;


            transform.localPosition = Vector3.Lerp(CenterPosition, EndPosition, t*t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = EndPosition;

        Destroy(this.gameObject);
    }



    /// <summary>
    /// Newsを解散します
    /// </summary>
    public void Breakup()
    {
        StartCoroutine(Center_to_Left());
    }
    // Update is called once per frame
    


    /// <summary>
    /// イベントデータを辞書にセット
    /// </summary>
    private void InitializeDictionary()
    {
        foreach (var eventData in allEvents)
        {
            if (!eventDictionary.ContainsKey(eventData.EventName))
            {
                eventDictionary[eventData.EventName] = eventData;
            }
            else
            {
                Debug.LogWarning($"イベント名 {eventData.EventName} が重複しています！");
            }
        }
    }

    /// <summary>
    /// 指定したイベント名のデータを取得
    /// </summary>
    public EventData GetEventData(string eventName)
    {
        if (eventDictionary.TryGetValue(eventName, out EventData eventData))
        {
            return eventData;
        }
        return null;
    }
}
