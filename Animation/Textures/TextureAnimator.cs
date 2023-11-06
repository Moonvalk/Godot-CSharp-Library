using System.Collections.Generic;
using Godot;
using Moonvalk.Utilities;

namespace Moonvalk.Animation {
	/// <summary>
	/// Handler for animating textures on a mesh instance.
	/// </summary>
	public class TextureAnimator : Node {
		#region Data Fields
		/// <summary>
		/// Path to the mesh instance that will have its texture adjusted.
		/// </summary>
		[Export] protected NodePath p_meshInstance { get; set; }

		/// <summary>
		/// Stores references to all available animations.
		/// </summary>
		[Export] public List<TextureAnimation> Animations { get; protected set; }

		/// <summary>
		/// Stores the current state of this animator.
		/// </summary>
		public TextureAnimatorState CurrentState { get; protected set; } = TextureAnimatorState.Stopped;

		/// <summary>
		/// Reference to the mesh instance that will have its texture adjusted.
		/// </summary>
		public MeshInstance Mesh { get; protected set; }
		
		/// <summary>
		/// Stores the current animation being played.
		/// </summary>
		public TextureAnimation CurrentAnimation { get; protected set; }

		/// <summary>
		/// Stores the index of the current frame.
		/// </summary>
		public int CurrentFrame { get; protected set; } = 0;
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready() {
			this.Mesh = this.GetNode<MeshInstance>(p_meshInstance);
			if (this.Animations.Count > 0) {
				this.CurrentAnimation = this.Animations[0];
				this.Play();
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Called to play the current animation.
		/// </summary>
		public void Play() {
			this.CurrentState = TextureAnimatorState.Playing;
			this.CurrentFrame = this.CurrentAnimation.Frames.Count - 1;
			this.NextFrame();
		}

		/// <summary>
		/// Called to jump to the next available frame.
		/// </summary>
		public void NextFrame() {
			if (this.CurrentState == TextureAnimatorState.Stopped) {
				return;
			}
			this.CurrentFrame++;
			if (this.CurrentFrame > this.CurrentAnimation.Frames.Count - 1) {
				this.CurrentFrame = 0;
			}
			this.adjustTexture();
			MoonTimer.Wait(this.CurrentAnimation.FrameDurations[this.CurrentFrame], this.NextFrame);
		}

		/// <summary>
		/// Called to stop the current animation.
		/// </summary>
		public void Stop() {
			this.CurrentState = TextureAnimatorState.Stopped;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Adjusts the texture on the stored mesh instance.
		/// </summary>
		protected void adjustTexture() {
			SpatialMaterial material = (this.Mesh.GetActiveMaterial(0) as SpatialMaterial);
			material.AlbedoTexture = this.CurrentAnimation.Frames[this.CurrentFrame];
		}
		#endregion
	}
}
