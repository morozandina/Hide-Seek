using System;
using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Character
{
    public class Seek : CharacterMovement
    {
        [Header("AI CONFIGURATION: ")]
        public Transform seekPoints;

        private readonly List<Transform> _wayPoints = new List<Transform>();
        private int _targetIndex;
        private int _lastTargetIndex;
        private Vector3 _target;
        
        private Vector3[] _waypoints;
        private Vector3 _waypoint;
        private int _wayIndex = 0;

        private void Start()
        {
            if (isMine) return;

            foreach (Transform child in seekPoints)
            {
                _wayPoints.Add(child);
            }

            UpdateDestination();
        }

        public override void ControlAi()
        {
            if (Vector3.Distance(transform.position, _target) < 1)
            {
                IterateWaypointIndex();
                UpdateDestination();
            }

            if (Vector3.Distance(transform.position, _waypoint) < 1)
            {
                UpdateWayPoint();
            }
            
            var direction = _waypoint - transform.position;
            MovementDirection = new Vector3(direction.x, 0, direction.z).normalized;
        }

        private void UpdateWayPoint()
        {
            if (_wayIndex + 1 >= _waypoints.Length - 1)
                _waypoint = _target;
            else
            {
                _waypoint = _waypoints[_wayIndex];
                _waypoint.y = transform.position.y;
                _wayIndex++;
            }
        }

        private void UpdateDestination()
        {
            _target = _wayPoints[_targetIndex].position;
            _wayIndex = 0;
            var path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, _target, NavMesh.AllAreas, path);
            _waypoints = path.corners;
        }

        private void IterateWaypointIndex()
        {
            while (_targetIndex == _lastTargetIndex)
            {
                _targetIndex = Random.Range(0, _wayPoints.Count - 1);
            }

            _lastTargetIndex = _targetIndex;
        }
    }
}
