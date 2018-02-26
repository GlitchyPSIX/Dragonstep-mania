using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 

This file checks for misses and if you've stepped correctly or incorrectly.

*/
public class CheckStep : MonoBehaviour
{
    public float beatMultiplier;
    public float beatdur;
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
        beatdur = GetComponent<Ticko>().beatdur * GetComponent<Ticko>().beatmultiplier;
        beatMultiplier = GetComponent<Ticko>().beatmultiplier;
        timeline = GetComponent<Timeline>();
    }

    // Update is called once per frame
    void Update()
    {
        beatdur = GetComponent<Ticko>().beatdur * GetComponent<Ticko>().beatmultiplier;
        if (Input.GetButtonDown("Step"))
        {
            //if inside the margin
            if (
                    (
                        ((GetComponent<Conductor>().songposition < (GetComponent<Ticko>().pastbeat + (0.25f * beatdur))
                        )
                            ||
                        (GetComponent<Conductor>().songposition > (GetComponent<Ticko>().pastbeat + (0.75f * beatdur))
                        ))
                    ) && timeline.autoMode == false
                )
            {
                performStep();
                caughtPosition = (GetComponent<Conductor>().songposition - GetComponent<Ticko>().pastbeat);
            }
            else
            {
                //if outside margin (Mis-press)
                missCounter++;
            }
        }
        //show info
        OSText.text = "Beat count:" + GetComponent<Ticko>().beatcount.ToString() +
        "\n" + "Song offset: " + (GetComponent<Conductor>().songposition + GetComponent<Ticko>().offset).ToString() +
        "\nLast Hit: " + pastHitPos.ToString() + "\n"
        + "Last beat: " + (GetComponent<Ticko>().pastbeat - (0.3 * (beatdur * GetComponent<Ticko>().beatmultiplier))).ToString()
        + "\n Is hurt?: " + GetComponent<CheckStep>().isHurt.ToString() +
        "\n Hurt Orientation: " + GetComponent<CheckStep>().hurtOrientation.ToString()
        + "\n Misses: " + GetComponent<CheckStep>().missCounter.ToString()
        + "\n Sucessful hits: " + GetComponent<CheckStep>().hitCounter.ToString()
        + "\n Snap: " + timeline.snap.ToString()
        + "\n Difference:" + (GetComponent<Conductor>().songposition - GetComponent<Ticko>().pastbeat).ToString();

    }

    public void performStep()
    {
        //add a successful hit
        hitCounter++;
        /*
          Case that I took way too long to figure out:
          I was solely taking in consideration if you stepped *after*, which lead to the question of
          what the h3ck would happen if you stepped before. So I worked on the treshold by making a difference
          and removing it from the beat duration so the spacing is even, unlike a case where it extended way past where it should have, 
          and computed the miss 2 beats later, and for the earlier step, just add the whole beat with no difference so it doesn't step before.
          I still have to finish the rest.. ?¿?
          Not working as expected.
          Classic...
        */
        if (pastHitPos > GetComponent<Ticko>().pastbeat)
        {
            pastHitPos += (beatdur - ((GetComponent<Conductor>().songposition - GetComponent<Ticko>().pastbeat) * 0.75));
        }
        else if (pastHitPos < GetComponent<Ticko>().pastbeat)
        {
            pastHitPos += (beatdur);
        }
            isHurt = false;
        if ((timeline.isPreparing) && (!(GetComponent<Ticko>().StepOnOffbeats)) && timeline.stayStill == false) //If onbeat(default) AND is preparing
        {
            GameObject.FindWithTag("Player").GetComponent<Step>().PrepareStep();
        }
        else if (!(GetComponent<Ticko>().StepOnOffbeats) && timeline.stayStill == false) //If onbeat
        {
            GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatStep();
        }
        else if ((GetComponent<Ticko>().StepOnOffbeats) && timeline.stayStill == false) //If at backbeat
        {
            GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatStep();
        }
        else
        {
            // Do not play any animation.
        }
    }

    public void CheckMiss(float treshold = 1.15f)
    {
        if (GetComponent<Timeline>().autoMode == false)
        {
            if (!isHurt)
            {
                //if NOT hurt, wait a little bit after the last beat (chance window)
                if (GetComponent<Conductor>().songposition > (pastHitPos + (treshold * beatdur)))
                {
                    //add miss and perform correct animation
                    isHurt = true;
                    missCounter++;
                    if (!timeline.switchingStep)
                    {
                        {
                            if (!(GetComponent<Ticko>().StepOnOffbeats))
                            {
                                GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatMiss();
                                hurtOrientation = 0;

                            }
                            else if ((GetComponent<Ticko>().StepOnOffbeats))
                            {
                                GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatMiss();
                                hurtOrientation = 1;
                            }
                        }
                    }
                    pastHitPos += (beatdur);
                }
            }
            else if (isHurt)
            {
                //if hurt, and the songposition already progressed a beat * multiplier
                if (GetComponent<Conductor>().songposition > (pastHitPos + beatdur))
                {
                    //add miss
                    missCounter++;
                    //this block below will make the orientation switch in case the step is switched
                    if (!timeline.switchingStep)
                    {
                        {
                            if (hurtOrientation == 1 && !(GetComponent<Ticko>().StepOnOffbeats))
                            {
                                GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatMiss();
                                hurtOrientation = 0;
                            }
                            else if (hurtOrientation == 0 && (GetComponent<Ticko>().StepOnOffbeats))
                            {
                                GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatMiss();
                                hurtOrientation = 1;
                            }
                        }
                    }
                    //increment the past hit by a beat (AUTO)
                    pastHitPos += beatdur;
                }
            }
            else if (GetComponent<Timeline>().autoMode == true)
            {
                //AUTO is enabled, no miss check should take in place.
            }
        }

    }
}