using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingingCapybara : Selectable
{
    KaraokeManager km;
    private AudioSource audioSource;

    public void Setup()
    {
        km = FindObjectOfType<KaraokeManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = km.GetNote(int.Parse(name.Substring(name.Length - 1)));
    }
    public override void OnSelect()
    {
        audioSource.Play();
    }
    public override void OnDeselect()
    {
        audioSource.Stop();
        audioSource.time = 0f;
    }
}
