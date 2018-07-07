using System.Collections;
using UnityEngine;

public class MasterMono : MonoBehaviour {

    public void StartChildCoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }
}
