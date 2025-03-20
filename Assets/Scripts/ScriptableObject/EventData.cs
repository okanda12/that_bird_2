using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EventData", menuName = "ScriptableObjects/EventData")]
public class EventData : ScriptableObject
{
    //NEWSの一元管理をするScriptableObjectです.

    public string EventName;//注意:GameManagerの辞書と同じ名前にしないと動きません
    [Multiline(5)]
    public string EventIntro;//イベントの紹介文です
    public Sprite EventSprite;//イベントのスプライトです全て同じ大きさにしないといけません
   

}
