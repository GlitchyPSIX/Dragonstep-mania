using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ticko : MonoBehaviour
{
    public float pastbeat = 0f;
    public float lastonbeat = 0f;
    public float lastoffbeat = 0f;
    public float offset = 0;
    public float beatcount = 0;
    public float halfbeatdur;
    public float beatdur;
    public Text OSNumber;
    public bool IsOffbeat = false;
    public bool StepOnOffbeats = false;
    GameObject[] BackgroundSwitchers;
    int onbeats;


    // Use this for initialization
    void Start()
    {
        //Specify beatduration and half-beat duration (why even?) 
        halfbeatdur = (60 / GetComponent<Conductor>().bpm) / 2;
        beatdur = (60 / GetComponent<Conductor>().bpm);
        //for every background stepswitcher
        BackgroundSwitchers = GameObject.FindGameObjectsWithTag("Background");
    }

    // Update is called once per frame
    void Update()
    {
        // debug - press A to make the BGswitchers switch to offbeat
        if (Input.GetKeyDown("a"))
        {
            StepOnOffbeats = !StepOnOffbeats;
        }
        // every halfbeat...
        if (GetComponent<Conductor>().songposition + offset > pastbeat + halfbeatdur)
        {
            beatcount += 0.5f;
            pastbeat += (halfbeatdur);
            IsOffbeat = !IsOffbeat;

            //attempt to record the last onbeat hit
            if (IsOffbeat == true)
            {
                lastonbeat += beatdur;
            }

            //same for the last offbeat
            else if (IsOffbeat == false)
            {
                lastoffbeat += beatdur;
            }
            //make bg stepswitchers step on beats or offbeats depending on if they should or not
            // (aka CPU logic)
            if (IsOffbeat == true && StepOnOffbeats == true)
            {
                foreach (GameObject switcher in BackgroundSwitchers)
                {
                    switcher.GetComponent<Step>().OffBeatStep();
                }

            }
            else if (IsOffbeat == false && StepOnOffbeats == false)
            {
                foreach (GameObject switcher in BackgroundSwitchers)
                {
                    switcher.GetComponent<Step>().OnBeatStep();
                }

            }

        }

        //debug - show info I need
        OSNumber.text = beatcount.ToString() + "\n" + (GetComponent<Conductor>().songposition + offset).ToString() + "\n" + onbeats.ToString() + "\n" + (GetComponent<Conductor>().songposition + offset).ToString() + "\n" + (pastbeat - (0.3 * halfbeatdur)).ToString();
    }
}