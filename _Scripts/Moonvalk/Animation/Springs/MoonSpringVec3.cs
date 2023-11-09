using Moonvalk.Utilities.Algorithms;
using Moonvalk.Accessory;
using Godot;
using System;

namespace Moonvalk.Animation {
	/// <summary>
	/// Object that handles Spring calculations for a Vector3 value.
	/// </summary>
	public class MoonSpringVec3 : BaseMoonSpring<Vector3> {
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		public MoonSpringVec3() : base() {
			// ...
		}

		/// <summary>
		/// Constructor for creating a new Spring.
		/// </summary>
		/// <param name="referenceValues_">Array of references to Vector3 values.</param>
		public MoonSpringVec3(params Ref<Vector3>[] referenceValues_) : base(referenceValues_) {
			// ...
		}

		/// <summary>
		/// Calculates the necessary velocities to be applied to all Spring properties each game tick.
		/// </summary>
		protected override void calculateForces() {
			for (int i = 0; i < this.Properties.Length; i++) {
				if (this.Properties[i] == null) {
					this.Delete();
					break;
				}
				float displacement = (this.TargetProperties[i].x - this.Properties[i]().x);
				this.CurrentForce[i].x = MotionAlgorithms.SimpleHarmonicMotion(this.Tension, displacement, this.Dampening, this.Speed[i].x);
				displacement = (this.TargetProperties[i].y - this.Properties[i]().y);
				this.CurrentForce[i].y = MotionAlgorithms.SimpleHarmonicMotion(this.Tension, displacement, this.Dampening, this.Speed[i].y);
				displacement = (this.TargetProperties[i].z - this.Properties[i]().z);
				this.CurrentForce[i].z = MotionAlgorithms.SimpleHarmonicMotion(this.Tension, displacement, this.Dampening, this.Speed[i].z);
			}
		}

		/// <summary>
		/// Applies force to properties each frame.
		/// </summary>
		/// <param name="deltaTime_">The time elapsed between last and current game tick.</param>
		protected override void applyForces(float deltaTime_) {
			for (int i = 0; i < this.Properties.Length; i++) {
				this.Speed[i].x += this.CurrentForce[i].x * deltaTime_;
				this.Speed[i].y += this.CurrentForce[i].y * deltaTime_;
				this.Speed[i].z += this.CurrentForce[i].z * deltaTime_;
				this.Properties[i]().x += this.Speed[i].x * deltaTime_;
				this.Properties[i]().y += this.Speed[i].y * deltaTime_;
				this.Properties[i]().z += this.Speed[i].z * deltaTime_;
			}
		}

		/// <summary>
		/// Determines if the minimum forces have been met to continue calculating Spring forces.
		/// </summary>
		/// <returns>Returns true if the minimum forces have been met.</returns>
		protected override bool minimumForcesMet() {
			for (int index = 0; index < CurrentForce.Length; index++) {
				bool metTarget = (ConversionHelpers.Abs(this.TargetProperties[index] - this.Properties[index]()) >= this.MinimumForce[index]);
				bool metMinimumForce = (ConversionHelpers.Abs(this.CurrentForce[index] + this.Speed[index]) >= this.MinimumForce[index]);
				if (metTarget && metMinimumForce) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Assigns the minimum force required until the Spring is completed based on inputs.
		/// </summary>
		protected override void setMinimumForce() {
			this.MinimumForce = new Vector3[this.Properties.Length];
			for (int index = 0; index < this.Properties.Length; index++) {
				this.MinimumForce[index] = MoonSpring._defaultMinimumForcePercentage *
					ConversionHelpers.Abs(this.TargetProperties[index] - this.Properties[index]());
			}
		}

		/// <summary>
		/// Determines if there is a need to apply force to this Spring to meet target values.
		/// </summary>
		/// <returns>Returns true if forces need to be applied</returns>
		protected override bool needToApplyForce() {
			for (int index = 0; index < this.Properties.Length; index++) {
				if (this.Properties[index]() != this.TargetProperties[index]) {
					return true;
				}
			}
			return false;
		}
	}
}
