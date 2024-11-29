using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Create/SkillSO")]
public class SkillSO : MonoBehaviour
{

}

[System.Serializable]
public class Skill: AbstractItem
{
    [SerializeField] private SkillType _skillType;

    [System.Serializable]
    public enum SkillType
    {

    }
}
