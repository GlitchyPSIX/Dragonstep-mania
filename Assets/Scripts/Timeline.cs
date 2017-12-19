﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{

    //This class should hold all the actions that happen at certain times, and perform them in time.

    List<actionElement> actionList;
    List<actionElement> surfaceActionList;
    Ticko updater;
    CheckStep stepcheck;
    public bool isPreparing;
    public bool stayStill;
    public bool autoMode = true;
    double pastHalfbeat;

    // Use this for initialization
    void Start()
    {
        actionList = new List<actionElement>();
        surfaceActionList = new List<actionElement>();
        updater = GetComponent<Ticko>();
        stepcheck = GetComponent<CheckStep>();
        pastHalfbeat = updater.pastbeat;
    }

    // Update is called once per frame
    void Update()
    {

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
            if (item.position == (updater.beatcount + (1 * updater.beatmultiplier)))
            {
                performAction(item.action, item.arg1);
            }
        }
        foreach (actionElement item in surfaceActionList)
        {
            if (item.position == (updater.beatcount + (1 * updater.beatmultiplier)))
            {
                performAction(item.action, item.arg1);
            }
        }
    }

    public void updateTimelinePosition(){
        if (GetComponent<Conductor>().songposition + updater.offset > pastHalfbeat + (updater.beatdur * 0.5f))
        {
            updater.beatcount += 0.5f;
            pastHalfbeat += (updater.beatdur * 0.5f);
        }
    }

    void performAction(byte actionType, string argument1 = "")
    {
        Debug.Log("Performing action: " + actionType.ToString());
        if (actionType == 8)
        {
            //playSound
            playSound(argument1);
        }
        if (actionType == 0)
        {
            //Prepare
            togglePrepare(int.Parse(argument1), true, false);
        }
        if (actionType == 1)
        {
            //Stop prepare
            togglePrepare(1, false, false);
        }
        if (actionType == 2)
        {
            //Switch beat
            switchStep(true);
        }
        if (actionType == 3)
        {
            //Swing tail
        }
        if (actionType == 4)
        {
            //Stay Still (Preparing) (Also affects CPU)
            togglePrepare(int.Parse(argument1), true, true);
        }
        if (actionType == 5)
        {
            //Move Again (Preparing) (Also affects CPU)
            togglePrepare(int.Parse(argument1), true, false);
        }
        if (actionType == 6)
        {
            //Toggle Auto
            autoMode = !autoMode;
        }
        if (actionType == 7)
        {
            //End game
        }
    }

    //ACTIONS START
    #region actions
    public void switchStep(bool halveBeat)
    {
        if (halveBeat == false)
        {
            //bring back the multiplier to one if we're transitioning, offbeat transition stopped
            if (updater.switchingStep == true)
            {
                updater.StepOnOffbeats = !updater.StepOnOffbeats;
                updater.beatmultiplier = 1;
                updater.switchingStep = false;
            }
            else //if it's already off-transition
            {
                updater.pastbeat += (updater.beatdur * updater.beatmultiplier);
            }
        }
        else if (halveBeat == true)
        {
            //halve the multiplier for a "beat", reduce the last beat by half a beat, so it goes into the backbeat
            updater.beatmultiplier = 0.5f;
            updater.switchingStep = true;
            updater.pastbeat += (updater.beatdur * updater.beatmultiplier);
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
        }
        else if (active == false)
        {
            updater.beatmultiplier = 1;
            isPreparing = false;
            stayStill = false;
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
