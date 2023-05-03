using System;
using UnityEngine;

namespace Character.AI
{
    public class Cover : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(transform.position + new Vector3(0, .15f, 0), Vector3.one * .3f);
        }
    }
}
