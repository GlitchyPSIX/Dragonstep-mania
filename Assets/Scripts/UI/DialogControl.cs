using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using DSMUI.Assets;


// Deprecated class. To be deleted soon.


public class DialogControl : MonoBehaviour
{
    Animator animataC;
    Text title;
    Text body;
    Text buttonLText;
    Text buttonMText;
    Text buttonRText;
    GameObject buttonL;
    GameObject buttonM;
    GameObject buttonR;
    AudioSource dialogSFX;
    AudioSource buttonLSFX;
    AudioSource buttonMSFX;
    AudioSource buttonRSFX;


    public void setDialog(string titleS, string textS, byte buttons, UnityAction scriptleft, UnityAction scriptmiddle,
                          UnityAction scriptright, AudioClip dialogASFX, AudioClip buttonLASFX, AudioClip buttonMASFX, AudioClip buttonRASFX,
                          string buttonLS = "", string buttonRS = "", string buttonMS = "")
    {
        //declare every variable at the same time we tell the dialog to start
        animataC = GetComponent<Animator>();
        buttonL = transform.Find("DialogBG").transform.Find("ButtonL").gameObject;
        buttonM = transform.Find("DialogBG").transform.Find("ButtonM").gameObject;
        buttonR = transform.Find("DialogBG").transform.Find("ButtonR").gameObject;
        title = transform.Find("DialogBG").transform.Find("DialogTitle").GetComponent<Text>();
        body = transform.Find("DialogBG").transform.Find("DialogText").GetComponent<Text>();
        buttonLText = buttonR.transform.Find("Text").GetComponent<Text>();
        buttonMText = buttonM.transform.Find("Text").GetComponent<Text>();
        buttonRText = buttonR.transform.Find("Text").GetComponent<Text>();
        buttonLSFX = buttonL.transform.Find("SFX").GetComponent<AudioSource>();
        buttonMSFX = buttonM.transform.Find("SFX").GetComponent<AudioSource>();
        buttonRSFX = buttonR.transform.Find("SFX").GetComponent<AudioSource>();
        dialogSFX = transform.Find("DialogBG").GetComponent<AudioSource>();
        
        //set the sound effects
        buttonLSFX.clip = buttonLASFX;
        buttonMSFX.clip = buttonMASFX;
        buttonRSFX.clip = buttonRASFX;
        dialogSFX.clip = dialogASFX;

        //set the dialog data
        title.text = titleS;
        body.text = textS;
        buttonLText.text = buttonLS;
        buttonMText.text = buttonMS;
        buttonRText.text = buttonRS;

        //set button layout
        // __ __ ##
        if (buttons == 0)
        {
            buttonL.SetActive(false);
            buttonM.SetActive(false);
            buttonR.SetActive(true);
        }
        // __ ## ##
        else if (buttons == 1)
        {
            buttonL.SetActive(false);
            buttonM.SetActive(true);
            buttonR.SetActive(true);
        }
        // ## ## ##
        else if (buttons == 2)
        {
            buttonL.SetActive(true);
            buttonM.SetActive(true);
            buttonR.SetActive(true);
        }
        // if invalid: __ __ __
        else
        {
            Debug.Log("Invalid button set attempted. The dialog with the title \"" + titleS + "\" will show no buttons and will be rendered unusable.");
            buttonL.SetActive(false);
            buttonM.SetActive(false);
            buttonR.SetActive(false);
        }
        //assign the correspondent scripts
        buttonL.GetComponent<Button>().onClick.AddListener(scriptleft);
        buttonR.GetComponent<Button>().onClick.AddListener(scriptright);
        buttonM.GetComponent<Button>().onClick.AddListener(scriptmiddle);
        animataC.Play("Enter");
        dialogSFX.Play();
    }

    public void closeDialog()
    {
        animataC.Play("Exit");
        // all UI SFX should be no more than 1.2s.
        Destroy(gameObject, 1.2f);
    }

}
