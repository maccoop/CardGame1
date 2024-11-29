using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionUI : AbstractUI
{
    [SerializeField] CollectionItemUI _prefab;
    [SerializeField] Transform _content;
    AssetService _assetService;
    UserService _userService;
    List<CollectionItemUI> _list = new();

    // Start is called before the first frame update
    void Awake()
    {
        ServiceLocator.Resolve<AssetService>(service =>
        {
            _assetService = service as AssetService;
        });
        ServiceLocator.Resolve<UserService>(service =>
        {
            _userService = service as UserService;
        });
    }

    public override void AfterEnable()
    {
        base.AfterEnable();
        var list = _userService.GetCollections();
        for (int i = 0; i < list.Count; i++)
        {
            var item = Instantiate(_prefab, _content);
            //item.Init(list[i]);
            _list.Add(item);
        }
    }
}
