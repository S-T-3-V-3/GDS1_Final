using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Audio Settings")]
public class AudioSettings : ScriptableObject
{
    public AudioClip backgroundAmbience;
    public AudioClip laserFireClip;
    public AudioClip laserHoldClip;

    [Space]
    public AudioClip[] playerFootstepClips;

    [Header("Audio Timers")]
    public float footstepRate = 0.7f;

    [Header("Sound Effects")]
    public List<Sound> soundEffect;
}

[System.Serializable]
public class Sound
{
    public SoundType type;
    public AudioClip[] soundClips;

    [HideInInspector]
    public AudioClip clip
    {
        get
        {
            int size = soundClips.Length;
            if (size == 1) return soundClips[0];

            int randomIndex = Random.Range(0, size);
            return soundClips[randomIndex];
        }
    }
}

public enum SoundType
{
    Rifle,
    Shotgun,
    Laser,
    Sniper,
    Footstep,
    Charge,
    Impact,
    PlayerImpact,
    LOADShotgun,
    LOADRifle,
    LOADMachine,
    Explosions,
    CritSound
}