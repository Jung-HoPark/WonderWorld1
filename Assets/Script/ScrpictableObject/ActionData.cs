using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAction", menuName = "Battle/Action")]
public class ActionData : ScriptableObject
{
    public string actionName;
    public int damageValue;
    public Sprite actionIcon;
    public string description;
    public float manaCost;
}
