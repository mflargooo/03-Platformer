using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<Image> slots = new List<Image>();
    public Sprite collectedItemSprite;
    private int currentIndex = 0;

    public void AddItemToInventory()
    {
        if (currentIndex < slots.Count)
        {
            slots[currentIndex].sprite = collectedItemSprite;
            slots[currentIndex].color = Color.white;
            currentIndex++;
        }
        else { Debug.Log("Inventory is full."); }
    }
}
