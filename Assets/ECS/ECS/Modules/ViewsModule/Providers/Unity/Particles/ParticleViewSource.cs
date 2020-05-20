﻿#if PARTICLES_VIEWS_MODULE_SUPPORT
namespace ME.ECS.Views.Providers {

    public abstract class ParticleViewSourceBase : ViewBase, IDoValidate {

        public abstract IView GetSource();

        [UnityEngine.ContextMenu("Validate")]
        public void DoValidate() {
            
            this.DoValidate(reset: true);
            
        }

        public virtual void DoValidate(bool reset) {
            
        }

        public void OnValidate() {

            this.DoValidate(reset: false);
            
        }

    }

    public abstract class ParticleViewSource<T> : ParticleViewSourceBase where T : ParticleViewBase, new() {

        public T data;
        
        public override IView GetSource() {

            return (IView)this.data;

        }

        public override void DoValidate(bool reset) {
            
            base.DoValidate(reset);
            
            var filters = this.GetComponentsInChildren<UnityEngine.MeshFilter>(true);
            var renderers = this.GetComponentsInChildren<UnityEngine.MeshRenderer>(true);
            var particleSystems = this.GetComponentsInChildren<UnityEngine.ParticleSystem>(true);

            if (this.data == null) this.data = new T();
            this.data.OnValidate(this.transform.position, this.transform.rotation.eulerAngles, this.transform.localScale, filters, renderers, particleSystems, reset);
            
        }

        public override string ToString() {

            return this.data.ToString();

        }
        
    }

}
#endif