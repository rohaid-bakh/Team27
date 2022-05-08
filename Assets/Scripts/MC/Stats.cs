using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Stats/Stats", order = 0)]
public class Stats : ScriptableObject
{
    public int health; 
    public int damage;
    public Material flash;
    // 0, Win. 1, Lose
    public Sprite[] DialougeSprites;
    // 0, Win. 1, Lose
    public string[] Dialouge;
}
