using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool disable = false;

    public Sound[] soundsList;
    // Using a Dictionary at Run-Time for Faster Searching
    private Dictionary<string, Sound> soundTable;

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
        
        soundTable = new Dictionary<string, Sound>();

        // Attach an AudioSource to the Manager for every Sound
        foreach(var sound in soundsList)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;

            soundTable.Add(sound.name, sound);

            // Don't Play any Sounds
            if (disable)
            {
                sound.source.volume = 0.0f;
            }
        }
    }

    private Sound FindSound(string soundName)
    {
        // Find the Sound if it exists
        if (soundTable.ContainsKey(soundName))
        {
            return soundTable[soundName];
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
