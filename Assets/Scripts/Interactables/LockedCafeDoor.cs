using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedCafeDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject cafe;
    [SerializeField] private GameObject puzzleToggle;

    public void Interact()
    {
        CameraViewManager.SetCurrLevel(cafe);
        puzzleToggle.SetActive(true);
        CameraViewManager.DisplayPuzzle();
    }
}
