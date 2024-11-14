using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public ItemSlot[] slots;

    public UIInventory inventory;

    public GameObject ShopWindow;
    public Transform slotPanel;

    [Header("Select item")]
    public Image selectedItemicon;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemPrice;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject BuyButton;

    private PlayerContoller controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipIndex;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;

        controller.Shop += Toggle;

        ShopWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].UIShop = this;
        }

        ClearSelectedItemWindow();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearSelectedItemWindow()
    {
        selectedItemicon.gameObject.SetActive(false);
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemPrice.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        BuyButton.SetActive(false);
    }

    public void Toggle()
    {
        if(isOpen())
        {
            ShopWindow.SetActive(false);
        }
        else
        {
            ShopWindow.SetActive(true);
        }
    }

    public bool isOpen()
    {
        return ShopWindow.activeInHierarchy;
    }

    void Additem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;
        // 아이템이 중복 가능한지 canStack
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

        // 비어있는 슬롯 가져온다.
        ItemSlot emptySlot = GetEmptySlot();

        // 있다면
        if(emptySlot != null)
        {
            emptySlot.Item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        // 없다면
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

        selectedItemPrice.text = selectedItem.Price.ToString() + "G";

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for(int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n"; 
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        BuyButton.SetActive(true);

    }

    public void OnBuyButton()
    {
        inventory.Additem(selectedItem);
    }
}
