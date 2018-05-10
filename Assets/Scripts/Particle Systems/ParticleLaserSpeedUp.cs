using UnityEngine;
using System.Collections;

public class ParticleLaserSpeedUp : MonoBehaviour {

    private ParticleSystem ps;

	// Use this for initialization
	void Start () {
        ps = this.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (ps.isPlaying)
        {
            ps.playbackSpeed += 0.07f;
        }
        else
        {
            ps.playbackSpeed = 1.0f;
        }
	}
}
