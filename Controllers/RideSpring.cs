using Godot;
using Godot.Collections;
using Moonvalk.Animation;
using Moonvalk.Utilities.Algorithms;

namespace Moonvalk.Controllers {
	public class RideSpring : Spatial {
		[Export] public RideSpringParams Properties { get; protected set; }

		public MoonSpring Spring { get; protected set; }

		protected float _currentRideHeight = 0f;
		protected Vector3 _currentVelocity = new Vector3();

		public override void _Ready() {
			this.Properties = this.Properties ?? new RideSpringParams();
			this.Spring = new MoonSpring(() => ref this._currentRideHeight);
		}

		public override void _PhysicsProcess(float delta_) {
			this.applySpringForce(delta_);
			this.Translation += this._currentVelocity;
		}

		protected void applySpringForce(float delta_) {
			PhysicsDirectSpaceState spaceState = this.GetWorld().DirectSpaceState;
			Vector3 rayStart = this.Transform.origin + (Vector3.Up * this.Properties.RaycastOriginOffset);
			Vector3 rayEnd = rayStart + (Vector3.Down * (this.Properties.RideSpringHeight + this.Properties.RaycastOriginOffset));
			
			Dictionary collision = spaceState.IntersectRay(rayStart, rayEnd, new Array() { this });
			if (collision != null) {
				Vector3 rayDirection = -this.Transform.basis.y;
				Vector3 otherVelocity = Vector3.Zero;
				Vector3 collisionPoint = (collision.Contains("position") && collision["position"] is Vector3 vector3 ? vector3 : rayEnd);
				RigidBody hitBody = (collision.Contains("collider") && collision["collider"] is RigidBody rigidBody ? rigidBody : null);
				if (hitBody != null) {
					otherVelocity = hitBody.LinearVelocity;
				}

				float rayDirectionVelocity = rayDirection.Dot(this._currentVelocity);
				float otherDirectionVelocity = rayDirection.Dot(otherVelocity);
				float relativeVelocity = (rayDirectionVelocity - otherDirectionVelocity);
				
				float offset = rayStart.DistanceTo(collisionPoint) - this.Properties.RideSpringHeight;
				float rideForce = MotionAlgorithms.SimpleHarmonicMotion(this.Properties.SpringParams.Tension, offset,
					this.Properties.SpringParams.Dampening, relativeVelocity);
				this._currentVelocity.y += (rideForce * delta_);

				if (hitBody != null) {
					hitBody.AddCentralForce(rayDirection * -rideForce);
				}
			}
		}
	}
}