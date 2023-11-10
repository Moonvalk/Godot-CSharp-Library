using System.Collections.Generic;
using Godot;
using Moonvalk.Animation;
using Moonvalk.Inputs;

namespace Moonvalk.Controllers {
	/// <summary>
	/// 
	/// </summary>
	public class CameraController : Node {
		#region Data Fields
		/// <summary>
		/// All available controls to move this camera.
		/// </summary>
		public enum Controls {
			Forward,
			Back,
			Left,
			Right,
		}

		/// <summary>
		/// Path to the camera object.
		/// </summary>
		[Export] protected NodePath p_camera { get; set; }

		/// <summary>
		/// Controls the speed of this controller's movement.
		/// </summary>
		[Export] public float MoveSpeed { get; protected set; }

		/// <summary>
		/// Stores reference to the main camera object.
		/// </summary>
		public Camera Camera { get; protected set; }

		/// <summary>
		/// 
		/// </summary>
		public Vector3 TargetPosition { get; protected set; }

		/// <summary>
		/// Stores the current move input being used.
		/// </summary>
		protected Vector2 _currentMoveInput = new Vector2();

		/// <summary>
		/// Creates a map of all camera controls for easily accessing string names.
		/// </summary>
		public Dictionary<Controls, string> ControlMap { get; protected set; } = new Dictionary<Controls, string>() {
			{ Controls.Forward, "camera_forward" },
			{ Controls.Back, "camera_back" },
			{ Controls.Left, "camera_left" },
			{ Controls.Right, "camera_right" },
		};
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready() {
			this.Camera = this.GetNode<Camera>(p_camera);
			this.TargetPosition = this.Camera.Translation;
			MoonInputManager.Add(
				new InputKeyPair(this.ControlMap[Controls.Left], KeyList.J),
				new InputKeyPair(this.ControlMap[Controls.Right], KeyList.L),
				new InputKeyPair(this.ControlMap[Controls.Forward], KeyList.I),
				new InputKeyPair(this.ControlMap[Controls.Back], KeyList.K));
		}

		/// <summary>
		/// Called once each game tick.
		/// </summary>
		/// <param name="delta_">The time elapsed between last and current frame.</param>
		public override void _Process(float delta_) {
			this.getMoveInput();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="delta_"></param>
		public override void _PhysicsProcess(float delta_) {
			this.handleMovement(delta_);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// 
		/// </summary>
		protected void getMoveInput() {
			this._currentMoveInput = new Vector2(Godot.Input.GetAxis(this.ControlMap[Controls.Left], this.ControlMap[Controls.Right]),
				Godot.Input.GetAxis(this.ControlMap[Controls.Forward], this.ControlMap[Controls.Back]));
		}

		/// <summary>
		/// 
		/// </summary>
		protected void handleMovement(float delta_) {
			if (this._currentMoveInput.Length() > 0f) {
				this.TargetPosition += new Vector3(this._currentMoveInput.x, 0f, this._currentMoveInput.y) * this.MoveSpeed * delta_;
				this.Camera.MoveTo(this.TargetPosition, new MoonTweenParams() { Duration = 0.5f, EasingFunction = Easing.Cubic.Out });
			}
		}
		#endregion
	}
}
