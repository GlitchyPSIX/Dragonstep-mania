using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckStep : MonoBehaviour
{
    public float beatMultiplier;
    public float beatdur;
    public Text OSText;

    // Use this for initialization
    void Start()
    {
        beatdur = GetComponent<Ticko>().beatdur * GetComponent<Ticko>().beatmultiplier;
        beatMultiplier = GetComponent<Ticko>().beatmultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        beatdur = GetComponent<Ticko>().beatdur * GetComponent<Ticko>().beatmultiplier;
        if (Input.GetButtonDown("Step"))
        {
            if (
                    (
                        (GetComponent<Conductor>().songposition < (GetComponent<Ticko>().pastbeat + (0.25f * beatdur))
                        )
                            ||
                        (GetComponent<Conductor>().songposition > (GetComponent<Ticko>().pastbeat + (0.75f * beatdur))
                        )
                    )
                )
            {
                if (!(GetComponent<Ticko>().StepOnOffbeats))
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatStep();
                }
                else if ((GetComponent<Ticko>().StepOnOffbeats))
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatStep();
                }
                else if ((GetComponent<Ticko>().IsOffbeat) && !(GetComponent<Ticko>().StepOnOffbeats))
                {
                    // Nothing
                }
            }
            else
            {
                GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatMiss();
            }
            // CanStep = true;
        }
        else
        {

        }
        // OSText.text = (calculation1 - calculation2).ToString();
    }
}
