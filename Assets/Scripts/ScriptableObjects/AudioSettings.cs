using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Audio Settings")]
public class AudioSettings : ScriptableObject
{
    public AudioClip backgroundAmbience;
    public AudioClip[] standardGunClips;
    public AudioClip[] shotgunClips;
    public AudioClip[] machineGunClips;
    public AudioClip[] sniperGunClips;
    public AudioClip laserFireClip;
    public AudioClip laserHoldClip;

    [Space]
    public AudioClip[] playerFootstepClips;

    [Header("Audio Timers")]
    public float footstepRate = 0.7f;
}
