using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBeatControl : MonoBehaviour
{
    public float pastbeat = 0f;
    public float offset = 0;
    public float beatcount = 0;
    public float halfbeatdur;
    public float beatdur;
    int onbeats;


    // Use this for initialization
    void Start()
    {
        beatdur = (60 / GetComponent<Conductor>().bpm);
    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<Conductor>().songposition + offset > pastbeat + beatdur)
        {
            GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatStep();
        }
    }
}
