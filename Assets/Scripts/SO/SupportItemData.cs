using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SupportItemData), menuName = "Create/SupportItemData")]
public class SupportItemSO: ScriptableObject
{
    public List<SupportItem> items;
}

[System.Serializable]
public class SupportItem: AbstractItem
{
    [SerializeField] private ItemType _type;
    [SerializeField] private BuffValue _dataBuffValue;
    [SerializeField] private ConvertUnit _dataConvert;
    [SerializeField] private UpgradeItem _dataUpgrade;


    [System.Serializable]
    public struct BuffValue
    {
        public bool percentValue;
        public float hp;
        public float damage;

        public BuffValue GetIncreaseValue(float hp, float damage)
        {
            BuffValue result = this;
            if (result.percentValue)
            {
                result.hp = hp * this.hp / 100;
                result.damage = hp * this.damage / 100;
            }
            return result;
        }
        public new string ToString()
        {
            string result = "Increase ";
            if (hp > 0)
            {
                result += "<color=green>{0}{1}</color> hp".FormatHelper(hp, percentValue ? "%" : "");
            }
            if (hp > 0 && damage > 0)
            {
                result += "and ";
            }
            if (damage > 0)
            {
                result += "<color=green>{0}</color> dmg".FormatHelper(damage);
            }
            return result;
        }
    }

    [System.Serializable]
    public struct UpgradeItem
    {
        public UnitType type;
        public float percentIncrease;

        public override string ToString()
        {
            return "Increase <color=green>{0}%</color> to upgrading <color=cyan>{1}</color>-type items".
                FormatHelper(percentIncrease, type.ToString());
        }
    }

    [System.Serializable]
    public struct ConvertUnit
    {
        public UnitType to;
        public override string ToString()
        {
            return "Change unit type card to <color=cyan>{0}</color>.".FormatHelper(to);
        }
    }
}

[System.Serializable]
public enum ItemType
{
    Buff, Upgrade, ConvertUnit, Roll
}
