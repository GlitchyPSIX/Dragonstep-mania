﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{

    private Animator ac;
    private AudioSource asource;
    public AudioClip march1;
    public AudioClip march2;
    public AudioClip backmarch1;
    public AudioClip backmarch2;
    public AudioClip ha;
    public AudioClip hoi;
    public AudioClip hm;
    public AudioClip hah;
    public AudioClip boh;
    public AudioClip cowbell;
    public AudioClip miss;

    //This clip controls the animations and sound effects for the steps

    // Use this for initialization
    void Start()
    {
        ac = GetComponent<Animator>();
        if (gameObject.CompareTag("Player") == true)
        {
            cowbell = Resources.Load<AudioClip>("Sounds/SFX/cowbell");
            asource = GetComponent<AudioSource>();
        }
    }
    
    public void OnBeatStep()
    {
        ac.Play("On Beat");
        if (gameObject.CompareTag("Player") == true)
        {
            asource.clip = march1;
            asource.Play();
        }
    }    
    
    public void OnBeatMiss()
    {
        ac.Play("X_Onbeat");
        if (gameObject.CompareTag("Player") == true)
        {
            asource.clip = miss;
            asource.Play();
        }
    }

        public void OffBeatMiss()
    {
        ac.Play("X_Offbeat");
        if (gameObject.CompareTag("Player") == true)
        {
            asource.clip = miss;
            asource.Play();
        }
    }

    public void OffBeatStep()
    {
        {
            ac.Play("Off Beat");
            if (gameObject.CompareTag("Player") == true)
            {
                asource.clip = backmarch1;
                asource.Play();
            }
        }


    }

    public void PrepareStep()
    {
        ac.Play("Prepare");
        if (gameObject.CompareTag("Player") == true)
        {
            asource.clip = cowbell;
            asource.Play();
        }
    }
}
