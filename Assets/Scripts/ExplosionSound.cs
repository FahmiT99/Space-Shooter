using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    //public AudioClip audioClip;
    public AudioSource damageSource;

    void Start()
    {
        if (gameObject.activeSelf)
        {
            // Play the audio
            //AudioSource.PlayClipAtPoint(audioClip, transform.position);
            damageSource.Play();
        }
    }


}
