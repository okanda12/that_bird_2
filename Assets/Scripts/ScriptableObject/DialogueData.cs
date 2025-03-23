using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NewDialogue",menuName="ScriptableObjects/Dialogue")]
public class DialogueData : ScriptableObject
{

    [TextArea(3, 10)] public List<string> dialogueText = new List<string>();//セリフ
    public AudioClip typeSound;//文字ごとのサウンド
                                                                            
}
