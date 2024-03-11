using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteraction : MonoBehaviour, IPointerEnterHandler 
{
    public AudioSource audioSource;
    public string MusicDifficulty = null;
    public Inventory inventory1;
    public Inventory inventory2;
    public int slotIndex;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null)
        {
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (var source in allAudioSources)
            {
                if (source != audioSource) 
                {
                    source.Stop();
                }
            }
            audioSource.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)//cannot be excuted
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
    public void OnPointerClickEasy(PointerEventData eventData)
    {
        SetMusicDifficulty("easy");
    }

    public void OnPointerClickMedium(PointerEventData eventData)
    {
        SetMusicDifficulty("Medium");
    }
    public void OnPointerClickHard(PointerEventData eventData)
    {
        SetMusicDifficulty("Hard");
    }

    public void SetMusicDifficulty(string difficulty)
    {
        MusicDifficulty = difficulty;
    }

    public void OnMouseDwon(PointerEventData eventData)
    {
        inventory1.OnSlotClicked(slotIndex);
        print("eepy");
    }
}



