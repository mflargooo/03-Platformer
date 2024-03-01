using UnityEngine;
using UnityEngine.EventSystems; 

public class SlotClickHandler : MonoBehaviour
{
    public Inventory inventory; 
    public int slotIndex; 

    public void OnMouseDwon(PointerEventData eventData)
    {
        inventory.OnSlotClicked(slotIndex); 
    }
    public void DoStuff()
    {
        print("eepy");
        inventory.OnSlotClicked(slotIndex);
    }
}

