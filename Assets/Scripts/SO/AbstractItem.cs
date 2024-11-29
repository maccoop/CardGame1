using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractItem : IItem
{
    [SerializeField] private string _id, _name, _description;
    [SerializeField] private Sprite _thumbnail;
    public string ID { get => _id; protected set => _id = value; }
    public string Name { get => _name; protected set => _name = value; }
    public string Description { get => _description; protected set => _description = value; }
    public Sprite Thumbnail { get => _thumbnail; protected set => _thumbnail = value; }
}
