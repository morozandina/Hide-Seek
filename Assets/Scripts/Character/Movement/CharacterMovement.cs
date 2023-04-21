using System;
using System.Collections;
using UnityEngine;

namespace Character.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        public float speed = 5.0f;
        public float swipeThreshold = 50.0f;
        public float holdTimeThreshold = 0.5f;
        

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
            
            // Move the character in the movement direction
            transform.position += _movementDirection * speed * Time.deltaTime;
            var angle = Mathf.Atan2(_movementDirection.x, _movementDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
