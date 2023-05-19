using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Skills
{
    public enum SkillType
    {
        Shield,
        Speed,
        Radar,
        Trough
    }
    public class Skill : MonoBehaviour
    {
        public ParticleSystem[] particles;
        public Action onCollect;

        [Space(5)][Header("Skill Type")]
        public SkillType skillType;
        
        

        #region SetUpSkill

        private void OnValidate()
        {
            gameObject.tag = "Skill";
            
            if (transform.childCount < 3)
                Debug.LogError("Your skill is not setup correctly {you need to have a 3 object}");
            else
            {
                particles = new ParticleSystem[3];
                foreach (var (item, i) in transform.Cast<Transform>().Select(t => t.GetComponent<ParticleSystem>()).WithIndex())
                    particles[i] = item;
            }

            AddCollider();
        }

        private void AddCollider()
        {
            if (!gameObject.HasComponent<SphereCollider>())
            {
                var sphereCollider = gameObject.AddComponent<SphereCollider>();
                sphereCollider.center = new Vector3(0, 0.4f, 0);
            }
            else
                gameObject.GetComponent<SphereCollider>().center = new Vector3(0, 0.4f, 0);
        }

        #endregion

        private void Start()
        {
            StartCoroutine(StartParticle());
        }

        private IEnumerator StartParticle()
        {
            particles[0].Play();
            yield return new WaitForSeconds(1);
            particles[1].Play();
            yield return new WaitForSeconds(.5f);
            particles[2].Play();

            yield return null;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag($"Hider"))
                return;
            
            onCollect?.Invoke();
        }
    }
}
