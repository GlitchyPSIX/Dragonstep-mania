using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;


public class MenuItemControl : MonoBehaviour {

    Animator animataM;
    Image menuSprite;
    Text menuText;
    //string menuSubtitleText;
    public Sprite uiIcons;

    // Use this for initialization
    void Start () {
        StartCoroutine(LateStart());
    }

    public void setMenuItem(string titleS, string subtitle, Sprite spriteS, UnityAction script, AudioClip buttonSFX){
        animataM = GetComponent<Animator>();
        menuSprite = transform.Find("MenuBG").gameObject.transform.Find("Image").GetComponent<Image>();
        menuText = transform.Find("MenuBG").gameObject.transform.Find("Text").GetComponent<Text>();
        menuSprite.sprite = spriteS;
        menuText.text = titleS;
        
        EventTrigger evtTrig = GetComponent<EventTrigger>();
        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        EventTrigger.Entry hoverEntry = new EventTrigger.Entry();
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerDown;
        hoverEntry.eventID = EventTriggerType.PointerEnter;
        exitEntry.eventID = EventTriggerType.PointerExit;
        clickEntry.callback.AddListener((data) => { script(); });
        clickEntry.callback.AddListener((data) => { OnClick((PointerEventData)data); });
        hoverEntry.callback.AddListener((data) => { OnMouseEnter((PointerEventData)data, subtitle); });
        exitEntry.callback.AddListener((data) => { OnMouseExit(); });

        evtTrig.triggers.Add(exitEntry);
        evtTrig.triggers.Add(hoverEntry);
        evtTrig.triggers.Add(clickEntry);
        animataM.Play("menuComeIn");
     }

    void OnClick(PointerEventData data){

    }

        void OnMouseEnter(PointerEventData data, string subt)
    {
        GameObject.FindGameObjectWithTag("UIMenuSubtitle").GetComponent<MenuSubtitleController>().ChangeText(subt);
        animataM.Play("menuHover");
    }

    void OnMouseExit(){
        animataM.Play("menuOut");
        StartCoroutine(GameObject.FindGameObjectWithTag("UIMenuSubtitle").GetComponent<MenuSubtitleController>().FadeTextOut());
    }

	public void killItem()
    {
        animataM.Play("menuHide");
        EventTrigger evtTrig = GetComponent<EventTrigger>();
        Destroy(evtTrig, 0);
        // all UI SFX should be no more than 1.5s.
        Destroy(gameObject, 1.5f);
    }

    IEnumerator LateStart(){
        yield return new WaitForEndOfFrame();
        animataM = GetComponent<Animator>();
    }
}
