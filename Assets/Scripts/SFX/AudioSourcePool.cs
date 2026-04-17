using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    [SerializeField] private int initialSize = 20;

    private Queue<AudioSource> pool = new Queue<AudioSource>();

    void Awake()
    {
        for (int i = 0; i < initialSize; i++)
            pool.Enqueue(Create());
    }

    private AudioSource Create()
    {
        var go = new GameObject("AudioSource");
        go.transform.parent = transform;

        var source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;

        return source;
    }

    public AudioSource Get()
    {
        return pool.Count > 0 ? pool.Dequeue() : Create();
    }

    public void Release(AudioSource source)
    {
        source.Stop();
        source.transform.parent = transform;
        pool.Enqueue(source);
    }
}