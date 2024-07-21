﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace flashlight
{
    public class FlashlightToggleAssetPack : MonoBehaviour
    {
        public GameObject lightGO; //light gameObject to work with
        public bool isOn = true; //is flashlight on or off?

        public Light outer;
        public Light mid;
        public Light inner;

        public float adjustmentFactor;

        // Use this for initialization
        void Start()
        {
            //set default off
            lightGO.SetActive(isOn);
        }

        // Update is called once per frame
        void Update()
        {
            //toggle flashlight on key down
            if (Input.GetKeyDown(KeyCode.F))
            {
                //toggle light
                isOn = !isOn;
                //turn light on
                if (isOn)
                {
                    lightGO.SetActive(true);
                }
                //turn light off
                else
                {
                    lightGO.SetActive(false);

                }
            }
        }

        public void IncreaseIntensity()
        {
            outer.intensity += adjustmentFactor;
            mid.intensity += adjustmentFactor;
            inner.intensity += adjustmentFactor;
        }

        public void DecreaseIntensity()
        {
            outer.intensity -= adjustmentFactor;
            mid.intensity -= adjustmentFactor;
            inner.intensity -= adjustmentFactor;
        }
    }
}
