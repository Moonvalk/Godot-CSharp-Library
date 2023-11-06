using Godot;
using Moonvalk;

namespace Game {
	/// <summary>
	/// Main game manager behavior.
	/// </summary>
	public partial class GameManager : Node {
		#region Data Fields
		/// <summary>
		/// Singleton instance of GameManager.
		/// </summary>
		public static GameManager Instance { get; private set; }
		#endregion

		/// <summary>
		/// A maximum time elapsed allowed to be sent for game ticks. This helps to
		/// minimize lag spikes applied to systems.
		/// </summary>
		public const float MAXIMUM_FRAME_DELTA = 0.033f;

		#region Godot Events
		/// <summary>
		/// Occurs once this object is initialized.
		/// </summary>
		public override void _Ready() {
			this.initialize();
		}
		
		/// <summary>
		/// Called each game tick.
		/// </summary>
		/// <param name="delta_">The time elapsed since last frame.</param>
		public override void _Process(float delta_) {
			float deltaCapped = Mathf.Min(delta_, MAXIMUM_FRAME_DELTA);
			Global.Systems.Update(deltaCapped);
		}
		#endregion

		/// <summary>
		/// Initializes this Component as a singleton which shall persist through Scenes.
		/// </summary>
		protected void initialize() {
			if (Instance == null) {
				Instance = this;
				return;
			}
			this.QueueFree();
		}
	}
}
