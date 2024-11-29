using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionViewUI : AbstractUI
{
    [SerializeField] Image image;
    public override void AfterEnable()
    {
        base.AfterEnable();
    }

    public override void AfterDisable()
    {
        base.AfterDisable();
        Destroy(image.sprite);
    }

    public override void Open(object @object)
    {
        base.Open(@object);
        var texture = @object as Texture2D;
        image.sprite =
            Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }

}
