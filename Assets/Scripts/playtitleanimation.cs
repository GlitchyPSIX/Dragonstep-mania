using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playtitleanimation : MonoBehaviour {



	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Step")){
			GetComponent<Animator>().Play("Draw Out");
		}
		//Visual Studio Code acted super dense for me today.
	}
}
