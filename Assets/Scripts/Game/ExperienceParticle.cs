using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceParticle : MonoBehaviour
{
    private void OnParticleCollision(GameObject other) {
        if (other.GetComponent<BasicPlayer>() != null) {
            print("A");
        }
    }
}
