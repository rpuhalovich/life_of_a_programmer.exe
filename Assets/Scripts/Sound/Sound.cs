using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField] public AudioClip clip;

    [SerializeField] public string name;

    [Range(0.0f, 1.0f)]
    [SerializeField] public float volume;
    [Range(-1.0f, 1.0f)]
    [SerializeField] public float pitch = 1.0f;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
