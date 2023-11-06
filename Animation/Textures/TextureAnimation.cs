using System.Collections.Generic;
using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Animation {
	/// <summary>
	/// Base class for animating textures found 
	/// </summary>
	[RegisteredType(nameof(TextureAnimation), "", nameof(Resource))]
	public class TextureAnimation : Resource {
		/// <summary>
		/// The name of this animation.
		/// </summary>
		[Export] public string Name { get; protected set; }

		/// <summary>
		/// All textures that will be displayed in order.
		/// </summary>
		[Export] public List<Texture> Frames { get; protected set; }

		/// <summary>
		/// The duration of each frame in seconds.
		/// </summary>
		[Export] public List<float> FrameDurations { get; protected set; }
	}
}
