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
    /*
    HURTING ORIENTATION GUIDE:
    It got hurt onbeat if it's 0.
    It got hurt at backbeat if it's 1.
    */
    int hitCounter = 0;
    int missCounter = 0;
    // Use this for initialization
    void Start()
    {
        beatdur = GetComponent<Ticko>().beatdur * GetComponent<Ticko>().beatmultiplier;
        beatMultiplier = GetComponent<Ticko>().beatmultiplier;
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
                        (GetComponent<Conductor>().songposition < (GetComponent<Ticko>().pastbeat + (0.25f * beatdur))
                        )
                            ||
                        (GetComponent<Conductor>().songposition > (GetComponent<Ticko>().pastbeat + (0.75f * beatdur))
                        )
                    )
                )
            {
                //add a successful hit
                hitCounter++;
                isHurt = false;
                if (!(GetComponent<Ticko>().StepOnOffbeats)) //If onbeat
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatStep();
                    pastHitPos = GetComponent<Conductor>().songposition;
                }
                else if ((GetComponent<Ticko>().StepOnOffbeats)) //If at backbeat
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatStep();
                    pastHitPos = GetComponent<Conductor>().songposition;
                }
            }
            else
            {
                //if outside margin (Mis-press)
            }
        }
        //check for miss each frame
        CheckMiss();
        //show info
        OSText.text = "Beat count:" + GetComponent<Ticko>().beatcount.ToString() +
        "\n" + "Song offset: " + (GetComponent<Conductor>().songposition + GetComponent<Ticko>().offset).ToString() +
        "\nLast Hit: " + pastHitPos.ToString() + "\n"
        + "Last beat: " + (GetComponent<Ticko>().pastbeat - (0.3 * (beatdur * GetComponent<Ticko>().beatmultiplier))).ToString()
        + "\n Is hurt?: " + GetComponent<CheckStep>().isHurt.ToString() +
        "\n Hurt Orientation: " + GetComponent<CheckStep>().hurtOrientation.ToString()
        + "\n Misses: " + GetComponent<CheckStep>().missCounter.ToString()
        + "\n Sucessful hits: " + GetComponent<CheckStep>().hitCounter.ToString();

    }
    void CheckMiss()
    {
        if (isHurt)
        {
            //if hurt, and the songposition already progressed a beat * multiplier
            if (GetComponent<Conductor>().songposition > (pastHitPos + beatdur))
            {
                //increment the past hit by a beat (AUTO)
                pastHitPos += beatdur;
                //add miss
                missCounter++;
                //this block below will make the orientation switch in case the step is switched
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
        else if (!isHurt)
        {
            //if NOT hurt, wait a little bit after the last beat (chance window)
            if (GetComponent<Conductor>().songposition > (pastHitPos + (1.15f * beatdur)))
            {
                //add miss and perform correct animation
                isHurt = true;
                missCounter++;
                pastHitPos = GetComponent<Ticko>().pastbeat;
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
    }

}
