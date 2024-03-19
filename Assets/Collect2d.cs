using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect2d : MonoBehaviour,IInteractable
{
    public void Interact()
    {
        CollectItem();
    }

    private void CollectItem()
    {
        //to be continue
        Destroy(gameObject);
    }
}
