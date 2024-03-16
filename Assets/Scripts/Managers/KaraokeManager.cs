using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaraokeManager : MonoBehaviour
{
    [SerializeField] private Camera slotsCamera;
    [Min(min: 1)]
    [SerializeField] private int numNotesInSoln;
    [SerializeField] private float slotSpacing;
    [SerializeField] private float slotSize;
    [SerializeField] private GameObject slot;

    public static GameObject[] slots;

    private void Start()
    {
        slots = new GameObject[numNotesInSoln];
        SetupSlots(numNotesInSoln, slotSpacing, slotSize);
        GetComponent<ClickAndDrag>().SetCamera(slotsCamera);
    }
    void SetupSlots(int numNotesInSoln, float slotSpacing, float slotSize)
    {
        Vector3 slotsCenter = slotsCamera.transform.position + slotsCamera.transform.forward * 5f - Vector3.up * 1f;
        float spawnOffset = (numNotesInSoln - 1) * (slotSpacing + slotSize) / 2;
        Vector3 spawnStart = slotsCenter - slotsCamera.transform.right * spawnOffset;

        for (int i = 0; i < numNotesInSoln; i++)
        {
            slots[i] = Instantiate(slot, spawnStart + slotsCamera.transform.right * i * (slotSpacing + slotSize), slot.transform.rotation);
            slots[i].name = i.ToString();
        }
    }
}
