using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ashsvp
{
    public class ResetVehicle : MonoBehaviour
    {
        public void resetVehicle()
        {
            var pos = transform.position;
            pos.y += 1;
            transform.position = pos;
            transform.rotation = Quaternion.identity;
        }


    }
}
