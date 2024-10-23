using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using Purrcifer.Data.Defaults;

[System.Serializable]
public class AudioCollection
{
    public AudioClip[] clips;
    public float volume = 1f;
    [Range(0f, 1f)]
    public float randomPitchVariance = 0.1f;
}

public class SoundManager : MonoBehaviour
{
    #region Singleton
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;
    #endregion

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup footstepsMixerGroup;

    [Header("Sound Effects")]
    [SerializeField] private AudioCollection footstepSounds;
    [SerializeField] private AudioCollection attackSounds;
    [SerializeField] private AudioCollection talismanSounds;
    [SerializeField] private AudioCollection doorSounds;
    [SerializeField] private AudioCollection damageSounds;
    [SerializeField] private float sfxFadeSpeed = 8f;

    [Header("Music")]
    [SerializeField] private AudioClip splashScreenMusic;
    [SerializeField] private AudioClip worldStartMusic;
    [SerializeField] private AudioClip worldWitchingMusic;
    [SerializeField] private AudioClip worldHellMusic;
    [SerializeField] private float musicTransitionDuration = 2f;
    [SerializeField] private float musicVolume = 0.5f;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource footstepSource;

    private bool isMoving = false;
    private bool isInGame = false;
    private Coroutine footstepFadeCoroutine;
    private Coroutine musicTransitionCoroutine;
    private WorldState currentState;

    void OnEnable()
    {
        #region Singleton Setup
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();

            // Start with splash screen music
            if (splashScreenMusic != null)
            {
                Debug.Log("Playing splash screen music");
                musicSource.clip = splashScreenMusic;
                musicSource.Play();
                musicSource.volume = musicVolume;
            }
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
        #endregion

        // Subscribe to world state changes and initialize with current state
        if (GameManager.Instance != null)
        {
            GameManager.Instance.WorldStateChange += OnWorldStateChanged;
            OnWorldStateChanged(GameManager.WorldClock.CurrentState);
        }
    }

