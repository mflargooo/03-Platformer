using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeObject : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip note;
    
    public void Interact()
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(note);
    }
}
