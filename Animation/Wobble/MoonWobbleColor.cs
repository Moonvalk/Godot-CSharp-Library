using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
	/// <summary>
	/// A basic Wobble which handles Color values.
	/// </summary>
	public class MoonWobbleColor : BaseMoonWobble<Godot.Color> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonWobbleColor() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Wobble.
		/// </summary>
		/// <param name="referenceValues_">Array of references to float values.</param>
		public MoonWobbleColor(params Ref<Color>[] referenceValues_) : base(referenceValues_) {
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
				this.Properties[index]().r = this.StartValues[index].r + (wave * this.Percentage.r);
				this.Properties[index]().g = this.StartValues[index].g + (wave * this.Percentage.g);
				this.Properties[index]().b = this.StartValues[index].b + (wave * this.Percentage.b);
				this.Properties[index]().a = this.StartValues[index].a + (wave * this.Percentage.a);
			}
		}
	}
}