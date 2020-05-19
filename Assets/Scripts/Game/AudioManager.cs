using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public AudioSettings audioSettings;

    public static AudioManager audioInstance = null;
    AudioSource backgroundAudioSource;
    AudioSource playerAudioSource;

    int clipSelection;
    float timeUntilNextStep = 0;

    void Awake()
    {
        if(audioInstance == null)
            audioInstance = this;
        else
            Destroy(this);

        audioSettings = GameManager.Instance.gameSettings.audioSettings;
    }

    void Start()
    {
        backgroundAudioSource = gameObject.AddComponent<AudioSource>();
        playerAudioSource = gameObject.AddComponent<AudioSource>();

        backgroundAudioSource.clip = audioSettings.backgroundAmbience;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    public void StandardGunFire()
    {
        clipSelection = Random.Range(0, audioSettings.standardGunClips.Length);
        playerAudioSource.PlayOneShot(audioSettings.standardGunClips[clipSelection]);
    }

    public void ShotgunFire()
    {
        clipSelection = Random.Range(0, audioSettings.shotgunClips.Length);
        playerAudioSource.PlayOneShot(audioSettings.standardGunClips[clipSelection]);
    }

    public void MachineGunFire()
    {
        clipSelection = Random.Range(0, audioSettings.machineGunClips.Length);
        playerAudioSource.PlayOneShot(audioSettings.standardGunClips[clipSelection]);
    }

    public void SniperGunFire()
    {
        clipSelection = Random.Range(0, audioSettings.sniperGunClips.Length);
        playerAudioSource.PlayOneShot(audioSettings.standardGunClips[clipSelection]);
    }

    public void LaserFire()
    {

    }

    public void PlayFootstep()
    {
        if (timeUntilNextStep >= 0)
        {
            timeUntilNextStep -= Time.deltaTime;
            return;
        }

        int selection = Random.Range(0, audioSettings.playerFootstepClips.Length);
        playerAudioSource.PlayOneShot(audioSettings.playerFootstepClips[selection]);
        timeUntilNextStep = audioSettings.footstepRate;
    }

}
