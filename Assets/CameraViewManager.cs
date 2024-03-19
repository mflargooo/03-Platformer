using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera karaokePuzzleCam;
    [SerializeField] private Camera trainPuzzleCam;

    [SerializeField] private GameObject player;
    private static Camera mc;
    private static Camera kpc;
    private static Camera tpc;
    private static GameObject pl;
    public static GameObject currLevel;

    private void Start()
    {
        mc = mainCam;
        kpc = karaokePuzzleCam;
        tpc = trainPuzzleCam;
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
        kpc.gameObject.SetActive(false);
        kpc.gameObject.SetActive(false);
        pl.SetActive(true);
    }

    public static void DisplayKaraokePuzzle()
    {
        if (currLevel) currLevel.SetActive(false);
        mc.gameObject.SetActive(false);
        kpc.gameObject.SetActive(true);
        tpc.gameObject.SetActive(false);
        pl.SetActive(false);
    }

    public static void DisplayTrainPuzzle()
    {
        if (currLevel) currLevel.SetActive(false);
        mc.gameObject.SetActive(false);
        kpc.gameObject.SetActive(false);
        tpc.gameObject.SetActive(true);
        pl.SetActive(false);
    }
}
