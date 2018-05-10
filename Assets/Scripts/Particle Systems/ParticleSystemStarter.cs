using UnityEngine;
using System.Collections;

public class ParticleSystemStarter : MonoBehaviour {

	public void StartParticleSystems()
    {
        foreach(ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }
    }
}
