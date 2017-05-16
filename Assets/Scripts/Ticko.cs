using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ticko : MonoBehaviour
{
    public float pastbeat = 0f;
    public float offset = 0;
    public float beatcount = 0;
    public float halfbeatdur;
    public float beatdur;
    public Text OSNumber;
    bool IsOffbeat = false;
    bool StepOnOffbeats = false;
    GameObject[] BackgroundSwitchers;
    int onbeats;


    // Use this for initialization
    void Start()
    {
        halfbeatdur = (60 / GetComponent<Conductor>().bpm) / 2;
        beatdur = (60 / GetComponent<Conductor>().bpm);
        BackgroundSwitchers = GameObject.FindGameObjectsWithTag("Background");
    }
	
    // Update is called once per frame
    void Update()
    {

        if (GetComponent<Conductor>().songposition + offset <= 0.1)
        {
            pastbeat = 0;  
        }

        if (GetComponent<Conductor>().songposition + offset > pastbeat + beatdur)
        {
            pastbeat += (beatdur);
            beatcount += 1f;
//            GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatStep();
        }

        if (Input.GetKeyDown("a"))
        {
            StepOnOffbeats = !StepOnOffbeats;
        }

        if (GetComponent<Conductor>().songposition + offset > pastbeat + halfbeatdur)
        {
            beatcount += 0.5f;
            pastbeat += (halfbeatdur);
            IsOffbeat = !IsOffbeat;
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
        OSNumber.text = beatcount.ToString() + "\n" + (GetComponent<Conductor>().songposition + offset).ToString() + "\n" + onbeats.ToString();
    }
}
