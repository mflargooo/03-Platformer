using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPuzzleConsole : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject trainLevel;
    [SerializeField] private GameObject puzzleToggle;
    [SerializeField] private TrainManager tm;
    public void Interact()
    {
        CameraViewManager.SetCurrLevel(trainLevel);
        puzzleToggle.SetActive(true);
        CameraViewManager.DisplayTrainPuzzle();
        StartCoroutine(tm.RestartPuzzle());
    }
}
