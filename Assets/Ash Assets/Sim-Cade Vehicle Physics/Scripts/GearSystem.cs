using System.Collections;
using UnityEngine;
using TMPro;
using System;

namespace Ashsvp
{
    public class GearSystem : MonoBehaviour
    {
        [SerializeField] private SimcadeVehicleController vehicleController;

        public float VehicleSpeed;
        public int currentGear;
        public int[] gearSpeeds = new int[] { 40, 80, 120, 160, 220 };

        public AudioSystem AudioSystem;
        public event Action<int> OnGearChanged;

        private float currentGearTemp;
        void Start()
        {
            currentGear = 1;
        }

        void Update()
        {
            float velocityMag = Vector3.ProjectOnPlane( vehicleController.localVehicleVelocity, transform.up).magnitude;
            if (vehicleController.vehicleIsGrounded)
            {
                velocityMag = vehicleController.localVehicleVelocity.magnitude;
            }

            VehicleSpeed = Mathf.RoundToInt(velocityMag * 3.6f); //car speed in Km/hr
            gearShift();

        }


        void gearShift()
        {
            int targetGear = currentGear; 

            for (int i = 0; i < gearSpeeds.Length; i++)
            {
                if (VehicleSpeed > gearSpeeds[i])
                {
                    targetGear = i + 1; 
                }
                else break;
            }

            currentGearTemp = Mathf.Lerp(currentGearTemp, targetGear, Time.deltaTime / 0.1f);


            if (Mathf.Abs(currentGearTemp - targetGear) < 0.45f)
            {
                currentGear = Mathf.RoundToInt(currentGearTemp);

                if (CurrentGearProperty != currentGear)
                {
                    CurrentGearProperty = currentGear;
                    OnGearChanged?.Invoke(currentGear);
                }
            }
        }

        public float CurrentGearProperty
        {
            get
            {                
                return currentGearTemp;
            }

            set
            {
                currentGearTemp = value;

                if (vehicleController.accelerationInput > 0 && vehicleController.localVehicleVelocity.z > 0 && !AudioSystem.GearSound.isPlaying && vehicleController.vehicleIsGrounded)
                {
                    vehicleController.VehicleEvents.OnGearChange.Invoke();
                    AudioSystem.GearSound.Play();
                    StartCoroutine(shiftingGear());
                }

                AudioSystem.engineSound.volume = 0.5f;
            }
        }

        IEnumerator shiftingGear()
        {
            vehicleController.CanAccelerate = false;
            yield return new WaitForSeconds(0.3f);
            vehicleController.CanAccelerate = true;
        }

    }
}
