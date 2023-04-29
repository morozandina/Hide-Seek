using System;
using System.Collections;
using UnityEngine;

namespace Character.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        public bool isCanMove = false;
        [Header("Character's setting")]
        public float speed = 5.0f;
        [Header("Character's components")]
        public Animator characterAnimator;
        public Rigidbody characterRigidbody;

        // Animation key
        protected static readonly int Speed = Animator.StringToHash("Speed");
        
        // Private var
        protected Vector3 movementDirection = Vector3.zero;

        public virtual void Update()
        {
        }

        private void FixedUpdate()
        {
            if (!isCanMove) return;
            if (!(movementDirection.sqrMagnitude > 0.0f)) return;
            
            var moveSpeed = movementDirection * speed * Time.deltaTime;
            var angle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
            
            characterAnimator.SetFloat(Speed, moveSpeed.magnitude);
            
            characterRigidbody.MovePosition(characterRigidbody.position + moveSpeed);
            characterRigidbody.MoveRotation(Quaternion.Euler(0, angle, 0));
        }
    }
}
