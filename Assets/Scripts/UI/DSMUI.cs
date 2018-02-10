using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DSMUI
{
    namespace Assets { 
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
            public static AudioClip Select = Resources.Load<AudioClip>("Sounds/SFX/UI/selection");
            public static AudioClip Back = Resources.Load<AudioClip>("Sounds/SFX/UI/back");
        }

    }

}