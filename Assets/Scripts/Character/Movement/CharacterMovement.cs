using System;
using System.Collections;
using UnityEngine;

namespace Character.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        public bool isCanMove = false;
        public bool isMine = false;
        [Header("Character's setting")]
        public float speed = 5.0f;
        public float swipeThreshold = 50.0f;
        public float holdTimeThreshold = 0.5f;
        [Header("Character's components")]
        public Animator characterAnimator;
        public Rigidbody characterRigidbody;
        
        private Vector2 _swipeStartPosition;
        private float _holdStartTime;
        private bool _isHolding = false;
        private bool _isSwiping = false;

        // Animation key
        protected static readonly int Speed = Animator.StringToHash("Speed");
        
        // Private var
        protected Vector3 MovementDirection = Vector3.zero;

        public void Update()
        {
            if (isMine)
                ControlMine();
            else
                ControlAi();
        }

        private void FixedUpdate()
        {
            if (!isCanMove) return;
            if (!(MovementDirection.sqrMagnitude > 0.0f)) return;
            
            var moveSpeed = MovementDirection * speed * Time.deltaTime;
            var angle = Mathf.Atan2(MovementDirection.x, MovementDirection.z) * Mathf.Rad2Deg;
            
            characterAnimator.SetFloat(Speed, moveSpeed.magnitude);
            
            characterRigidbody.MovePosition(characterRigidbody.position + moveSpeed);
            characterRigidbody.MoveRotation(Quaternion.Euler(0, angle, 0));
        }

        protected void ControlMine()
        {
            if (Input.touchCount <= 0) return;
            
            var touch = Input.GetTouch(0);
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _swipeStartPosition = touch.position;
                    _holdStartTime = Time.time;
                    _isHolding = true;
                    _isSwiping = false;
                    break;
                case TouchPhase.Moved when _isHolding:
                {
                    // Calculate the movement direction based on the swipe distance
                    var swipeDirection = touch.position - _swipeStartPosition;
                    if (swipeDirection.magnitude > swipeThreshold)
                    {
                        _isSwiping = true;
                        MovementDirection = new Vector3(swipeDirection.x, 0.0f, swipeDirection.y).normalized;
                    }
                    else
                    {
                        _isSwiping = false;
                        MovementDirection = Vector3.zero;
                    }
            
                    break;
                }
                case TouchPhase.Ended:
                {
                    if (!_isSwiping && Time.time - _holdStartTime < holdTimeThreshold)
                    {
                        // Perform some action when the user taps instead of swiping
                        Debug.Log("Tap!");
                    }
            
                    // Reset the movement direction
                    MovementDirection = Vector3.zero;
                    characterAnimator.SetFloat(Speed, 0);
                    _isHolding = false;
                    _isSwiping = false;
                    break;
                }
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void ControlAi()
        {
            
        }
    }
}
