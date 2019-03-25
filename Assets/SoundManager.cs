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
        private float valueBackgroundParameter = 0;

        float timer = 2;
        float curTimer;

        [Header("EventsMusic")]
        [FMODUnity.EventRef]
        public string eventBackgroundMusic;

        [FMODUnity.EventRef]
        public string eventFloraSoundtrack;

        [FMODUnity.EventRef]
        public string eventTheCapitalSoundtrack;

        [FMODUnity.EventRef]
        public string eventLumenSountrack;


        [Header("EventsSFX")]
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

        //City Music
        public FMOD.Studio.EventInstance theCapitalSoundtrack;
        public FMOD.Studio.EventInstance floraSoundtrack;
        public FMOD.Studio.EventInstance lumenSoundtrack;

        private void Start() {
            //Music
            backgroundMusic = RuntimeManager.CreateInstance(eventBackgroundMusic);
            theCapitalSoundtrack = RuntimeManager.CreateInstance(eventTheCapitalSoundtrack);
            lumenSoundtrack = RuntimeManager.CreateInstance(eventLumenSountrack);
            floraSoundtrack = RuntimeManager.CreateInstance(eventFloraSoundtrack);

            //SFX
            fuelWarning = RuntimeManager.CreateInstance(eventFuelWarning);
            shipEngine = RuntimeManager.CreateInstance(eventShipEngine);

            StartEngine();
            StartMusic();
        }

        private void Update() {
            UpdateEngineSound();
            pBackgroundMusic.setValue(valueBackgroundParameter);
            if (Ship.Instance.currentFuel < 25 && !warningWentOff) {
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
            backgroundMusic.start();

            //CityMusic;
            lumenSoundtrack.setVolume(0);
            theCapitalSoundtrack.setVolume(0);
            floraSoundtrack.setVolume(0);

            lumenSoundtrack.start();
            theCapitalSoundtrack.start();
            floraSoundtrack.start();

            lumenSoundtrack.setPaused(true);
            theCapitalSoundtrack.setPaused(true);
            floraSoundtrack.setPaused(true);
        }

        private void StartEngine() {
            shipEngine.getParameter("Speed", out pShipEngine);
            //  shipEngine.start();
        }

        private void UpdateEngineSound() {
            speed = rb.velocity.magnitude / 10;
            pShipEngine.setValue(speed);
        }

        private void FuelWarning() {
            fuelWarning.start();
        }

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("MusicTrigger")) {
                if (!isInNight) {
                    StartCoroutine(FadeToNight());
                    isInNight = true;
                }
                else {
                    StartCoroutine(FadeToDay());
                    isInNight = false;
                }
            }

            if (other.CompareTag("City"))
            {
                switch (other.gameObject.layer)
                {
                    //Capital
                    case 9:
                        StartCoroutine(VolumeDown(backgroundMusic));
                        StartCoroutine(VolumeUp(theCapitalSoundtrack));
                        break;
                    //FLora
                    case 10:
                        StartCoroutine(VolumeDown(backgroundMusic));
                        StartCoroutine(VolumeUp(floraSoundtrack));
                        break;
                    //Lumen
                    case 11:
                        StartCoroutine(VolumeDown(backgroundMusic));
                        StartCoroutine(VolumeUp(lumenSoundtrack));
                        break;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("City"))
            {
                switch (other.gameObject.layer)
                {
                    //Capital
                    case 9:
                        StartCoroutine(VolumeUp(backgroundMusic));
                        StartCoroutine(VolumeDown(theCapitalSoundtrack));
                        break;
                    //FLora
                    case 10:
                        StartCoroutine(VolumeUp(backgroundMusic));
                        StartCoroutine(VolumeDown(floraSoundtrack));
                        break;
                    //Lumen
                    case 11:
                        StartCoroutine(VolumeUp(backgroundMusic));
                        StartCoroutine(VolumeDown(lumenSoundtrack));
                        break;
                }
            }
        }

        IEnumerator FadeToDay()
        {
            for (float f = 2f; f >= 0; f -= 0.005f)
            {
                valueBackgroundParameter = f;
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator FadeToNight()
        {
            for (float f = 0f; f <= 2; f += 0.005f)
            {
                valueBackgroundParameter = f;
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator VolumeDown(FMOD.Studio.EventInstance music)
        {
            for (float f = 1f; f >= 0; f -= 0.005f)
            {
                music.setVolume(f);
                yield return new WaitForEndOfFrame();
            }
            music.setPaused(true);
        }

        IEnumerator VolumeUp(FMOD.Studio.EventInstance music)
        {
            music.setPaused(false);
            for (float f = 0f; f <= 1; f += 0.005f)
            {
                music.setVolume(f);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
