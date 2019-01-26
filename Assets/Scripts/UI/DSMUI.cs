using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
            public static GameObject DialogButtonObject = Resources.Load<GameObject>("Prefabs/UI/DialogButton");
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
        /// <summary>
        /// Class containing everything related to common custom buttons.
        /// </summary>
        public class UIButton
        {
            protected static GameObject DialogButtonPrefab = Assets.Objects.DialogButtonObject.gameObject;
            protected GameObject returnedButtonItem = Object.Instantiate(DialogButtonPrefab);
            protected string _name;
            protected AudioClip _sfx;

            public string Text{
                get { return returnedButtonItem.transform.GetChild(1).GetComponent<Text>().text; }
                set { returnedButtonItem.transform.GetChild(1).GetComponent<Text>().text = value; }
}

            public AudioClip SFX
            {
                get { return returnedButtonItem.transform.GetChild(0).GetComponent<AudioSource>().clip; }
                set { returnedButtonItem.transform.GetChild(0).GetComponent<AudioSource>().clip = value; }
            }

            public GameObject gameObject
            {
                get { return returnedButtonItem; }
            }
            public UnityAction Action { get; set; }

            public UIButton(float x, float y, Transform transf = null)
            {
                returnedButtonItem.SetActive(true);
                if (transf != null)
                { returnedButtonItem.transform.SetParent(transf); }
                else
                { returnedButtonItem.GetComponent<RectTransform>().position = new Vector2(x, y); }
            }
        }

        /// <summary>
        /// Class containing everything related to Menu items (Squares).
        /// </summary>
        public class MenuItem
        {
            private UnityAction action;
            static GameObject MenuItemPrefab = Assets.Objects.MenuItemObject.gameObject;
            GameObject returnedMenuItem = Object.Instantiate(MenuItemPrefab);
            //ONE OF THESE MUST ALWAYS BE PRESENT
            MenuSubtitleController subc = GameObject.FindGameObjectWithTag("UIMenuSubtitle").GetComponent<MenuSubtitleController>();

            public UnityAction Action
            {
                get
                {
                    return action;
                }

                set
                {
                    EventTrigger evtTrig = returnedMenuItem.GetComponent<EventTrigger>();
                    EventTrigger.Entry clickEntry = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerDown
                    };
                    clickEntry.callback.AddListener((data) => Action());
                    clickEntry.callback.AddListener((data) => { OnClick((PointerEventData)data); });
                    evtTrig.triggers.Add(clickEntry);
                    action = value;
                }
            }

            public string Subtitle { get; set; }

            public string Title { get; set; }

            public Sprite Icon { get; set; }

            public AudioClip SFX { get; set; }

            public MenuItem()
            {
                EventTrigger evtTrig = returnedMenuItem.GetComponent<EventTrigger>();
                EventTrigger.Entry hoverEntry = new EventTrigger.Entry();
                EventTrigger.Entry exitEntry = new EventTrigger.Entry();

                hoverEntry.eventID = EventTriggerType.PointerEnter;
                exitEntry.eventID = EventTriggerType.PointerExit;

                hoverEntry.callback.AddListener((data) => { OnMouseEnter((PointerEventData)data, Subtitle); });
                exitEntry.callback.AddListener((data) => { OnMouseExit(); });
                evtTrig.triggers.Add(exitEntry);
                evtTrig.triggers.Add(hoverEntry);
            }

            void OnClick(PointerEventData data)
            {
                // When clicked (SFX, the other one is declared up there and it can be the action passed at construction time)
                returnedMenuItem.GetComponent<AudioSource>().clip = SFX;
                returnedMenuItem.GetComponent<AudioSource>().Play();
            }

            void OnMouseEnter(PointerEventData data, string Subt)
            {
                // Mouse hover (Animation and subtitle)
                subc.ChangeText(Subt);
                returnedMenuItem.GetComponent<AudioSource>().clip = Assets.SoundEffects.Hover;
                returnedMenuItem.GetComponent<AudioSource>().Play();
                returnedMenuItem.GetComponent<Animator>().Play("menuHover");
            }

            void OnMouseExit()
            {
                // Mouse Exit (Fade subtitle out)
                returnedMenuItem.GetComponent<Animator>().Play("menuOut");
                subc.FadeTextOut();
            }

            public void Show(Transform tr)
            {
                /* attach this menuitem to a transform that has the required components to be scrollable and stuff
                 * the model one is in the scene called mainMenu
                 * ----
                 * apparently, attributes like Sprite and Text aren't added when a instance is just called
                 * however, addcallback does. huh
                */
                returnedMenuItem.transform.Find("MenuBG").gameObject.transform.Find("Image").GetComponent<Image>().sprite = Icon;
                returnedMenuItem.transform.Find("MenuBG").gameObject.transform.Find("Text").GetComponent<Text>().text = Title;
                returnedMenuItem.SetActive(true);
                returnedMenuItem.transform.SetParent(tr, false);
                returnedMenuItem.GetComponent<Animator>().Play("menuComeIn");
            }

            public void Kill()
            {
                //Own method to destroy the gameobject we just made for the menu as we no longer need it.
                returnedMenuItem.GetComponent<Animator>().Play("menuHide");
                EventTrigger evtTrig = returnedMenuItem.GetComponent<EventTrigger>();
                Object.Destroy(evtTrig, 0);
                // all UI SFX should be no more than 1.5s.
                Object.Destroy(returnedMenuItem, 1.5f);
            }

        }

        /// <summary>
        /// Class containing everything related to Dialogs.
        /// <param name="Title">The title of the dialog</param>
        /// </summary>
        /// Man, this is probably the worst thing I've done in a LONG while
        public class Dialog
        {
            public static GameObject DialogPrefab = Assets.Objects.DialogObject.gameObject;
            GameObject returnedDialog = Object.Instantiate(DialogPrefab);
            string _title;
            string _body;
            GameObject buttonL;
            GameObject buttonM;
            GameObject buttonR;
            AudioClip _dialogSFX;
            AudioClip _buttonLSFX;
            AudioClip buttonMSFX;
            AudioClip buttonRSFX;
            public Button buttonActionR;
            public Button buttonActionM;
            public Button buttonActionL;
            public GameObject icon;

            public Dialog(string Title, string Body, string ButtonText, UnityAction Action, AudioClip ButtonSFX, Sprite Icon = null, AudioClip DialogSFX = null)
            {
                /*
                 * nevermind it was still ugly af
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
                    //dialogSFX = Assets.SoundEffects.DialogDefault;
                }
                else
                {
                    //dialogSFX = DialogSFX;
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
                // the legend
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

    namespace Actions
    {
        public class TransitionActions : MonoBehaviour {

            public GameObject TransitionController;

            public void StartSceneTransition(string scenepath, Sprite mask)
            {
                StartCoroutine(GotoScene(scenepath, mask));
            }

            public void TransitionFace(Sprite face, bool ComingIn)
            {
                TransitionController.GetComponent<TransitionControl>().PlayTransition(face, ComingIn);
            }

            public IEnumerator GotoScene(string scenetgt, Sprite face)
            {
                TransitionFace(face, false);
                yield return new WaitForSeconds(TransitionController.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length /* + TransitionController.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime */);
                AsyncOperation IsSceneLoaded = SceneManager.LoadSceneAsync(scenetgt, LoadSceneMode.Additive);
                Scene currscene = SceneManager.GetActiveScene();
                Debug.Log("Loading scene: " + scenetgt);
                while (!IsSceneLoaded.isDone)
                {
                    yield return null;
                }
                IsSceneLoaded.allowSceneActivation = true;
                TransitionController.transform.SetParent(null);
                Scene scr = SceneManager.GetSceneByName(scenetgt);
                SceneManager.MoveGameObjectToScene(TransitionController, scr);
                SceneManager.SetActiveScene(scr);
                SceneManager.UnloadSceneAsync(currscene);
                //TransitionController.transform.SetParent(GameObject.FindGameObjectWithTag("UICanvas").transform);
                Debug.Log("Scene " + scenetgt + " loaded.");
                TransitionController.GetComponent<TransitionControl>().PlayTransition(face, true);   
            }

        }
    }

}

