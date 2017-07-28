using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playtitleanimation : MonoBehaviour {

    AsyncOperation LoadDebug;
    Text VersionText;

    void Start(){
        LoadDebug.allowSceneActivation = false;
        LoadDebug = SceneManager.LoadSceneAsync("debug");
        VersionText = GameObject.Find("VersionTag").GetComponent<Text>();
        VersionText.text = "Loading...";
    }

	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown("Step")){
			GetComponent<Animator>().Play("Draw Out");
		}

	}
}
