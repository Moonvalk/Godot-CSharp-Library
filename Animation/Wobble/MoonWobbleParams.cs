using Godot;

namespace Moonvalk.Animation {
	/// <summary>
	/// A grouping of parameters which change how MoonWobbles operate.
	/// </summary>
	public class MoonWobbleParams : Resource {
		/// <summary>
		/// The duration in seconds of the Wobble.
		/// </summary>
		public float Duration { get; set; } = -1f;

		/// <summary>
		/// The frequency of the sin wave applied to achieve animation.
		/// </summary>
		public float Frequency { get; set; } = 5f;

		/// <summary>
		/// The amplitude of the sin wave applied to achieve animation.
		/// </summary>
		public float Amplitude { get; set; } = 10f;
	}
}