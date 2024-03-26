using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSuccess : MonoBehaviour
{
    [SerializeField] private GameObject puzzleToggle;
    [SerializeField] private GameObject currLevel;
    [SerializeField] private GameObject transition;
    public void Succeed()
    {
        currLevel.SetActive(true);
        transition.SetActive(true);
        puzzleToggle.SetActive(false);
        CameraViewManager.DisplayMain();
    }
}