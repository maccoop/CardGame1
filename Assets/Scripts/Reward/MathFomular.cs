using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class MathFomular
{
    /// <summary>
    /// E, D, C, B, A, S, SS
    /// </summary>
    public int AMOUNT_TYPE_ITEM = 7;
    public int[] AMOUNT_PACK = { 7, 7, 5, 5, 4, 4, 2 };
    public int[] AMOUNT_ITEM = { 500, 300, 200, 100, 50, 25, 5 };
    public float[] WEIGHT = { 0, 2, 4, 6, 7, 8, 9 };
    private int _totalItem;
    private float[] _baseRate;
    private int TotalItem
    {
        get
        {
            if (_totalItem != 0)
                return _totalItem;
            for (int i = 0; i < AMOUNT_TYPE_ITEM; i++)
            {
                _totalItem += AMOUNT_PACK[i] * AMOUNT_ITEM[i];
            }
            return _totalItem;
        }
    }
    private float[] BaseRate
    {
        get
        {
            if (_baseRate != null && _baseRate.Length == AMOUNT_TYPE_ITEM)
                return _baseRate;
            _baseRate = new float[AMOUNT_TYPE_ITEM];
            for (int i = 0; i < AMOUNT_TYPE_ITEM; i++)
            {
                _baseRate[i] = AMOUNT_ITEM[i] * AMOUNT_PACK[i] * 1f / TotalItem * 100;
            }
            return _baseRate;
        }
    }
    public AnimationCurve _curve;

    public MathFomular()
    {
        Debug.Log("Gen Base Rate Math Fomular: " + BaseRate);
    }

    private float GetTotalAdjust(out float[] adjustI, int numberRoll, float percentIncrease)
    {
        var totalAdjustRate = 0f;
        adjustI = new float[AMOUNT_TYPE_ITEM];
        int i = 0;
        for (i = 0; i < AMOUNT_TYPE_ITEM; i++)
        {
            adjustI[i] = AdjustRateI(i, numberRoll, percentIncrease);
            totalAdjustRate += adjustI[i];
        }
        for (i = 0; i < AMOUNT_TYPE_ITEM; i++)
        {
            adjustI[i] /= totalAdjustRate;
        }
        return totalAdjustRate;
    }

    private float AdjustRateI(int index, int numberRoll, float percentIncrease)
    {
        if (!index.RangeIn(0, AMOUNT_TYPE_ITEM - 1))
        {
            throw new System.IndexOutOfRangeException();
        }
        var bonus = Mathf.Log(numberRoll + 1);
        var adjustRateI = BaseRate[index] + bonus * WEIGHT[index] + percentIncrease / 100f * WEIGHT[index];
        return adjustRateI;
    }

    [Button]
    private void Reset()
    {
        _baseRate = null;
        _totalItem = 0;
        Debug.Log(BaseRate);
    }

    [Button]
    public int GetRewardType(int numberRoll, float percentIncrease)
    {
        float[] adjustRate;
        float totalAdjustRate = GetTotalAdjust(out adjustRate, numberRoll, percentIncrease);
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        float currentValue = 0f;
        int result = -1;
        _curve = new AnimationCurve();
        for (int i = 0; i < adjustRate.Length; i++)
        {
            currentValue += adjustRate[i];
            _curve.AddKey(i, adjustRate[i]);
            if (currentValue >= randomValue && result == -1)
            {
                Debug.Log("Random: {0}, value: {1}".FormatHelper(randomValue, i));
                result = i;
            }
        }
        return result;
    }

    public int GetRewardIndex(int type)
    {
        return UnityEngine.Random.Range(0, AMOUNT_PACK[type]);
    }
}
