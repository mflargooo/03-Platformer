using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Matching : MonoBehaviour
{
    public Image[] selectionSlots; 
    public Image[] resultSlots; 
    public Sprite[] correctOrderSprites; 
    public Sprite errorSprite; 
    public int[] correctOrder; 
    private int currentResultIndex = 0; 

    public void OnGridClicked(int index)
    {
        if (correctOrder[currentResultIndex] == index)
        {
            resultSlots[currentResultIndex].sprite = correctOrderSprites[index];
            currentResultIndex++; 
        }
        else
        {
            resultSlots[currentResultIndex].sprite = errorSprite;
            currentResultIndex = 0;
        }
    }
}

