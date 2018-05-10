using UnityEngine;
using System.Collections;

public class ParticleSystemSetPlaybackSpeed : MonoBehaviour {
    public float speed = 3.0f;
    private ParticleSystem ps;

    // Use this for initialization
    void Start()
    {
        ps = this.GetComponent<ParticleSystem>();
        ps.playbackSpeed = 3.0f;
    }
}
