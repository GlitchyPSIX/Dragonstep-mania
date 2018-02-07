using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TransitionControl : MonoBehaviour
{

    Sprite[] transitionFaces;
    Animator ac;
    Image maskimage;

    // Use this for initialization
    void Start()
    {
        transitionFaces = Resources.LoadAll<Sprite>("Sprites/UI/TransitionMasks");
        ac = GetComponent<Animator>();
        maskimage = transform.Find("Mask").GetComponent<Image>();
    }

    public void PlayTransition(int face, bool comingIn)
    {
        maskimage.sprite = transitionFaces[face];
        if (comingIn == true)
        {
            ac.Play("transitionIn");
        }
        else
        {
            ac.Play("transitionOut");
        }
    }
}
