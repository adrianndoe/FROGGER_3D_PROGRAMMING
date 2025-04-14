using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum SoundTypeEffects
{
    COLLECT_BONUS_FROG,
    COLLECT_FLY,
    FALL_IN_WATER,
    GAME_OVER,
    HIT_BY_CAR,
    JUMP,
    LEVEL_COMPLETE,
    LILY_PAD,
    OUT_OF_TIME,
    ATE_BY_ALIGATOR
}

public enum SoundTypeBackground
{
    BACKGROUND_GAME
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    [SerializeField] private AudioClip[] soundListBackground;
    [SerializeField] private AudioClip[] soundListEffects;
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource soundEffectsSource;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null) instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        instance.backgroundMusicSource.clip = instance.soundListBackground[0];
        instance.backgroundMusicSource.Play();
    }

    public static void PlaySound(SoundTypeEffects sound)
    {
        // PlayOneShot - play a clip one time with settings set in the AudioSource
        instance.soundEffectsSource.PlayOneShot(instance.soundListEffects[(int)sound]);
    }

    public IEnumerator WaitForSound(AudioSource audioSource)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        Debug.Log("Sound has finished playing!");
    }

    // play a sound aand wait until complete
    public static void PlaySoundWaitForCompletion(SoundTypeEffects sound)
    {
        PlaySound(sound);
        instance.StartCoroutine(instance.WaitForSound(instance.soundEffectsSource)); // wait for the audio clip to complete before continuing
    }

    public static void PlayBackgroundMusic(SoundTypeBackground sound)
    {
        instance.backgroundMusicSource.Stop();
        instance.backgroundMusicSource.clip = instance.soundListBackground[(int)sound];
        instance.backgroundMusicSource.loop = true;
        instance.backgroundMusicSource.Play();
    }

    public AudioSource getSoundEffectsSource() { return soundEffectsSource; }

    public static void OnAwake()
    {
        PlayBackgroundMusic(SoundTypeBackground.BACKGROUND_GAME);
    }
}
