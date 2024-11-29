using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AssetService : MonoBehaviour, Service
{
    [SerializeField] private AssetData assetData;
    [SerializeField] private AssetData supportItems;
    public void Init()
    {
    }

    public int Length => assetData.cells.Count;

    internal Texture2D GetTexture(int key)
    {
        return assetData.cells[key];
    }

    internal CardDetail GetCardDetail(int key, int value)
    {
        return assetData.cards[key][value];
    }
}
