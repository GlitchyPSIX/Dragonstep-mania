using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{

    //This class holds the beat duration.

    public float bpm = 120;
    public float offset = 0.2f;
    public float songposition;
    public AudioSource song;

    // Use this for initialization
    void Start()
    {
        song = GetComponent<AudioSource>();
    }
	
    // Update is called once per frame
    void Update()
    {
        songposition = (float)(song.time) * song.pitch - offset;
    }
}
