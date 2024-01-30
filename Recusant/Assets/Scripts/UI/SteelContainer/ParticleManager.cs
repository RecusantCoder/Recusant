using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    public List<ParticleSystem> stubbornParticleSystems = new List<ParticleSystem>();
    

    private void OnEnable()
    {
        StopParticles();
        if (GameManager.instance != null)
        {
            Debug.Log("GameManager is not null current");
            if (GameManager.instance.pickedUpSteelContainer)
            {
                AudioManager.instance.PauseCurrentMusic(true);
                AudioManager.instance.Play("VictoryChimeShort");
                GameManager.instance.pickedUpSteelContainer = false;
            }
            else
            {
                Debug.Log("pikup status current: " + GameManager.instance.pickedUpSteelContainer);
            }
        }
    }

    // Method to start all particle systems in the list
    public void StartParticles()
    {
        AudioManager.instance.Play("VictoryShortFinal");
        foreach (ParticleSystem ps in particleSystems)
        {
            if (!ps.isPlaying)
            {
                ps.Play();
            }
        }
        
        foreach (ParticleSystem sps in stubbornParticleSystems)
        {
            if (!sps.isPlaying)
            {
                sps.Play();
            }
        }
        
        
    }

    // Method to stop all particle systems in the list
    public void StopParticles()
    {
        AudioManager.instance.PauseCurrentMusic(false);
        AudioManager.instance.DestroySound("VictoryShortFinal");
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps.isPlaying)
            {
                ps.Stop();
                Debug.Log("stopped particle system " + ps.name);
            }
        }
        
        foreach (ParticleSystem sps in stubbornParticleSystems)
        {
            if (sps.isPlaying)
            {
                sps.Stop();
                Debug.Log("stopped stubborn particle system " + sps.name);
            }
        }
    }
    
}
