﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip[] ac = new AudioClip[3];
    AudioSource audiosource;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        musicChange();
    }

    private void Update()
    {
        
    }

    public void musicChange()
    {
        string scenename = SceneManager.GetActiveScene().name;

        switch (scenename)
        {
            case "SampleScene":
                audiosource.clip = ac[0];
                audiosource.Play();
                break;
            case "scenes1":
                audiosource.clip = ac[1];
                audiosource.Play();
                break;
            case "scenes2":
                audiosource.clip = ac[2];
                audiosource.Play();
                break;
        }
    }

}
