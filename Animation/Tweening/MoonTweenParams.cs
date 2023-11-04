using Godot;

namespace Moonvalk.Animation {
	/// <summary>
	/// A grouping of parameters which change how MoonTweens operate.
	/// </summary>
	public class MoonTweenParams : Resource {
		/// <summary>
		/// The duration in seconds of the Tween.
		/// </summary>
		public float Duration { get; set; } = 1f;

		/// <summary>
		/// A delay in seconds before the Tween begins its animation.
		/// </summary>
		public float Delay { get; set; } = 0f;

		/// <summary>
		/// An Easing Type applied to the Tween. This value is used to find a easing method applied to Tween logic.
		/// </summary>
		public Easing.Types EasingType { get; set; } = Easing.Types.CubicInOut;
	}
}