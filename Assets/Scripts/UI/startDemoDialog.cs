using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class startDemoDialog : MonoBehaviour
{

    GameObject dialogObj;
    GameObject dialogPref;
    GameObject menuObj;
    GameObject menuPref;

    // Use this for initialization
    void Start()
    {

        dialogPref = Resources.Load<GameObject>("Prefabs/UI/Dialog");
        menuPref = Resources.Load<GameObject>("Prefabs/UI/MenuItem");
        dialogObj = Instantiate(dialogPref);
        menuObj = Instantiate(menuPref);
        dialogObj.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform, false);
        menuObj.transform.SetParent(GameObject.FindGameObjectWithTag("UIMenuContainer").transform, false);
        dialogObj.SetActive(true);
        StartDemoDialogAfter();
    }

    // Update is called once per frame
    void StartDemoDialogAfter()
    {
        menuObj.GetComponent<MenuItemControl>().setMenuItem("Play Demo", "Play this demo.", Resources.Load<Sprite>("Sprites/particleStar"), menuObj.GetComponent<MenuItemControl>().killItem, Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection"));

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
