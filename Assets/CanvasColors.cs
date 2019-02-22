using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasColors : MonoBehaviour
{
    public Text[] textColor;
    public Image[] imageColor;
    public Color[] colors;

    private void Start() {
        for (int i = 0; i < textColor.Length; i++) {
            textColor[i].color = colors[1];
        }

        for (int i = 0; i < imageColor.Length; i++) {
            imageColor[i].color = colors[1];
        }
    }
}


