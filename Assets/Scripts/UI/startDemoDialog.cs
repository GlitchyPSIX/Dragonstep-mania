using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class startDemoDialog : MonoBehaviour {

    GameObject dialogObj;
    GameObject dialogPref;

    // Use this for initialization
    void Start () {
        
        dialogPref =  Resources.Load<GameObject>("Prefabs/UI/Dialog");
        dialogObj = Instantiate(dialogPref);
        dialogObj.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform, false);
        dialogObj.SetActive(true);
        StartDemoDialogAfter();
    }
	
	// Update is called once per frame
	void StartDemoDialogAfter() {
        dialogObj.GetComponent<DialogControl>().setDialog(
                                            "Demo",
                                            "Remember this is a demo. Any error or missing thing should be reported to me in the respective channel.",
                                            0,
                                            null,
                                            null,
                                            dialogObj.GetComponent<DialogControl>().closeDialog,
                                            null,
                                            null,
                                            Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection"), "", "OK", "");
    }
}
