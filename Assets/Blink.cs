using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour {
    public Image m;
    public Image x;
    private Color c;
    private float a;
    private bool goingUp;
    float height;

    private void Start() {

        height = Random.Range(0.05f, .1f);
        c = m.color;
        goingUp = true;
    }
    private void Update() {
        c.a = goingUp ? .005f + c.a : c.a - .005f;

        if (c.a >= height)
            goingUp = false;
        else if (c.a <= 00) {
            height = Random.Range(0.0f, .1f);
            goingUp = true;
        }
        m.color = c;
        x.color = c;
    }
}
