using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXsource;

    public AudioClip background;
    public AudioClip collectItemSFX;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    private void Update()
    {
        if (!musicSource.isPlaying) { musicSource.Play(); }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXsource.PlayOneShot(clip, 0.3f);
    }
}
