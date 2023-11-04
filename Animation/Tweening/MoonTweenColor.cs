using Moonvalk.Accessory;

namespace Moonvalk.Animation {
    /// <summary>
    /// A basic Tween which handles Color value properties.
    /// </summary>
    public class MoonTweenColor : BaseMoonTween<Godot.Color> {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonTweenColor() : base() {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Tween.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonTweenColor(params Ref<Godot.Color>[] referenceValues_) : base(referenceValues_) {
            // ...
        }

        /// <summary>
        /// Method used to update all properties available to this object.
        /// </summary>
        protected override void updateProperties() {
            // Apply easing and set properties.
            for (int i = 0; i < this.Properties.Length; i++) {
				if (this.Properties[i] == null) {
					this.Stop();
					break;
				}
				this.Properties[i]().r = this.EasingFunctions[i](this.Percentage, this.StartValues[i].r, this.TargetValues[i].r);
				this.Properties[i]().g = this.EasingFunctions[i](this.Percentage, this.StartValues[i].g, this.TargetValues[i].g);
				this.Properties[i]().b = this.EasingFunctions[i](this.Percentage, this.StartValues[i].b, this.TargetValues[i].b);
				this.Properties[i]().a = this.EasingFunctions[i](this.Percentage, this.StartValues[i].a, this.TargetValues[i].a);
            }
        }
    }
}