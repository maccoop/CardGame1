using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetData", menuName = "AssetData", order = 0)]
public class AssetData : ScriptableObject
{
    public List<Texture2D> cells;
    public List<List<CardDetail>> cards;
}

[System.Serializable]
public enum UnitType
{
    Metal, Wood, Water, Fire, Earth, Light, Shadow
}

[System.Serializable]
public struct CardDetail
{
    [SerializeField] private string id;
    [SerializeField] private string _name;
    [SerializeField] private string _title;
    [SerializeField] private Texture2D _thumb;
    [SerializeField] private Texture2D _sprite;
    [SerializeField] private UnitType _category;
    [SerializeField] private int _damage;
    [SerializeField] private int _armor;
    [SerializeField] private int _hp;
    public UnitType Category => _category;
    public string Title { get => _title; }
    public string Name { get => _name; }
    public int Damage { get => _damage; }
    public int Armor { get => _armor; }
    public int Hp { get => _hp; }
    public Texture2D Thumbnail { get => _thumb; }
    public Texture2D FullCard { get => _sprite; }
}
