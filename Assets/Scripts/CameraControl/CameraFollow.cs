using UnityEngine;

namespace CameraControl
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothSpeed = 0.125f;
        public Vector3 offsetCamera;

        private Vector3 desiredPosition;
        private void FixedUpdate()
        {
            desiredPosition = target.position + offsetCamera;
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition; 
        }
    }
}
