using System;
using System.Collections.Generic;
using Moonvalk.Accessory;

namespace Moonvalk.Animation {
	/// <summary>
	/// Base class for Spring objects.
	/// </summary>
	/// <typeparam name="Unit">The type of value that will be affected by Spring forces</typeparam>
	public abstract class BaseMoonSpring<Unit> : IMoonSpring<Unit> {
		#region Data Fields
		/// <summary>
		/// A reference to the property value(s) that will be modified.
		/// </summary>
		public Ref<Unit>[] Properties { get; private set; }

		/// <summary>
		/// The target value that will be reached.
		/// </summary>
		public Unit[] TargetProperties { get; private set; }

		/// <summary>
		/// The tension value applied to this spring.
		/// </summary>
		public float Tension { get; private set; } = 50f;

		/// <summary>
		/// The dampening value applied to this spring.
		/// </summary>
		public float Dampening { get; private set; } = 10f;

		/// <summary>
		/// The current speed applied to this spring.
		/// </summary>
		public Unit[] Speed { get; private set; }

		/// <summary>
		/// The amount of force to be applied each frame.
		/// </summary>
		public Unit[] CurrentForce { get; private set; }

		/// <summary>
		/// The minimum force applied to a Spring before it is no longer updated until settings change.
		/// </summary>
		public Unit[] MinimumForce { get; protected set; }

		/// <summary>
		/// The default percentage of total distance springed that will be assigned as a minimum force.
		/// </summary>
		protected const float _defaultMinimumForcePercentage = 0.0001f;

		/// <summary>
		/// The current state of this Spring object.
		/// </summary>
		public MoonSpringState CurrentState { get; private set; } = MoonSpringState.Stopped;

		/// <summary>
		/// Should this animation begin as soon as a Target value is assigned?
		/// </summary>
		public bool StartOnTargetAssigned { get; set; } = false;

		/// <summary>
		/// A map of Actions that will occur while this Spring is in an active state.
		/// </summary>
		protected Dictionary<MoonSpringState, InitValue<List<Action>>> _functions;

		/// <summary>
		/// Stores reference to custom Springs applied to user generated values.
		/// </summary>
		public static Dictionary<Ref<Unit>, BaseMoonSpring<Unit>> CustomSprings { get; protected set; }
		#endregion

		#region Constructor(s)
		/// <summary>
		/// Default constructor made without setting up references.
		/// </summary>
		protected BaseMoonSpring() {
			this.setup();
		}

