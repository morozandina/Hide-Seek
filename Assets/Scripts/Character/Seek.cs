using System;
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
        public NavMeshAgent agent;
        public Transform seekPoints;

        private readonly List<Transform> _wayPoints = new List<Transform>();
        private int _wayIndex;
        private int _lastWayIndex;
        private Vector3 _target;

        private void Start()
        {
            if (isMine) return;

            agent.enabled = true;
            foreach (Transform child in seekPoints)
            {
                _wayPoints.Add(child);
            }

            UpdateDestination();
            agent.updatePosition = true;
        }

        public override void ControlAi()
        {

            if (Vector3.Distance(transform.position, _target) < 1)
            {
                IterateWaypointIndex();
                UpdateDestination();
            }
            
            foreach (var x in agent.path.corners)
            {
                Debug.Log(x);
            }
            
            // var heading = agent.nextPosition;
            // var distance = heading.magnitude;
            // var direction = heading / distance;
            // MovementDirection = new Vector3(direction.x, 0.0f, direction.z).normalized;

        }

        private void UpdateDestination()
        {
            _target = _wayPoints[_wayIndex].position;
            agent.SetDestination(_target);
        }

        private void IterateWaypointIndex()
        {
            while (_wayIndex == _lastWayIndex)
            {
                _wayIndex = Random.Range(0, _wayPoints.Count - 1);
            }

            _lastWayIndex = _wayIndex;
        }
    }
}
