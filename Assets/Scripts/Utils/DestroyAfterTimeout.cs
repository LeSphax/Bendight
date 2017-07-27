using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimeout : MonoBehaviour {

    public float time;

    private void Start()
    {
        Invoke("Destroy", time);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
