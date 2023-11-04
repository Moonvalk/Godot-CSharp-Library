using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
    /// <summary>
    /// A basic Wobble which handles Vector3 values.
    /// </summary>
    public class MoonWobbleVec3 : BaseMoonWobble<Vector3> {
        /// <summary>
        /// Default constructor made without setting up references.
        /// </summary>
        public MoonWobbleVec3() : base() {
            // ...
        }

        /// <summary>
        /// Constructor for creating a new Wobble.
        /// </summary>
        /// <param name="referenceValues_">Array of references to float values.</param>
        public MoonWobbleVec3(params Ref<Vector3>[] referenceValues_) : base(referenceValues_) {
            // ...
        }

        /// <summary>
        /// Method used to update all properties available to this object.
        /// </summary>
        protected override void updateProperties() {
            // Apply easing and set properties.
            float wave = Mathf.Sin(this.Time * this.Frequency) * this.Amplitude * this._strength;
            for (int index = 0; index < this.Properties.Length; index++) {
                if (this.Properties[index] == null) {
                    this.Delete();
                    break;
                }
				this.Properties[index]().x = this.StartValues[index].x + (wave * this.Percentage.x);
				this.Properties[index]().y = this.StartValues[index].y + (wave * this.Percentage.y);
				this.Properties[index]().z = this.StartValues[index].z + (wave * this.Percentage.z);
            }
        }
    }
}