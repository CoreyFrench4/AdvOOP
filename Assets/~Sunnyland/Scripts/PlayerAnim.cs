using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunnyLand
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerAnim : MonoBehaviour
    {
        private PlayerController player;
        private Animator anim;

        #region Unity Functions
        // Use this for initialization
        void Awake()
        {

        }
        void OnEnable()
        {
            print("Enabled");
        }
        void OnDisable()
        {
            print("Disabled");
        }
        void Start()
        {
            anim = GetComponent<Animator>();
            player = GetComponent<PlayerController>();
            player.onGroundedChanged += OnGroundedChanged;
        }
        #endregion

        #region Custom Functions
        void OnGroundedChanged(bool isGrounded)
        {
            if (isGrounded)
            {
                print("I'm grounded :(");
            }
            else
            {
                print("I'm not grounded! :)");
            }
        }
        #endregion
    }
}