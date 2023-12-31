using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
	/// <summary>
	/// A basic Wobble which handles float values.
	/// </summary>
	public class MoonWobble : BaseMoonWobble<float> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonWobble() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Wobble.
		/// </summary>
		/// <param name="referenceValues_">Array of references to float values.</param>
		public MoonWobble(params Ref<float>[] referenceValues_) : base(referenceValues_) {
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
				this.Properties[index]() = this.StartValues[index] + (wave * this.Percentage);
			}
		}
	}
}