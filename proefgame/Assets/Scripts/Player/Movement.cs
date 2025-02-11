using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.InputSystem;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// This version includes a dash mechanic and works without animations.
    /// </summary>
    public class Movement : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public AudioClip dashAudio; // Audio clip for dash

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        /// <summary>
        /// Dash speed multiplier.
        /// </summary>
        public float dashSpeed = 14;

        /// <summary>
        /// Duration of the dash in seconds.
        /// </summary>
        public float dashDuration = 0.2f;

        /// <summary>
        /// Cooldown time for the dash in seconds.
        /// </summary>
        public float dashCooldown = 1f;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        public Collider2D collider2d;
        public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        bool isDashing;
        float dashEndTime;
        float dashCooldownEndTime;

        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private InputAction m_MoveAction;
        private InputAction m_JumpAction;
        private InputAction m_DashAction;

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();

            m_MoveAction = InputSystem.actions.FindAction("Player/Move");
            m_JumpAction = InputSystem.actions.FindAction("Player/Jump");
            m_DashAction = InputSystem.actions.FindAction("Player/Dash"); // Dash action

            m_MoveAction.Enable();
            m_JumpAction.Enable();
            m_DashAction.Enable();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = m_MoveAction.ReadValue<Vector2>().x;

                if (jumpState == JumpState.Grounded && m_JumpAction.WasPressedThisFrame())
                    jumpState = JumpState.PrepareToJump;
                else if (m_JumpAction.WasReleasedThisFrame())
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().movement = this;
                }

                // Check for dash input
                if (m_DashAction.WasPressedThisFrame() && Time.time >= dashCooldownEndTime)
                {
                    StartDash();
                }
            }
            else
            {
                move.x = 0;
            }

            UpdateJumpState();
            UpdateDashState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().movement = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().movement = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        void StartDash()
        {
            if (!isDashing)
            {
                isDashing = true;
                dashEndTime = Time.time + dashDuration;
                dashCooldownEndTime = Time.time + dashCooldown;

                // Play dash audio if available
                if (dashAudio != null)
                {
                    audioSource.PlayOneShot(dashAudio);
                }
            }
        }

        void UpdateDashState()
        {
            if (isDashing && Time.time >= dashEndTime)
            {
                isDashing = false;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            // Apply dash velocity if dashing
            if (isDashing)
            {
                targetVelocity = new Vector2(move.x * dashSpeed, velocity.y);
            }
            else
            {
                targetVelocity = move * maxSpeed;
            }
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}