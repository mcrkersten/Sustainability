using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatLightFeedback : MonoBehaviour
{
    [SerializeField]
    private Renderer[] lightRend;
    public bool[] lightSwitch;
    private Ship ship;

    // Start is called before the first frame update
    void Start() {

        Ship.OnPersonEnter += SwitchLight;
        Ship.OnTurnSeatLightsOff += TurnSeatLightsOff;


        for (var i = 0; i < lightSwitch.Length; i++) {
            SwitchLight(i, lightSwitch[i]);
        }
    }

    private void OnValidate() {
        //Adjust bool[] to size
        if (!Application.isPlaying) {
            bool[] copy = lightSwitch;
            lightSwitch = new bool[lightRend.Length];
            for (var i = 0; i < lightSwitch.Length; i++) {
                if (i < copy.Length)
                    lightSwitch[i] = copy[i];
            }
        }
    }

    public void SwitchLight(int light, bool onOff) {
        if (light < lightSwitch.Length) {
            if (onOff)
                lightRend[light].material.EnableKeyword("_EMISSION");
            else
                lightRend[light].material.DisableKeyword("_EMISSION");

        }
    }

    public void TurnSeatLightsOff() {
        for(int i = 0; i < lightSwitch.Length; i++) {
            lightRend[i].material.DisableKeyword("_EMISSION");
            lightSwitch[i] = false;
        }
    }
}
