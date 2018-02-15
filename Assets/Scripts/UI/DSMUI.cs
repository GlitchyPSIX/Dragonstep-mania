using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace DSMUI
{
    namespace Assets
    {

        public class Transitions
        {
            static Sprite[] TransitionSprites = Resources.LoadAll<Sprite>("Sprites/UI/transitionMasks");
            public static Sprite EvilGrin = TransitionSprites[0];
            public static Sprite Fancy = TransitionSprites[1];
            public static Sprite Miss = TransitionSprites[2];
            public static Sprite Normal = TransitionSprites[3];
            public static Sprite Wink = TransitionSprites[4];
        }

        public class InterfaceIcons
        {
            static Sprite[] UIIcons = Resources.LoadAll<Sprite>("Sprites/UI/menuDialog");
            public static Sprite Endurance = UIIcons[0];
            public static Sprite Error = UIIcons[1];
            public static Sprite Exit = UIIcons[2];
            public static Sprite Info = UIIcons[3];
            public static Sprite OK = UIIcons[4];
            public static Sprite Play = UIIcons[5];
            public static Sprite Settings = UIIcons[6];
            public static Sprite Warning = UIIcons[7];
        }

        public class Objects
        {
            public static GameObject DialogObject = Resources.Load<GameObject>("Prefabs/UI/Dialog");
            public static GameObject MenuItemObject = Resources.Load<GameObject>("Prefabs/UI/MenuItem");
            public static GameObject TransitionOverlay = Resources.Load<GameObject>("Prefabs/UI/TransitionBG");
        }

        public class SoundEffects
        {
            public static AudioClip Confirm = Resources.Load<AudioClip>("Sounds/SFX/UI/hiSelection");
            public static AudioClip Select = Resources.Load<AudioClip>("Sounds/SFX/UI/select");
            public static AudioClip Back = Resources.Load<AudioClip>("Sounds/SFX/UI/back");
            public static AudioClip DialogDefault = Resources.Load<AudioClip>("Sounds/SFX/UI/dialogDefault");
            public static AudioClip Info = Resources.Load<AudioClip>("Sounds/SFX/UI/info");
            public static AudioClip Advice = Resources.Load<AudioClip>("Sounds/SFX/UI/advice");
            public static AudioClip Hover = Resources.Load<AudioClip>("Sounds/SFX/UI/hover");
        }

    }

    namespace Objects
    {
        public class Dialog
        {
            /// <summary>
            /// Class containing everything related to Dialogs.
            /// </summary>

            public static GameObject DialogPrefab = Assets.Objects.DialogObject.gameObject;
            GameObject returnedDialog = Object.Instantiate(DialogPrefab);
            public string title;
            public string body;
            public GameObject buttonL;
            public GameObject buttonM;
            public GameObject buttonR;
            public string buttonLText;
            public string buttonMText;
            public string buttonRText;
            public AudioClip dialogSFX;
            public AudioClip buttonLSFX;
            public AudioClip buttonMSFX;
            public AudioClip buttonRSFX;
            public Button buttonActionR;
            public Button buttonActionM;
            public Button buttonActionL;
            public GameObject icon;

            public Dialog(string Title, string Body, string ButtonText, UnityAction Action, AudioClip ButtonSFX, Sprite Icon = null, AudioClip DialogSFX = null)
            {
                /*
                 * oh my god this is all so ugly
                 * at least it's better than before
                 * way better if I say so myself
                 */

                icon = returnedDialog.transform.Find("DialogBG").transform.Find("DialogIcon").transform.Find("DialogIconBG").transform.Find("Icon").gameObject;
                buttonL = returnedDialog.transform.Find("DialogBG").transform.Find("ButtonL").gameObject;
                buttonM = returnedDialog.transform.Find("DialogBG").transform.Find("ButtonM").gameObject;
                buttonR = returnedDialog.transform.Find("DialogBG").transform.Find("ButtonR").gameObject;

                //Override with only the right button
                buttonM.SetActive(false);
                buttonL.SetActive(false);
                returnedDialog.transform.Find("DialogBG").transform.Find("DialogTitle").GetComponent<Text>().text = Title;
                returnedDialog.transform.Find("DialogBG").transform.Find("DialogText").GetComponent<Text>().text = Body;
                returnedDialog.transform.Find("DialogBG").transform.Find("ButtonR").transform.Find("Text").GetComponent<Text>().text = ButtonText;
                buttonR.GetComponent<Button>().onClick.AddListener(Action);
                //Set dialog enter SFX
                if (DialogSFX == null)
                {
                    returnedDialog.GetComponent<AudioSource>().clip = Assets.SoundEffects.DialogDefault;
                }
                else
                {
                    returnedDialog.GetComponent<AudioSource>().clip = DialogSFX;
                }
                //Set button SFX
                //Right button
                if (ButtonSFX == null)
                {
                    buttonR.transform.Find("SFX").GetComponent<AudioSource>().clip = Assets.SoundEffects.Select;
                }
                else
                {
                    buttonR.transform.Find("SFX").GetComponent<AudioSource>().clip = ButtonSFX;
                }
                //Set icon
                if (Icon == null)
                {
                    icon.GetComponent<Image>().sprite = Assets.InterfaceIcons.Info;
                }
                else
                {
                    icon.GetComponent<Image>().sprite = Icon;
                }
                return;
            }

            public Dialog(string Title, string Body, string ButtonRText, UnityAction ActionR, AudioClip ButtonRSFX, string ButtonMText, UnityAction ActionM, AudioClip ButtonMSFX, Sprite Icon = null, AudioClip DialogSFX = null)
            {
                icon = returnedDialog.transform.Find("DialogBG").transform.Find("DialogIcon").transform.Find("DialogIconBG").transform.Find("Icon").gameObject;
                buttonL = returnedDialog.transform.Find("DialogBG").transform.Find("ButtonL").gameObject;
                buttonM = returnedDialog.transform.Find("DialogBG").transform.Find("ButtonM").gameObject;
                buttonR = returnedDialog.transform.Find("DialogBG").transform.Find("ButtonR").gameObject;

                //Override with middle and right buttons
                buttonL.SetActive(false);
                returnedDialog.transform.Find("DialogBG").transform.Find("DialogTitle").GetComponent<Text>().text = Title;
                returnedDialog.transform.Find("DialogBG").transform.Find("DialogText").GetComponent<Text>().text = Body;
                returnedDialog.transform.Find("DialogBG").transform.Find("ButtonR").transform.Find("Text").GetComponent<Text>().text = ButtonRText;
                returnedDialog.transform.Find("DialogBG").transform.Find("ButtonM").transform.Find("Text").GetComponent<Text>().text = ButtonMText;
                buttonR.GetComponent<Button>().onClick.AddListener(ActionR);
                buttonM.GetComponent<Button>().onClick.AddListener(ActionM);
                //Set dialog enter SFX
                if (DialogSFX == null)
                {
                    returnedDialog.GetComponent<AudioSource>().clip = Assets.SoundEffects.DialogDefault;
                }
                else
                {
                    returnedDialog.GetComponent<AudioSource>().clip = DialogSFX;
                }
                //Set button SFX
                //Right button
                if (ButtonRSFX == null)
                {
                    buttonR.transform.Find("SFX").GetComponent<AudioSource>().clip = Assets.SoundEffects.Select;
                }
                else
                {
                    buttonR.transform.Find("SFX").GetComponent<AudioSource>().clip = ButtonRSFX;
                }
                //Middle button
                if (ButtonMSFX == null)
                {
                    buttonM.transform.Find("SFX").GetComponent<AudioSource>().clip = Assets.SoundEffects.Select;
                }
                else
                {
                    buttonM.transform.Find("SFX").GetComponent<AudioSource>().clip = ButtonMSFX;
                }
                //Set icon
                if (Icon == null)
                {
                    icon.GetComponent<Image>().sprite = Assets.InterfaceIcons.Info;
                }
                else
                {
                    icon.GetComponent<Image>().sprite = Icon;
                }
            }

            public Dialog(string Title, string Body, string ButtonRText, UnityAction ActionR, AudioClip ButtonRSFX, string ButtonMText, UnityAction ActionM, AudioClip ButtonMSFX, string ButtonLText, UnityAction ActionL, AudioClip ButtonLSFX, Sprite Icon = null, AudioClip DialogSFX = null)
            {
                icon = returnedDialog.transform.Find("DialogBG").transform.Find("DialogIcon").transform.Find("DialogIconBG").transform.Find("Icon").gameObject;
                buttonL = returnedDialog.transform.Find("DialogBG").transform.Find("ButtonL").gameObject;
                buttonM = returnedDialog.transform.Find("DialogBG").transform.Find("ButtonM").gameObject;
                buttonR = returnedDialog.transform.Find("DialogBG").transform.Find("ButtonR").gameObject;
                //Override with all buttons
                returnedDialog.transform.Find("DialogBG").transform.Find("DialogTitle").GetComponent<Text>().text = Title;
                returnedDialog.transform.Find("DialogBG").transform.Find("DialogText").GetComponent<Text>().text = Body;
                returnedDialog.transform.Find("DialogBG").transform.Find("ButtonR").transform.Find("Text").GetComponent<Text>().text = ButtonRText;
                returnedDialog.transform.Find("DialogBG").transform.Find("ButtonM").transform.Find("Text").GetComponent<Text>().text = ButtonMText;
                returnedDialog.transform.Find("DialogBG").transform.Find("ButtonL").transform.Find("Text").GetComponent<Text>().text = ButtonLText;
                buttonR.GetComponent<Button>().onClick.AddListener(ActionR);
                buttonL.GetComponent<Button>().onClick.AddListener(ActionL);
                buttonM.GetComponent<Button>().onClick.AddListener(ActionM);
                //Set dialog enter SFX
                if (DialogSFX == null)
                {
                    dialogSFX = Assets.SoundEffects.DialogDefault;
                }
                else
                {
                    dialogSFX = DialogSFX;
                }
                //Set button SFX
                //Right button
                if (ButtonRSFX == null)
                {
                    buttonR.transform.Find("SFX").GetComponent<AudioSource>().clip = Assets.SoundEffects.Select;
                }
                else
                {
                    buttonR.transform.Find("SFX").GetComponent<AudioSource>().clip = ButtonRSFX;
                }
                //Middle button
                if (ButtonMSFX == null)
                {
                    buttonM.transform.Find("SFX").GetComponent<AudioSource>().clip = Assets.SoundEffects.Select;
                }
                else
                {
                    buttonM.transform.Find("SFX").GetComponent<AudioSource>().clip = ButtonMSFX;
                }
                //Left button
                if (ButtonLSFX == null)
                {
                    buttonL.transform.Find("SFX").GetComponent<AudioSource>().clip = Assets.SoundEffects.Select;
                }
                else
                {
                    buttonL.transform.Find("SFX").GetComponent<AudioSource>().clip = ButtonLSFX;
                }
                //Set icon
                if (Icon == null)
                {
                    icon.GetComponent<Image>().sprite = Assets.InterfaceIcons.Info;
                }
                else
                {
                    icon.GetComponent<Image>().sprite = Icon;
                }
            }

            public void ShowDialog()
            {
                returnedDialog.SetActive(true);
                returnedDialog.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform, false);
                returnedDialog.GetComponent<Animator>().Play("Enter");
                returnedDialog.GetComponent<AudioSource>().Play();
            }

            public void DestroyDialog()
            {
                returnedDialog.GetComponent<Animator>().Play("Exit");
                // all UI SFX should be no more than 1.2s.
                Object.Destroy(returnedDialog, 1.2f);
            }

        }

    }
}

