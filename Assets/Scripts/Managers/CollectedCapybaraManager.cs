using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedCapybaraManager : MonoBehaviour
{
    [SerializeField] private SingingCapybara[] capybaras;

    [Min(min: 1)]
    private int numCapybaraNotes;
    [SerializeField] private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        numCapybaraNotes = capybaras.Length;
        DeactivateCapybaraList();
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
            capybaras[i].transform.rotation = cam.transform.rotation;
            capybaras[i].SetHomePos(homePos);
            capybaras[i].Setup();
            capybaras[i].ResetPos();
        }
    }

    public void DeactivateCapybaraList()
    {
        for (int i = 0; i < numCapybaraNotes; i++)
        {
            capybaras[i].gameObject.SetActive(false);
        }
    }

    public void ActivateCapybara(int i)
    {
        capybaras[i].gameObject.SetActive(true);
    }

    public Selectable[] GetCapybaraList()
    {
        return capybaras;
    }
}
