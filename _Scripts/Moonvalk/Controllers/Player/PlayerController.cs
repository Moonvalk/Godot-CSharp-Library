using System.Collections.Generic;
using Godot;
using Moonvalk.Accessory;
using Moonvalk.Utilities.Algorithms;

namespace Moonvalk.Controllers {
	/// <summary>
	/// 
	/// </summary>
	public class PlayerController : RigidBody {
		#region Data Fields
		/// <summary>
		/// 
		/// </summary>
		[Export] public PlayerControllerParams Properties { get; protected set; }

		/// <summary>
		/// 
		/// </summary>
		[Export] protected NodePath p_playerState { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Export] protected NodePath p_playerVisual { get; set; }
		// [Export] protected NodePath p_camera { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public PlayerState State { get; protected set; }

		/// <summary>
		/// 
		/// </summary>
		public PlayerVisual Visual { get; protected set; }
		// public Camera Camera { get; protected set; }

		/// <summary>
		/// 
		/// </summary>
		public Vector3 PreviousMoveInput { get; protected set; } = Vector3.Zero;

		/// <summary>
		/// 
		/// </summary>
		public Vector3 CurrentMoveInput { get; protected set; } = Vector3.Zero;

		/// <summary>
		/// 
		/// </summary>
		protected Vector3 _currentMoveSpeed = Vector3.Zero;

		/// <summary>
		/// 
		/// </summary>
		[Signal] public delegate void OnGrounded();

		/// <summary>
		/// 
		/// </summary>
		[Signal] public delegate void OnJump();
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready() {
			this.State = GetNode<PlayerState>(p_playerState);
			this.Visual = GetNode<PlayerVisual>(p_playerVisual);
			// this.Camera = GetNode<Camera>(p_camera);
			this.State.SetProperties(this.Properties);
		}

		public override void _Process(float delta_) {
			this.CurrentMoveInput = new Vector3(Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"), 0f,
				Input.GetActionStrength("move_up") - Input.GetActionStrength("move_down"));
		}

		/// <summary>
		/// Called each physics frame.
		/// </summary>
		/// <param name="delta_"></param>
		public override void _PhysicsProcess(float delta_) {
			this.handleFriction();
			this.handleMovement();

			this.checkGroundedState();
			this.handleJump();
			this.applySpringForce();
		}
		#endregion

		#region Public Methods
		
		#endregion

		#region Private Methods
		/// <summary>
		/// 
		/// </summary>
		protected void handleFriction() {
			Vector3 friction = new Vector3(-this.LinearVelocity.x * this.Properties.MovementFriction, 0f,
				-this.LinearVelocity.z * this.Properties.MovementFriction);
			this.AddCentralForce(friction);
		}

		/// <summary>
		/// 
		/// </summary>
		protected void handleMovement() {
			this.getAxialVelocity(this.CurrentMoveInput.x, ref this._currentMoveSpeed.x);
			this.getAxialVelocity(this.CurrentMoveInput.z, ref this._currentMoveSpeed.z);
			// Vector3 force = (this._currentMoveSpeed.x * this)
			Vector3 force = this._currentMoveSpeed;
			this.AddCentralForce(force);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input_"></param>
		/// <param name="axialVelocity_"></param>
		protected void getAxialVelocity(float input_, ref float axialVelocity_) {
			float absoluteInput = Mathf.Abs(input_);
			if (absoluteInput >= this.Properties.MinimumInputThreshold) {
				axialVelocity_ += input_ * this.Properties.Acceleration;
			} else if (Mathf.Abs(axialVelocity_) > this.Properties.Deceleration) {
				axialVelocity_ -= Mathf.Sign(axialVelocity_) * this.Properties.Deceleration;
			} else {
				axialVelocity_ = 0f;
			}
			float moveSpeed = this.Properties.MovementSpeed;
			axialVelocity_ = Mathf.Clamp(axialVelocity_, -moveSpeed, moveSpeed);
		}

		/// <summary>
		/// 
		/// </summary>
		protected void checkGroundedState() {

		}

		/// <summary>
		/// 
		/// </summary>
		protected void handleJump() {

		}
		
		/// <summary>
		/// 
		/// </summary>
		protected void applySpringForce() {
			PhysicsDirectSpaceState spaceState = this.GetWorld().DirectSpaceState;
			Vector3 rayStart = this.Transform.origin + (Vector3.Up * this.Properties.RaycastOriginOffset);
			Vector3 rayEnd = rayStart + (Vector3.Down * (this.Properties.RideSpringHeight + this.Properties.RaycastOriginOffset));

			Godot.Collections.Dictionary collision = spaceState.IntersectRay(rayStart, rayEnd, new Godot.Collections.Array() { this });
			if (collision != null) {
				Vector3 rayDirection = -this.Transform.basis.y;
				Vector3 otherVelocity = Vector3.Zero;
				Vector3 collisionPoint = (collision.Contains("position") && collision["position"] is Vector3 vector3 ? vector3 : rayEnd);
				RigidBody hitBody = (collision.Contains("collider") && collision["collider"] is RigidBody rigidBody ? rigidBody : null);
				if (hitBody != null) {
					otherVelocity = hitBody.LinearVelocity;
				}

				float rayDirectionVelocity = rayDirection.Dot(this.LinearVelocity);
				float otherDirectionVelocity = rayDirection.Dot(otherVelocity);
				float relativeVelocity = (rayDirectionVelocity - otherDirectionVelocity);

				float offset = rayStart.DistanceTo(collisionPoint) - this.Properties.RideSpringHeight;
				float rideForce = MotionAlgorithms.SimpleHarmonicMotion(this.Properties.RideSpringParams.Tension, offset,
					this.Properties.RideSpringParams.Dampening, relativeVelocity);
				this.AddCentralForce(rayDirection * rideForce);

				if (hitBody != null) {
					hitBody.AddCentralForce(rayDirection * -rideForce);
				}
			}
		}
		#endregion
	}
}
