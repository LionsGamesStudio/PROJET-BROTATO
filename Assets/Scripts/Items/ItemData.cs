using System;
using UnityEngine;

public class ItemData : ScriptableObject, IItem
{
    [Header("Base Item Info")]
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemType type;
    [SerializeField] private GameObject prefab;

    [SerializeField]
    private int id;

    public int ID
    {
        get
        {
            if (id == 0)
            {
                id = Guid.NewGuid().GetHashCode();
            }
            return id;
        }
    }
    public string Name => itemName;
    public string Description => description;
    public Sprite Icon => icon;
    public ItemType Type => type;
    public GameObject Prefab => prefab;
}