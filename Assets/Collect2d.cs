using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect2d : MonoBehaviour, IInteractable
{
    public GameController gameController;

    public void Interact()
    {
        CollectItem();
    }

    private void CollectItem()
    {
        if (gameController != null)
        {
            gameController.ItemCollected();
        }
        Destroy(gameObject);
    }
}

