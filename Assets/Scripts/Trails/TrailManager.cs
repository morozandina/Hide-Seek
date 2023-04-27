using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Trails
{
    public class TrailManager : MonoBehaviour
    {
        [Header("Target SkinnedMeshRenderer")]
        public GameObject gameObjTarget;
        private Vector3 _v3PositionGameObjTargetBefore;
        public int trailCount;
        [Range(0f, 1f)] public float trailAlpha;
        public float trailIntervalTime;
        public float trailDisappearSpeed;

        [Header("Trail Color")]
        [ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
        public Color colorTrail;

        // Start is called before the first frame update
        private void OnEnable()
        {
            if (this.trailCount <= 0 || this.gameObjTarget == null) return;
            
            //Generate trail objects
            for (var i = 0; i < this.trailCount; i++)
            {
                var trail = new GameObject("trail" + i);
                trail.transform.SetParent(this.transform);
                trail.AddComponent<MeshFilter>();
                trail.AddComponent<MeshRenderer>();
                trail.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; //no shadow
                trail.layer = gameObjTarget.layer;

                var mat = new Material(Shader.Find("EasyGameStudio/Trail"));
                mat.SetTexture("main_texture", this.gameObjTarget.GetComponent<SkinnedMeshRenderer>().material.mainTexture);
                mat.SetColor("color_fresnel_emission", this.colorTrail);
                trail.GetComponent<MeshRenderer>().material = mat;
                var trailControl = trail.AddComponent<Trail_control>();

                trail.SetActive(false);
            }

            StartCoroutine(this.trail_start());
        }

        private void OnDisable()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        //The coroutine starts to make the tail
        private IEnumerator trail_start()
        {
            while (true) 
            {
                for (var i = 0; i < this.trailCount; i++)
                {

                    //If the position does not change, the tail will not be set
                    if (this._v3PositionGameObjTargetBefore!= this.gameObjTarget.transform.position)
                    {
                        GameObject trail = this.transform.GetChild(i).gameObject;
                        trail.transform.position = this.gameObjTarget.transform.position;
                        trail.transform.rotation = this.gameObjTarget.transform.rotation;
                        if (trail.activeSelf == false)
                            trail.SetActive(true);
                        trail.GetComponent<Trail_control>().init(this.trailDisappearSpeed, this.gameObjTarget.GetComponent<SkinnedMeshRenderer>(), this.trailAlpha);               
                    }
                    this._v3PositionGameObjTargetBefore = this.gameObjTarget.transform.position;

                    yield return new WaitForSeconds(this.trailIntervalTime);
                }
            }
        }
    }
}
