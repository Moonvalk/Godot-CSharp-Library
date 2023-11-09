using Godot;
using Moonvalk.Resources;
using Moonvalk.Animation;

namespace Moonvalk.Controllers {
	/// <summary>
	/// A grouping of parameters which change how MoonSprings operate.
	/// </summary>
	[RegisteredType(nameof(RideSpringParams), "", nameof(Resource))]
	public class RideSpringParams : Resource {
		/// <summary>
		/// 
		/// </summary>
		[Export] public MoonSpringParams SpringParams { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Export] public float RaycastOriginOffset { get; set; } = 0.05f;

		/// <summary>
		/// 
		/// </summary>
		[Export] public float GroundRayHeight { get; set; } = 0.4f;

		/// <summary>
		/// 
		/// </summary>
		[Export] public float RideSpringHeight { get; set; } = 0.2f;

		/// <summary>
		/// Default constructor for new ride spring parameters.
		/// </summary>
		public RideSpringParams() {
			this.SpringParams = this.SpringParams ?? new MoonSpringParams();
		}
	}
}
