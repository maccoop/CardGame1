using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardService : MonoBehaviour, Service
{
    [SerializeField] private MathFomular _collectionMathFomular;
    [SerializeField] private MathFomular _rewardMathFomular;
    private AssetService _assetService;
    public void Init()
    {
        ServiceLocator.Resolve<AssetService>(service =>
        {
            _assetService = service as AssetService;
        });
    }
}
