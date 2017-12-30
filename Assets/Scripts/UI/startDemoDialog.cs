using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class startDemoDialog : MonoBehaviour
{

    GameObject dialogObj;
    GameObject dialogPref;
    GameObject menuObj;
    GameObject menuObj1;
    GameObject menuObj2;
    GameObject menuPref;

    // Use this for initialization
    void Start()
    {

        dialogPref = Resources.Load<GameObject>("Prefabs/UI/Dialog");
        menuPref = Resources.Load<GameObject>("Prefabs/UI/MenuItem");
        dialogObj = Instantiate(dialogPref);
        menuObj = Instantiate(menuPref);
        menuObj1 = Instantiate(menuPref);
        menuObj2 = Instantiate(menuPref);
        dialogObj.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform, false);
        menuObj.transform.SetParent(GameObject.FindGameObjectWithTag("UIMenuContainer").transform, false);
        menuObj1.transform.SetParent(GameObject.FindGameObjectWithTag("UIMenuContainer").transform, false);
        menuObj2.transform.SetParent(GameObject.FindGameObjectWithTag("UIMenuContainer").transform, false);
        dialogObj.SetActive(true);
        StartCoroutine(StartDemoDialogAfter());
    }

    // Update is called once per frame
    IEnumerator StartDemoDialogAfter()
    {
        yield return new WaitForEndOfFrame();
        menuObj.GetComponent<MenuItemControl>().setMenuItem("Play Demo", "Play this demo.", Resources.LoadAll<Sprite>("Sprites/UI/Icons/menuDialog")[0], menuObj.GetComponent<MenuItemControl>().killItem, Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection"));
        menuObj1.GetComponent<MenuItemControl>().setMenuItem("Play Demo 1", "Play this demo 1.", Resources.LoadAll<Sprite>("Sprites/UI/Icons/menuDialog")[1], menuObj1.GetComponent<MenuItemControl>().killItem, Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection"));
        menuObj2.GetComponent<MenuItemControl>().setMenuItem("Play Demo 2", "Play this demo 2.", Resources.LoadAll<Sprite>("Sprites/UI/Icons/menuDialog")[2], menuObj2.GetComponent<MenuItemControl>().killItem, Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection"));

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
