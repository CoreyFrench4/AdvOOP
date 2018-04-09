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

namespace SunnyLand
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public int health = 100;
        public int damage = 50;
        [Header("Movement")]
        public float speed = 5f;
        public float maxVelocity = 2f;
        [Header("Grounding")]
        public float rayDistance = .5f;
        public bool isGrounded = false;
        public float maxSlopeAngle = 45f;
        [Header("Crouch")]
        public bool isCrouching = false;
        [Header("Jump")]
        public float jumpHeight = 2f;
        public int maxJumpCount = 2;
        public bool isJumping = false;
        [Header("Climb")]
        public float climbSpeed = 5f;
        public bool isClimbing = false;
        public bool isOnSlope = false;
        [Header("References")]
        public Collider2D defaultCollider;
        public Collider2D crouchCollider;

        // Delegates
        #region Delegates
        public EventCallback onJump;
        public EventCallback onHurt;
        public BoolCallback onCrouchChanged;
        public BoolCallback onGroundedChanged;
        public BoolCallback onSlopeChanged;
        public BoolCallback onClimbChanged;
        public FloatCallback onMove;
        public FloatCallback onClimb;
        #endregion

        private Vector3 groundNormal = Vector3.up;
        private Vector3 moveDirection;
        private int currentJump = 0;

        private float inputH, inputV;

        private float vertical, horizontal;

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
            // Drawing the 'right' ray
            Vector3 right = Vector3.Cross(groundNormal, Vector3.forward);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position - right,
                            transform.position + right);
        }

        #endregion

        #region Custom Functions
        void PerformMove()
        {
            if (isOnSlope && // If the player is standing on a slope AND
               inputH == 0 && // No input is on horizontal AND
               isGrounded) // Player is grounded
            {
                // Cancel the velocity
                rigid.velocity = Vector3.zero;
            }

            Vector3 right = Vector3.Cross(groundNormal, Vector3.back);
            rigid.AddForce(right * inputH * speed);

            // Limit the velocity max velocity
            LimitVelocity();
        }
        void PerformJump()
        {
            // If player is jumping
            if (isJumping)
            {
                // If player is allowed to jump
                if (currentJump < maxJumpCount)
                {
                    // Increase the jump count
                    currentJump++;
                    // Perform jump logic
                    rigid.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                }
                // Reset jump input
                isJumping = false;
            }
        }
        bool CheckSlope(RaycastHit2D hit)
        {
            // Grab the angle in degrees of the surface we're standing on
            float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            // If the angle is greater than max
            if (slopeAngle >= maxSlopeAngle)
            {
                // Make player slide down surface
                rigid.AddForce(Physics.gravity);
            }
            if (slopeAngle > 0 && slopeAngle < maxSlopeAngle)
            {
                return true;
            }
            return false;
        }
        bool CheckGround(RaycastHit2D hit)
        {
            // Check if:
            if (hit.collider != null && // It hit something AND
               hit.collider.name != name && // It didn't hit me AND 
               !hit.collider.isTrigger) // It didnt hit a trigger
            {
                // Reset the jump count
                currentJump = 0;
                // Is grounded!
                isGrounded = true;
                // Set ground normal now that we're grounded
                groundNormal = -hit.normal;
                // Record 'isOnSlope' value 
                bool wasOnSlope = isOnSlope;
                // Check if we're on a slope!
                isOnSlope = CheckSlope(hit);
                // Has the 'isOnSlope' value changed?
                if (wasOnSlope != isOnSlope)
                {
                    // Invoke event
                    if (onSlopeChanged != null)
                    {
                        onSlopeChanged.Invoke(isOnSlope);
                    }
                }
                // We have found our ground so exit the function
                // (No need to check any more hits)
                return true;
            }
            else
            {
                // We are no longer grounded
                isGrounded = false;

            }

            // Haven't found the ground (so keep looking)
            return false;
        }
        void DetectGround()
        {
            // Record a copy of what isGrounded was
            bool wasGrounded = isGrounded; 

            // Create a ray going down
            Ray groundRay = new Ray(transform.position, Vector3.down);
            // Set Hit to 2D Raycast
            RaycastHit2D[] hits = Physics2D.RaycastAll(groundRay.origin, groundRay.direction, rayDistance);
            foreach (var hit in hits)
            {
                if (CheckGround(hit))
                {
                    // We found the ground! So exit the function
                    break;
                }

                // If hit collider is not null
                // Reset currentJump
            }
            // Check if:
            if (wasGrounded != isGrounded && // IsGrounded has changed since before the detection AND
                onGroundedChanged != null) // Something is subscribed to this event
            {
                // Run all the things subscribed to event and give it "isGrounded" value
                onGroundedChanged.Invoke(isGrounded);
            }

        }
        void DetectClimbable()
        {

        }

        void LimitVelocity()
    {
            // If Rigid's velocity (magnitude) is greater than maxVelocity
            if (rigid.velocity.magnitude > maxVelocity)
            {
                // Set Rigid velocity to velocity normalized x maxVelocity
                rigid.velocity = rigid.velocity.normalized * maxVelocity;
            }
        }
        void UpdateCollider()
        {

        }

        public void Jump()
        {
            isJumping = true;

            if (onJump != null)
            {
                onJump.Invoke();
            }
        }
        public void Move(float horizontal)
        {
            if (horizontal != 0)
            {
                rend.flipX = horizontal < 0;
            }

            inputH = horizontal;

            // Invoke event
            if (onMove != null)
            {
                onMove.Invoke(inputH);
            }
        }
        public void Climb(float vertical)
        {
            // CHALLENGE
        }
        public void Crouch()
        {
            isCrouching = true;
            // Invoke event
            if (onCrouchChanged != null)
            {
                onCrouchChanged.Invoke(isCrouching);
            }
        }
        public void UnCrouch()
        {

        }
        public void Hurt(int damage, Vector2? hitNormal = null)
        {
            // Set a default hit direction
            Vector2 force = Vector2.up;
            if (hitNormal != null) // If a hitNormal exists
            {
                // Use the hit normal as a direction
                force = hitNormal.Value;
            }

            health -= damage;

            // Add force in the hit direction
            rigid.AddForce(force * damage, ForceMode2D.Impulse);
            
            // Invoke event
            if (onHurt != null)
            {
                // Play hurt sound or animation
                onHurt.Invoke();
            }
        }
        #endregion
    }
}