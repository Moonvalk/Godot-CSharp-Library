using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
    /// <summary>
    /// A Tween object which handles Vector2 values.
    /// </summary>
    public class MoonTweenVec2 : BaseMoonTween<Vector2> {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonTweenVec2() : base() {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Tween.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonTweenVec2(params Ref<Vector2>[] referenceValues_) : base(referenceValues_) {
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
            }
        }
    }
}