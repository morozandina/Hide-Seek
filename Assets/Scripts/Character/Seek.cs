using System.Collections.Generic;
using Character.Movement;
using Pathfinding;
using UnityEngine;
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

        public Seeker seeker;
        
        //The max distance from the AI to a waypoint for it to continue to the next waypoint
        public float nextWaypointDistance = 3;
        
        //The calculated path
        private Path _path;
        
        //The waypoint we are currently moving towards
        private int _currentWaypoint = 0;
        
        private void Start()
        {
            if (isMine) return;
            
            foreach (Transform child in seekPoints)
            {
                _wayPoints.Add(child);
            }

            IterateWaypointIndex();
            UpdateDestination();
        }

        public override void ControlAi()
        {
            // We have no path to move after yet
            if (_path == null) return;

            if (_currentWaypoint >= _path.vectorPath.Count)
            {
                IterateWaypointIndex();
                UpdateDestination();
                return;
            }
            
            //Direction to the next waypoint
            var dir = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
            MovementDirection = new Vector3(dir.x, 0, dir.z).normalized;
            
            //Check if we are close enough to the next waypoint
            //If we are, proceed to follow the next waypoint
            if (Vector3.Distance (transform.position,_path.vectorPath[_currentWaypoint]) < nextWaypointDistance)
                _currentWaypoint++;
        }

        private void UpdateDestination()
        {
            _target = _wayPoints[_targetIndex].position;
            seeker.StartPath (transform.position,_target, OnPathComplete);
        }

        private void IterateWaypointIndex()
        {
            while (_targetIndex == _lastTargetIndex)
            {
                _targetIndex = Random.Range(0, _wayPoints.Count - 1);
            }

            _lastTargetIndex = _targetIndex;
        }
        
        private void OnPathComplete (Path p)
        {
            if (p.error) return;
            
            _path = p;
            //Reset the waypoint counter
            _currentWaypoint = 0;
        }
    }
}
