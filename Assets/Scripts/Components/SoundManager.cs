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
    [SerializeField] private AudioCollection damageTakenSounds;
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

        Debug.Log($"Audio sources setup complete. Music Source: {musicSource != null}, SFX Source: {sfxSource != null}, Footstep Source: {footstepSource != null}");
    }
    #endregion

    #region Level and State Management
    public void OnLevelLoaded()
    {
        Debug.Log("=== LEVEL LOADED EVENT ===");
        Debug.Log($"Previous state - IsInGame: {isInGame}, Current Music: {(musicSource.clip != null ? musicSource.clip.name : "null")}");

        isInGame = true;

        if (GameManager.WorldClock != null)
        {
            WorldState currentState = GameManager.WorldClock.CurrentState;
            Debug.Log($"Level Loaded - Current world state: {currentState}");
            Debug.Log("Forcing music update for level load");
            UpdateMusicBasedOnWorldState(currentState);
        }
        else
        {
            Debug.LogError("WorldClock is null during OnLevelLoaded");
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

        // First verify we have all music clips assigned
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
        else
        {
            Debug.Log($"No music change needed. Current: {(musicSource.clip != null ? musicSource.clip.name : "null")}");
        }
    }

    private void OnWorldStateChanged(WorldState newState)
    {
        Debug.Log($"World state changed to: {newState}");
        currentState = newState;
        UpdateMusicBasedOnWorldState(newState);
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

    public void OnTalismanCast()
    {
        if (talismanSounds.clips != null && talismanSounds.clips.Length > 0)
        {
            AudioClip clip = talismanSounds.clips[Random.Range(0, talismanSounds.clips.Length)];
            sfxSource.pitch = 1f + Random.Range(-talismanSounds.randomPitchVariance, talismanSounds.randomPitchVariance);
            sfxSource.PlayOneShot(clip, talismanSounds.volume);
        }
    }

    public void OnPlayerDamaged()
    {
        if (damageTakenSounds.clips != null && damageTakenSounds.clips.Length > 0)
        {
            AudioClip clip = damageTakenSounds.clips[Random.Range(0, damageTakenSounds.clips.Length)];
            sfxSource.pitch = 1f + Random.Range(-damageTakenSounds.randomPitchVariance, damageTakenSounds.randomPitchVariance);
            sfxSource.PlayOneShot(clip, damageTakenSounds.volume);
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

    public void OnAttack()
    {
        if (attackSounds.clips != null && attackSounds.clips.Length > 0)
        {
            AudioClip clip = attackSounds.clips[Random.Range(0, attackSounds.clips.Length)];
            sfxSource.pitch = 1f + Random.Range(-attackSounds.randomPitchVariance, attackSounds.randomPitchVariance);
            sfxSource.PlayOneShot(clip, attackSounds.volume);
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
        Debug.Log($"TransitionMusic started. Target track: {newTrack.name}");

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

        // Store the target volume for later
        float targetVolume = musicVolume;

        // If we're currently playing something, fade it out
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

            musicSource.volume = 0f; // Ensure we're fully faded out
        }

        // Change the track
        Debug.Log($"Changing clip from {(musicSource.clip != null ? musicSource.clip.name : "null")} to {newTrack.name}");
        musicSource.Stop();
        musicSource.clip = newTrack;
        musicSource.Play();

        // Verify the clip changed and is playing
        if (musicSource.clip != newTrack || !musicSource.isPlaying)
        {
            Debug.LogError($"Failed to change music! Clip assigned: {musicSource.clip == newTrack}, Is Playing: {musicSource.isPlaying}");
            yield break;
        }

        // Fade in the new track
        float fadeInTime = 0f;
        while (fadeInTime < musicTransitionDuration * 0.5f)
        {
            fadeInTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, targetVolume, fadeInTime / (musicTransitionDuration * 0.5f));
            yield return null;
        }

        musicSource.volume = targetVolume;
        Debug.Log($"Music transition complete. Now playing: {musicSource.clip.name} at volume {musicSource.volume}");
    }
    #endregion

    #region Volume Controls
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        audioMixer.SetFloat("FootstepsVolume", Mathf.Log10(volume) * 20);
    }

    public void StopAllAudio()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
        if (footstepSource != null)
        {
            footstepSource.Stop();
        }
    }
    #endregion

    #region Debug
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            WorldState state = GameManager.WorldClock.CurrentState;
            Debug.Log($"Debug Check - World State: {state}");
            Debug.Log($"Debug Check - Current Music: {(musicSource.clip != null ? musicSource.clip.name : "null")}");
            Debug.Log($"Debug Check - Is Playing: {musicSource.isPlaying}");
            Debug.Log($"Debug Check - Volume: {musicSource.volume}");
            Debug.Log($"Debug Check - Correct clip for state is loaded: {IsCorrectMusicForState(state)}");
        }
    }

    private bool IsCorrectMusicForState(WorldState state)
    {
        switch (state)
        {
            case WorldState.WORLD_START:
                return musicSource.clip == worldStartMusic;
            case WorldState.WORLD_WITCHING:
                return musicSource.clip == worldWitchingMusic;
            case WorldState.WORLD_HELL:
                return musicSource.clip == worldHellMusic;
            default:
                return false;
        }
    }
    #endregion
}