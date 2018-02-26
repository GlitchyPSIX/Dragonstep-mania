using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{

    //This class should hold all the actions that happen at certain times, and perform them in time.

    List<actionElement> actionList;
    List<actionElement> surfaceActionList;
    Ticko updater;
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
        checkstepM = GetComponent<CheckStep>();
        snap = 0.125f;
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

    public void checkTimeline()
    {
        foreach (actionElement item in actionList)
        {
            if (item.position == (updater.beatcount))
            {
                performAction(item.action, item.arg1);
            }
        }
        foreach (actionElement item in surfaceActionList)
        {
            if (item.position == (updater.beatcount))
            {
                performAction(item.action, item.arg1);
            }
        }
    }

    public void updateTimelinePosition()
    {
        if (GetComponent<Conductor>().songposition + updater.offset > pastRevolution + (updater.beatdur * snap))
        {
            checkTimeline();
            updater.beatcount += snap;
            pastRevolution += (updater.beatdur * snap);
        }

    }

    void performAction(byte actionType, string argument1 = "")
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
        }
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
                    updater.StepOnOffbeats = !updater.StepOnOffbeats;
                    updater.beatmultiplier = 1;
                    switchingStep = false;
            }
                else //if it's already off-transition
                {
                updater.pastbeat += (updater.beatdur * updater.beatmultiplier);
                checkstepM.CheckMiss(1.15f);
            }
            }
            else if (halveBeat == true)
            {
            //halve the multiplier for a "beat", reduce the last beat by half a beat, so it goes into the backbeat
            updater.pastbeat += (updater.beatdur * updater.beatmultiplier);
            switchingStep = true;
            updater.beatmultiplier = 0.5f;
            updater.pastbeat -= (updater.beatdur * updater.beatmultiplier);
        }
    }

        public void togglePrepare(float multiplier, bool active, bool doNotMove)
        {
            if (active == true)
            {
                updater.beatmultiplier = multiplier;
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
                updater.beatmultiplier = 1;
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
