using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunnyLand
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnim : MonoBehaviour
    {
        private PlayerController player;

        // Use this for initialization
        void Start()
        {
            player = GetComponent<PlayerController>();

        }

        void OnGroundedChanged(bool isGrounded)
        {

        }
    }
}