using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    [SerializeField] private string[] soundNames;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        foreach (string sn in soundNames)
        {
            Play(sn);
        }
    }

    public void Play(string name)
    {
        Sound s = FindSound(name);
        s.source.Play();
    }

    public void PlayRandom(string name)
    {
        Sound s = FindSound(name);
        s.source.time = UnityEngine.Random.Range(0.0f, s.source.clip.length);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = FindSound(name);
        s.source.Stop();
    }

    public bool isPlaying(string name)
    {
        Sound s = FindSound(name);
        return s.source.isPlaying;
    }

    public void SetVolume(string name, float v)
    {
        Sound s = FindSound(name);
        s.volume = v;
    }

    private Sound FindSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return null;
        }
        return s;
    }
}
