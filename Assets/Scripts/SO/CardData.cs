using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Create/CardData")]
public class CardData : ScriptableObject
{

}

public struct CardInventory
{
    public string id;
    public string name;
    public int level;
}
