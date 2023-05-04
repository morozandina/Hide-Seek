using UnityEngine;

namespace Character.Movement
{
    public class StepControl : MonoBehaviour
    {
        [Header("Foot step particle")]
        public ParticleSystem[] step;
        
        public void FootStepEvent(int witchFoot) => step[witchFoot].Play();
    }
}
