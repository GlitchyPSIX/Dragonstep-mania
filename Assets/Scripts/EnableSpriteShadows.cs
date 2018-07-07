using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnableSpriteShadows : MonoBehaviour {

public bool CastShadows;
public bool ReceiveShadows;
SpriteRenderer srenderer;

	// Use this for initialization
	void Start () {
		srenderer = GetComponent<SpriteRenderer>();
        srenderer.shadowCastingMode = ShadowCastingMode.On;
    }

}
