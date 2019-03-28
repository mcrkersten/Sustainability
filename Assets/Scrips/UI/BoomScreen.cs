using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BoomScreen : MonoBehaviour
{
    public Animator animat;
    public TextMeshProUGUI textstuff;
    bool started;
    [TextArea(2, 10)]
    public string text;
    public GameObject button;

    // Start is called before the first frame update
    private void Update() {
        if (animat.GetCurrentAnimatorStateInfo(0).IsName("boom") &&
            animat.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            if (!started) {
                StartCoroutine(ShowText(text));
                print("go");
                button.SetActive(true);
                started = true;
            }

        }
    }

    IEnumerator ShowText(string text) {
        string currentText = "";
        for (int i = 0; i <= text.Length; i++) {
            currentText = text.Substring(0, i);
            textstuff.text = currentText;
            yield return new WaitForEndOfFrame();
        }
    }
}
