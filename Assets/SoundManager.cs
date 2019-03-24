using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

namespace FMODUnity {
    public class SoundManager : MonoBehaviour {
        public Rigidbody rb;
        private float speed;
        private bool warningWentOff;
        private bool isInNight;

        float timer = 2;
        float curTimer;

        [Header("Events")]
        [FMODUnity.EventRef]
        public string eventBackgroundMusic;

        [FMODUnity.EventRef]
        public string eventFuelWarning;

        [FMODUnity.EventRef]
        public string eventShipEngine;

        //Overworld
        public FMOD.Studio.EventInstance backgroundMusic;
        public FMOD.Studio.ParameterInstance pBackgroundMusic;

        //FuelWarning
        public FMOD.Studio.EventInstance fuelWarning;

        //ShipEngine
        public FMOD.Studio.EventInstance shipEngine;
        public FMOD.Studio.ParameterInstance pShipEngine;

        private void Start() {
            backgroundMusic = RuntimeManager.CreateInstance(eventBackgroundMusic);
            fuelWarning = RuntimeManager.CreateInstance(eventFuelWarning);
            shipEngine = RuntimeManager.CreateInstance(eventShipEngine);

            StartEngine();
            StartMusic();
        }

        private void Update() {
            UpdateEngineSound();
            if(Ship.Instance.currentFuel < 25 && !warningWentOff) {
                FuelWarning();
                curTimer = timer;
                warningWentOff = true;
            }
            if (warningWentOff) {
                curTimer -= Time.deltaTime;
                if (curTimer < 0) {
                    warningWentOff = false;
                    curTimer = timer;
                }
            }
        }

        private void StartMusic() {
            backgroundMusic.getParameter("level of night", out pBackgroundMusic);
            backgroundMusic.setVolume(.2f);
            backgroundMusic.start();
        }

        private void StartEngine() {
            shipEngine.getParameter("Speed", out pShipEngine);
            shipEngine.start();
        }

        private void UpdateEngineSound() {
            speed = rb.velocity.magnitude / 10;
            pShipEngine.setValue(speed);
        }

        private void FuelWarning() {
            fuelWarning.start();
        }

        private void OnTriggerEnter(Collider other) {
            print("Col");
            if(other.CompareTag("MusicTrigger")) {
                if (!isInNight) {
                    pBackgroundMusic.setValue(0);
                    isInNight = true;
                }
                else {
                    pBackgroundMusic.setValue(1);
                    isInNight = false;
                }
            }
        }
    }
}
