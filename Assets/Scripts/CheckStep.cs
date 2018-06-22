using System;
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
        beatMultiplier = timeline.beatmultiplier;
        beatdur = timeline.beatdur * timeline.beatmultiplier;

    }

    // Update is called once per frame
    void Update()
    {
        beatdur = timeline.beatdur * timeline.beatmultiplier;
        if (Input.GetButtonDown("Step"))
        {
            if (
            (
                ((GetComponent<Conductor>().songposition > (timeline.pastbeat + (0.75d * beatdur))
                )
                    ||
                (GetComponent<Conductor>().songposition < (timeline.pastbeat + (0.25d * beatdur))
                ))
            ) && timeline.autoMode == false
        )
            {
                //if inside the margin

                performStep();
            }
            else if (
                     (
                         ((GetComponent<Conductor>().songposition < (timeline.pastbeat + (0.75d * beatdur))
                         )
                             ||
                         (GetComponent<Conductor>().songposition > (timeline.pastbeat + (0.25d * beatdur))
                         ))
                     ) && timeline.autoMode == false
                 )
            {
                //if outside margin (Mis-press)
                Debug.Log("Oops.");
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
        caughtPosition = (GetComponent<Conductor>().songposition - (timeline.pastbeat + beatdur * 0.75d));
        if (timeline.autoMode == true || timeline.isPreparing == true)
        {
            animateStep();
            isHurt = false;
            //add a successful hit
            hitCounter++;
        }
        else if (timeline.timeState == (byte)Timeline.timeStates.Early)
        {
            animateStep();
            isHurt = false;
            //add a successful hit
            hitCounter++;
        }
        else if (timeline.timeState == (byte)Timeline.timeStates.Late)
        {
            animateStep();
            Debug.Log("Late hit.");
            isHurt = false;
            //add a successful hit
            hitCounter++;
        }
        else if ((GetComponent<Conductor>().songposition == (timeline.pastbeat + beatdur)))
        {
            Debug.Log("PERFECT (Are you even human?) hit.");
            animateStep();
        }

        /*
          Working on this stuff once agian, I think I've found one of the core issues.
        */
    }

    void animateStep()
    {
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

    public void CheckMiss(double treshold = 0.30d)
    {
        if (timeline.autoMode == false && timeline.isPreparing == false)
        {
            if (!isHurt)
            {
                //if NOT hurt, wait a little bit after the last beat (chance window)
                if ((GetComponent<Conductor>().songposition > (timeline.pastbeat + (treshold * beatdur))) && timeline.timeState == (byte)Timeline.timeStates.Late)
                {
                    Debug.Log("MISSED");
                    timeline.timeState = (byte)Timeline.timeStates.Neutral;
                    //add miss and perform correct animation
                    isHurt = true;
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
            else if (GetComponent<Timeline>().autoMode == true || GetComponent<Timeline>().isPreparing == true)
            {
                //AUTO is enabled, no miss check should take in place.
            }
        }
    }
}