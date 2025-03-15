using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NewItemData",menuName="ScriptableObjects/ItemData")]

public class ItemData : ScriptableObject
{
    //アイテムの特徴を一元管理するScriptableオブジェクトです.
    //これをアイテムのprefabやアイテムスロットで参照して管理を簡易にします


    public string itemName;//注意：GameManagerの辞書と同じ名前にしないと動きません
    public string itemName_JP;//日本語名です.かわいくしてね
    [Multiline(5)] 
    public string itemIntro;//紹介文です.効果などを書きましょう
    public Sprite itemSprite;//アイテムのスプライトを貼ってください
    public bool hasRigidbody;//Rigidbodyの有無
    public Sprite itemSprite_Highlight;//アイテムにカーソルを向けたときに出るハイライトを貼ってください
    public Sprite itemSprite_Kage;//アイテムの影のスプライトを貼ってください
    public Color itemColor;//アイテムのイメージカラーです



}
