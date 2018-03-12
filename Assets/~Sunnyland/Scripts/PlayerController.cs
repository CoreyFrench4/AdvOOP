/*=============================================
-----------------------------------
Copyright (c) 2018 Corey French
-----------------------------------
@file: FollowTarget.cs
@date: 12/03/2018
@author: Corey French
@brief: Script to control the Player
===============================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunnyLand.Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5f;
        public float maxVelocity = 2f;
        public float rayDistance = .5f;
        public float jumpHeight = 2f;
        public int maxJumpCount = 2;
        public LayerMask groundLayer;

        private Vector3 moveDirection;
        private int currentJump = 0;

        // References
        private SpriteRenderer rend;
        private Animator anim;
        private Rigidbody2D rigid;

        #region Unity Functions
        void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
        }
        void Update()
        {

        }
        void FixedUpdate()
        {
            // Feel for the ground
            DetectGround();
        }
        void OnDrawGizmos()
        {
            Ray groundRay = new Ray(transform.position, Vector3.down);
            Gizmos.DrawLine(groundRay.origin, groundRay.origin + groundRay.direction * rayDistance);
        }
    
        #endregion

        #region Custom Functions 
        void DetectGround()
        {
            Ray ray = new Physics2D.Raycast();

        }
        void LimitVelocity()
        {

        }

        public void Jump()
        {

        }
        public void Climb()
        {

        }
        public void Move(float horizontal)
        {

        }
        #endregion
    }
}