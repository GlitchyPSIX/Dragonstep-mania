using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ticko : MonoBehaviour
{
    public bool switchingStep;
    public double pastbeat = 0d;
    public float offset = 0;
    public float beatcount = 0;
    public float beatmultiplier;
    public float beatdur;
    public bool StepOnOffbeats = false;
    GameObject[] BackgroundSwitchers;
    Timeline timeline;

    // This class holds the CPU logic (step each beat), and the step switch.

    // Use this for initialization
    void Start()
    {
        //Specify beatduration and background stepswitchers
        beatmultiplier = 1f;
        beatdur = (60 / GetComponent<Conductor>().bpm);
        //for every background stepswitcher
        BackgroundSwitchers = GameObject.FindGameObjectsWithTag("Background");
        timeline = GetComponent<Timeline>();
    }

    // Update is called once per frame
    void Update()
    {
        // testing purposes - press A to make the whole game orientation switch to offbeat
        if (Input.GetKeyDown("a"))
        {
            timeline.addAction(2, 7);
            timeline.addAction(2, 29.5f);
        }

        if (Input.GetKeyDown("s"))
        {
            //emergency switch
            timeline.switchStep(true);
        }

        if (Input.GetKeyDown("d")){
            timeline.autoMode = !timeline.autoMode;
        }

        // every beat (With the multiplier in action, probably gonna use this for swing beats)
        if (GetComponent<Conductor>().songposition + offset > pastbeat + (beatdur * beatmultiplier))
        {
            // AUTO / PREPARE
            if (timeline.autoMode == true){
                GetComponent<CheckStep>().performStep();
            }
            beatcount += (1 * beatmultiplier);
            // ^ makes sure the beat is 1x when required (used to switch to offbeat)
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
            
            timeline.switchStep(false);
            timeline.checkTimeline();
        }
    }
}