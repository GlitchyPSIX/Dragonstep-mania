﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ticko : MonoBehaviour
{
    private bool switchingStep;
    public float pastbeat = 0f;
    public float lastonbeat = 0f;
    public float lastoffbeat = 0f;
    public float offset = 0;
    public float beatcount = 0;
    public float beatmultiplier;
    public float beatdur;
    public Text OSNumber;
    public bool IsOffbeat = false;
    public bool StepOnOffbeats = false;
    GameObject[] BackgroundSwitchers;
    int onbeats;


    // Use this for initialization
    void Start()
    {
        //Specify beatduration and background stepswitchers
        beatmultiplier = 1f;
        beatdur = (60 / GetComponent<Conductor>().bpm);
        //for every background stepswitcher
        BackgroundSwitchers = GameObject.FindGameObjectsWithTag("Background");
    }

    // Update is called once per frame
    void Update()
    {
        // debug - press A to make the whole game orientation switch to offbeat
        if (Input.GetKeyDown("a"))
        {
            updateOrientation(1);
        }

        // every beat (With the multiplier in action, probably gonna use this for swing beats)
        if (GetComponent<Conductor>().songposition + offset > pastbeat + (beatdur * beatmultiplier))
        {
            updateOrientation(0);
            beatcount += 1f;


            /* CPU LOGIC START
            I'm trying to figure out my own slightly spaghettified code. God I am bad, dude...
            */
            if (StepOnOffbeats == true)
            {
                //if offbeat
                foreach (GameObject switcher in BackgroundSwitchers)
                {
                    switcher.GetComponent<Step>().OffBeatStep();
                }

            }
            else if (StepOnOffbeats == false)
            {
                //if not offbeat
                foreach (GameObject switcher in BackgroundSwitchers)
                {
                    switcher.GetComponent<Step>().OnBeatStep();
                }

            }

        }
    }

    void updateOrientation(byte UpdateOrNot)
    {
        if (UpdateOrNot == 0)
        {
            //bring back the multiplier to one if we're transitioning, offbeat transition stopped
            if (switchingStep == true)
            {
                beatmultiplier = 1;
                StepOnOffbeats = !StepOnOffbeats;
                switchingStep = false;
            }
            else
            {
                pastbeat += (beatdur * beatmultiplier);
            }
        }
        if (UpdateOrNot == 1)
        {
            //halve the multiplier for a bit so it goes into the backbeat
            switchingStep = true;
            beatmultiplier = 0.5f;
            pastbeat -= (beatdur * beatmultiplier);
        }
    }
}