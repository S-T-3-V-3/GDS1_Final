using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public AudioSettings audioSettings;

    // Static members should always be Pascal case (was audioInstance, can just be Instance in this situation, but AudioInstance would've been correct)
    public static AudioManager Instance = null;
    AudioSource backgroundAudioSource;
    AudioSource generalAudioSource;

    public SoundEvent onSoundEvent;

    int clipSelection;
    float timeUntilNextStep = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        audioSettings = GameManager.Instance.gameSettings.audioSettings;
        onSoundEvent.AddListener(PlaySoundEffect);
    }

    void Start()
    {
        backgroundAudioSource = gameObject.AddComponent<AudioSource>();
        generalAudioSource = gameObject.AddComponent<AudioSource>();

        generalAudioSource.volume = backgroundAudioSource.volume = 0.1f;

        backgroundAudioSource.clip = audioSettings.backgroundAmbience;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    public void SetVolume(float value)
    {
        generalAudioSource.volume = backgroundAudioSource.volume = value;
    }

    public void PlaySoundEffect(SoundType soundType)
    {
        Sound soundSelection = audioSettings.soundEffects.Where(x => x.type == soundType).First();
        AudioClip sound = soundSelection.clip;
        generalAudioSource.PlayOneShot(sound, soundSelection.volume);
    }
}

[System.Serializable]
public class SoundEvent : UnityEvent<SoundType> { }

