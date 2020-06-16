using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPauser : MonoBehaviour, IPausable
{
    List<ParticleSystem> particles;
    List<LineRenderer> lineRenderers;

    public void Pause()
    {
        if (particles == null) GetParticles();
        //if (lineRenderers == null) GetLines();

        foreach (ParticleSystem p in particles)
            p.Pause();
    }

    public void UnPause()
    {
        if (particles == null) GetParticles();
        //if (lineRenderers == null) GetLines();

        foreach (ParticleSystem p in particles) {
            p.Play();
        }
    }

    void GetParticles() {
        particles = new List<ParticleSystem>();
        particles.AddRange(this.gameObject.GetComponentsInChildren<ParticleSystem>(false).ToList());
        particles.AddRange(this.gameObject.GetComponents<ParticleSystem>().ToList());
    }

    void GetLines() {
        lineRenderers = new List<LineRenderer>();
        lineRenderers.AddRange(this.gameObject.GetComponentsInChildren<LineRenderer>(false).ToList());
        lineRenderers.AddRange(this.gameObject.GetComponents<LineRenderer>().ToList());
    }
}
