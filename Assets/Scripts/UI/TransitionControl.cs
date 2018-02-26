using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionControl : MonoBehaviour
{

    Animator ac;
    Image maskimage;

    // Use this for initialization
    void Start()
    {
        ac = GetComponent<Animator>();
        maskimage = transform.Find("Mask").GetComponent<Image>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayTransition(Sprite face, bool comingIn)
    {
        maskimage.sprite = face;
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
