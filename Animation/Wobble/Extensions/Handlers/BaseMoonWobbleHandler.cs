using System;

namespace Moonvalk.Animation {
	/// <summary>
	/// Base class for MoonWobbleHandlers. These are containers that automate updating
	/// Node data each game tick with the use of extension methods.
	/// </summary>
	/// <typeparam name="Unit">The type of unit that will be animated.</typeparam>
	/// <typeparam name="WobbleType">The type of MoonWobble object that will be used.</typeparam>
	/// <typeparam name="ParentType">The type of Node which is being animated.</typeparam>
	public abstract class BaseMoonWobbleHandler<Unit, WobbleType, ParentType> : IMoonWobbleHandler
		where WobbleType : BaseMoonWobble<Unit>, new() {
		/// <summary>
		/// The value used for animating properties.
		/// </summary>
		protected Unit _value;

		/// <summary>
		/// The property this container will animate upon.
		/// </summary>
		public MoonWobbleProperty Property { get; private set; }

		/// <summary>
		/// Any actions that will occur during Wobble updates managed by this container.
		/// </summary>
		public Action OnUpdate { get; private set; }

		/// <summary>
		/// Stores the Wobble object being managed.
		/// </summary>
		public WobbleType Wobble { get; private set; }

		/// <summary>
		/// Constructor for a new handler.
		/// </summary>
		/// <param name="object_">The object that will be animated.</param>
		/// <param name="property_">The property found on object_ that will be manipulated.</param>
		/// <param name="percentage_">The percentage of the property that will be affected. This is useful for
		/// multi-axis values that need to be affected differently.</param>
		/// <param name="parameters_">Properties that determine how the animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <param name="onComplete_">An action to be run when this Wobble is complete. This is primarily used
		/// to remove a Wobble reference once finished.</param>
		protected BaseMoonWobbleHandler(
			ref ParentType object_,
			MoonWobbleProperty property_,
			Unit percentage_,
			MoonWobbleParams parameters_, 
			bool start_,
			Action onComplete_
		) {
			this.bindData(ref object_, property_);
			this.setupWobble(parameters_, percentage_, start_, onComplete_);
		}

		/// <summary>
		/// Gets the Wobble found within this container casted to the requested type if applicable.
		/// </summary>
		/// <typeparam name="Type">The type of value being animated.</typeparam>
		/// <returns>Returns the Wobble casted to the requested type if available.</returns>
		public IMoonWobble<Type> GetWobble<Type>() {
			return (IMoonWobble<Type>)this.Wobble;
		}

		/// <summary>
		/// Deletes the Wobble found within this container.
		/// </summary>
		public void Delete() {
			this.Wobble.Delete();
			this.Wobble = null;
			this.OnUpdate = null;
		}
		
		/// <summary>
		/// Starts the Wobble found within this container.
		/// </summary>
		public void Start() {
			this.Wobble.Start();
		}

		/// <summary>
		/// Called to bind required data to this container on initialization.
		/// </summary>
		/// <param name="object_">The object that this handler will manipulate.</param>
		/// <param name="property_">The property that will be adjusted.</param>
		protected void bindData(ref ParentType object_, MoonWobbleProperty property_) {
			this._value = this.getInitialPropertyValue(ref object_, property_);
			this.OnUpdate = this.assignUpdateAction(ref object_, property_);
		}

		/// <summary>
		/// Called to set up new Wobble objects managed by this container.
		/// </summary>
		/// <param name="parameters_">Parameters used to manipulate how the animation will play.</param>
		/// <param name="percentage_">The percentage of the property that will be affected. This is useful for
		/// multi-axis values that need to be affected differently.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <param name="onComplete_">An action to be run when this Wobble is complete. This is primarily used
		/// to remove a Wobble reference once finished.</param>
		protected void setupWobble(MoonWobbleParams parameters_, Unit percentage_, bool start_, Action onComplete_) {
			this.Wobble = new WobbleType();
			this.Wobble.SetReferences(() => ref this._value).SetParameters(parameters_).SetPercentage(percentage_);
			this.Wobble.OnUpdate(this.OnUpdate).OnComplete(() => {
				onComplete_?.Invoke();
			});
			if (start_) {
				this.Wobble.Start();
			}
		}

		/// <summary>
		/// Called to assign a new Action called each game tick during animations that will
		/// manipulate the Node property.
		/// </summary>
		/// <param name="object_">The object that this handler will manipulate.</param>
		/// <param name="property_">The property to be adjusted.</param>
		/// <returns>Returns a new Action.</returns>
		protected abstract Action assignUpdateAction(ref ParentType object_, MoonWobbleProperty property_);

		/// <summary>
		/// Gets the starting value for the Wobble object to begin at.
		/// </summary>
		/// <param name="object_">The object that this handler will manipulate.</param>
		/// <param name="property_">The property to be adjusted.</param>
		/// <returns>Returns the initial value.</returns>
		protected abstract Unit getInitialPropertyValue(ref ParentType object_, MoonWobbleProperty property_);
	}
}
