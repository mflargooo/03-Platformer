using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCapybara : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        CollectedCapybaraManager ccm = FindObjectOfType<CollectedCapybaraManager>();
        Shoot sh = FindObjectOfType<Shoot>();
        ccm.ActivateCapybara(int.Parse(name.Substring(name.Length - 1)));
        sh.IncrementProjCount();
        sh.GetComponent<CapybaraCluster>().AddCapybara(int.Parse(name.Substring(name.Length - 1)));
        Destroy(gameObject);
    }
}
