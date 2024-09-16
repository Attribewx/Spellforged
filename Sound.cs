using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    public AudioMixerGroup Audino;

    [Range(0f, 1f)]
    public float volume = .7f;
    [Range(.1f, 3f)]
    public float pitch = 1;

    public bool isLooping;


    [HideInInspector]
    public AudioSource source;
}
