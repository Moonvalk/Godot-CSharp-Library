using System.Collections.Generic;
using Godot;

namespace Moonvalk.Animation {
	public abstract class BaseTextureAnimation<FrameType> : Resource {
		/// <summary>
		/// The name of this animation.
		/// </summary>
		[Export] public string Name { get; protected set; }

		/// <summary>
		/// All textures that will be displayed in order.
		/// </summary>
		[Export] public List<FrameType> Frames { get; protected set; }

		/// <summary>
		/// The duration of each frame in seconds.
		/// </summary>
		[Export] public List<float> FrameDurations { get; protected set; }
	}
}