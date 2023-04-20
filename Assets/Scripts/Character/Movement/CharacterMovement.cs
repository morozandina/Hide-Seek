using System.Collections;
using UnityEngine;

namespace Character.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        public float speed = 5.0f;
    public float swipeThreshold = 50.0f;
    public float holdTimeThreshold = 0.5f;

    private Rigidbody rigidBody;
    private Vector2 swipeStartPosition;
    private Vector2 swipeEndPosition;
    private float holdStartTime;
    private bool isHolding = false;
    private bool isSwiping = false;
    private Vector3 movementDirection = Vector3.zero;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                swipeStartPosition = touch.position;
                holdStartTime = Time.time;
                isHolding = true;
                isSwiping = false;
            }
            else if (touch.phase == TouchPhase.Moved && isHolding)
            {
                // Calculate the movement direction based on the swipe distance
                Vector2 swipeDirection = touch.position - swipeStartPosition;
                if (swipeDirection.magnitude > swipeThreshold)
                {
                    isSwiping = true;
                    movementDirection = new Vector3(swipeDirection.x, 0.0f, swipeDirection.y).normalized;
                }
                else
                {
                    isSwiping = false;
                    movementDirection = Vector3.zero;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!isSwiping && Time.time - holdStartTime < holdTimeThreshold)
                {
                    // Perform some action when the user taps instead of swiping
                    Debug.Log("Tap!");
                }

                // Reset the movement direction
                movementDirection = Vector3.zero;
                isHolding = false;
                isSwiping = false;
            }
        }
    }

    void FixedUpdate()
    {
        // Move the character in the movement direction
        rigidBody.AddForce(movementDirection * speed, ForceMode.VelocityChange);
    }
    }
}
