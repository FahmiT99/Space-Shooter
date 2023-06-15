using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayElectroSound : MonoBehaviour
{
    public AudioSource audioSource;
    public ParticleSystem particleSystem;

    private bool isPlaying = false;
    private int playCount = 0;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (particleSystem.isPlaying && !isPlaying)
        {
            isPlaying = true;
            if (audioSource != null)
            {
                playCount = 0;
                PlaySound();
            }
        }
    }

    private void PlaySound()
    {
        if (playCount < 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
            playCount++;
            Invoke("PlaySound", audioSource.clip.length);
        }
        else
        {
            isPlaying = false;
        }
    }
}
