using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ticko : MonoBehaviour
{
    public double pastbeat = 0d;
    public float offset = 0;
    public float beatcount = 1f;
    public float beatmultiplier;
    public float beatdur;
    public bool StepOnOffbeats = false;
    GameObject[] BackgroundSwitchers;
    Timeline timeline;
    CheckStep checkstepM;

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
        checkstepM = GetComponent<CheckStep>();
        StartCoroutine(addSampleBeatmap(0));
    }

    // Update is called once per frame
    void Update()
    {
        timeline.updateTimelinePosition();
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
        if (Input.GetKeyDown("a"))
        {
            //pause and unpause
            AudioListener.pause = !AudioListener.pause;
        }

        if (Input.GetKeyDown("d"))
        {
            //toggle Auto
            timeline.autoMode = !timeline.autoMode;
        }
    }

    IEnumerator addSampleBeatmap(byte sample = 0)
    {
        yield return new WaitForEndOfFrame();
        if (sample == 0)
        {
            //sample beatmap
            timeline.addAction(5, 0.5f, "1");
            timeline.addAction(1, 4, "1");
            timeline.addAction(1, 6, "1");
            timeline.addAction(2, 8f);
            timeline.addAction(8, 13f, "cowbell");
            timeline.addAction(8, 14f, "cowbell");
            timeline.addAction(8, 15f, "cowbell");
            timeline.addAction(2, 14.5f);
            timeline.addAction(8, 21, "cowbell");
            timeline.addAction(8, 22, "cowbell");
            timeline.addAction(8, 23, "cowbell");
            timeline.addAction(2, 22.5f);
            timeline.addAction(8, 29.5f, "cowbell");
            timeline.addAction(8, 30.5f, "cowbell");
            timeline.addAction(8, 31.5f, "cowbell");
            timeline.addAction(2, 30.5f);
            timeline.addAction(8, 31.5f, "cowbell");
        }
        else if (sample == 1){
            timeline.addAction(0, 1, "1");
            timeline.addAction(4, 0, "1");
            timeline.addAction(0, 12, "3");
            timeline.addAction(0, 24, "1");
            timeline.addAction(2, 35);
            timeline.addAction(2, 36);
            timeline.addAction(1, 29);
            timeline.addAction(2, 54);
            timeline.addAction(2, 68);
            timeline.addAction(2, 77);
            timeline.addAction(2, 78);
        }
    }


}