using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSMUI.Assets;

public class startDemoDialog : MonoBehaviour
{

    DSMUI.Objects.Dialog dialogObj;
    MenuListControl mlc;
    Transform containerTransform;

    // Use this for initialization
    void Start()
    {
        containerTransform = GameObject.FindGameObjectWithTag("UIMenuContainer").transform;
        mlc = GetComponent<MenuListControl>();
        StartDemoDialogAfter();
    }

    void mainMenu()
    {
        //load main menu's items
        mlc.addMenuElement("Play Demo", "Play a sample song.", InterfaceIcons.Play, UnimplementedFunctionDialog, SoundEffects.Select);
        mlc.addMenuElement("Endurance Mode", "This isn't available now.", InterfaceIcons.Endurance, UnimplementedFunctionDialog, SoundEffects.Select);
        mlc.addMenuElement("Exit", "Close the game.", InterfaceIcons.Exit, UnimplementedFunctionDialog, SoundEffects.Select);
        mlc.listMenuElements();
    }
    
    void UnimplementedFunctionDialog(){
        //dialogObj = Instantiate(Objects.DialogObject);
        //dialogObj.SetActive(true);
        //dialogObj.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform, false);
        //dialogObj.GetComponent<DialogControl>().setDialog(
        //                                    "Not implemented",
        //                                    "The function for this menu item has not been implemented yet. Sorry!",
        //                                    0,
        //                                    null,
        //                                    null,
        //                                    dialogObj.GetComponent<DialogControl>().closeDialog,
        //                                    null,
        //                                    null,
        //                                    SoundEffects.Select, SoundEffects.Confirm, "", "OK", "");
        
    }
    void StartDemoDialogAfter()
    {
        //dialogObj = Instantiate(Objects.DialogObject);
        //dialogObj.SetActive(true);
        //dialogObj.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform, false);
        //dialogObj.GetComponent<DialogControl>().setDialog(
        //                                    "Demo",
        //                                    "Remember this is a demo. Any error or missing thing should be reported to me in the respective channel.",
        //                                    0,
        //                                    null,
        //                                    null,
        //                                    () => { mainMenu(); StartCoroutine(mlc.showMenuElements(containerTransform)); dialogObj.GetComponent<DialogControl>().closeDialog(); },
        //                                    null,
        //                                    null,
        //                                     SoundEffects.Confirm, SoundEffects.DialogDefault, "", "OK", "");
        dialogObj = new DSMUI.Objects.Dialog("Demo", "Remember this is a demo. Any error or missing thing should be reported to me in the respective channel.", "Gotcha", () => { dialogObj.DestroyDialog(); }, SoundEffects.Select, InterfaceIcons.Info, SoundEffects.DialogDefault);
        dialogObj.ShowDialog();
    }
}
