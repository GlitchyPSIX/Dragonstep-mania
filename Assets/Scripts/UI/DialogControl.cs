using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class DialogControl : MonoBehaviour {
    Animator animataC;
    UnityAction defaultBehav;
    Text title;
    Text body;
    Text buttonLText;

    Text buttonMText;
    Text buttonRText;
    GameObject buttonL;
	GameObject buttonM;
	GameObject buttonR;

    // Use this for initialization
    void Start () {
        animataC = GetComponent<Animator>();
        defaultBehav = GetComponent<DialogControl>().closeDialog;
        title = transform.Find("DialogBG").transform.Find("DialogTitle").GetComponent<Text>();
		body = transform.Find("DialogBG").transform.Find("DialogText").GetComponent<Text>();
        buttonL = transform.Find("DialogBG").transform.Find("ButtonL").gameObject;
		buttonM = transform.Find("DialogBG").transform.Find("ButtonM").gameObject;
		buttonR = transform.Find("DialogBG").transform.Find("ButtonR").gameObject;
        buttonLText = transform.Find("DialogBG").transform.Find("ButtonL").transform.Find("Text").GetComponent<Text>();
		buttonMText = transform.Find("DialogBG").transform.Find("ButtonM").transform.Find("Text").GetComponent<Text>();
		buttonRText = transform.Find("DialogBG").transform.Find("ButtonR").transform.Find("Text").GetComponent<Text>();
    }

	/// <summary>
    /// Sets the data and actions for this dialog box.
    /// </summary>
    /// <param name="titleS">The title of the dialog.</param>
    /// <param name="textS">The title of the dialog.</param>
	/// <param name="buttons">Button types. 0 for only left. 1 for left and right. 2 for all three buttons.</param>
	/// <param name="scriptleft">UnityAction to call for the left button.</param>
	/// <param name="scriptmiddle">UnityAction to call for the middle button.</param>
	/// <param name="scriptright">UnityAction to call for the right button.</param>
	/// <param name="buttonLS">Text of the left button.</param>
	/// <param name="buttonMS">Text of the middle button.</param>
	/// <param name="buttonRS">Text of the right button.</param>
    public void setDialog(string titleS, string textS, byte buttons, UnityAction scriptleft, UnityAction scriptmiddle,
                    UnityAction scriptright, string buttonLS = "", string buttonRS = "", string buttonMS = ""){
        title.text = titleS;
        body.text = textS;
        buttonLText.text = buttonLS;
		buttonMText.text = buttonMS;
		buttonRText.text = buttonRS;
        if (buttons == 0){
            buttonL.SetActive(false);
			buttonM.SetActive(false);
			buttonR.SetActive(true);
        }
		else if (buttons == 1){
			buttonL.SetActive(true);
			buttonM.SetActive(false);
			buttonR.SetActive(true);
		}
		else if (buttons == 2){
			buttonL.SetActive(true);
			buttonM.SetActive(true);
			buttonR.SetActive(true);
		}
		else{
            Debug.Log("Invalid button set attempted. The dialog with the title \"" + titleS + "\" will show no buttons and will be rendered unusable.");
            buttonL.SetActive(false);
			buttonM.SetActive(false);
			buttonR.SetActive(false);
		}
        buttonL.GetComponent<Button>().onClick.AddListener(scriptleft);
		buttonR.GetComponent<Button>().onClick.AddListener(scriptright);
		buttonM.GetComponent<Button>().onClick.AddListener(scriptmiddle);
        animataC.Play("Enter");
    }

	public void closeDialog(){
        animataC.Play("Exit");
        Destroy(gameObject, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
