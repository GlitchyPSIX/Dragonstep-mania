using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DSMUI.Assets;

/*
 * TODO:
 * Replace this man right here, just like the dialog he has a very ugly method.
 */

public class MenuListControl : MonoBehaviour
{

    List<menuElement> menustack;
    public Sprite[] iconlist;
    // Use this for initialization
    void Start()
    {
        //initialize the list
        menustack = new List<menuElement>();
    }

    public IEnumerator showMenuElements(Transform menuContainer)
    {
        // for every element in the menu stack list
        foreach (menuElement elm in menustack)
        {
            yield return new WaitForSeconds(0.03f);
            //wait some time so it looks like they're coming in progressively
            GameObject menuObj;
            menuObj = Instantiate(Objects.MenuItemObject);
            menuObj.transform.SetParent(menuContainer.transform, false);
            menuObj.GetComponent<MenuItemControl>().setMenuItem(elm.name, elm.subtitle, elm.icon, elm.action, elm.sfx);
        }
        //remove all the elements from the stack, they're already displayed
        menustack.RemoveRange(0, menustack.Count);
    }

    public void listMenuElements()
    {
        //for every element in the menu stack list, show basic info, not what it does (debug purposes)
        foreach (menuElement elm in menustack)
        {
            Debug.Log("MENUTITLE: " + elm.name + "\n" + elm.icon.ToString() + "\n" + elm.subtitle);
        }
    }

    public void addMenuElement(string N, string S, Sprite I, UnityAction A, AudioClip F)
    {
        //create a new menuElement in the stack
        menustack.Add(new menuElement(N, S, I, A, F));
    }

    struct menuElement
    {
        public string name;
        public Sprite icon;
        public string subtitle;
        public UnityAction action;

        public AudioClip sfx;
        //set the properties of the menu item
        public menuElement(string n, string s, Sprite i, UnityAction a, AudioClip snd)
        {
            if (snd == null)
            {
                sfx = SoundEffects.Select;
            }
            else
            {
                sfx = snd;
            }
            name = n;
            icon = i;
            subtitle = s;
            action = a;
        }

    }

}
