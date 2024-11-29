using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Create/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public List<Character> characters;
}

[System.Serializable]
public class Character : AbstractItem
{

}
