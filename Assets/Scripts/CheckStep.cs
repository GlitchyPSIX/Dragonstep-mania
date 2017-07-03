using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckStep : MonoBehaviour
{
    public float halfbeatdur;
    public float beatdur;
    public Text OSText;

    // Use this for initialization
    void Start()
    {
        beatdur = GetComponent<Ticko>().beatdur;
        halfbeatdur = GetComponent<Ticko>().halfbeatdur;
    }

    public bool isOnTime(float margin, float beattime, float position = 999)
    {

        if (position == 999)
        {
            position = GetComponent<Conductor>().songposition;
        }

        return Mathf.Abs(position - beattime) < margin;

    }

    // Update is called once per frame
    void Update()
    {
        //WIP - add time window for the player to be able to step
        // Ignore for now, trying to fully understand what fizzd-sensei recommended me
        //IT SEEMS THAT I WAS AN IDIOT AND I UH
        //WHY DID I EVEN RE-REFERENCE THE VALUES FROM TICKO WTF
        // I'm sure that contributes to calculation lag
        if (Input.GetButtonDown("Step"))
        {
            if (
                    (
                        (GetComponent<Conductor>().songposition < (GetComponent<Ticko>().lastonbeat + (0.4f * beatdur))
                        )
                            ||
                        (GetComponent<Conductor>().songposition > (GetComponent<Ticko>().lastonbeat + (0.75f * beatdur))
                        )
                    )
                )
            {
                if (!(GetComponent<Ticko>().IsOffbeat) && !(GetComponent<Ticko>().StepOnOffbeats))
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatStep();
                }
                else if ((GetComponent<Ticko>().IsOffbeat) && (GetComponent<Ticko>().StepOnOffbeats))
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatStep();
                }
                else if ((GetComponent<Ticko>().IsOffbeat) && !(GetComponent<Ticko>().StepOnOffbeats))
                {
                    //No hacer nada (Do nothing)
                }
            }
            // CanStep = true;
        }
        else
        {
            // CanStep = false;
        }
        // OSText.text = (calculation1 - calculation2).ToString();
    }
}
