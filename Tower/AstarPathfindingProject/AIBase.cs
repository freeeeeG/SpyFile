using System;
using Pathfinding.RVO;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000006 RID: 6
	[RequireComponent(typeof(Seeker))]
	public abstract class AIBase : VersionedMonoBehaviour
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003555 File Offset: 0x00001755
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00003562 File Offset: 0x00001762
		public float repathRate
		{
			get
			{
				return this.autoRepath.period;
			}
			set
			{
				this.autoRepath.period = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003570 File Offset: 0x00001770
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00003580 File Offset: 0x00001780
		public bool canSearch
		{
			get
			{
				return this.autoRepath.mode > AutoRepathPolicy.Mode.Never;
			}
			set
			{
				if (value)
				{
					if (this.autoRepath.mode == AutoRepathPolicy.Mode.Never)
					{
						this.autoRepath.mode = AutoRepathPolicy.Mode.EveryNSeconds;
						return;
					}
				}
				else
				{
					this.autoRepath.mode = AutoRepathPolicy.Mode.Never;
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000035AB File Offset: 0x000017AB
		// (set) Token: 0x06000060 RID: 96 RVA: 0x000035B9 File Offset: 0x000017B9
		[Obsolete("Use the height property instead (2x this value)")]
		public float centerOffset
		{
			get
			{
				return this.height * 0.5f;
			}
			set
			{
				this.height = value * 2f;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000061 RID: 97 RVA: 0x000035C8 File Offset: 0x000017C8
		// (set) Token: 0x06000062 RID: 98 RVA: 0x000035D3 File Offset: 0x000017D3
		[Obsolete("Use orientation instead")]
		public bool rotationIn2D
		{
			get
			{
				return this.orientation == OrientationMode.YAxisForward;
			}
			set
			{
				this.orientation = (value ? OrientationMode.YAxisForward : OrientationMode.ZAxisForward);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000035E2 File Offset: 0x000017E2
		public Vector3 position
		{
			get
			{
				if (!this.updatePosition)
				{
					return this.simulatedPosition;
				}
				return this.tr.position;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000035FE File Offset: 0x000017FE
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000361A File Offset: 0x0000181A
		public Quaternion rotation
		{
			get
			{
				if (!this.updateRotation)
				{
					return this.simulatedRotation;
				}
				return this.tr.rotation;
			}
			set
			{
				if (this.updateRotation)
				{
					this.tr.rotation = value;
					return;
				}
				this.simulatedRotation = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003638 File Offset: 0x00001838
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00003640 File Offset: 0x00001840
		protected bool usingGravity { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000068 RID: 104 RVA: 0x0000364C File Offset: 0x0000184C
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00003674 File Offset: 0x00001874
		[Obsolete("Use the destination property or the AIDestinationSetter component instead")]
		public Transform target
		{
			get
			{
				AIDestinationSetter component = base.GetComponent<AIDestinationSetter>();
				if (!(component != null))
				{
					return null;
				}
				return component.target;
			}
			set
			{
				this.targetCompatibility = null;
				AIDestinationSetter aidestinationSetter = base.GetComponent<AIDestinationSetter>();
				if (aidestinationSetter == null)
				{
					aidestinationSetter = base.gameObject.AddComponent<AIDestinationSetter>();
				}
				aidestinationSetter.target = value;
				this.destination = ((value != null) ? value.position : new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity));
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000036D6 File Offset: 0x000018D6
		// (set) Token: 0x0600006B RID: 107 RVA: 0x000036DE File Offset: 0x000018DE
		public Vector3 destination { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000036E7 File Offset: 0x000018E7
		public Vector3 velocity
		{
			get
			{
				if (this.lastDeltaTime <= 1E-06f)
				{
					return Vector3.zero;
				}
				return (this.prevPosition1 - this.prevPosition2) / this.lastDeltaTime;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003718 File Offset: 0x00001918
		public Vector3 desiredVelocity
		{
			get
			{
				if (this.lastDeltaTime <= 1E-05f)
				{
					return Vector3.zero;
				}
				return this.movementPlane.ToWorld(this.lastDeltaPosition / this.lastDeltaTime, this.verticalVelocity);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000374F File Offset: 0x0000194F
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00003757 File Offset: 0x00001957
		public bool isStopped { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003760 File Offset: 0x00001960
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003768 File Offset: 0x00001968
		public Action onSearchPath { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003771 File Offset: 0x00001971
		protected virtual bool shouldRecalculatePath
		{
			get
			{
				return !this.waitingForPathCalculation && this.autoRepath.ShouldRecalculatePath(this.position, this.radius, this.destination);
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000379C File Offset: 0x0000199C
		protected AIBase()
		{
			this.destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003864 File Offset: 0x00001A64
		public virtual void FindComponents()
		{
			this.tr = base.transform;
			this.seeker = base.GetComponent<Seeker>();
			this.rvoController = base.GetComponent<RVOController>();
			this.controller = base.GetComponent<CharacterController>();
			this.rigid = base.GetComponent<Rigidbody>();
			this.rigid2D = base.GetComponent<Rigidbody2D>();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000038B9 File Offset: 0x00001AB9
		protected virtual void OnEnable()
		{
			this.FindComponents();
			Seeker seeker = this.seeker;
			seeker.pathCallback = (OnPathDelegate)Delegate.Combine(seeker.pathCallback, new OnPathDelegate(this.OnPathComplete));
			this.Init();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000038EF File Offset: 0x00001AEF
		protected virtual void Start()
		{
			this.startHasRun = true;
			this.Init();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000038FE File Offset: 0x00001AFE
		private void Init()
		{
			if (this.startHasRun)
			{
				if (this.canMove)
				{
					this.Teleport(this.position, false);
				}
				this.autoRepath.Reset();
				if (this.shouldRecalculatePath)
				{
					this.SearchPath();
				}
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003938 File Offset: 0x00001B38
		public virtual void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			if (clearPath)
			{
				this.ClearPath();
			}
			this.simulatedPosition = newPosition;
			this.prevPosition2 = newPosition;
			this.prevPosition1 = newPosition;
			if (this.updatePosition)
			{
				this.tr.position = newPosition;
			}
			if (this.rvoController != null)
			{
				this.rvoController.Move(Vector3.zero);
			}
			if (clearPath)
			{
				this.SearchPath();
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000039A2 File Offset: 0x00001BA2
		protected void CancelCurrentPathRequest()
		{
			this.waitingForPathCalculation = false;
			if (this.seeker != null)
			{
				this.seeker.CancelCurrentPathRequest(true);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000039C8 File Offset: 0x00001BC8
		protected virtual void OnDisable()
		{
			this.ClearPath();
			Seeker seeker = this.seeker;
			seeker.pathCallback = (OnPathDelegate)Delegate.Remove(seeker.pathCallback, new OnPathDelegate(this.OnPathComplete));
			this.velocity2D = Vector3.zero;
			this.accumulatedMovementDelta = Vector3.zero;
			this.verticalVelocity = 0f;
			this.lastDeltaTime = 0f;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003A34 File Offset: 0x00001C34
		protected virtual void Update()
		{
			if (this.shouldRecalculatePath)
			{
				this.SearchPath();
			}
			this.usingGravity = (!(this.gravity == Vector3.zero) && (!this.updatePosition || ((this.rigid == null || this.rigid.isKinematic) && (this.rigid2D == null || this.rigid2D.isKinematic))));
			if (this.rigid == null && this.rigid2D == null && this.canMove)
			{
				Vector3 nextPosition;
				Quaternion nextRotation;
				this.MovementUpdate(Time.deltaTime, out nextPosition, out nextRotation);
				this.FinalizeMovement(nextPosition, nextRotation);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003AEC File Offset: 0x00001CEC
		protected virtual void FixedUpdate()
		{
			if ((!(this.rigid == null) || !(this.rigid2D == null)) && this.canMove)
			{
				Vector3 nextPosition;
				Quaternion nextRotation;
				this.MovementUpdate(Time.fixedDeltaTime, out nextPosition, out nextRotation);
				this.FinalizeMovement(nextPosition, nextRotation);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003B34 File Offset: 0x00001D34
		public void MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			this.lastDeltaTime = deltaTime;
			this.MovementUpdateInternal(deltaTime, out nextPosition, out nextRotation);
		}

		// Token: 0x0600007E RID: 126
		protected abstract void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation);

		// Token: 0x0600007F RID: 127 RVA: 0x00003B46 File Offset: 0x00001D46
		protected virtual void CalculatePathRequestEndpoints(out Vector3 start, out Vector3 end)
		{
			start = this.GetFeetPosition();
			end = this.destination;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003B60 File Offset: 0x00001D60
		public virtual void SearchPath()
		{
			if (float.IsPositiveInfinity(this.destination.x))
			{
				return;
			}
			if (this.onSearchPath != null)
			{
				this.onSearchPath();
			}
			Vector3 start;
			Vector3 end;
			this.CalculatePathRequestEndpoints(out start, out end);
			ABPath path = ABPath.Construct(start, end, null);
			this.SetPath(path, false);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003BAE File Offset: 0x00001DAE
		public virtual Vector3 GetFeetPosition()
		{
			return this.position;
		}

		// Token: 0x06000082 RID: 130
		protected abstract void OnPathComplete(Path newPath);

		// Token: 0x06000083 RID: 131
		protected abstract void ClearPath();

		// Token: 0x06000084 RID: 132 RVA: 0x00003BB8 File Offset: 0x00001DB8
		public void SetPath(Path path, bool updateDestinationFromPath = true)
		{
			if (updateDestinationFromPath)
			{
				ABPath abpath = path as ABPath;
				if (abpath != null && !(path is RandomPath))
				{
					this.destination = abpath.originalEndPoint;
				}
			}
			if (path == null)
			{
				this.CancelCurrentPathRequest();
				this.ClearPath();
				return;
			}
			if (path.PipelineState == PathState.Created)
			{
				this.waitingForPathCalculation = true;
				this.seeker.CancelCurrentPathRequest(true);
				this.seeker.StartPath(path, null);
				this.autoRepath.DidRecalculatePath(this.destination);
				return;
			}
			if (path.PipelineState != PathState.Returned)
			{
				throw new ArgumentException("You must call the SetPath method with a path that either has been completely calculated or one whose path calculation has not been started at all. It looks like the path calculation for the path you tried to use has been started, but is not yet finished.");
			}
			if (this.seeker.GetCurrentPath() != path)
			{
				this.seeker.CancelCurrentPathRequest(true);
				this.OnPathComplete(path);
				return;
			}
			throw new ArgumentException("If you calculate the path using seeker.StartPath then this script will pick up the calculated path anyway as it listens for all paths the Seeker finishes calculating. You should not call SetPath in that case.");
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003C74 File Offset: 0x00001E74
		protected void ApplyGravity(float deltaTime)
		{
			if (this.usingGravity)
			{
				float num;
				this.velocity2D += this.movementPlane.ToPlane(deltaTime * (float.IsNaN(this.gravity.x) ? Physics.gravity : this.gravity), out num);
				this.verticalVelocity += num;
				return;
			}
			this.verticalVelocity = 0f;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003CE8 File Offset: 0x00001EE8
		protected Vector2 CalculateDeltaToMoveThisFrame(Vector2 position, float distanceToEndOfPath, float deltaTime)
		{
			if (this.rvoController != null && this.rvoController.enabled)
			{
				return this.movementPlane.ToPlane(this.rvoController.CalculateMovementDelta(this.movementPlane.ToWorld(position, 0f), deltaTime));
			}
			return Vector2.ClampMagnitude(this.velocity2D * deltaTime, distanceToEndOfPath);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003D4B File Offset: 0x00001F4B
		public Quaternion SimulateRotationTowards(Vector3 direction, float maxDegrees)
		{
			return this.SimulateRotationTowards(this.movementPlane.ToPlane(direction), maxDegrees);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003D60 File Offset: 0x00001F60
		protected Quaternion SimulateRotationTowards(Vector2 direction, float maxDegrees)
		{
			if (direction != Vector2.zero)
			{
				Quaternion quaternion = Quaternion.LookRotation(this.movementPlane.ToWorld(direction, 0f), this.movementPlane.ToWorld(Vector2.zero, 1f));
				if (this.orientation == OrientationMode.YAxisForward)
				{
					quaternion *= Quaternion.Euler(90f, 0f, 0f);
				}
				return Quaternion.RotateTowards(this.simulatedRotation, quaternion, maxDegrees);
			}
			return this.simulatedRotation;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003DDE File Offset: 0x00001FDE
		public virtual void Move(Vector3 deltaPosition)
		{
			this.accumulatedMovementDelta += deltaPosition;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003DF2 File Offset: 0x00001FF2
		public virtual void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation)
		{
			if (this.enableRotation)
			{
				this.FinalizeRotation(nextRotation);
			}
			this.FinalizePosition(nextPosition);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003E0C File Offset: 0x0000200C
		private void FinalizeRotation(Quaternion nextRotation)
		{
			this.simulatedRotation = nextRotation;
			if (this.updateRotation)
			{
				if (this.rigid != null)
				{
					this.rigid.MoveRotation(nextRotation);
					return;
				}
				if (this.rigid2D != null)
				{
					this.rigid2D.MoveRotation(nextRotation.eulerAngles.z);
					return;
				}
				this.tr.rotation = nextRotation;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003E78 File Offset: 0x00002078
		private void FinalizePosition(Vector3 nextPosition)
		{
			Vector3 vector = this.simulatedPosition;
			bool flag = false;
			if (this.controller != null && this.controller.enabled && this.updatePosition)
			{
				this.tr.position = vector;
				this.controller.Move(nextPosition - vector + this.accumulatedMovementDelta);
				vector = this.tr.position;
				if (this.controller.isGrounded)
				{
					this.verticalVelocity = 0f;
				}
			}
			else
			{
				float lastElevation;
				this.movementPlane.ToPlane(vector, out lastElevation);
				vector = nextPosition + this.accumulatedMovementDelta;
				if (this.usingGravity)
				{
					vector = this.RaycastPosition(vector, lastElevation);
				}
				flag = true;
			}
			bool flag2 = false;
			vector = this.ClampToNavmesh(vector, out flag2);
			if ((flag || flag2) && this.updatePosition)
			{
				if (this.rigid != null)
				{
					this.rigid.MovePosition(vector);
				}
				else if (this.rigid2D != null)
				{
					this.rigid2D.MovePosition(vector);
				}
				else
				{
					this.tr.position = vector;
				}
			}
			this.accumulatedMovementDelta = Vector3.zero;
			this.simulatedPosition = vector;
			this.UpdateVelocity();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003FAC File Offset: 0x000021AC
		protected void UpdateVelocity()
		{
			int frameCount = Time.frameCount;
			if (frameCount != this.prevFrame)
			{
				this.prevPosition2 = this.prevPosition1;
			}
			this.prevPosition1 = this.position;
			this.prevFrame = frameCount;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003FE7 File Offset: 0x000021E7
		protected virtual Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
		{
			positionChanged = false;
			return position;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003FF0 File Offset: 0x000021F0
		protected Vector3 RaycastPosition(Vector3 position, float lastElevation)
		{
			float num;
			this.movementPlane.ToPlane(position, out num);
			float num2 = this.tr.localScale.y * this.height * 0.5f + Mathf.Max(0f, lastElevation - num);
			Vector3 vector = this.movementPlane.ToWorld(Vector2.zero, num2);
			RaycastHit raycastHit;
			if (Physics.Raycast(position + vector, -vector, out raycastHit, num2, this.groundMask, QueryTriggerInteraction.Ignore))
			{
				this.verticalVelocity *= Math.Max(0f, 1f - 5f * this.lastDeltaTime);
				return raycastHit.point;
			}
			return position;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000409F File Offset: 0x0000229F
		protected virtual void OnDrawGizmosSelected()
		{
			if (Application.isPlaying)
			{
				this.FindComponents();
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000040B0 File Offset: 0x000022B0
		protected virtual void OnDrawGizmos()
		{
			if (!Application.isPlaying || !base.enabled)
			{
				this.FindComponents();
			}
			Color color = AIBase.ShapeGizmoColor;
			if (this.rvoController != null && this.rvoController.locked)
			{
				color *= 0.5f;
			}
			if (this.orientation == OrientationMode.YAxisForward)
			{
				Draw.Gizmos.Cylinder(this.position, Vector3.forward, 0f, this.radius * this.tr.localScale.x, color);
			}
			else
			{
				Draw.Gizmos.Cylinder(this.position, this.rotation * Vector3.up, this.tr.localScale.y * this.height, this.radius * this.tr.localScale.x, color);
			}
			if (!float.IsPositiveInfinity(this.destination.x) && Application.isPlaying)
			{
				Draw.Gizmos.CircleXZ(this.destination, 0.2f, Color.blue, 0f, 6.2831855f);
			}
			this.autoRepath.DrawGizmos(this.position, this.radius);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000041DF File Offset: 0x000023DF
		protected override void Reset()
		{
			this.ResetShape();
			base.Reset();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000041F0 File Offset: 0x000023F0
		private void ResetShape()
		{
			CharacterController component = base.GetComponent<CharacterController>();
			if (component != null)
			{
				this.radius = component.radius;
				this.height = Mathf.Max(this.radius * 2f, component.height);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004238 File Offset: 0x00002438
		protected override int OnUpgradeSerializedData(int version, bool unityThread)
		{
			if (unityThread && !float.IsNaN(this.centerOffsetCompatibility))
			{
				this.height = this.centerOffsetCompatibility * 2f;
				this.ResetShape();
				RVOController component = base.GetComponent<RVOController>();
				if (component != null)
				{
					this.radius = component.radiusBackingField;
				}
				this.centerOffsetCompatibility = float.NaN;
			}
			if (unityThread && this.targetCompatibility != null)
			{
				this.target = this.targetCompatibility;
			}
			if (version <= 3)
			{
				this.repathRate = this.repathRateCompatibility;
				this.canSearch = this.canSearchCompability;
			}
			return 5;
		}

		// Token: 0x04000044 RID: 68
		public float radius = 0.5f;

		// Token: 0x04000045 RID: 69
		public float height = 2f;

		// Token: 0x04000046 RID: 70
		public bool canMove = true;

		// Token: 0x04000047 RID: 71
		[FormerlySerializedAs("speed")]
		public float maxSpeed = 1f;

		// Token: 0x04000048 RID: 72
		public Vector3 gravity = new Vector3(float.NaN, float.NaN, float.NaN);

		// Token: 0x04000049 RID: 73
		public LayerMask groundMask = -1;

		// Token: 0x0400004A RID: 74
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("centerOffset")]
		private float centerOffsetCompatibility = float.NaN;

		// Token: 0x0400004B RID: 75
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("repathRate")]
		private float repathRateCompatibility = float.NaN;

		// Token: 0x0400004C RID: 76
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("canSearch")]
		[FormerlySerializedAs("repeatedlySearchPaths")]
		private bool canSearchCompability;

		// Token: 0x0400004D RID: 77
		[FormerlySerializedAs("rotationIn2D")]
		public OrientationMode orientation;

		// Token: 0x0400004E RID: 78
		public bool enableRotation = true;

		// Token: 0x0400004F RID: 79
		protected Vector3 simulatedPosition;

		// Token: 0x04000050 RID: 80
		protected Quaternion simulatedRotation;

		// Token: 0x04000051 RID: 81
		private Vector3 accumulatedMovementDelta = Vector3.zero;

		// Token: 0x04000052 RID: 82
		protected Vector2 velocity2D;

		// Token: 0x04000053 RID: 83
		protected float verticalVelocity;

		// Token: 0x04000054 RID: 84
		protected Seeker seeker;

		// Token: 0x04000055 RID: 85
		protected Transform tr;

		// Token: 0x04000056 RID: 86
		protected Rigidbody rigid;

		// Token: 0x04000057 RID: 87
		protected Rigidbody2D rigid2D;

		// Token: 0x04000058 RID: 88
		protected CharacterController controller;

		// Token: 0x04000059 RID: 89
		protected RVOController rvoController;

		// Token: 0x0400005A RID: 90
		public IMovementPlane movementPlane = GraphTransform.identityTransform;

		// Token: 0x0400005B RID: 91
		[NonSerialized]
		public bool updatePosition = true;

		// Token: 0x0400005C RID: 92
		[NonSerialized]
		public bool updateRotation = true;

		// Token: 0x0400005D RID: 93
		public AutoRepathPolicy autoRepath = new AutoRepathPolicy();

		// Token: 0x0400005F RID: 95
		protected float lastDeltaTime;

		// Token: 0x04000060 RID: 96
		protected int prevFrame;

		// Token: 0x04000061 RID: 97
		protected Vector3 prevPosition1;

		// Token: 0x04000062 RID: 98
		protected Vector3 prevPosition2;

		// Token: 0x04000063 RID: 99
		protected Vector2 lastDeltaPosition;

		// Token: 0x04000064 RID: 100
		protected bool waitingForPathCalculation;

		// Token: 0x04000065 RID: 101
		[FormerlySerializedAs("target")]
		[SerializeField]
		[HideInInspector]
		private Transform targetCompatibility;

		// Token: 0x04000066 RID: 102
		private bool startHasRun;

		// Token: 0x0400006A RID: 106
		public static readonly Color ShapeGizmoColor = new Color(0.9411765f, 0.8352941f, 0.11764706f);
	}
}
