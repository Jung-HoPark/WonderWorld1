using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAction", menuName = "Battle/Action")]
public class ActionData : ScriptableObject
{
    public string actionName;
    public int damageValue;
    public float costMp;
    public Sprite actionIcon;
    public string description;
}
