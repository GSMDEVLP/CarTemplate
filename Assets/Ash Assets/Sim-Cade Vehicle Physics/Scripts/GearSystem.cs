using System.Collections;
using UnityEngine;
using TMPro;

namespace Ashsvp
{
    public class GearSystem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _gearText;
        public float VehicleSpeed;
        public int currentGear;
        private SimcadeVehicleController vehicleController;
        public int[] gearSpeeds = new int[] { 40, 80, 120, 160, 220 };

        public AudioSystem AudioSystem;

        private float currentGearTemp;
        private bool _newGear = true;
        void Start()
        {
            vehicleController = GetComponent<SimcadeVehicleController>();
            currentGear = 1;
            _gearText.text = currentGear.ToString();
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
            int targetGear = currentGear; // Целевая передача

            // Определяем целевую передачу на основе скорости
            for (int i = 0; i < gearSpeeds.Length; i++)
            {
                if (VehicleSpeed > gearSpeeds[i])
                {
                    targetGear = i + 1; // Увеличиваем целевую передачу
                }
                else break;
            }

            currentGearTemp = Mathf.Lerp(currentGearTemp, targetGear, Time.deltaTime / 0.1f);


            if (Mathf.Abs(currentGearTemp - targetGear) < 0.01f)
            {
                currentGear = Mathf.RoundToInt(currentGearTemp);

                // Обновляем текст только при изменении передачи
                if (CurrentGearProperty != currentGear)
                {
                    CurrentGearProperty = currentGear; // Обновляем свойство передачи
                    _gearText.text = CurrentGearProperty.ToString(); // Обновляем текст
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
