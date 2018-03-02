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
    Timeline timeline;
    public bool trainingMode;

    // Use this for initialization
    void Start()
    {
        timeline = GetComponent<Timeline>();
        song = GetComponent<AudioSource>();
        resetStartOfSong(false);
    }

    // Update is called once per frame
    void Update()
    {
        songposition = ((AudioSettings.dspTime) - dspOffset) * song.pitch - offset;
    }

    public void resetStartOfSong(bool pauseAtStart){
        dspOffset = AudioSettings.dspTime;
        if (pauseAtStart) { 
            AudioListener.pause = true;
        }
    }

    public IEnumerator resetBeatmap()
    {
        timeline.beatcount = 1;
        song.Play();
        yield return null;
    }

}
