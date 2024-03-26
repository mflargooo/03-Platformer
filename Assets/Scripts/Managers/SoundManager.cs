using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioSource throwSource;
    public static AudioClip tsnd;
    public static AudioSource tsrc;

    [SerializeField] private AudioClip oniDeathSound;
    [SerializeField] private AudioSource oniDeathSource;
    public static AudioClip ondsnd;
    public static AudioSource ondsrc;

    [SerializeField] private AudioClip trainWhistleSound;
    [SerializeField] private AudioSource trainWhistleSource;
    public static AudioClip twsnd;
    public static AudioSource twsrc;

    [SerializeField] private AudioClip trainChuggingSound;
    [SerializeField] private AudioSource trainChuggingSource;
    public static AudioClip tcsnd;
    public static AudioSource tcsrc;

    [SerializeField] private AudioClip doorUnlockSound;
    [SerializeField] private AudioSource doorUnlockSource;
    public static AudioClip dusnd;
    public static AudioSource dusrc;

    [SerializeField] private AudioClip bossDeathSound;
    [SerializeField] private AudioSource bossDeathSource;
    public static AudioClip bdsnd;
    public static AudioSource bdsrc;

    [SerializeField] private AudioClip bossCackleSound;
    [SerializeField] private AudioSource bossCackleSource;
    public static AudioClip bcsnd;
    public static AudioSource bcsrc;

    [SerializeField] private AudioClip trackRotateSound;
    [SerializeField] private AudioSource trackRotateSource;
    public static AudioClip trsnd;
    public static AudioSource trsrc;

    [SerializeField] private AudioClip karaokeButtonSound;
    [SerializeField] private AudioSource karaokeButtonSource;
    public static AudioClip kbsnd;
    public static AudioSource kbsrc;

    public static SoundManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        tsnd = throwSound;
        tsrc = throwSource;
        ondsnd = oniDeathSound;
        ondsrc = oniDeathSource;
        twsnd = trainWhistleSound;
        twsrc = trainWhistleSource;
        tcsnd = trainChuggingSound;
        tcsrc = trainChuggingSource;
        dusnd = doorUnlockSound;
        dusrc = doorUnlockSource;
        bdsnd = bossDeathSound;
        bdsrc = bossDeathSource;
        bcsnd = bossCackleSound;
        bcsrc = bossCackleSource;
        trsnd = trackRotateSound;
        trsrc = trackRotateSource;
        kbsnd = karaokeButtonSound;
        kbsrc = karaokeButtonSource;
    }

    public static void PlayThrowSound()
    {
        tsrc.PlayOneShot(tsnd);
    }

    public static void PlayOniDeathSound()
    {
        ondsrc.PlayOneShot(ondsnd);
    }

    public static void PlayTrainWhistleSound()
    {
        twsrc.PlayOneShot(twsnd);
    }

    public static void PlayTrainChuggingSound()
    {
        tcsrc.PlayOneShot(tcsnd);
    }

    public static void PlayDoorUnlockSound()
    {
        dusrc.PlayOneShot(dusnd);
    }

    public static void PlayBossDeathSound()
    {
        bdsrc.PlayOneShot(bdsnd);
    }

    public static void PlayBossCackleSound()
    {
        bcsrc.PlayOneShot(bcsnd);
    }

    public static void PlayTrackRotateSound()
    {
        trsrc.PlayOneShot(trsnd);
    }
    public static void PlayKaraokeButtonSound()
    {
        kbsrc.PlayOneShot(kbsnd);
    }
}
