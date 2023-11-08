using Godot;

namespace Moonvalk.Controllers {
	/// <summary>
	/// 
	/// </summary>
	public class PlayerState : Node {
		/// <summary>
		/// 
		/// </summary>
		protected PlayerControllerParams _properties;

		/// <summary>
		/// 
		/// </summary>
		public bool IsGrounded { get; protected set; } = false;

		/// <summary>
		/// 
		/// </summary>
		public bool IsJumping { get; protected set; } = false;

		/// <summary>
		/// 
		/// </summary>
		public int CurrentJumpCount { get; protected set; } = 0;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="properties_"></param>
		public void SetProperties(PlayerControllerParams properties_) {
			this._properties = properties_;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="flag_"></param>
		/// <returns></returns>
		public bool SetGrounded(bool flag_) {
			if (this.IsGrounded != flag_) {
				this.IsGrounded = flag_;
				if (flag_) {
					this.CurrentJumpCount = 0;
					this.IsJumping = false;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool CanJump() {
			if (this.IsGrounded) {
				return true;
			}
			if (this.CurrentJumpCount < this._properties.JumpCount) {
				this.CurrentJumpCount = (this.CurrentJumpCount == 0) ? 1 : this.CurrentJumpCount;
				return true;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool ConsumeJump() {
			bool isJumpAvailable = this.CanJump();
			if (isJumpAvailable) {
				this.IsJumping = true;
				this.IsGrounded = false;
				this.CurrentJumpCount++;
			}
			return isJumpAvailable;
		}
	}
}