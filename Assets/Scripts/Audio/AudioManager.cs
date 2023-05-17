using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] soundsList;

    public static AudioManager instance;

    void Awake()
    {
        // Singleton Pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Persist
        DontDestroyOnLoad(gameObject);

        // Attach an AudioSource to the Manager for every Sound
        foreach(var sound in soundsList)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }
    }

    private Sound FindSound(string soundName)
    {
        // Find the Sound if it exists
        for (int i = 0; i < soundsList.Length; i++)
        {
            if (soundName.Equals(soundsList[i].name))
            {
                return soundsList[i];
            }
        }

        return null;
    }

    public void Play(string soundName, bool playTillComplete = false)
    {
        Sound sound = FindSound(soundName);
        if (sound == null)
        {
            Debug.LogWarning("No Sound Found. Name Given: " + soundName);
            return;
        }

        // Don't Play if PlayToCompletion Flag is On
        if (playTillComplete && sound.source.isPlaying)
        {
            return;
        }

        // Play the Sound
        sound.source.Play();

    }

    public void Stop(string soundName)
    {
        Sound sound = FindSound(soundName);
        if (sound == null)
        {
            Debug.LogWarning("No Sound Found. Name Given: " + soundName);
            return;
        }

        // Stop Playing
        sound.source.Stop();
    }
}
