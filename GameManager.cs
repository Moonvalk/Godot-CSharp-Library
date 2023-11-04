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
			Global.Systems.Update(delta_);
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
