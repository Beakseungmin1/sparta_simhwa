using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum EquipType
{
    Weapon,
    Wing,
}

public enum ConsumableType
{
    Health,
    Speed,
    Jump
}

[Serializable]

public class ItemDataConsumbale
{
    public ConsumableType type;
    public float value;
    public float duration;
}

[Serializable]
public class ItemDataEquip
{
    public EquipType type;
    public float value;
}

[CreateAssetMenu(fileName = "item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumbale")]
    public ItemDataConsumbale[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
    public ItemDataEquip[] Equips;

}
