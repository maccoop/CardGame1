using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIService : MonoBehaviour, Service
{
    [SerializeField] AbstractUI _collectionUI;
    [SerializeField] AbstractUI _collectionView;
    public void Init()
    {
        _collectionUI.OnEnableUI += DisableGesture;
        _collectionView.OnEnableUI += DisableGesture;
        _collectionUI.OnDisableUI += EnableGesture;
        _collectionView.OnDisableUI += EnableGesture;
    }

    private void EnableGesture()
    {
        Gesture.Enable = !_collectionUI.Active && !_collectionView.Active;
    }

    private void DisableGesture()
    {
        Gesture.Enable = false;
    }

    internal void ViewCollection(Texture2D main)
    {
        _collectionView.Open(main);
    }
}
