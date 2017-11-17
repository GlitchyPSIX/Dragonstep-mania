using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckStep : MonoBehaviour
{
    public float beatMultiplier;
    public float beatdur;
    public float pastHitPos;
    public Text OSText;
    public bool isHurt = false;
    public byte hurtOrientation = 0;

    /*
    HURTING ORIENTATION GUIDE:
    It got hurt onbeat if it's 0.
    It got hurt at backbeat if it's 1.
    */
    int hitCounter = 0;
    int missCounter = 0;

    float timeDifference;

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
        timeDifference = pastHitPos - GetComponent<Ticko>().pastbeat;

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
                if (!(GetComponent<Ticko>().StepOnOffbeats)) //If onbeat
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatStep();
                    isHurt = false;
                    pastHitPos = GetComponent<Conductor>().songposition;
                }
                else if ((GetComponent<Ticko>().StepOnOffbeats)) //If at backbeat
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatStep();
                    isHurt = false;
                    pastHitPos = GetComponent<Conductor>().songposition;
                }
            }
            else
            {
                pastHitPos = GetComponent<Conductor>().songposition;
            }
            // CanStep = true;
        }
        CheckMiss();
        OSText.text = "Beat count:" + GetComponent<Ticko>().beatcount.ToString() + "\n" + "Song offset: " + (GetComponent<Conductor>().songposition + GetComponent<Ticko>().offset).ToString() + "\nLast Hit: " + pastHitPos.ToString() + "\n" + "Last beat: " + (GetComponent<Ticko>().pastbeat - (0.3 * (beatdur * GetComponent<Ticko>().beatmultiplier))).ToString() + "\n Is hurt?: " + GetComponent<CheckStep>().isHurt.ToString() + "\n Hurt Orientation: " + GetComponent<CheckStep>().hurtOrientation.ToString();

    }
    void CheckMiss()
    {
        if (isHurt)
        {
            if (GetComponent<Conductor>().songposition >= (pastHitPos + beatdur))
            {
                pastHitPos += beatdur;
                missCounter++;
                if (hurtOrientation == 1 && !(GetComponent<Ticko>().StepOnOffbeats))
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatMiss();
                    hurtOrientation = 0;
                }
                else if (hurtOrientation == 0 && (GetComponent<Ticko>().StepOnOffbeats))
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatMiss();
                    hurtOrientation = 1;
                }

            }
        }
        else if (!isHurt)
        {
            if (GetComponent<Conductor>().songposition >= (pastHitPos + (1.05f * beatdur)))
            {
                isHurt = true;
                missCounter++;
                pastHitPos = GetComponent<Ticko>().pastbeat;
                if (!(GetComponent<Ticko>().StepOnOffbeats))
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OnBeatMiss();
                    hurtOrientation = 0;

                }
                else if ((GetComponent<Ticko>().StepOnOffbeats))
                {
                    GameObject.FindWithTag("Player").GetComponent<Step>().OffBeatMiss();
                    hurtOrientation = 1;
                }
            }
        }
    }

}
