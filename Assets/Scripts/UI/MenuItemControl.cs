using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;


public class MenuItemControl : MonoBehaviour {

    Animator animataM;
    Image menuSprite;
    GameObject menuBtn;
    Text menuText;
    string menuSubtitleText;
    public Sprite uiIcons;

    // Use this for initialization
    void Start () {
        
        }

    void OnMouseEnter()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        animataM.Play("menuHover");
    }

    void OnMouseExit(){
        animataM.Play("menuOut");
    }

    public void setMenuItem(string titleS, string subtitle, Sprite spriteS, UnityAction script, AudioClip buttonSFX){
        animataM = GetComponent<Animator>();
        menuSprite = transform.Find("MenuBG").gameObject.transform.Find("Image").GetComponent<Image>();
        menuText = transform.Find("MenuBG").gameObject.transform.Find("Text").GetComponent<Text>();
        menuBtn = transform.Find("MenuBG").gameObject;
        menuSprite.sprite = spriteS;
        menuText.text = titleS;
        menuBtn.GetComponent<Button>().onClick.AddListener(script);
        animataM.Play("menuComeIn");
     }

	public void killItem()
    {
        animataM.Play("menuHide");
        // all UI SFX should be no more than 1.5s.
        Destroy(gameObject, 1.5f);
    }
}
