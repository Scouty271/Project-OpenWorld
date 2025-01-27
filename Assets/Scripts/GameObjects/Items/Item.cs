using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public enum ItemTypes
    {
        wood,
        stone
    }
    public ItemTypes itemType;

    public int price;
    public int weight;
    public int size;

    private void Awake()
    {
        switch (itemType)
        {
            case ItemTypes.wood:
                name = "Wood";
                price = 5;
                weight = 2;
                size = 3;
                break;
            case ItemTypes.stone:
                name = "Stone";
                price = 3;
                weight = 4;
                size = 2;
                break;
            default:
                break;
        }
    }
}
