using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSpriteShadows : MonoBehaviour {

public bool CastShadows;
public bool ReceiveShadows;
public SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<SpriteRenderer>();
		renderer.castShadows = CastShadows;
	}

}
