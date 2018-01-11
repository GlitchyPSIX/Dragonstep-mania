using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

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
    AudioSource buttonLSFX;
    AudioSource buttonMSFX;
    AudioSource buttonRSFX;
    public AudioClip hiSelectfx;
    public AudioClip selectfx;
    public AudioClip backfx;

    // Use this for initialization
    void Start()
    {
        //why hello there, preload the sounds
        hiSelectfx = Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelect");
        selectfx = Resources.Load<AudioClip>("Sounds/SFX/UI/select");
        backfx = Resources.Load<AudioClip>("Sounds/SFX/UI/back");
    }
    #region setDialog_doc
    /// <summary>
    /// Sets the data and actions for this dialog box.
    /// </summary>
    ///
    /// <param name="titleS">The title of the dialog.</param>
    /// <param name="textS">The title of the dialog.</param>
    /// <param name="buttons">Button types. 0 for only left. 1 for middle and right. 2 for all three buttons.</param>
    /// <param name="scriptleft">UnityAction to call for the left button.</param>
    /// <param name="scriptmiddle">UnityAction to call for the middle button.</param>
    /// <param name="scriptright">UnityAction to call for the right button.</param>
    /// <param name="buttonLS">Text of the left button.</param>
    /// <param name="buttonMS">Text of the middle button.</param>
    /// <param name="buttonRS">Text of the right button.</param>
    #endregion
    public void setDialog(string titleS, string textS, byte buttons, UnityAction scriptleft, UnityAction scriptmiddle,
                          UnityAction scriptright, AudioClip buttonLASFX, AudioClip buttonMASFX, AudioClip buttonRASFX,
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

        //set the sound effects
        buttonLSFX.clip = buttonLASFX;
        buttonMSFX.clip = buttonMASFX;
        buttonRSFX.clip = buttonRASFX;

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
    }

    public void closeDialog()
    {
        animataC.Play("Exit");
        // all UI SFX should be no more than 1.5s.
        Destroy(gameObject, 1.5f);
    }

}
