using UnityEngine;

namespace Trails
{
    public class Trail_control : MonoBehaviour
    {
        //The speed at which the tail disappears------------尾巴消失的速度
        private float disappear_speed;

        //SkinnedMeshRenderer passed in from outside---------外部传进来的SkinnedMeshRenderer
        public SkinnedMeshRenderer skinned_mesh_renderer;

        //Baked mesh result-----烘焙出的mesh结果
        private Mesh baked_mesh_result;

        //material
        private Material material;

        //alpha
        private float alpha;

        public void init(float disappear_speed, SkinnedMeshRenderer skinned_mesh_renderer,float alpha)
        {
            this.disappear_speed = disappear_speed;
            this.skinned_mesh_renderer = skinned_mesh_renderer;
            this.alpha = alpha;

            if (this.baked_mesh_result == null)
            {
                this.baked_mesh_result = new Mesh();
            }

            //Render mesh---------渲染mesh
            this.skinned_mesh_renderer.BakeMesh(this.baked_mesh_result);
            this.GetComponent<MeshFilter>().mesh = this.baked_mesh_result;

            //Set the material of this object-------------设置这个物体的材质
            this.material = this.GetComponent<MeshRenderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            if (this.material != null)
            {
                this.alpha = Mathf.Lerp(this.alpha,0,this.disappear_speed*Time.deltaTime);

                this.material.SetFloat("alpha", this.alpha);

                if (this.alpha < 0.01f)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
