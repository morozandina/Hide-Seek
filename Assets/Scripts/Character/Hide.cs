using System.Collections;
using Character.Movement;
using UnityEngine;

namespace Character
{
    public class Hide : CharacterMovement
    {

        private Coroutine MovementCoroutine;
        private Collider[] Colliders = new Collider[10]; // more is less performant, but more options
        
        [Header("Control's parameters")]
        public ParticleSystem starStunned;
        private static readonly int IsCaught = Animator.StringToHash("IsCaught");

        public void Caught()
        {
            isCanMove = false;
            characterAnimator.SetBool(IsCaught, true);
            starStunned.gameObject.SetActive(true);
            starStunned.Play();
        }
    }
}
