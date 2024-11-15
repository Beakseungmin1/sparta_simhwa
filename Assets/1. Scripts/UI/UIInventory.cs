using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;
    public float Gold;
    public TextMeshProUGUI GoldValue;

    [Header("Select item")]
    public Image selectedItemicon;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerContoller controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipIndex;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];
        ClearSelectedItemWindow();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].Inventory = this;
        }
    }

    private void Update()
    {
        GoldValue.text = Gold.ToString();
    }

    void ClearSelectedItemWindow()
    {
        selectedItemicon.gameObject.SetActive(false);
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if(isOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool isOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void Additem(ItemData data)
    {

        if(data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.Item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void SelectItem(int index)
    {
        if (slots[index].Item == null) return;

        selectedItem = slots[index].Item;
        selectedItemIndex = index;

        selectedItemicon.gameObject.SetActive(true);
        selectedItemicon.sprite = selectedItem.icon;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;
        
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for(int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n"; 
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);

    }

    public void OnUseButton()
    {
        if(selectedItem.type == ItemType.Consumable)
        {
            for(int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch(selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Speed:
                        condition.SpeedBoost(selectedItem.consumables[i].value, selectedItem.consumables[i].duration);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].Item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();  
        }
        UpdateUI();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        if (selectedItem.type == ItemType.Equipable)
        {
            for (int i = 0; i < selectedItem.Equips.Length; i++)
            {
                switch (selectedItem.Equips[i].type)
                {
                    //무기추가하면 수정예정
                    case EquipType.Weapon:
                        controller.Damage += selectedItem.Equips[i].value;
                        break;
                }
            }
        }
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        if (selectedItem.type == ItemType.Equipable)
        {
            for (int i = 0; i < selectedItem.Equips.Length; i++)
            {
                switch (selectedItem.Equips[i].type)
                {
                    case EquipType.Weapon:
                        controller.Damage -= selectedItem.Equips[i].value;
                        break;
                }
            }
        }
        UpdateUI();

        if(selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

    public void GetGold(float amount)
    {
        Gold += amount;
    }

}
