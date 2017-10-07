using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playtitleanimation : MonoBehaviour {

    AsyncOperation LoadDebug;
    Text VersionText;

    void Start(){
        LoadDebug = SceneManager.LoadSceneAsync("debug");
        LoadDebug.allowSceneActivation = false;
        VersionText = GameObject.Find("VersionTag").GetComponent<Text>();
        VersionText.text = "Loading...";
    }

	// Update is called once per frame
	void Update () {
		if (LoadDebug.progress == 0.9f){
            VersionText.text = "Loading complete. press S to start this thing";
            if (Input.GetKeyDown(KeyCode.S) == true){
                LoadDebug.allowSceneActivation = true;
            }
        }
		if (Input.GetButtonDown("Step")){
			GetComponent<Animator>().Play("Draw Out");
		}

	}
}
