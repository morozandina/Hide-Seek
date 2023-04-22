using System.Threading.Tasks;
using UnityEngine;

namespace Shaders.Scanner
{
    public class ScannerController : MonoBehaviour
    {
        [Header("Set Up")]
        public float speed;
        public float delayDestroyTime;
        public int startDelay;
        public bool isLast;

        private bool _animate = false;

        private async void Start()
        {
            await Task.Delay(startDelay * 100);
            _animate = true;
            Destroy(isLast ? transform.parent.gameObject : gameObject, delayDestroyTime);
        }

        private void Update()
        {
            if (!_animate) return;
            
            var vectorMesh = transform.localScale;
            var growing = speed * Time.deltaTime;
            transform.localScale = new Vector3(vectorMesh.x + growing, vectorMesh.y, vectorMesh.z + growing);
        }
    }
}
