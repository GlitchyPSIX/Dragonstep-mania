using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckStep : MonoBehaviour {

    public float pastbeat; 
    public float offset;
    public float halfbeatdur;
    public float beatdur;
    public bool IsOffbeat;
    public bool StepOnOffbeats;
	public Text OSText;
    bool CanStep = false;
	float calculation1;
	float calculation2;

	// Use this for initialization
	void Start () {
pastbeat = GetComponent<Ticko>().pastbeat;
offset = GetComponent<Ticko>().offset;
beatdur = GetComponent<Ticko>().beatdur;
halfbeatdur = GetComponent<Ticko>().halfbeatdur;
IsOffbeat = GetComponent<Ticko>().IsOffbeat;
StepOnOffbeats = GetComponent<Ticko>().StepOnOffbeats;
	}
	
	// Update is called once per frame
	void Update () {
		pastbeat = GetComponent<Ticko>().pastbeat;
offset = GetComponent<Ticko>().offset;
beatdur = GetComponent<Ticko>().beatdur;
halfbeatdur = GetComponent<Ticko>().halfbeatdur;
IsOffbeat = GetComponent<Ticko>().IsOffbeat;
StepOnOffbeats = GetComponent<Ticko>().StepOnOffbeats;

		//WIP - add time window for the player to be able to step
		calculation1 = (GetComponent<Conductor>().songposition + offset - halfbeatdur);
		calculation2 = (pastbeat + offset - (halfbeatdur * 0.3f));
        if ( calculation1 < calculation2 )
        {
            if (Input.GetButtonDown("Step"))
            {

                GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatStep();

            }
            // CanStep = true;

        }
        else
        {
            // CanStep = false;
        }
		OSText.text = (calculation1 - calculation2).ToString();
	}
}
