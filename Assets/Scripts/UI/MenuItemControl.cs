using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;


public class MenuItemControl : MonoBehaviour {

    Sprite menuSprite;
    Text menuText;
    string menuSubtitleText;

    // Use this for initialization
    void Start () {
		
	}

    void OnMouseEnter()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }

    public void setMenuItem(string titleS, string subtitle, Sprite spriteS, UnityAction script, AudioClip buttonSFX){

    }

	
}
