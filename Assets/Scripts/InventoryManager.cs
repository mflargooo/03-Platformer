using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<Image> slots = new List<Image>(); 
    public Color correctColor = Color.green; 
    public Color wrongColor = Color.red;

    private int currentIndex = 0;

    public List<GameObject> itemObjects = new List<GameObject>();
    public List<int> correctOrder;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                int itemIndex = itemObjects.IndexOf(hitObject);
                if (itemIndex != -1){OnItemClick(itemIndex);}
            }
        }
    }

    public void RegisterItemObject(GameObject itemObject)
    {
        if (!itemObjects.Contains(itemObject))
        {
            itemObjects.Add(itemObject);
        }
    }
    void OnItemClick(int itemIndex)
    {
        GameObject currObject = itemObjects[itemIndex];
        AudioSource audioSource = currObject.GetComponent<AudioSource>();
        audioSource.Play(); 
       
        if (correctOrder[currentIndex] == itemIndex)
        {
            slots[currentIndex].color = correctColor;
            currentIndex++;
        }
        else
        {
            slots[currentIndex].color = wrongColor;
        }
        if (currentIndex >= slots.Count)
        {
            Debug.Log("Inventory is full or completed.");
        }
    }
}
