using System;
using Character.Movement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Character
{
    public class Seek : CharacterMovement
    {
        private Vector2 _swipeStartPosition;
        private float _holdStartTime;
        private bool _isHolding = false;
        private bool _isSwiping = false;
        
        [Header("Control's parameters")]
        public float swipeThreshold = 50.0f;
        public float holdTimeThreshold = 0.5f;
        
        public override void Update()
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
                        movementDirection = new Vector3(swipeDirection.x, 0.0f, swipeDirection.y).normalized;
                    }
                    else
                    {
                        _isSwiping = false;
                        movementDirection = Vector3.zero;
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
                    movementDirection = Vector3.zero;
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
    }
}
