using Moonvalk.Accessory;

namespace Moonvalk.Animation {
    /// <summary>
    /// A basic Tween which handles singular float value properties.
    /// </summary>
    public class MoonTween : BaseMoonTween<float> {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonTween() : base() {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Tween.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonTween(params Ref<float>[] referenceValues_) : base(referenceValues_) {
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
                this.Properties[i]() = this.EasingFunctions[i](this.Percentage, this.StartValues[i], this.TargetValues[i]);
            }
        }
    }
}