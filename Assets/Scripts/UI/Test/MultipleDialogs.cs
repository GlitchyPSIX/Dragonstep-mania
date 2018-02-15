using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSMUI.Objects;
using DSMUI.Assets;

public class MultipleDialogs : MonoBehaviour {

    Dialog dlg;

    // Use this for initialization
    void Start () {
        DialogStart();
	}
	
	void DialogStart()
    {
        dlg = new Dialog("Hello.", "This is a test to see how multiple dialogs would work.", "Oh ok", () => { dlg.DestroyDialog(); DialogValentine(); }, SoundEffects.Select, InterfaceIcons.Info, SoundEffects.DialogDefault);
        dlg.ShowDialog();
    }

    void DialogValentine()
    {
        dlg = new Dialog("owo", "...did you have a half orange this last feb 14th?", "yah", () => { dlg.DestroyDialog(); ResultValentine(0); }, SoundEffects.Select, "nah...", () => { dlg.DestroyDialog(); ResultValentine(1); }, SoundEffects.Select, InterfaceIcons.Info, SoundEffects.DialogDefault);
        dlg.ShowDialog();
    }

    void ResultValentine(byte type)
    {
        if(type == 0)
        {
            dlg = new Dialog("YaY", "oh thats nice", "ikr", () => { dlg.DestroyDialog(); SeeDiffAnswer(); }, SoundEffects.Select, InterfaceIcons.OK, SoundEffects.DialogDefault);
            dlg.ShowDialog();
        }
        else if(type == 1)
        {
            dlg = new Dialog("...oh", "oh...\n well if it makes you feel better I didnt have one either", "oh", () => { dlg.DestroyDialog(); SeeDiffAnswer(); }, SoundEffects.Select, InterfaceIcons.Error, SoundEffects.DialogDefault);
            dlg.ShowDialog();

        }
    }

    void OhOkay()
    {
        dlg = new Dialog("alright", "oh ok", "yeah", () => { dlg.DestroyDialog(); }, SoundEffects.Select, InterfaceIcons.Info, SoundEffects.DialogDefault);
        dlg.ShowDialog();
    }

    void SeeDiffAnswer()
    {
        dlg = new Dialog("...curiosity?", "do you want to see the other answer?", "ye", () => { dlg.DestroyDialog(); DialogStart(); }, SoundEffects.Select, "nay", () => { dlg.DestroyDialog(); OhOkay(); }, SoundEffects.Back, InterfaceIcons.Info, SoundEffects.DialogDefault);
        dlg.ShowDialog();
    }
}
