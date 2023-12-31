using Godot;
using System.Collections.Generic;
using Moonvalk.Accessory;

namespace Moonvalk.Audio {
	/// <summary>
	/// Container representing a sound queue that can be played within Godot.
	/// </summary>
	[Tool] public partial class SoundQueue : Node {
		/// <summary>
		/// Stores reference to the next index to be played.
		/// </summary>
		public int NextIndex { get; private set; } = 0;

		/// <summary>
		/// A list of all audio players available for this queue.
		/// </summary>
		public List<AudioStreamPlayer> AudioStreamPlayers { get; private set; } = new List<AudioStreamPlayer>();

		/// <summary>
		/// The number of audio stream players to duplicate on load for uninterrupted playback.
		/// </summary>
		[Export] public int Count { get; set; } = 1;

		/// <summary>
		/// Called when this object is initialized.
		/// </summary>
		public override void _Ready() {
			if (GetChildCount() == 0) {
				GD.Print("No AudioStreamPlayer child found.");
				return;
			}

			Node child = GetChild(0);
			if (child is AudioStreamPlayer audioStreamPlayer) {
				this.AudioStreamPlayers.Add(audioStreamPlayer);
				for (int index = 0; index < this.Count; index++) {
					AudioStreamPlayer duplicate = audioStreamPlayer.Duplicate() as AudioStreamPlayer;
					AddChild(duplicate);
					this.AudioStreamPlayers.Add(duplicate);
				}
			}
		}

		/// <summary>
		/// Called by the Godot editor to get potential warnings with setup.
		/// </summary>
		/// <returns>A string representing a warning with current configuration.</returns>
		public override string _GetConfigurationWarning() {
			if (GetChildCount() == 0) {
				return "No children found. Expected one AudioStreamPlayer child.";
			}
			if (!GetChild(0).IsType<AudioStreamPlayer>()) {
				return "Expected first child to be an AudioStreamPlayer.";
			}
			return "";
		}

		/// <summary>
		/// Called to play the sound effect stored within this queue.
		/// </summary>
		public void PlaySound() {
			if (!this.AudioStreamPlayers[this.NextIndex].Playing) {
				this.AudioStreamPlayers[this.NextIndex++].Play();
				this.NextIndex %= this.AudioStreamPlayers.Count;
			}
		}
	}
}
