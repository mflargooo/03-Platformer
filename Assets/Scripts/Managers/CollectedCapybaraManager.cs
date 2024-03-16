using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedCapybaraManager : MonoBehaviour
{
    [Min(min: 1)]
    [SerializeField] private GameObject[] capybaras;

    private bool[] collectedCapybaras;

    private int numCapybaraNotes;
    // Start is called before the first frame update
    void Start()
    {
        numCapybaraNotes = capybaras.Length;
        collectedCapybaras = new bool[numCapybaraNotes];
    }
    
    void ActivateCapybaraList()
    {
        for (int i = 0; i < numCapybaraNotes; i++)
        {
            if (collectedCapybaras[i]) capybaras[i].SetActive(true);
        }
    }
}
