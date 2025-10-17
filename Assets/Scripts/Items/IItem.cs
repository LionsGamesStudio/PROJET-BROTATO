using UnityEngine;

public interface IItem
{
    int ID { get; }
    string Name { get; }
    string Description { get; }
    Sprite Icon { get; }
    ItemType Type { get; }
    GameObject Prefab { get; }
}

public enum ItemType
{
    Weapon,
    Consumable
}