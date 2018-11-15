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


    public double nextbeat;
    public double earlyLimit;
    public double lateLimit;
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
        StartCoroutine(addSampleBeatmap(1));

        conductor = GetComponent<Conductor>();
        checkstepM = GetComponent<CheckStep>();
        snap = 0.125f;
        nextbeat += beatdur * beatmultiplier;
    }

    void Update()
    {
        //Make sure the limits are within the beat margins, so to avoid halfbeat discrepancy thinking the switch has no miss
        earlyLimit = nextbeat - (beatdur * beatmultiplier * 0.25d);
        lateLimit = nextbeat + (beatdur * beatmultiplier * 0.25d);

        //Check for miss every frame (value) and update the Timeline's position
        updateTimelinePosition();
        checkstepM.CheckMiss();
        /* TODO START -------------------
         * 
         *  Y A T T A ! ! !
         *  Now I just need to clean up
         *  
         *  TODO END -------------------
         */

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

        if (Input.GetKeyDown("q") && AudioListener.pause == true)
        {
            //start song
            StartCoroutine(conductor.resetBeatmap());
            AudioListener.pause = false;
        }
    }

    public void updateNextBeat(string reason = "")
    {
        //THE SOLUTION TO MY DILEMMA
        //Why didn't I think of this before
        nextbeat += beatdur * beatmultiplier;
        //Debug: to check why did the next beat get updated
        Debug.Log("Updated nextbeat to: " + nextbeat + " (" + reason + ")");
    }


    public void addAction(actionElement.actions actionType, float position, string argument1 = "")
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
                List<string> itemargs = new List<string>();
                foreach(string argument in item.args)
                {
                    itemargs.Add(argument);
                }
                itemargs.Add((beatcount + 1).ToString());
                StartCoroutine(performAction(item.action, itemargs.ToArray()));
            }
        }
        foreach (actionElement item in surfaceActionList)
        {
            if (item.position == (beatcount + 1))
            {
                StartCoroutine(performAction(item.action, item.args));
            }
        }
    }

    IEnumerator performAction(actionElement.actions actionType, string[] args = null)
    {
        Debug.Log("Performing action: " + actionType.ToString());
        switch (actionType)
        {
            case actionElement.actions.Preparation:
                {
                    //Prepare
                    togglePrepare(int.Parse(args[0]), true, false);
                    break;
                }
            case actionElement.actions.Active:
                {
                    //Stop prepare
                    togglePrepare(1, false, false);
                    break;
                }
            case actionElement.actions.BeatSwitch:
                {
                    //Switch beat
                    switchStep(true);
                    break;
                }
            case actionElement.actions.Tailswing:
                {
                    //Swing tail
                    break;
                }
            case actionElement.actions.CPUStill:
                {
                    //Stay Still (Preparing) (Also affects CPU)
                    togglePrepare(int.Parse(args[0]), true, true);
                    break;
                }
            case actionElement.actions.CPUActive:
                {
                    //Move Again (Preparing) (Also affects CPU)
                    togglePrepare(int.Parse(args[0]), true, false);
                    break;
                }
            case actionElement.actions.ToggleAuto:
                {
                    //Toggle Auto
                    autoMode = !autoMode;
                    break;
                }
            case actionElement.actions.EndGame:
                {
                    //End game
                    break;
                }
            case actionElement.actions.PlaySound:
                {
                    //playSound
                    playSound(args[0]);
                    break;
                }
            case actionElement.actions.SnapAdjust:
                {
                    //Change snap
                    snap = int.Parse(args[0]);
                    break;
                }
            case actionElement.actions.Reset:
                {
                    //Restart beatmap
                    StartCoroutine(conductor.resetBeatmap());
                    break;
                }
            case actionElement.actions.Pause:
                {
                    //Pause
                    conductor.resetStartOfSong(false);
                    break;
                }
            case actionElement.actions.PauseAndReset:
                {
                    //Reset, pause
                    conductor.resetStartOfSong(true);
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
                    addAction(actionElement.actions.PlaySound, 4, "cowbell");
                    break;
                }
        }
    }

}

public struct actionElement
{
    public float position;
    public enum actions {
        Preparation,
        Still,
        Active,
        CPUStill,
        CPUActive,
        ToggleAuto,
        BeatSwitch,
        Tailswing,
        PlaySound,
        SnapAdjust,
        Pause,
        PauseAndStop,
        PauseAndReset,
        EndGame,
        Reset
    }
    public actions action;
    public string[] args;
    //TODO: Use an enum here
    public actionElement(actions act, float pos, string argm = null)
    {
        position = pos;
        action = act;
        if (argm != null)
        {
            args = argm.Split(' ');
        }
        else
        {
            args = new string[0];
        }
    }
}
