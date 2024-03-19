using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera puzzleCam;

    [SerializeField] private GameObject player;
    private static Camera mc;
    private static Camera pc;
    private static GameObject pl;
    public static GameObject currLevel;

    private void Start()
    {
        mc = mainCam;
        pc = puzzleCam;
        pl = player;
        DisplayMain();
    }

    public static void SetCurrLevel(GameObject obj)
    {
        currLevel = obj;
    }

    public static void DisplayMain()
    {
        if (currLevel) currLevel.SetActive(true);
        mc.gameObject.SetActive(true);
        pc.gameObject.SetActive(false);
        pl.SetActive(true);
    }

    public static void DisplayPuzzle()
    {
        if (currLevel) currLevel.SetActive(false);
        mc.gameObject.SetActive(false);
        pc.gameObject.SetActive(true);
        pl.SetActive(false);
    }
}
