using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int totalItems = 2; 
    private int collectedItems = 0; 
    private bool gameComplete = false; 

    public void ItemCollected()
    {
        collectedItems += 1;
        CheckGameCompletion();
    }

    private void CheckGameCompletion()
    {
        if (collectedItems >= totalItems)
        {
            gameComplete = true;
            CloseGame();
        }
    }
    public void CloseGame()
    {
        StopAllCoroutines();
        CameraViewManager.DisplayMain();
    }
}




