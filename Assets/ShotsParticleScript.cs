using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotsParticleScript : MonoBehaviour
{
    public List<ParticleSystem> particles;
    public ParticleSystem currParticlePlaying;
    private bool isPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        PlayRandomParticles();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying && currParticlePlaying.isStopped) Destroy(this.gameObject);
    }

    public void PlayRandomParticles()
    {
        var random = Random.Range(0, particles.Count);
        var p = particles[random];
        p.Play();
        currParticlePlaying = p;
        isPlaying = true;
    }
}
