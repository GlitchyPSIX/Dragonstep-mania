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
    MenuListControl mlc;
    Transform containerTransform;

    // Use this for initialization
    void Start()
    {
        containerTransform = GameObject.FindGameObjectWithTag("UIMenuContainer").transform;
        mlc = GetComponent<MenuListControl>();
        StartDemoDialogAfter();
        mlc.listMenuElements();        
    }

    void mainMenu()
    {
        //load main menu's icons
        mlc.addMenuElement("Play Demo", "Play a sample song.", mlc.iconlist[5], UnimplementedFunctionDialog, Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection"));
        mlc.addMenuElement("Endurance Mode", "This isn't available now.", mlc.iconlist[0], UnimplementedFunctionDialog, Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection"));
        mlc.addMenuElement("Exit", "Close the game.", mlc.iconlist[6], UnimplementedFunctionDialog, Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection"));
    }
    
    void UnimplementedFunctionDialog(){
        dialogPref = Resources.Load<GameObject>("Prefabs/UI/Dialog");
        dialogObj = Instantiate(dialogPref);
        dialogObj.SetActive(true);
        dialogObj.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform, false);
        dialogObj.GetComponent<DialogControl>().setDialog(
                                            "Not implemented",
                                            "The function for this menu item has not been implemented yet. Sorry!",
                                            0,
                                            null,
                                            null,
                                            dialogObj.GetComponent<DialogControl>().closeDialog,
                                            null,
                                            null,
                                            Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection"), "", "OK", "");
    }
    void StartDemoDialogAfter()
    {
        dialogPref = Resources.Load<GameObject>("Prefabs/UI/Dialog");
        dialogObj = Instantiate(dialogPref);
        dialogObj.SetActive(true);
        dialogObj.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform, false);
        dialogObj.GetComponent<DialogControl>().setDialog(
                                            "Demo",
                                            "Remember this is a demo. Any error or missing thing should be reported to me in the respective channel.",
                                            0,
                                            null,
                                            null,
                                            () => { mainMenu(); StartCoroutine(mlc.showMenuElements(containerTransform)); dialogObj.GetComponent<DialogControl>().closeDialog(); },
                                            null,
                                            null,
                                            Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection"), "", "OK", "");
    }
}