    private void Start()
    {
        LoadVolumeSettings();
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.WorldStateChange -= OnWorldStateChanged;
        }
    }

    #region Audio Setup
    private void SetupAudioSources()
    {
        Debug.Log("Setting up audio sources");

        if (musicMixerGroup == null)
        {
            Debug.LogError("Music Mixer Group not assigned!");
            return;
        }

        // Setup music source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicMixerGroup;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        // Setup SFX source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = sfxMixerGroup;
        sfxSource.volume = 1f;
        sfxSource.playOnAwake = false;

        // Setup footsteps source
        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.outputAudioMixerGroup = footstepsMixerGroup;
        footstepSource.volume = 0f;
        footstepSource.loop = true;
        footstepSource.playOnAwake = false;
    }
    #endregion

    #region Volume Controls
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        float volume;
        audioMixer.GetFloat("SFXVolume", out volume);
        return Mathf.Pow(10, volume / 20); // Convert from decibels back to 0-1 range
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
        SaveVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        float dbValue = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("SFXVolume", dbValue);
        audioMixer.SetFloat("FootstepsVolume", dbValue);
        SaveVolumeSettings();
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("BGMVolume"));
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
        }
    }

    private void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", GetSFXVolume());
        PlayerPrefs.Save();
    }

    public void ResetToDefault()
    {
        SetMusicVolume(1f);
        SetSFXVolume(1f);
        SaveVolumeSettings();
    }
    #endregion

    #region Sound Effects
    public void OnMovementStateChanged(bool moving)
    {
        if (moving != isMoving)
        {
            isMoving = moving;
            if (moving)
            {
                StartFootsteps();
            }
            else
            {
                StopFootsteps();
            }
        }
    }

    public void OnAttack()
    {
        if (attackSounds.clips != null && attackSounds.clips.Length > 0)
        {
            AudioClip clip = attackSounds.clips[Random.Range(0, attackSounds.clips.Length)];
            sfxSource.pitch = 1f + Random.Range(-attackSounds.randomPitchVariance, attackSounds.randomPitchVariance);
            sfxSource.PlayOneShot(clip, attackSounds.volume);
        }
    }

    public void OnTalismanCast()
    {
        if (talismanSounds.clips != null && talismanSounds.clips.Length > 0)
        {
            AudioClip clip = talismanSounds.clips[Random.Range(0, talismanSounds.clips.Length)];
            sfxSource.pitch = 1f + Random.Range(-talismanSounds.randomPitchVariance, talismanSounds.randomPitchVariance);
            sfxSource.PlayOneShot(clip, talismanSounds.volume);
        }
    }

    public void OnDoorStateChanged()
    {
        if (doorSounds.clips != null && doorSounds.clips.Length > 0)
        {
            AudioClip clip = doorSounds.clips[Random.Range(0, doorSounds.clips.Length)];
            sfxSource.pitch = 1f + Random.Range(-doorSounds.randomPitchVariance, doorSounds.randomPitchVariance);
            sfxSource.PlayOneShot(clip, doorSounds.volume);
        }
    }

    public void OnPlayerDamaged()
    {
        if (damageSounds.clips != null && damageSounds.clips.Length > 0)
        {
            AudioClip clip = damageSounds.clips[Random.Range(0, damageSounds.clips.Length)];
            sfxSource.pitch = 1f + Random.Range(-damageSounds.randomPitchVariance, damageSounds.randomPitchVariance);
            sfxSource.PlayOneShot(clip, damageSounds.volume);
        }
    }

    private void StartFootsteps()
    {
        if (footstepSounds.clips != null && footstepSounds.clips.Length > 0)
        {
            footstepSource.clip = footstepSounds.clips[Random.Range(0, footstepSounds.clips.Length)];
            footstepSource.Play();
            if (footstepFadeCoroutine != null)
            {
                StopCoroutine(footstepFadeCoroutine);
            }
            footstepFadeCoroutine = StartCoroutine(FadeFootsteps(true));
        }
    }

    private void StopFootsteps()
    {
        if (footstepFadeCoroutine != null)
        {
            StopCoroutine(footstepFadeCoroutine);
        }
        footstepFadeCoroutine = StartCoroutine(FadeFootsteps(false));
    }
    #endregion

    #region Music Management
    public void OnLevelLoaded()
    {
        Debug.Log("Level loaded - transitioning from splash music to game music");
        isInGame = true;
        if (GameManager.WorldClock != null)
        {
            WorldState currentState = GameManager.WorldClock.CurrentState;
            Debug.Log($"Current world state on level load: {currentState}");
            OnWorldStateChanged(currentState);
        }
        else
        {
            Debug.LogError("WorldClock is null on level load");
        }
    }

    private void UpdateMusicBasedOnWorldState(WorldState state)
    {
        if (!isInGame)
        {
            Debug.Log("Not in game yet, keeping splash music");
            return;
        }

        Debug.Log($"Updating music for state: {state}, Current music: {(musicSource.clip != null ? musicSource.clip.name : "null")}");
        AudioClip targetMusic = null;

        bool hasAllClips = worldStartMusic != null && worldWitchingMusic != null && worldHellMusic != null;
        if (!hasAllClips)
        {
            Debug.LogError($"Missing music clips! Start: {worldStartMusic != null}, Witching: {worldWitchingMusic != null}, Hell: {worldHellMusic != null}");
            return;
        }

        switch (state)
        {
            case WorldState.WORLD_START:
                if (musicSource.clip != worldStartMusic)
                {
                    targetMusic = worldStartMusic;
                    Debug.Log("Switching to start music");
                }
                break;

            case WorldState.WORLD_WITCHING:
                if (musicSource.clip != worldWitchingMusic)
                {
                    targetMusic = worldWitchingMusic;
                    Debug.Log("Switching to witching music");
                }
                break;

            case WorldState.WORLD_HELL:
                if (musicSource.clip != worldHellMusic)
                {
                    targetMusic = worldHellMusic;
                    Debug.Log("Switching to hell music");
                }
                break;

            default:
                Debug.LogWarning($"Unhandled world state: {state}");
                break;
        }

        if (targetMusic != null && targetMusic != musicSource.clip)
        {
            Debug.Log($"Starting music transition from {(musicSource.clip != null ? musicSource.clip.name : "null")} to {targetMusic.name}");
            if (musicTransitionCoroutine != null)
            {
                StopCoroutine(musicTransitionCoroutine);
            }
            musicTransitionCoroutine = StartCoroutine(TransitionMusic(targetMusic));
        }
    }

    private void OnWorldStateChanged(WorldState newState)
    {
        //Debug.Log($"World state changed to: {newState}");
        currentState = newState;
        UpdateMusicBasedOnWorldState(newState);
    }
    #endregion

    #region Audio Transitions
    private IEnumerator FadeFootsteps(bool fadeIn)
    {
        float startVolume = footstepSource.volume;
        float targetVolume = fadeIn ? footstepSounds.volume : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < 1f / sfxFadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            footstepSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime * sfxFadeSpeed);
            yield return null;
        }

        footstepSource.volume = targetVolume;
        if (!fadeIn)
        {
            footstepSource.Stop();
        }
    }

    private IEnumerator TransitionMusic(AudioClip newTrack)
    {
        if (newTrack == null)
        {
            Debug.LogError("Attempted to transition to null music track");
            yield break;
        }

        if (musicSource == null)
        {
            Debug.LogError("Music source is null");
            yield break;
        }

        float targetVolume = musicVolume;

        if (musicSource.clip != null && musicSource.isPlaying)
        {
            float startVolume = musicSource.volume;
            float elapsedTime = 0f;

            while (elapsedTime < musicTransitionDuration * 0.5f)
            {
                elapsedTime += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / (musicTransitionDuration * 0.5f));
                yield return null;
            }

            musicSource.volume = 0f;
        }

        musicSource.clip = newTrack;
        musicSource.Play();

        float fadeInTime = 0f;
        while (fadeInTime < musicTransitionDuration * 0.5f)
        {
            fadeInTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, targetVolume, fadeInTime / (musicTransitionDuration * 0.5f));
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
    #endregion
}