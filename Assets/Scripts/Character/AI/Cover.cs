using System;
using UnityEngine;

namespace Character.AI
{
    public class Cover : MonoBehaviour
    {
        public bool isSeeker;
        [HideInInspector]
        public bool isBusy = false;
        public Vector3 Position => transform.position;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = isSeeker ? Color.magenta : Color.cyan;
            Gizmos.DrawCube(transform.position + new Vector3(0, .15f, 0), Vector3.one * .3f);
        }
    }
}
