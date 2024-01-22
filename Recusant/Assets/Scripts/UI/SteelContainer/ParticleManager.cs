using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private void Start()
    {
        // Stop all particle systems initially
        StopParticles();
    }

    private void OnEnable()
    {
        StopParticles();
    }

    // Method to start all particle systems in the list
    public void StartParticles()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            if (!ps.isPlaying)
            {
                ps.Play();
            }
        }
    }

    // Method to stop all particle systems in the list
    public void StopParticles()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps.isPlaying)
            {
                ps.Stop();
                Debug.Log("stopped particle system " + ps.name);
            }
        }
    }
}
