using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                tc.PlayTransition(0, true);
            }
            else if (Input.GetKey("w"))
            {
                tc.PlayTransition(1, true);
            }
            else if (Input.GetKey("e"))
            {
                tc.PlayTransition(2, true);
            }
            else if (Input.GetKey("r"))
            {
                tc.PlayTransition(3, true);
            }
            else if (Input.GetKey("t"))
            {
                tc.PlayTransition(4, true);
            }
        }
        else if (Input.GetKey("s"))
        {
            if (Input.GetKey("q"))
            {
                tc.PlayTransition(0, false);
            }
            else if (Input.GetKey("w"))
            {
                tc.PlayTransition(1, false);
            }
            else if (Input.GetKey("e"))
            {
                tc.PlayTransition(2, false);
            }
            else if (Input.GetKey("r"))
            {
                tc.PlayTransition(3, false);
            }
            else if (Input.GetKey("t"))
            {
                tc.PlayTransition(4, false);
            }
        }
	}
}
