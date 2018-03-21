using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{

    //This class should hold all the actions that happen at certain times, and perform them in time.

    List<actionElement> actionList;
    List<actionElement> surfaceActionList;
    Ticko updater;

    public double pastbeat;
    public double offset = 0;
    public double beatcount = -1f;
    public double beatmultiplier;
    public double beatdur;
    public bool StepOnOffbeats = false;
    GameObject[] BackgroundSwitchers;
    Conductor conductor;
    CheckStep checkstepM;
    public bool isPreparing;
    public bool stayStill;
    public bool autoMode = true;
    double pastRevolution;
    public float snap;
    public bool switchingStep;

    // Use this for initialization
    void Start()
    {
        actionList = new List<actionElement>();
        surfaceActionList = new List<actionElement>();
        updater = GetComponent<Ticko>();
        //brought from Ticko
        //Specify beatduration and background stepswitchers
        beatmultiplier = 1f;
        beatdur = (60 / GetComponent<Conductor>().bpm);
        //for every background stepswitcher
        BackgroundSwitchers = GameObject.FindGameObjectsWithTag("Background");
        checkstepM = GetComponent<CheckStep>();
        StartCoroutine(addSampleBeatmap(3));

        conductor = GetComponent<Conductor>();
        checkstepM = GetComponent<CheckStep>();
        snap = 0.125f;
    }

    void Update()
    {
        updateTimelinePosition();
        // every beat (With the multiplier in action, probably gonna use this for swing beats)
        if (GetComponent<Conductor>().songposition + offset > pastbeat + (beatdur * beatmultiplier))
        {
            /* CPU LOGIC START
            */
            if (StepOnOffbeats == true && stayStill == false)
            {
                //if offbeat
                foreach (GameObject switcher in BackgroundSwitchers)
                {
                    switcher.GetComponent<Step>().OffBeatStep();
                }

            }
            else if (StepOnOffbeats == false && stayStill == false)
            {
                //if not offbeat
                //if Preparing is enabled
                if (isPreparing)
                {
                    foreach (GameObject switcher in BackgroundSwitchers)
                    {
                        switcher.GetComponent<Step>().PrepareStep();
                    }
                }
                //if Preparing is disabled
                else if (!(isPreparing))
                {
                    foreach (GameObject switcher in BackgroundSwitchers)
                    {
                        switcher.GetComponent<Step>().OnBeatStep();
                    }
                }
                // AUTO / PREPARE
                if (autoMode == true || isPreparing == true)
                {
                    checkstepM.performStep();
                }
            }
            switchStep(false);
            // ^ makes sure the beat is 1x when required (used to switch to offbeat)
            checkstepM.CheckMiss(0.80f);
        }
        if (Input.GetKeyDown("a"))
        {
            //pause and unpause
            AudioListener.pause = !AudioListener.pause;
        }

        if (Input.GetKeyDown("d"))
        {
            //toggle Auto
            autoMode = !autoMode;
        }
    }


    public void addAction(byte actionType, float position, string argument1 = "")
    {
        if (argument1 == "")
        {
            actionList.Add(new actionElement(actionType, position, argument1));
            Debug.Log("An event has been added at beat number: " + position.ToString() + "." + "(" + actionType.ToString() + ")");
        }
        else if (argument1 != "")
        {
            surfaceActionList.Add(new actionElement(actionType, position, argument1));
            Debug.Log("An event has been added at beat number: " + position.ToString() + "." + "(" + actionType.ToString() + ") [SURFACE EVENT]");
        }

    }

    public void updateTimelinePosition()
    {
        if (GetComponent<Conductor>().songposition + offset > pastRevolution + (beatdur * snap))
        {
            checkTimeline();
            beatcount += snap;
            pastRevolution += (beatdur * snap);
        }
    }

    public void checkTimeline()
    {
        foreach (actionElement item in actionList)
        {
            if (item.position == (beatcount + 1))
            {
                StartCoroutine(performAction(item.action, item.arg1));
            }
        }
        foreach (actionElement item in surfaceActionList)
        {
            if (item.position == (beatcount + 1))
            {
                StartCoroutine(performAction(item.action, item.arg1));
            }
        }
    }



    IEnumerator performAction(byte actionType, string argument1 = "")
    {
        Debug.Log("Performing action: " + actionType.ToString());
        switch (actionType)
        {
            case 0:
                {
                    //Prepare
                    togglePrepare(int.Parse(argument1), true, false);
                    break;
                }
            case 1:
                {
                    //Stop prepare
                    togglePrepare(1, false, false);
                    break;
                }
            case 2:
                {
                    //Switch beat
                    switchStep(true);
                    break;
                }
            case 3:
                {
                    //Swing tail
                    break;
                }
            case 4:
                {
                    //Stay Still (Preparing) (Also affects CPU)
                    togglePrepare(int.Parse(argument1), true, true);
                    break;
                }
            case 5:
                {
                    //Move Again (Preparing) (Also affects CPU)
                    togglePrepare(int.Parse(argument1), true, false);
                    break;
                }
            case 6:
                {
                    //Toggle Auto
                    autoMode = !autoMode;
                    break;
                }
            case 7:
                {
                    //End game
                    break;
                }
            case 8:
                {
                    //playSound
                    playSound(argument1);
                    break;
                }
            case 9:
                {
                    //Change snap
                    snap = int.Parse(argument1);
                    break;
                }
            case 12:
                {
                    //Restart beatmap
                    StartCoroutine(conductor.resetBeatmap());
                    break;
                }
        }
        yield return null;
    }

    //ACTIONS START
    #region actions
    public void switchStep(bool halveBeat)
    {
        if (halveBeat == false)
        {
            //bring back the multiplier to one if we're transitioning, offbeat transition stopped
            if (switchingStep == true)
            {
                StepOnOffbeats = !StepOnOffbeats;
                beatmultiplier = 1;
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
            pastbeat += (beatdur - (beatdur / 2));
            beatmultiplier = 0.5f;
        }
    }

    public void togglePrepare(float multiplier, bool active, bool doNotMove)
    {
        if (active == true)
        {
            beatmultiplier = multiplier;
            isPreparing = true;
            if (doNotMove)
            {
                stayStill = true;
            }
            else
            {
                stayStill = false;
            }
        }
        else if (active == false)
        {
            beatmultiplier = 1;
            stayStill = false;
            isPreparing = false;
        }
    }
    public void playSound(string soundFile)
    {
        AudioClip soundtoplay;
        soundtoplay = Resources.Load<AudioClip>("Sounds/SFX/" + soundFile);
        GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().PlayOneShot(soundtoplay);
    }
    #endregion

    IEnumerator addSampleBeatmap(byte sample = 1)
    {
        yield return new WaitForEndOfFrame();
        switch (sample)
        {
            case 1:
                {
                    //sample beatmap
                    addAction(5, 1f, "1");
                    addAction(1, 4, "1");
                    addAction(1, 6, "1");
                    addAction(2, 7.5f);
                    addAction(8, 13f, "cowbell");
                    addAction(8, 14f, "cowbell");
                    addAction(8, 15f, "cowbell");
                    addAction(2, 15.5f);
                    addAction(8, 21, "cowbell");
                    addAction(8, 22, "cowbell");
                    addAction(8, 23, "cowbell");
                    addAction(2, 22.5f);
                    addAction(8, 29.5f, "cowbell");
                    addAction(8, 30.5f, "cowbell");
                    addAction(8, 31.5f, "cowbell");
                    addAction(2, 30.5f);
                    addAction(8, 31.5f, "cowbell");
                    break;
                }
            case 2:
                {
                    addAction(12, 113f, "ahaha");
                    break;
                }
            case 3:
                {
                    addAction(4, 0f, "1");
                    addAction(5, 4f, "2");
                    addAction(5, 12f, "1");
                    addAction(1, 15f, "1");
                    break;
                }
        }
    }

}

struct actionElement
{
    public float position;
    public byte action;
    public string arg1;

    public actionElement(byte act, float pos, string a1 = "")
    {
        position = pos;
        action = act;
        arg1 = a1;
    }
}
