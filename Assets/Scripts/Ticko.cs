using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ticko : MonoBehaviour
{
    private bool switchingStep;
    public double pastbeat = 0d;
    public float offset = 0;
    public float beatcount = 0;
    public float beatmultiplier;
    public float beatdur;
    public bool StepOnOffbeats = false;
    GameObject[] BackgroundSwitchers;

    // This class holds the CPU logic (step each beat), and the step switch.

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
        // testing purposes - press A to make the whole game orientation switch to offbeat
        if (Input.GetKeyDown("a"))
        {
            switchStep(true);
        }

        // every beat (With the multiplier in action, probably gonna use this for swing beats)
        if (GetComponent<Conductor>().songposition + offset > pastbeat + (beatdur * beatmultiplier))
        {
            switchStep(false);
            // ^ makes sure the beat is 1x when required (used to switch to offbeat)
            beatcount += (1 * beatmultiplier);

            /* CPU LOGIC START
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
    public void switchStep(bool halveBeat)
    {
        if (halveBeat == false)
        {
            //bring back the multiplier to one if we're transitioning, offbeat transition stopped
            if (switchingStep == true)
            {
                beatmultiplier = 1;
                StepOnOffbeats = !StepOnOffbeats;
                switchingStep = false;
            }
            else //if it's already off-transition
            {
                pastbeat += (beatdur * beatmultiplier);
            }
        }
        else if (halveBeat == true)
        {
            //halve the multiplier for a "beat", reduce the last beat by half a beat, so it goes into the backbeat
            switchingStep = true;
            beatmultiplier = 0.5f;
            pastbeat -= (beatdur * beatmultiplier);
        }
    }
}