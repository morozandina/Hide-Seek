using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.AI;
using Character.Movement;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character
{
    public class Hide : CharacterMovement
    {
        [Header("AI CONFIGURATION: ")]
        public Transform hidePoints;
        public float nextWaypointDistance = 0.75f;

        private List<Cover> _wayPoints = new List<Cover>();
        private Cover _lastPoint;
            
        private int _targetIndex;
        private int _lastTargetIndex;

        private Seeker _seeker;
        private Path _path;
        private int _currentWaypoint = 0;

        private bool _isHide = false;

        private void Start()
        {
            if (isMine) return;
            _seeker = GetComponent<Seeker>();

            _wayPoints = hidePoints.GetComponentsInChildren<Cover>().ToList();

            UpdateDestination(_wayPoints.Where((x, i) => !x.isBusy).ToList().PickRandom());
        }

        public override void ControlAi()
        {
            if (_path == null) return;
            if (_isHide) return;

            if (_currentWaypoint >= _path.vectorPath.Count)
            {
                // On end patch
                _isHide = true;
                MovementDirection = Vector3.zero;
                characterAnimator.SetFloat(Speed, 0);
                return;
            }
            
            var dir = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
            MovementDirection = new Vector3(dir.x, 0, dir.z).normalized;
            
            if (Vector3.Distance (transform.position,_path.vectorPath[_currentWaypoint]) < nextWaypointDistance)
                _currentWaypoint++;
        }
        
        private void UpdateDestination(Cover target)
        {
            if (_lastPoint != null)
                _lastPoint.isBusy = false;
            
            target.isBusy = true;
            _seeker.StartPath (transform.position,target.Position, OnPathComplete);
            _lastPoint = target;
        }

        private void OnPathComplete (Path p)
        {
            if (p.error) return;
            
            _path = p;
            _currentWaypoint = 0;
            _isHide = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // TODO Write AI logic here
            if (!other.CompareTag("Seeker")) return;

            foreach (var t in from t in _wayPoints
                     let distance = Vector3.Distance(other.transform.position, t.Position)
                     let dotProduct =
                         Vector3.Dot(other.transform.forward, (t.Position - other.transform.position).normalized)
                     where distance > 6 && dotProduct > 0 && !t.isBusy
                     select t)
            {
                    UpdateDestination(t);
                    break;
            }
        }

        public void Caught()
        {
            isCanMove = false;
            characterAnimator.SetBool(IsCaught, true);
            starStunned.gameObject.SetActive(true);
            starStunned.Play();
        }
    }
}
