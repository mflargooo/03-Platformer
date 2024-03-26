using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera karaokeMenuCam;
    [SerializeField] private Camera karaokePuzzleCam;
    [SerializeField] private Camera trainPuzzleCam;

    [SerializeField] private GameObject player;
    private static Camera mc;
    private static Camera kmc;
    private static Camera kpc;
    private static Camera tpc;
    private static GameObject pl;
    public static GameObject currLevel;

    private void Start()
    {
        mc = mainCam;
        kmc = karaokeMenuCam;
        kpc = karaokePuzzleCam;
        tpc = trainPuzzleCam;
        pl = player;
        DisplayKaraokeMenu();
    }

    public static void SetCurrLevel(GameObject obj)
    {
        currLevel = obj;
    }

    public static void DisplayMain()
    {
        if (currLevel) currLevel.SetActive(true);
        mc.gameObject.SetActive(true);
        kmc.gameObject.SetActive(false);
        kpc.gameObject.SetActive(false);
        kpc.gameObject.SetActive(false);
        pl.SetActive(true);
        SoundManager.instance.transform.parent = mc.transform;
        SoundManager.instance.transform.localPosition = Vector3.zero;
    }

    public static void DisplayKaraokeMenu()
    {
        if (currLevel) currLevel.SetActive(true);
        mc.gameObject.SetActive(false);
        kmc.gameObject.SetActive(true);
        kpc.gameObject.SetActive(false);
        kpc.gameObject.SetActive(false);
        pl.SetActive(false);
    }

    public static void DisplayKaraokePuzzle()
    {
        if (currLevel) currLevel.SetActive(false);
        mc.gameObject.SetActive(false);
        kpc.gameObject.SetActive(true);
        tpc.gameObject.SetActive(false);
        pl.SetActive(false);
        SoundManager.instance.transform.parent = kpc.transform;
        SoundManager.instance.transform.localPosition = Vector3.zero;
    }

    public static void DisplayTrainPuzzle()
    {
        if (currLevel) currLevel.SetActive(false);
        mc.gameObject.SetActive(false);
        kpc.gameObject.SetActive(false);
        tpc.gameObject.SetActive(true);
        pl.SetActive(false);
        SoundManager.instance.transform.parent = tpc.transform;
        SoundManager.instance.transform.localPosition = Vector3.zero;
    }
}
