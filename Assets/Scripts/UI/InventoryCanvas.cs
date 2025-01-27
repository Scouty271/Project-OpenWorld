using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvas : MonoBehaviour
{
    private Human player;
    private VerticalLayoutGroup buttonList;

    public List<Button> inventoryButtons = new List<Button>();

    public Button buttonInventory;

    private void Awake()
    {
        player = GetComponentInParent<GameController>().GetComponentInChildren<WorldController>().mainCharacter;
        buttonList = GetComponentInChildren<VerticalLayoutGroup>();
    }

    public void refreshInventory()
    {
        foreach (var item in inventoryButtons)
        {
            Destroy(item.gameObject);
        }

        inventoryButtons.Clear();

        foreach (var item in player.GetComponent<Inventory>().items)
        {
            buttonInventory.GetComponentInChildren<Text>().text = item.name;

            inventoryButtons.Add(Instantiate(buttonInventory, buttonList.transform));
        }
    }
}
