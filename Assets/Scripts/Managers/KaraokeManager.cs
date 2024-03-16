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

    private GameObject slotsParent;
    private GameObject[] slots;

    private ClickAndDrag cad;
    private CollectedCapybaraManager ccm;

    private int[] solution;
    private AudioClip[] notes;
    private AudioClip correct;

    [SerializeField] private Button verifyButton;

    private void Start()
    {
        cad = GetComponent<ClickAndDrag>();
        ccm = GetComponent<CollectedCapybaraManager>();
        EasyPuzzle();
        cad.SetCamera(slotsCamera);
        ccm.SetReferenceCamera(slotsCamera);
        ccm.DeactivateCapybaraList();
        ccm.SetupList();
        ccm.ActivateCapybaraList();
    }

    public void EasyPuzzle()
    {
        solution = easySolution;
        notes = easyNotes;
        correct = easyCorrect;
        SetupSlots(easySolution.Length, 1f, slotSize);
    }
    public void MediumPuzzle()
    {
        solution = mediumSolution;
        notes = mediumNotes;
        correct = mediumCorrect;
        SetupSlots(mediumSolution.Length, .67f, slotSize);
    }
    public void HardPuzzle()
    {
        solution = hardSolution;
        notes = hardNotes;
        correct = hardCorrect;
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

    public AudioClip GetNote(int i)
    {
        return notes[i];
    }

    public IEnumerator VerifyAnswer()
    {
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

            if (solution[i] == int.Parse(entry.name.Substring(entry.name.Length - 1)))
                karaokeAS.PlayOneShot(notes[i]);
            else
            {
                status = false;
                break;
            }
            yield return new WaitForSeconds(notes[i].length);
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
        }
        else
        {
            karaokeAS.PlayOneShot(falseSolnClip);
            verifyButton.interactable = true;
        }
    }

    public void Verify()
    {
        verifyButton.interactable = false;
        StartCoroutine(VerifyAnswer());
    }
}
