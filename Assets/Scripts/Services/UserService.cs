using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System;

public class UserService : MonoBehaviour, Service
{
    [SerializeField] private UserData _userData;
    private AssetService _assetService;
    public int currentScore;
    public string CurrentIdGame { get; set; }
    const string KEY = "USERDATA";

    public void Init()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            _userData = JsonConvert.DeserializeObject<UserData>(PlayerPrefs.GetString(KEY));
        }
        else
        {
            _userData = new UserData();
            PlayerPrefs.SetString(KEY, JsonConvert.SerializeObject(_userData));
            PlayerPrefs.Save();
        }
        ServiceLocator.Resolve<AssetService>(service =>
        {
            _assetService = service as AssetService;
        });
    }

    public int HighScore(string idGame) => _userData.highScore[idGame];

    public void SetHighScore(string iD_GAME, int highscore)
    {
        CurrentIdGame = iD_GAME;
        if (!this._userData.highScore.ContainsKey(iD_GAME))
        {
            this._userData.highScore.Add(iD_GAME, 0);
        }
        if (highscore > this._userData.highScore[iD_GAME])
        {
            this._userData.highScore[iD_GAME] = highscore;
            Save();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString(KEY, JsonUtility.ToJson(_userData));
        PlayerPrefs.Save();
    }

    internal float GetPercentIncreaseRoll()
    {
        return 0;
    }

    public void ReceiveReward(string id, int amount)
    {
        if (!_userData.inventory.ContainsKey(id))
        {
            _userData.inventory.Add(id, 0);
        }
        _userData.inventory[id] += amount;
    }
    public void GetRewards()
    {

    }

    public CardUnlock UnlockCollection(int key, int value)
    {
        CardUnlock collection = new CardUnlock(key, value);
        collection.InitData(_assetService.GetCardDetail(key, value));
        _userData.collections.Add(collection);
        return collection;
    }

    public List<CardUnlock> GetCollections()
    {
        return _userData.collections;
    }
}

[System.Serializable]
public class UserData
{
    public Dictionary<string, int> highScore;
    public List<CardUnlock> collections;
    public Dictionary<string, int> inventory;

    public UserData()
    {
        highScore = new();
        collections = new();
    }
}

[System.Serializable]
public class CardUnlock
{
    [SerializeField] private string _id;
    [SerializeField] private int _key;
    [SerializeField] private int _value;
    [SerializeField] private UnitType _unit;
    [SerializeField] private int _level;
    [SerializeField] private int _damage;
    [SerializeField] private int _hp;
    [SerializeField] private int _armor;
    public CardDetail Detail { get; set; }

    public CardUnlock(int key, int value)
    {
        this._key = key;
        this._value = value;
        _id = Guid.NewGuid().ToString();
    }

    internal void InitData(CardDetail cardDetail)
    {
        Detail = cardDetail;
        _level = 0;
        _unit = Detail.Category;
        _damage = Detail.Damage;
        _hp = Detail.Hp;
        _armor = Detail.Armor;
    }

    public int Key { get => _key; }
    public int Value { get => _value; }
    public UnitType Element { get => _unit; set => _unit = value; }
    public int Level { get => _level; set => _level = value; }
    public int Damge { get => _damage; set => _damage = value; }
    public int Hp { get => _hp; set => _hp = value; }
    public int Armor { get => _armor; set => _armor = value; }
}
