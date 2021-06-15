using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgm;
    public AudioSource[] soundEfftecs;//create array for all audio sources

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void PlaySFX(int sfxNumber)
    {
        soundEfftecs[sfxNumber].Stop();
        soundEfftecs[sfxNumber].Play();
    }

    public void StopSFX(int sfxNumber)
    {
        soundEfftecs[sfxNumber].Stop();//just in case that needed
    }
}
