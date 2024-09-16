using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager arduino;
    public AudioMixer audioSettings;
    private bool bossMixing;
    private bool oneTime;

    void Awake()
    {
        if (arduino == null)
        {
            arduino = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.isLooping;
            s.source.outputAudioMixerGroup = s.Audino;
        }
    }

    void Update()
    {
        if (bossMixing)
        {
            float bossSFX;
            audioSettings.GetFloat("Boss", out bossSFX);
            float worldSFX;
            audioSettings.GetFloat("World", out worldSFX);
            audioSettings.SetFloat("Boss", LeanTween.clerp(bossSFX, 0, .01f));
            audioSettings.SetFloat("World", LeanTween.clerp(worldSFX, -80, .01f));
        }
        else if (!bossMixing && oneTime)
        {
            float bossSFX;
            audioSettings.GetFloat("Boss", out bossSFX);
            float worldSFX;
            audioSettings.GetFloat("World", out worldSFX);
            audioSettings.SetFloat("Boss", LeanTween.clerp(bossSFX, -80, .001f));
            audioSettings.SetFloat("World", LeanTween.clerp(worldSFX, 0, .001f));

            if(worldSFX >= -.05f && worldSFX <= .05f)
            {
                audioSettings.SetFloat("World", 0);
                audioSettings.SetFloat("Boss", -80);
                oneTime = false;
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("No Sound Found For: " + name);
            return;
        }
        s.source.Play();
    }

    public void Play2(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("No Sound Found For: " + name);
            return;
        }
        s.source.PlayOneShot(s.source.clip);
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            return false;
        }
        else
        {
            return s.source.isPlaying;
        }
    }

    public void BossMix(bool bossStart)
    {
        
        if (bossStart)
        {
            bossMixing = true;
        }
        else
        {
            bossMixing = false;
            oneTime = true;
        }
    }
}
