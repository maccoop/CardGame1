using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollected
{
    private AssetService _assetService;
    public ItemCollected()
    {
        ServiceLocator.Resolve<AssetService>(service =>
        {
            _assetService = service as AssetService;
            //_assetService.get
        });
    }


}
