using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

namespace FMODUnity
{
    public class sample : MonoBehaviour
    {
        float valueThing = 0;
        [FMODUnity.EventRef]
        public string eventTemp = "event:/Overworld music";
        public float curve;
        public FMODUnity.StudioEventEmitter emitter;
        public FMOD.Studio.EventInstance test;
        public FMOD.Studio.ParameterInstance pTest;

        void Start()
        {
            test = FMODUnity.RuntimeManager.CreateInstance(eventTemp);
            test.getParameter("level of night", out pTest);
            test.start();
        }

        void OnTriggerStay(Collider other)
        {
            print("weHaveEntered");
            valueThing += .002f;
            print(test.isValid());
            pTest.setValue(valueThing);
            emitter.SetParameter("level of night", valueThing);

        }
    }
}
