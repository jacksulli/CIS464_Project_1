using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Code is a modified version of the AudioManager from this video: https://www.youtube.com/watch?v=VFOGGml_N6g

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; //Reference to the global instance of this script

    public Sound[] musicSounds, sfxSounds; //Array for SFX and Music
    public AudioSource musicSource, sfxSource;


    private void Awake()
    {
        //Make this object a singleton so there can only be one in the project
        //If the game doesn't detect an audiomanager, make a new one
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayMusic(string _name)
    {

    }

    public void PlaySound(string _name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == _name); //Find the sound with the name name being inputted

        //If there is no sound with that name
        if(s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip); //Play the sound
        }
    }
}
