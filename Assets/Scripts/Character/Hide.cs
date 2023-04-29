using Character.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class Hide : CharacterMovement
    {
        [Header("Control's parameters")]
        public Transform target;
        public NavMeshAgent navMeshAgent;
        private static readonly int IsCaught = Animator.StringToHash("IsCaught");

        public void Caught()
        {
            isCanMove = false;
            characterAnimator.SetBool(IsCaught, true);
        }
    }
}
