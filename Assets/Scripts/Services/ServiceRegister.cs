using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ServiceRegister : MonoBehaviour
{
    [SerializeField] private PoolingService _poolingService;
    [SerializeField] private AssetService _assetService;
    [SerializeField] private UserService _userService;
    [SerializeField] private UIService UIService;
    [SerializeField] private AdService AdService;
    [SerializeField] private RewardService RewardService;

    private void Start()
    {
        Register();
    }

    public void Register()
    {
        ServiceLocator.Register(_poolingService);
        ServiceLocator.Register(_assetService);
        ServiceLocator.Register(_userService);
        ServiceLocator.Register(UIService);
        ServiceLocator.Register(AdService);
        ServiceLocator.Register(RewardService);
    }
}