		/// <summary>
		/// Constructor for creating a new BaseSpring.
		/// </summary>
		/// <param name="referenceValues_">Array of references to values.</param>
		protected BaseMoonSpring(params Ref<Unit>[] referenceValues_) {
			this.SetReferences(referenceValues_);
			this.setup();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Sets all reference values that this Spring will manipulate.
		/// </summary>
		/// <param name="referenceValues_">Array of references to values.</param>
		public BaseMoonSpring<Unit> SetReferences(params Ref<Unit>[] referenceValues_) {
			// Store reference to properties and build function maps.
			this.Properties = referenceValues_;
			
			// Create new array for storing property targets.
			this.TargetProperties = new Unit[referenceValues_.Length];
			this.CurrentForce = new Unit[referenceValues_.Length];
			this.Speed = new Unit[referenceValues_.Length];
			return this;
		}

		/// <summary>
		/// Updates this Spring.
		/// </summary>
		/// <param name="deltaTime_">The duration of time between last and current game tick.</param>
		/// <returns>Returns true when this Spring is active and false when it is complete.</returns>
		public bool Update(float deltaTime_) {
			if (this.CurrentState == MoonSpringState.Complete) {
				return false;
			}
			if (this.CurrentState == MoonSpringState.Stopped) {
				return true;
			}
			this.CurrentState = MoonSpringState.Update;
			this.handleTasks(this.CurrentState);

			// Update springs each frame until settled.
			this.calculateForces();
			this.applyForces(deltaTime_);
			if (!this.minimumForcesMet()) {
				this.snapSpringToTarget();
				this.CurrentState = MoonSpringState.Complete;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Starts this Spring with the current settings if there is a need to apply forces.
		/// </summary>
		/// <returns>Returns reference to this Spring.</returns>
		public BaseMoonSpring<Unit> Start() {
			if (this.needToApplyForce())  {
				this.CurrentState = MoonSpringState.Start;
				this.handleTasks(this.CurrentState);
				(Global.GetSystem<MoonSpringSystem>() as MoonSpringSystem).Add(this);
			}
			return this;
		}

		/// <summary>
		/// Stops this Spring.
		/// </summary>
		/// <returns>Returns reference to this Spring.</returns>
		public BaseMoonSpring<Unit> Stop() {
			this.CurrentState = MoonSpringState.Stopped;
			return this;
		}

		/// <summary>
		/// Sets the dampening factor applied to this spring.
		/// </summary>
		/// <param name="dampening_">New dampening factor.</param>
		/// <returns>Returns reference to this spring.</returns>
		public BaseMoonSpring<Unit> SetDampening(float dampening_) {
			this.Dampening = dampening_;
			return this;
		}

		/// <summary>
		/// Sets the tension factor applied to this spring.
		/// </summary>
		/// <param name="tension_">New tension factor.</param>
		/// <returns>Returns reference to this spring.</returns>
		public BaseMoonSpring<Unit> SetTension(float tension_) {
			this.Tension = tension_;
			return this;
		}

		/// <summary>
		/// Called to set all parameters from a reference object.
		/// </summary>
		/// <param name="parameters_">All properties that will be assigned.</param>
		/// <returns>Returns this Spring object.</returns>
		public BaseMoonSpring<Unit> SetParameters(MoonSpringParams parameters_) {
			this.SetDampening(parameters_.Dampening).SetTension(parameters_.Tension);
			return this;
		}

		/// <summary>
		/// Applies a new target spring height and begins animating towards reaching that value.
		/// </summary>
		/// <param name="targetProperties_">Target spring heights for each property.</param>
		/// <returns>Returns reference to this spring.</returns>
		public BaseMoonSpring<Unit> To(params Unit[] targetProperties_) {
			this.TargetProperties = targetProperties_;
			this.setMinimumForce();
			if (this.StartOnTargetAssigned) {
				this.Start();
			}
			return this;
		}

		/// <summary>
		/// Snaps each spring property to the provided target values.
		/// </summary>
		/// <param name="targetProperties_">Target spring heights for each property.</param>
		/// <returns>Returns reference to this spring.</returns>
		public BaseMoonSpring<Unit> Snap(params Unit[] targetProperties_) {
			this.TargetProperties = targetProperties_;
			this.snapSpringToTarget();
			return this;
		}

		/// <summary>
		/// Removes this Spring on the following game tick by forcing completion.
		/// </summary>
		/// <returns>Returns reference to this Spring.</returns>
		public BaseMoonSpring<Unit> Delete() {
			this.CurrentState = MoonSpringState.Complete;
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur when this Spring begins.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Spring object.</returns>
		public BaseMoonSpring<Unit> OnStart(params Action[] tasksToAdd_) {
			addTasks(MoonSpringState.Start, tasksToAdd_);
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur when this Spring updates.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Spring object.</returns>
		public BaseMoonSpring<Unit> OnUpdate(params Action[] tasksToAdd_) {
			addTasks(MoonSpringState.Update, tasksToAdd_);
			return this;
		}

		/// <summary>
		/// Defines Actions that will occur once this Spring has completed.
		/// </summary>
		/// <param name="tasksToAdd_">Array of Actions to add.</param>
		/// <returns>Returns this Spring object.</returns>
		public BaseMoonSpring<Unit> OnComplete(params Action[] tasksToAdd_) {
			addTasks(MoonSpringState.Complete, tasksToAdd_);
			return this;
		}

		/// <summary>
		/// Handles all tasks for the current state of this base spring.
		/// </summary>
		public void HandleTasks() {
			this.handleTasks(this.CurrentState);
		}

		/// <summary>
		/// Gets the current state of this Spring.
		/// </summary>
		/// <returns>Returns the current state.</returns>
		public MoonSpringState GetCurrentState() {
			return this.CurrentState;
		}

		/// <summary>
		/// Initializes a custom Spring based on a reference value as a property.
		/// </summary>
		/// <typeparam name="SpringType">The type of Spring that will be used.</typeparam>
		/// <param name="referenceValue_">The property to be animated.</param>
		/// <param name="target_">The target value.</param>
		/// <param name="parameters_">Properties that adjust how this animation will look.</param>
		/// <param name="start_">Flag that determines if this animation should begin immediately.</param>
		/// <returns>Returns reference to the new Spring object.</returns>
		public static BaseMoonSpring<Unit> CustomSpringTo<SpringType>(
			Ref<Unit> referenceValue_,
			Unit target_,
			MoonSpringParams parameters_ = null,
			bool start_ = true
		) where SpringType : BaseMoonSpring<Unit>, new() {
			BaseMoonSpring<Unit>.CustomSprings = BaseMoonSpring<Unit>.CustomSprings ?? new Dictionary<Ref<Unit>, BaseMoonSpring<Unit>>();
			if (BaseMoonSpring<Unit>.CustomSprings.ContainsKey(referenceValue_)) {
				BaseMoonSpring<Unit>.CustomSprings[referenceValue_].Delete();
				BaseMoonSpring<Unit>.CustomSprings.Remove(referenceValue_);
			}
			BaseMoonSpring<Unit> tween = new SpringType() { StartOnTargetAssigned = start_ };
			tween.SetReferences(referenceValue_).SetParameters(parameters_ ?? new MoonSpringParams()).OnComplete(() => {
				BaseMoonSpring<Unit>.CustomSprings.Remove(referenceValue_);
			}).To(target_);

			BaseMoonSpring<Unit>.CustomSprings.Add(referenceValue_, tween);
			return tween;
		}

		/// <summary>
		/// Gets a custom Spring object for the provided reference value, if it exists.
		/// </summary>
		/// <typeparam name="Unit">The type of used for this reference value.</typeparam>
		/// <param name="referenceValue_">The reference value a Spring object is applied to.</param>
		/// <returns>Returns the requested Spring object if it exists or null if it cannot be found.</returns>
		public static BaseMoonSpring<Unit> GetCustomSpring(Ref<Unit> referenceValue_) {
			if (BaseMoonSpring<Unit>.CustomSprings.ContainsKey(referenceValue_)) {
				return BaseMoonSpring<Unit>.CustomSprings[referenceValue_];
			}
			return null;
		}

		/// <summary>
		/// Returns true when this object is complete.
		/// </summary>
		/// <returns>True when state is complete.</returns>
		public bool IsComplete() {
			return this.CurrentState == MoonSpringState.Complete;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Calculates the necessary velocities to be applied to all Spring properties each game tick.
		/// </summary>
		protected abstract void calculateForces();

		/// <summary>
		/// Applies force to properties each frame.
		/// </summary>
		/// <param name="deltaTime_">The time elapsed between last and current game tick.</param>
		protected abstract void applyForces(float deltaTime_);

		/// <summary>
		/// Determines if the minimum forces have been met to continue calculating Spring forces.
		/// </summary>
		/// <returns>Returns true if the minimum forces have been met.</returns>
		protected abstract bool minimumForcesMet();

		/// <summary>
		/// Determines if there is a need to apply force to this Spring to meet target values.
		/// </summary>
		/// <returns>Returns true if forces need to be applied</returns>
		protected abstract bool needToApplyForce();

		/// <summary>
		/// Assigns the minimum force required until the Spring is completed based on inputs.
		/// </summary>
		protected abstract void setMinimumForce();

		/// <summary>
		/// Snaps all Spring properties directly to their target values. 
		/// </summary>
		protected void snapSpringToTarget() {
			for (int index = 0; index < this.Properties.Length; index++) {
				this.Properties[index]() = this.TargetProperties[index];
			}
		}

		/// <summary>
		/// Adds an array of new Actions to a SpringState.
		/// </summary>
		/// <param name="state_">The SpringState to add tasks for.</param>
		/// <param name="tasksToAdd_">The tasks to add.</param>
		protected void addTasks(MoonSpringState state_, params Action[] tasksToAdd_) {
			_functions[state_].Value.Clear();
			foreach (Action task in tasksToAdd_) {
				_functions[state_].Value.Add(task);
			}
		}

		/// <summary>
		/// Handles all tasks for the specified SpringState.
		/// </summary>
		/// <param name="state_">The state to run tasks for.</param>
		protected void handleTasks(MoonSpringState state_) {
			for (int index = 0; index < _functions[state_].Value.Count; index++) {
				this._functions[state_].Value[index]();
			}
		}

		/// <summary>
		/// Sets up this container for handling Spring forces.
		/// </summary>
		protected void setup() {
			// Build function maps.
			this._functions = new Dictionary<MoonSpringState, InitValue<List<Action>>>();
			foreach (MoonSpringState state in Enum.GetValues(typeof(MoonSpringState))) {
				this._functions.Add(state, new InitValue<List<Action>>(() => { return new List<Action>(); }));
			}
		}
		#endregion
	}
}
