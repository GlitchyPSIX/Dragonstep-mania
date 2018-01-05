using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class MenuListControl : MonoBehaviour
{

    List<menuElement> menustack;
    GameObject menuPref;

    // Use this for initialization
    void Start()
    {
        menustack = new List<menuElement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void showMenuElements(Transform menuContainer){
        foreach(menuElement element in menustack){
            GameObject menuObj;
            menuObj = Instantiate(menuPref);
            menuObj.transform.SetParent(menuContainer.transform);
            menuObj.GetComponent<MenuItemControl>().setMenuItem(element.name, element.subtitle, element.icon, element.action, element.sfx);
        }
        menustack.RemoveRange(0, menustack.Count);
    }

    public void addMenuElement(string N, Sprite I, string S, UnityAction A, AudioClip F)
    {
        menuElement elementToAdd;
        elementToAdd = new menuElement(N, I, S, A, F);
        menustack.Add(elementToAdd);
    }

    struct menuElement
    {
        public string name;
        public Sprite icon;
        public string subtitle;
        public UnityAction action;

        public AudioClip sfx;

        public menuElement(string n, Sprite i, string s, UnityAction a, AudioClip snd)
        {
            if (snd = null){
                sfx = Resources.Load<AudioClip>("SFX/UI/hiSelect");
            }
            else{
                sfx = snd;
            }
            name = n;
            icon = i;
            subtitle = s;
            action = a;
        }

    }

}
