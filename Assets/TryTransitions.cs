using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSMUI.Assets;

public class TryTransitions : MonoBehaviour {
    TransitionControl tc;
	// Use this for initialization
	void Start () {
        tc = GetComponent<TransitionControl>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("a"))
        {
            if (Input.GetKey("q"))
            {
                tc.PlayTransition(Transitions.EvilGrin, true);
            }
            else if (Input.GetKey("w"))
            {
                tc.PlayTransition(Transitions.Fancy, true);
            }
            else if (Input.GetKey("e"))
            {
                tc.PlayTransition(Transitions.Normal, true);
            }
            else if (Input.GetKey("r"))
            {
                tc.PlayTransition(Transitions.Miss, true);
            }
            else if (Input.GetKey("t"))
            {
                tc.PlayTransition(Transitions.Wink, true);
            }
        }
        else if (Input.GetKey("s"))
        {
            if (Input.GetKey("q"))
            {
                tc.PlayTransition(Transitions.EvilGrin, false);
            }
            else if (Input.GetKey("w"))
            {
                tc.PlayTransition(Transitions.Fancy, false);
            }
            else if (Input.GetKey("e"))
            {
                tc.PlayTransition(Transitions.Normal, false);
            }
            else if (Input.GetKey("r"))
            {
                tc.PlayTransition(Transitions.Miss, false);
            }
            else if (Input.GetKey("t"))
            {
                tc.PlayTransition(Transitions.Wink, false);
            }
        }
	}
}
