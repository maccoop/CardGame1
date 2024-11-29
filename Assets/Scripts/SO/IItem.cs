using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public string ID { get; }
    public string Name { get; }
    public string Description { get; }
    public Sprite Thumbnail { get; }
}
