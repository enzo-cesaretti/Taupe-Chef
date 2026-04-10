using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Pool")]
    [SerializeField] private AudioSourcePool pool;

    [Header("Volumes")]
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;
    [Range(0, 1)] public float musicVolume = 1f;
    [Range(0, 1)] public float uiVolume = 1f;

    private AudioSource musicSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
    }

    #region PLAY METHODS
    
    public void Play(AudioData data)
    {
        var source = pool.Get();

        Configure(source, data);

        source.Play();

        StartCoroutine(ReturnToPool(source, data.clip.length));
    }

    public void PlayAtPosition(AudioData data)
    {
        var source = pool.Get();

        Configure(source, data);

        source.transform.position = data.targetObject.position;
        source.Play();

        StartCoroutine(ReturnToPool(source, data.clip.length));
    }

    public void PlayAttached(AudioData data)
    {
        var source = pool.Get();

        Configure(source, data);

        source.transform.SetParent(data.targetObject);
        source.transform.localPosition = Vector3.zero;

        source.Play();

        StartCoroutine(ReturnToPool(source, data.clip.length));
    }

    public void PlayUI(AudioData data)
    {
        var source = pool.Get();

        Configure(source, data);
        source.spatialBlend = 0f;

        source.Play();

        StartCoroutine(ReturnToPool(source, data.clip.length));
    }

    #endregion

    #region MUSIC

    public void PlayMusic(AudioData data, float fadeDuration = 1f)
    {
        StartCoroutine(FadeMusic(data, fadeDuration));
    }

    private IEnumerator FadeMusic(AudioData newMusic, float duration)
    {
        if (musicSource.isPlaying)
        {
            yield return FadeOut(musicSource, duration);
        }

        Configure(musicSource, newMusic);
        musicSource.loop = true;

        musicSource.Play();

        yield return FadeIn(musicSource, duration);
    }

    #endregion

    #region HELPERS

    private void Configure(AudioSource source, AudioData data)
    {
        source.clip = data.clip;
        source.volume = GetCategoryVolume(data);
        source.pitch = data.pitch;
        source.loop = data.loop;

        source.spatialBlend = data.spatial ? 1f : 0f;
        source.minDistance = data.minDistance;
        source.maxDistance = data.maxDistance;
    }

    private float GetCategoryVolume(AudioData data)
    {
        float cat = data.category switch
        {
            AudioCategory.SFX => sfxVolume,
            AudioCategory.UI => uiVolume,
            AudioCategory.Music => musicVolume,
            _ => 1f
        };

        return masterVolume * cat * data.volume;
    }

    private IEnumerator ReturnToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        pool.Release(source);
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float start = source.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(start, 0, t / duration);
            yield return null;
        }

        source.Stop();
    }

    private IEnumerator FadeIn(AudioSource source, float duration)
    {
        float target = source.volume;
        source.volume = 0;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(0, target, t / duration);
            yield return null;
        }
    }

    #endregion
}