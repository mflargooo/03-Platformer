using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCapybara : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        CollectedCapybaraManager ccm = FindObjectOfType<CollectedCapybaraManager>();
        Destroy(gameObject);
        ccm.ActivateCapybara(int.Parse(name.Substring(name.Length - 1)));
    }
}
