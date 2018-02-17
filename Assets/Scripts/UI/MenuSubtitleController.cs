using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuSubtitleController : MonoBehaviour {


    Animator ac;
    Text t;

    // Use this for initialization
    void Start () {
        t = GetComponent<Text>();
        ac = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	public void ChangeText(string subt){
        t.text = subt;
        ac.Play("changeSubtitleIn");
    }

    public void FadeTextOut(){
        ac.Play("changeSubtitleOut");
    }
}
