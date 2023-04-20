using UnityEditor;
using UnityEngine;

namespace FieldOfView.Editor
{
    [CustomEditor (typeof (FieldOfView))]
    public class FieldOfViewEditor : UnityEditor.Editor {
        private void OnSceneGUI() {
            var fow = (FieldOfView)target;
            Handles.color = Color.white;
            Handles.DrawWireArc (fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
            var viewAngleA = fow.DirFromAngle (-fow.viewAngle / 2, false);
            var viewAngleB = fow.DirFromAngle (fow.viewAngle / 2, false);

            Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
            Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

            Handles.color = Color.red;
            foreach (var visibleTarget in fow.visibleTargets) {
                Handles.DrawLine (fow.transform.position, visibleTarget.position);
            }
        }

    }
}