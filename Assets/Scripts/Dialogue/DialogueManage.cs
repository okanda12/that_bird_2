using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class DialogueManage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;//UIのテキスト
    [SerializeField] DialogueData dialogueData;//SciptableObjectのデータ
    [SerializeField] float typingSpeed = 0.05f;//文字の表示速度
    [SerializeField] AudioSource audioSource;//音声用

    private int currentIndex = 0;//現在のセリフのインデックス
    private bool isTyping = false;//タイピング中かどうかのフラグ
    private Coroutine typingCoroutine;//コルーチンを管理


    private void Start()
    {
        ShowDialogue();//最初のセリフをﾋｮ時
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))//クリック時
        {
            if (isTyping)
            {
                //タイピング中ならスキップ（全文を即座に表示）
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueData.dialogueText[currentIndex];
                isTyping = false;

            }
            else
            {
                //次のセリフへ進む
                ShowNextDialogue();
            }

        }
    }


    private void ShowDialogue()
    {
        if (currentIndex < dialogueData.dialogueText.Count)//まだ会話がある
        {
            typingCoroutine = StartCoroutine(TypeDialogue(dialogueData.dialogueText[currentIndex]));
        }
        else
        {
            dialogueText.text = "End of Dialogue";//全てのセリフが終了
        }


    }

    IEnumerator TypeDialogue(string text)
    {
        isTyping = true;

        dialogueText.text = "";//初期化

        foreach (char letter in text)
        {
            dialogueText.text += letter;//一文字ずつ表示
            if (dialogueData.typeSound && audioSource)
            {
                audioSource.PlayOneShot(dialogueData.typeSound);//音を再生

            }
            yield return new WaitForSeconds(typingSpeed);//速度調整
        }

        isTyping = false;
    }

    private void ShowNextDialogue()
    {
        currentIndex++;
        if (currentIndex<dialogueData.dialogueText.Count)
        {
            ShowDialogue();
        }
        else
        {
            dialogueText.text = "End of Dialogue";//全部終わったらメッセージ
        }

    }


}
