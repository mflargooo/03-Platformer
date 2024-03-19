using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaraokeManager : MonoBehaviour
{
    [SerializeField] private Camera slotsCamera;
    [Min(min: 1)]
    [SerializeField] private float slotSize;
    [SerializeField] private GameObject slot;
    [SerializeField] private float slotsDepth;

    [SerializeField] private AudioSource karaokeAS;
    [SerializeField] private AudioClip falseSolnClip;

    [SerializeField] private int[] easySolution;
    [SerializeField] private int[] mediumSolution;
    [SerializeField] private int[] hardSolution;

    [SerializeField] private AudioClip[] easyNotes;
    [SerializeField] private AudioClip[] mediumNotes;
    [SerializeField] private AudioClip[] hardNotes;

    [SerializeField] private AudioClip easyCorrect;
    [SerializeField] private AudioClip mediumCorrect;
    [SerializeField] private AudioClip hardCorrect;

    [SerializeField] private AudioClip easyHint;
    [SerializeField] private AudioClip mediumHint;
    [SerializeField] private AudioClip hardHint;

    private GameObject slotsParent;
    private GameObject[] slots;

    [SerializeField] private ClickAndDrag cad;
    [SerializeField] private CollectedCapybaraManager ccm;

    private int[] solution;
    public static AudioClip[] notes;
    private AudioClip correct;

    [SerializeField] private GameObject puzzleUI;
    [SerializeField] private GameObject puzzleToggle;
    [SerializeField] private LockedCafeDoor lockedDoor;

    [SerializeField] private Button hintButton;


    private void Start()
    {
        EasyPuzzle();
        puzzleToggle.SetActive(false);
    }

    private void Update()
    {
        if (puzzleToggle.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDragMenu();
        }
        if (puzzleToggle.activeInHierarchy && Input.GetKeyDown(KeyCode.R))
        {
            PlayHint();
        }
        if (puzzleToggle.activeInHierarchy && Input.GetKeyDown(KeyCode.V))
        {
            Verify();
        }
    }

    public void EasyPuzzle()
    {
        solution = easySolution;
        notes = easyNotes;
        correct = easyCorrect;
        karaokeAS.clip = easyHint;
        SetupSlots(easySolution.Length, 1f, slotSize);
    }
    public void MediumPuzzle()
    {
        solution = mediumSolution;
        notes = mediumNotes;
        correct = mediumCorrect;
        karaokeAS.clip = mediumHint;
        SetupSlots(mediumSolution.Length, .67f, slotSize);
    }
    public void HardPuzzle()
    {
        solution = hardSolution;
        notes = hardNotes;
        correct = hardCorrect;
        karaokeAS.clip = hardHint;
        SetupSlots(hardSolution.Length, .33f, slotSize);
    }
    private void SetupSlots(int numNotesInSoln, float slotSpacing, float slotSize)
    {
        if (slotsParent) return;

        slotsParent = new GameObject();
        slotsParent.transform.parent = transform;
        slotsParent.name = "Slots";

        slots = new GameObject[numNotesInSoln];

        Vector3 slotsCenter = slotsCamera.transform.position - slotsCamera.transform.forward * slotsDepth - Vector3.up * 1f;
        float spawnOffset = (numNotesInSoln - 1) * (slotSpacing + slotSize) / 2;
        Vector3 spawnStart = slotsCenter - slotsCamera.transform.right * spawnOffset;

        for (int i = 0; i < numNotesInSoln; i++)
        {
            GameObject slotInstance = Instantiate(slot, spawnStart + slotsCamera.transform.right * i * (slotSpacing + slotSize), Quaternion.Euler(slot.transform.eulerAngles + slotsCamera.transform.eulerAngles));
            slotInstance.name = i.ToString();
            slotInstance.transform.parent = slotsParent.transform;
            slots[i] = slotInstance;
        }
    }

    public IEnumerator VerifyAnswer()
    {
        cad.enabled = false;
        bool status = true;
        for (int i = 0; i < solution.Length; i++)
        {
            Transform entry;
            try
            {
                entry = slots[i].transform.GetChild(0);
            }
            catch
            {
                status = false;
                break;
            }

            karaokeAS.PlayOneShot(notes[solution[i]]);
            yield return new WaitForSeconds(notes[solution[i]].length);

            if (solution[i] != int.Parse(entry.name.Substring(entry.name.Length - 1)))
            {
                status = false;
                break;
            }
            
        }

        if(status)
        {
            karaokeAS.PlayOneShot(correct);
            Selectable[] capys = ccm.GetCapybaraList();
            for (int i = 0; i < capys.Length; i++)
            {
                capys[i].tag = "Untagged";
                Destroy(capys[i].GetComponent<SingingCapybara>());
            }
            yield return new WaitForSeconds(correct.length + .2f);
            CompletedPuzzle();
        }
        else
        {
            karaokeAS.PlayOneShot(falseSolnClip);
            puzzleUI.SetActive(true);
            cad.enabled = true;
        }
    }

    void CompletedPuzzle()
    {
        GameObject door = lockedDoor.gameObject;
        door.layer = 0;
        CloseDragMenu();
        Destroy(lockedDoor);
        Destroy(door); /* Play door anim instead */
    }

    public void Verify()
    {
        StopAllCoroutines();
        hintButton.interactable = true;
        karaokeAS.Stop();
        karaokeAS.time = 0f;
        puzzleUI.SetActive(false);
        StartCoroutine(VerifyAnswer());
    }

    public void CloseDragMenu()
    {
        StopAllCoroutines();
        hintButton.interactable = true;
        karaokeAS.Stop();
        karaokeAS.time = 0f;
        puzzleToggle.SetActive(false);
        CameraViewManager.DisplayMain();
    }

    IEnumerator Hint()
    {
        hintButton.interactable = false;
        karaokeAS.Play();
        yield return new WaitForSeconds(karaokeAS.clip.length + .1f);
        hintButton.interactable = true;
    }

    public void PlayHint()
    {
        StartCoroutine(Hint());
    }
}
