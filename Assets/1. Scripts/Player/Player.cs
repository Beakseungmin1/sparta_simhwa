using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public UIInventory inventory;
    public PlayerContoller controller;
    public PlayerCondition condition;
    public Equipment equip;

    public ItemData itemData;

    public Transform dropPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerContoller>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();

    }
}
