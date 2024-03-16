using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedCapybaraManager : MonoBehaviour
{
    [SerializeField] private SingingCapybara[] capybaras;
    private bool[] collectedCapybaras;

    [Min(min: 1)]
    private int numCapybaraNotes;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        numCapybaraNotes = capybaras.Length;
        collectedCapybaras = new bool[numCapybaraNotes];
    }

    public void SetReferenceCamera(Camera cam)
    {
        this.cam = cam;
    }

    public void SetupList()
    {
        float capybaraSize = 1f;
        float spacing = .45f;
        float start = (numCapybaraNotes - 1) * (capybaraSize + spacing) / 2;
        Vector3 center = cam.transform.position + cam.transform.forward * 5f - cam.transform.up * 2f;
        for (int i = 0; i < numCapybaraNotes; i++)
        {
            Vector3 homePos = cam.transform.right * (i * (capybaraSize + spacing) - start) + center;
            capybaras[i].transform.position = homePos;
            capybaras[i].transform.rotation = cam.transform.rotation;
            capybaras[i].SetHomePos(homePos);
            capybaras[i].Setup();
            capybaras[i].ResetPos();
        }
    }

    public void ActivateCapybaraList()
    {
        for (int i = 0; i < numCapybaraNotes; i++)
        {
            if (collectedCapybaras[i]) capybaras[i].gameObject.SetActive(true);
        }
    }

    public void DeactivateCapybaraList()
    {
        for (int i = 0; i < numCapybaraNotes; i++)
        {
            capybaras[i].gameObject.SetActive(false);
        }
    }

    public Selectable[] GetCapybaraList()
    {
        return capybaras;
    }
}
