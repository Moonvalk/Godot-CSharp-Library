using Godot;
using Moonvalk.Animation;
using Moonvalk.Resources;

namespace Moonvalk.Controllers {
	/// <summary>
	/// Resource used for sharing properties that determine how a player controller will react to input.
	/// </summary>
	[RegisteredType(nameof(PlayerControllerParams), "", nameof(Resource))]
	public class PlayerControllerParams : Resource {
		#region Movement Settings
		/// <summary>
		/// 
		/// </summary>
		[Export] public float MovementSpeed { get; set; } = 2.0f;

		/// <summary>
		/// 
		/// </summary>
		[Export] public float MovementFriction { get; set; } = 20.0f;

		/// <summary>
		/// 
		/// </summary>
		[Export] public float Acceleration { get; set; } = 0.5f;

		/// <summary>
		/// 
		/// </summary>
		[Export] public float Deceleration { get; set; } = 0.5f;

		/// <summary>
		/// 
		/// </summary>
		[Export] public float MinimumInputThreshold { get; set; } = 0.1f;
		#endregion

		#region Jump Settings
		/// <summary>
		/// 
		/// </summary>
		[Export] public float JumpVelocity { get; set; } = 10.0f;

		/// <summary>
		/// 
		/// </summary>
		[Export] public int JumpCount { get; set; } = 2;

		/// <summary>
		/// 
		/// </summary>
		[Export] public float MaximumGravityVelocity { get; set; } = 20.0f;
		#endregion

		#region Raycast Settings
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
		/// 
		/// </summary>
		[Export] public MoonSpringParams RideSpringParams { get; set; }
		#endregion
	}
}