﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{

    //This class should hold all the actions that happen at certain times, and perform them in time.

    List<actionElement> actionList;
    Ticko updater;
    Conductor conductor;

    // Use this for initialization
    void Start()
    {
        actionList = new List<actionElement>();
        updater = GetComponent<Ticko>();
        conductor = GetComponent<Conductor>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addAction(byte actionType, float position)
    {
        actionList.Add(new actionElement(actionType, position));
        Debug.Log("An event has been added at beat number: " + position.ToString() + "." + "(" + actionType.ToString() + ")");
    }

    public void checkTimeline()
    {
        foreach (actionElement item in actionList)
        {
            if (item.position == updater.beatcount)
            {
                performAction(item.action);
            }
        }
    }

    void performAction(byte actionType)
    {
        Debug.Log("Performing action: " + actionType.ToString());
        if (actionType == 0)
        {
            //Prepare
        }
        else if (actionType == 1)
        {
            //Stop prepare
        }
        else if (actionType == 2)
        {
            //Switch beat
            switchStep(true);
        }
        else if (actionType == 3)
        {
            //Swing tail
        }
        else if (actionType == 4)
        {
            //End
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
			updater.pastbeat -= (updater.beatdur * updater.beatmultiplier);
			updater.switchingStep = true;
        }
    }
    #endregion

}

struct actionElement
{
    public float position;
    public byte action;

    public actionElement(byte act, float pos)
    {
        position = pos;
        action = act;
    }
}
