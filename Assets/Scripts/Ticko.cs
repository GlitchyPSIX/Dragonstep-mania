using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ticko : MonoBehaviour
{
    public bool switchingStep;
    public double pastbeat = 0d;
    public float offset = 0;
    public float beatcount = -1.0f;
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
        StartCoroutine(addSampleBeatmap(1));
    }

    // Update is called once per frame
    void Update()
    {
        // every beat (With the multiplier in action, probably gonna use this for swing beats)
        if (GetComponent<Conductor>().songposition + offset > pastbeat + (beatdur * beatmultiplier))
        {
            // AUTO / PREPARE
            if (timeline.autoMode == true || timeline.isPreparing == true)
            {
                GetComponent<CheckStep>().performStep();
            }
            /* CPU LOGIC START
            */
            if (StepOnOffbeats == true && timeline.stayStill == false)
            {
                //if offbeat
                foreach (GameObject switcher in BackgroundSwitchers)
                {
                    switcher.GetComponent<Step>().OffBeatStep();
                }

            }
            else if (StepOnOffbeats == false && timeline.stayStill == false)
            {
                //if not offbeat
                //if Preparing is enabled
                if (timeline.isPreparing)
                {
                    foreach (GameObject switcher in BackgroundSwitchers)
                    {
                        switcher.GetComponent<Step>().PrepareStep();
                    }
                }
                //if Preparing is disabled
                else if (!(timeline.isPreparing))
                {
                    foreach (GameObject switcher in BackgroundSwitchers)
                    {
                        switcher.GetComponent<Step>().OnBeatStep();
                    }
                }

            }
            timeline.switchStep(false);
            // ^ makes sure the beat is 1x when required (used to switch to offbeat)
        }
        timeline.updateTimelinePosition();
        if (Input.GetKeyDown("a"))
        {
            //emergency switch
            timeline.switchStep(true);
        }

        if (Input.GetKeyDown("d"))
        {
            timeline.autoMode = !timeline.autoMode;
        }
    }

    IEnumerator addSampleBeatmap(byte sample = 0)
    {
        yield return new WaitForEndOfFrame();
        if (sample == 0)
        {
            timeline.addAction(4, 1, "2");
            timeline.addAction(5, 3, "1");
            timeline.addAction(1, 6, "1");
            timeline.addAction(2, 7);
            timeline.addAction(8, 7, "cowbell");
            timeline.addAction(8, 12.5f, "cowbell");
            timeline.addAction(8, 13.5f, "cowbell");
            timeline.addAction(8, 14.5f, "cowbell");
            timeline.addAction(2, 15.5f);
            timeline.addAction(8, 15.5f, "cowbell");
            timeline.addAction(8, 20, "cowbell");
            timeline.addAction(8, 21, "cowbell");
            timeline.addAction(8, 22, "cowbell");
            timeline.addAction(2, 23);
            timeline.addAction(8, 23, "cowbell");
            timeline.addAction(8, 28.5f, "cowbell");
            timeline.addAction(8, 29.5f, "cowbell");
            timeline.addAction(8, 30.5f, "cowbell");
            timeline.addAction(2, 31.5f);
            timeline.addAction(8, 31.5f, "cowbell");
        }
        else if (sample == 1){
            timeline.addAction(0, 1, "1");
            timeline.addAction(4, 0, "1");
            timeline.addAction(0, 12, "3");
            timeline.addAction(0, 24, "1");
            timeline.addAction(1, 29);
        }
    }


}