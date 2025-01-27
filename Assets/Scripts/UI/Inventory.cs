using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item item;

    public List<Item> items = new List<Item>();

    public void addItem(Item.ItemTypes type)
    {
        item.itemType = type;
        items.Add(Instantiate(item, gameObject.transform));
    }
}
