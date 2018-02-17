using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSMUI.Assets;
using DSMUI.Objects;

public class startDemoDialog : MonoBehaviour
{

    DSMUI.Objects.Dialog dialogObj;
    Transform containerTransform;
    List<MenuItem> mlm;

    // Use this for initialization
    void Start()
    {
        containerTransform = GameObject.FindGameObjectWithTag("UIMenuContainer").transform;
        mlm = new List<MenuItem>()
        {
            new MenuItem()
            {
                Title = "Play Demo",
                Subtitle = "Play a sample song.",
                Icon = InterfaceIcons.Play,
                Action = UnimplementedFunctionDialog,
                SFX = SoundEffects.Select
            },
            new MenuItem()
            {
                Title = "Endurance Mode",
                Subtitle = "This is unavailable.",
                Icon = InterfaceIcons.Endurance,
                Action = UnimplementedFunctionDialog,
                SFX = SoundEffects.Select
            },
            new MenuItem()
            {
                Title = "Exit",
                Subtitle = "Closes the game.",
                Icon = InterfaceIcons.Exit,
                Action = UnimplementedFunctionDialog,
                SFX = SoundEffects.Select
            }
        };
        StartDemoDialogAfter();
    }

    void mainMenu()
    {
        //load main menu's items
        //mlc.addMenuElement("Play Demo", "Play a sample song.", InterfaceIcons.Play, UnimplementedFunctionDialog, SoundEffects.Select);
        //mlc.addMenuElement("Endurance Mode", "This isn't available now.", InterfaceIcons.Endurance, UnimplementedFunctionDialog, SoundEffects.Select);
        //mlc.addMenuElement("Exit", "Close the game.", InterfaceIcons.Exit, UnimplementedFunctionDialog, SoundEffects.Select);
        //mlc.listMenuElements();
        foreach(MenuItem ml in mlm)
        {
            ml.Show(containerTransform);
        }
    }
    
    void UnimplementedFunctionDialog(){
        dialogObj = new Dialog("Not implemented",
            "This function isn't done yet. Sorry!",
            "Alright",
            () => { dialogObj.DestroyDialog(); },
            SoundEffects.Select, InterfaceIcons.Warning, SoundEffects.DialogDefault);
        dialogObj.ShowDialog();
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
        dialogObj = new Dialog("Demo", "Remember this is a demo. Any error or missing thing should be reported to me in the respective channel.", "Gotcha", () => { dialogObj.DestroyDialog(); mainMenu(); }, SoundEffects.Select, InterfaceIcons.Info, SoundEffects.DialogDefault);
        dialogObj.ShowDialog();
    }
}
