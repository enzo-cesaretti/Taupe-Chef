using UnityEngine;

[CreateAssetMenu(menuName = "SFX/Audio Data")]
public class AudioData : ScriptableObject
{
    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;

    public bool loop = false;
    public bool spatial = true;

    [Header("3D Settings")]
    public float minDistance = 1f;
    public float maxDistance = 20f;
    public GameObject targetObject;

    [Header("Category")]
    public AudioCategory category;
}

public enum AudioCategory
{
    SFX,
    UI,
    Music,
    Ambience
}