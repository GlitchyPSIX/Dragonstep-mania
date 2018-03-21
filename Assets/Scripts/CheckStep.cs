﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 

This file checks for misses and if you've stepped correctly or incorrectly.

*/
public class CheckStep : MonoBehaviour
{
    public double beatMultiplier;
    public double beatdur;
    public double pastHitPos;
    public Text OSText;
    public bool isHurt = false;
    public byte hurtOrientation = 0;
    public double caughtPosition;
    /*
    HURTING ORIENTATION GUIDE:
    It got hurt onbeat if it's 0.
    It got hurt at backbeat if it's 1.
    */
    int hitCounter = 0;
    int missCounter = 0;
    Timeline timeline;

    // Use this for initialization
    void Start()
    {
        timeline = GetComponent<Timeline>();
        beatdur = timeline.beatdur * timeline.beatmultiplier;
        beatMultiplier = timeline.beatmultiplier;

    }

    // Update is called once per frame
    void Update()
    {
        beatdur = timeline.beatdur * timeline.beatmultiplier;
        if (Input.GetButtonDown("Step"))
        {
            //if inside the margin
            if (
                    (
                        ((GetComponent<Conductor>().songposition > (timeline.pastbeat + (0.75f * beatdur))
                        )
                            ||
                        (GetComponent<Conductor>().songposition < (timeline.pastbeat + (0.25f * beatdur))
                        ))
                    ) && timeline.autoMode == false
                )
            {
                performStep();
            }
            else
            {
                //if outside margin (Mis-press)
                missCounter++;
            }
        }
        //show info
        OSText.text = "Beat count:" + timeline.beatcount.ToString() +
        "\n" + "Song offset: " + (GetComponent<Conductor>().songposition + timeline.offset).ToString() +
        "\nLast Hit: " + pastHitPos.ToString() + "\n"
        + "Caught position: " + caughtPosition.ToString() + "\n"
        + "Last beat: " + (timeline.pastbeat).ToString()
        + "\n Is hurt?: " + GetComponent<CheckStep>().isHurt.ToString() +
        "\n Hurt Orientation: " + GetComponent<CheckStep>().hurtOrientation.ToString()
        + "\n Misses: " + GetComponent<CheckStep>().missCounter.ToString()
        + "\n Sucessful hits: " + GetComponent<CheckStep>().hitCounter.ToString()
        + "\n Snap: " + timeline.snap.ToString()
        + "\n Difference:" + (timeline.pastbeat - caughtPosition).ToString();

    }

    public void performStep()
    {
        caughtPosition = Math.Abs((GetComponent<Conductor>().songposition - (timeline.pastbeat + beatdur)));
        if (timeline.autoMode == true)
        {
            pastHitPos += (beatdur);
        }
        else if (GetComponent<Conductor>().songposition > (timeline.pastbeat + (0.75f * beatdur)))
        {
            pastHitPos += (beatdur + (caughtPosition));
        }
        else if (GetComponent<Conductor>().songposition < (timeline.pastbeat + (0.25f * beatdur)))
        {
            pastHitPos += (beatdur - (caughtPosition));
        }
        
        //add a successful hit
        hitCounter++;
        /*
          Working on this stuff once agian, I think I've found one of the core issues.
        */
        isHurt = false;
        if ((timeline.isPreparing) && (!(timeline.StepOnOffbeats)) && timeline.stayStill == false) //If onbeat(default) AND is preparing
        {
            GameObject.FindWithTag("Player").GetComponent<Step>().PrepareStep();
        }
        else if (!(timeline.StepOnOffbeats) && timeline.stayStill == false) //If onbeat
        {
            GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatStep();
        }
        else if ((timeline.StepOnOffbeats) && timeline.stayStill == false) //If at backbeat
        {
            GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatStep();
        }
        else
        {
            // Do not play any animation.
        }
    }

    public void CheckMiss(float treshold = 1.25f)
    {
        if (GetComponent<Timeline>().autoMode == false)
        {
            if (!isHurt)
            {
                //if NOT hurt, wait a little bit after the last beat (chance window)
                if (GetComponent<Conductor>().songposition > (pastHitPos + (beatdur + (treshold * beatdur))))
                {
                    //add miss and perform correct animation
                    isHurt = true;
                    pastHitPos += (beatdur - (beatdur * treshold) );
                    missCounter++;
                    if (!timeline.switchingStep)
                    {
                        {
                            if (!(timeline.StepOnOffbeats))
                            {
                                GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatMiss();
                                hurtOrientation = 0;

                            }
                            else if ((timeline.StepOnOffbeats))
                            {
                                GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatMiss();
                                hurtOrientation = 1;
                            }
                        }
                    }

                }
            }
            else if (isHurt)
            {
                //if hurt, and the songposition already progressed a beat * multiplier
                if (GetComponent<Conductor>().songposition > (timeline.pastbeat))
                {
                    pastHitPos += (beatdur + Math.Abs((GetComponent<Conductor>().songposition - (timeline.pastbeat + beatdur))));
                    //add miss
                    missCounter++;
                    //this block below will make the orientation switch in case the step is switched
                    if (!timeline.switchingStep)
                    {
                        {
                            if (hurtOrientation == 1 && !(timeline.StepOnOffbeats))
                            {
                                GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatMiss();
                                hurtOrientation = 0;
                            }
                            else if (hurtOrientation == 0 && (timeline.StepOnOffbeats))
                            {
                                GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatMiss();
                                hurtOrientation = 1;
                            }
                        }
                    }
                    //increment the past hit by a beat (AUTO)

                }
            }
            else if (GetComponent<Timeline>().autoMode == true)
            {
                //AUTO is enabled, no miss check should take in place.
            }
        }
    }
}