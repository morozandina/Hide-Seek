using System;
using System.Collections;
using UnityEngine;

namespace Character.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Character's setting")]
        public float speed = 5.0f;
        [Header("Control's parameters")]
        public float swipeThreshold = 50.0f;
        public float holdTimeThreshold = 0.5f;
        [Header("Character's components")]
        public Animator characterAnimator;
        public Rigidbody characterRigidbody;
        [Header("Foot step particle")]
        public ParticleSystem[] step;

        // Animation key
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        // Private var
        private Vector2 _swipeStartPosition;
        private Vector2 _swipeEndPosition;
        private float _holdStartTime;
        private bool _isHolding = false;
        private bool _isSwiping = false;
        private Vector3 _movementDirection = Vector3.zero;
        
        private void Update()
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
                        _movementDirection = new Vector3(swipeDirection.x, 0.0f, swipeDirection.y).normalized;
                    }
                    else
                    {
                        _isSwiping = false;
                        _movementDirection = Vector3.zero;
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
                    _movementDirection = Vector3.zero;
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

        private void FixedUpdate()
        {
            if (!(_movementDirection.sqrMagnitude > 0.0f)) return;
            
            var moveSpeed = _movementDirection * speed * Time.deltaTime;
            var angle = Mathf.Atan2(_movementDirection.x, _movementDirection.z) * Mathf.Rad2Deg;
            
            characterAnimator.SetFloat(Speed, moveSpeed.magnitude);
            
            characterRigidbody.MovePosition(characterRigidbody.position + moveSpeed);
            characterRigidbody.MoveRotation(Quaternion.Euler(0, angle, 0));
        }

        public void FootStepEvent(int witchFoot)
        {
            step[witchFoot].Play();
        }
    }
}
