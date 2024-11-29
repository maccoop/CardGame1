using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CollectionItemUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private UnityEvent<bool> _active;
    [SerializeField] private UnityEvent<bool> _inActive;
    private Texture2D _icon;
    private UIService _service;

    private void Start()
    {
        ServiceLocator.Resolve<UIService>(service =>
        {
            _service = service as UIService;
        });
    }

    internal void SetImage(Texture2D icon)
    {
        this._icon = icon;
        this._image.sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), Vector2.one * .5f);
    }

    internal void SetData()
    {

    }
}
