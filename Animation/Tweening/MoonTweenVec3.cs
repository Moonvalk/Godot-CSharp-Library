using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
    /// <summary>
    /// A Tween object which handles Vector3 values.
    /// </summary>
    public class MoonTweenVec3 : BaseMoonTween<Vector3> {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonTweenVec3() : base() {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Tween.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonTweenVec3(params Ref<Vector3>[] referenceValues_) : base(referenceValues_) {
            // ...
        }

        /// <summary>
        /// Method used to update all properties available to this object.
        /// </summary>
        protected override void updateProperties() {
            // Apply easing and set properties.
            for (int i = 0; i < this.Properties.Length; i++) {
                if (this.Properties[i] == null) {
                    this.Delete();
                    break;
                }
                this.Properties[i]().x = this.EasingFunctions[i](this.Percentage, this.StartValues[i].x, this.TargetValues[i].x);
                this.Properties[i]().y = this.EasingFunctions[i](this.Percentage, this.StartValues[i].y, this.TargetValues[i].y);
                this.Properties[i]().z = this.EasingFunctions[i](this.Percentage, this.StartValues[i].z, this.TargetValues[i].z);
            }
        }
    }
}