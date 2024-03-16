using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingingCapybara : Selectable
{
    private AudioSource audioSource;

    public void Setup()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = KaraokeManager.notes[int.Parse(name.Substring(name.Length - 1))];
    }
    public override void OnSelect()
    {
        audioSource.volume = 1f;
        audioSource.Play();
    }
    public override void OnDeselect()
    {
        audioSource.volume = 0f;
        audioSource.Stop();
        audioSource.time = 0f;
    }
}
