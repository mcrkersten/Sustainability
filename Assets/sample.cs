using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

namespace FMODUnity
{
    public class sample : MonoBehaviour
    {
        public Rigidbody rigidbody;
        float speed = 0;
        [FMODUnity.EventRef]
        public string eventTemp = "event:/Overworld music";
        public float curve;
        public FMODUnity.StudioEventEmitter emitter;
        public FMOD.Studio.EventInstance test;
        public FMOD.Studio.ParameterInstance pTest;

        void Start()
        {
            test = FMODUnity.RuntimeManager.CreateInstance(eventTemp);
            test.getParameter("Speed", out pTest);
            test.start();
        }

        private void Update() {
            speed = rigidbody.velocity.magnitude /10;
            pTest.setValue(speed);
        }
    }
}
