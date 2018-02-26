using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Conductor : MonoBehaviour
{

    //This class holds the beat duration.
    //thanks, fizzd, I owe you a big one.

    public float bpm = 120;
    public float offset = 0;
    public double songposition;
    double dspOffset = 0d;
    public AudioSource song;
    public AudioListener gameSpeaker;

    public bool trainingMode;

    // Use this for initialization
    void Start()
    {
        song = GetComponent<AudioSource>();
        resetStartOfSong(false);
    }

    // Update is called once per frame
    void Update()
    {
        songposition = ((double)(AudioSettings.dspTime) - dspOffset) * song.pitch - offset;
    }

    public void resetStartOfSong(bool pauseAtStart){
        dspOffset = AudioSettings.dspTime;
        if (pauseAtStart) { 
            AudioListener.pause = true;
        }
    }

}
