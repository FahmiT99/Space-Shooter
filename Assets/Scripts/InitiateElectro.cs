using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateElectro : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }
}

