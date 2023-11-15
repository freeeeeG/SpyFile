using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000007 RID: 7
	[RequireComponent(typeof(Seeker))]
	[AddComponentMenu("Pathfinding/AI/AILerp (2D,3D)")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_a_i_lerp.php")]
	public class AILerp : VersionedMonoBehaviour, IAstarAI
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000042E9 File Offset: 0x000024E9
		// (set) Token: 0x06000097 RID: 151 RVA: 0x000042F6 File Offset: 0x000024F6
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

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004304 File Offset: 0x00002504
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004314 File Offset: 0x00002514
		public bool canSearch
		{
			get
			{
				return this.autoRepath.mode > AutoRepathPolicy.Mode.Never;
			}
			set
			{
				this.autoRepath.mode = (value ? AutoRepathPolicy.Mode.EveryNSeconds : AutoRepathPolicy.Mode.Never);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004328 File Offset: 0x00002528
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00004333 File Offset: 0x00002533
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

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004342 File Offset: 0x00002542
		// (set) Token: 0x0600009D RID: 157 RVA: 0x0000434A File Offset: 0x0000254A
		public bool reachedEndOfPath { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004354 File Offset: 0x00002554
		public bool reachedDestination
		{
			get
			{
				if (!this.reachedEndOfPath || !this.interpolator.valid)
				{
					return false;
				}
				Vector3 vector = this.destination - this.interpolator.endPoint;
				if (this.orientation == OrientationMode.YAxisForward)
				{
					vector.z = 0f;
				}
				else
				{
					vector.y = 0f;
				}
				return this.remainingDistance + vector.magnitude < 0.05f;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000043CA File Offset: 0x000025CA
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x000043D2 File Offset: 0x000025D2
		public Vector3 destination { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000043DC File Offset: 0x000025DC
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004404 File Offset: 0x00002604
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004466 File Offset: 0x00002666
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

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004482 File Offset: 0x00002682
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000449E File Offset: 0x0000269E
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

		// Token: 0x060000A6 RID: 166 RVA: 0x000044BC File Offset: 0x000026BC
		void IAstarAI.Move(Vector3 deltaPosition)
		{
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000044BE File Offset: 0x000026BE
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x000044C5 File Offset: 0x000026C5
		float IAstarAI.radius
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000044C7 File Offset: 0x000026C7
		// (set) Token: 0x060000AA RID: 170 RVA: 0x000044CE File Offset: 0x000026CE
		float IAstarAI.height
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000044D0 File Offset: 0x000026D0
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000044D8 File Offset: 0x000026D8
		float IAstarAI.maxSpeed
		{
			get
			{
				return this.speed;
			}
			set
			{
				this.speed = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000044E1 File Offset: 0x000026E1
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000044E9 File Offset: 0x000026E9
		bool IAstarAI.canSearch
		{
			get
			{
				return this.canSearch;
			}
			set
			{
				this.canSearch = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000044F2 File Offset: 0x000026F2
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000044FA File Offset: 0x000026FA
		bool IAstarAI.canMove
		{
			get
			{
				return this.canMove;
			}
			set
			{
				this.canMove = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004503 File Offset: 0x00002703
		public Vector3 velocity
		{
			get
			{
				if (Time.deltaTime <= 1E-05f)
				{
					return Vector3.zero;
				}
				return (this.previousPosition1 - this.previousPosition2) / Time.deltaTime;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004532 File Offset: 0x00002732
		Vector3 IAstarAI.desiredVelocity
		{
			get
			{
				return ((IAstarAI)this).velocity;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000453A File Offset: 0x0000273A
		Vector3 IAstarAI.steeringTarget
		{
			get
			{
				if (!this.interpolator.valid)
				{
					return this.simulatedPosition;
				}
				return this.interpolator.position + this.interpolator.tangent;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000456B File Offset: 0x0000276B
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00004582 File Offset: 0x00002782
		public float remainingDistance
		{
			get
			{
				return Mathf.Max(this.interpolator.remainingDistance, 0f);
			}
			set
			{
				this.interpolator.remainingDistance = Mathf.Max(value, 0f);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000459A File Offset: 0x0000279A
		public bool hasPath
		{
			get
			{
				return this.interpolator.valid;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000045A7 File Offset: 0x000027A7
		public bool pathPending
		{
			get
			{
				return !this.canSearchAgain;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000045B2 File Offset: 0x000027B2
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x000045BA File Offset: 0x000027BA
		public bool isStopped { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000045C3 File Offset: 0x000027C3
		// (set) Token: 0x060000BB RID: 187 RVA: 0x000045CB File Offset: 0x000027CB
		public Action onSearchPath { get; set; }

		// Token: 0x060000BC RID: 188 RVA: 0x000045D4 File Offset: 0x000027D4
		protected AILerp()
		{
			this.destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000466D File Offset: 0x0000286D
		protected override void Awake()
		{
			base.Awake();
			this.tr = base.transform;
			this.seeker = base.GetComponent<Seeker>();
			this.seeker.startEndModifier.adjustStartPoint = (() => this.simulatedPosition);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000046A9 File Offset: 0x000028A9
		protected virtual void Start()
		{
			this.startHasRun = true;
			this.Init();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000046B8 File Offset: 0x000028B8
		protected virtual void OnEnable()
		{
			Seeker seeker = this.seeker;
			seeker.pathCallback = (OnPathDelegate)Delegate.Combine(seeker.pathCallback, new OnPathDelegate(this.OnPathComplete));
			this.Init();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000046E8 File Offset: 0x000028E8
		private void Init()
		{
			if (this.startHasRun)
			{
				this.Teleport(this.position, false);
				this.autoRepath.Reset();
				if (this.shouldRecalculatePath)
				{
					this.SearchPath();
				}
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004718 File Offset: 0x00002918
		public void OnDisable()
		{
			this.ClearPath();
			Seeker seeker = this.seeker;
			seeker.pathCallback = (OnPathDelegate)Delegate.Remove(seeker.pathCallback, new OnPathDelegate(this.OnPathComplete));
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004748 File Offset: 0x00002948
		public void GetRemainingPath(List<Vector3> buffer, out bool stale)
		{
			buffer.Clear();
			if (!this.interpolator.valid)
			{
				buffer.Add(this.position);
				stale = true;
				return;
			}
			stale = false;
			this.interpolator.GetRemainingPath(buffer);
			buffer[0] = this.position;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004794 File Offset: 0x00002994
		public void Teleport(Vector3 position, bool clearPath = true)
		{
			if (clearPath)
			{
				this.ClearPath();
			}
			this.previousPosition2 = position;
			this.previousPosition1 = position;
			this.simulatedPosition = position;
			if (this.updatePosition)
			{
				this.tr.position = position;
			}
			this.reachedEndOfPath = false;
			if (clearPath)
			{
				this.SearchPath();
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000047E7 File Offset: 0x000029E7
		protected virtual bool shouldRecalculatePath
		{
			get
			{
				return this.canSearchAgain && this.autoRepath.ShouldRecalculatePath(this.position, 0f, this.destination);
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000480F File Offset: 0x00002A0F
		[Obsolete("Use SearchPath instead")]
		public virtual void ForceSearchPath()
		{
			this.SearchPath();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004818 File Offset: 0x00002A18
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
			Vector3 feetPosition = this.GetFeetPosition();
			this.canSearchAgain = false;
			this.SetPath(ABPath.Construct(feetPosition, this.destination, null), false);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000486D File Offset: 0x00002A6D
		public virtual void OnTargetReached()
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004870 File Offset: 0x00002A70
		protected virtual void OnPathComplete(Path _p)
		{
			ABPath abpath = _p as ABPath;
			if (abpath == null)
			{
				throw new Exception("This function only handles ABPaths, do not use special path types");
			}
			this.canSearchAgain = true;
			abpath.Claim(this);
			if (abpath.error)
			{
				abpath.Release(this, false);
				return;
			}
			if (this.interpolatePathSwitches)
			{
				this.ConfigurePathSwitchInterpolation();
			}
			ABPath abpath2 = this.path;
			this.path = abpath;
			this.reachedEndOfPath = false;
			RandomPath randomPath = this.path as RandomPath;
			if (randomPath != null)
			{
				this.destination = randomPath.originalEndPoint;
			}
			else
			{
				MultiTargetPath multiTargetPath = this.path as MultiTargetPath;
				if (multiTargetPath != null)
				{
					this.destination = multiTargetPath.originalEndPoint;
				}
			}
			if (this.path.vectorPath != null && this.path.vectorPath.Count == 1)
			{
				this.path.vectorPath.Insert(0, this.GetFeetPosition());
			}
			this.ConfigureNewPath();
			if (abpath2 != null)
			{
				abpath2.Release(this, false);
			}
			if (this.interpolator.remainingDistance < 0.0001f && !this.reachedEndOfPath)
			{
				this.reachedEndOfPath = true;
				this.OnTargetReached();
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000497C File Offset: 0x00002B7C
		protected virtual void ClearPath()
		{
			if (this.seeker != null)
			{
				this.seeker.CancelCurrentPathRequest(true);
			}
			this.canSearchAgain = true;
			this.reachedEndOfPath = false;
			if (this.path != null)
			{
				this.path.Release(this, false);
			}
			this.path = null;
			this.interpolator.SetPath(null);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000049DC File Offset: 0x00002BDC
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
				this.ClearPath();
				return;
			}
			if (path.PipelineState == PathState.Created)
			{
				this.canSearchAgain = false;
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

		// Token: 0x060000CB RID: 203 RVA: 0x00004A94 File Offset: 0x00002C94
		protected virtual void ConfigurePathSwitchInterpolation()
		{
			bool flag = this.interpolator.valid && this.interpolator.remainingDistance < 0.0001f;
			if (this.interpolator.valid && !flag)
			{
				this.previousMovementOrigin = this.interpolator.position;
				this.previousMovementDirection = this.interpolator.tangent.normalized * this.interpolator.remainingDistance;
				this.pathSwitchInterpolationTime = 0f;
				return;
			}
			this.previousMovementOrigin = Vector3.zero;
			this.previousMovementDirection = Vector3.zero;
			this.pathSwitchInterpolationTime = float.PositiveInfinity;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004B3B File Offset: 0x00002D3B
		public virtual Vector3 GetFeetPosition()
		{
			return this.position;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004B44 File Offset: 0x00002D44
		protected virtual void ConfigureNewPath()
		{
			bool valid = this.interpolator.valid;
			Vector3 vector = valid ? this.interpolator.tangent : Vector3.zero;
			this.interpolator.SetPath(this.path.vectorPath);
			this.interpolator.MoveToClosestPoint(this.GetFeetPosition());
			if (this.interpolatePathSwitches && this.switchPathInterpolationSpeed > 0.01f && valid)
			{
				float num = Mathf.Max(-Vector3.Dot(vector.normalized, this.interpolator.tangent.normalized), 0f);
				this.interpolator.distance -= this.speed * num * (1f / this.switchPathInterpolationSpeed);
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004C08 File Offset: 0x00002E08
		protected virtual void Update()
		{
			if (this.shouldRecalculatePath)
			{
				this.SearchPath();
			}
			if (this.canMove)
			{
				Vector3 nextPosition;
				Quaternion nextRotation;
				this.MovementUpdate(Time.deltaTime, out nextPosition, out nextRotation);
				this.FinalizeMovement(nextPosition, nextRotation);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004C44 File Offset: 0x00002E44
		public void MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			if (this.updatePosition)
			{
				this.simulatedPosition = this.tr.position;
			}
			if (this.updateRotation)
			{
				this.simulatedRotation = this.tr.rotation;
			}
			Vector3 direction;
			nextPosition = this.CalculateNextPosition(out direction, this.isStopped ? 0f : deltaTime);
			if (this.enableRotation)
			{
				nextRotation = this.SimulateRotationTowards(direction, deltaTime);
				return;
			}
			nextRotation = this.simulatedRotation;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004CC4 File Offset: 0x00002EC4
		public void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation)
		{
			this.previousPosition2 = this.previousPosition1;
			this.simulatedPosition = nextPosition;
			this.previousPosition1 = nextPosition;
			this.simulatedRotation = nextRotation;
			if (this.updatePosition)
			{
				this.tr.position = nextPosition;
			}
			if (this.updateRotation)
			{
				this.tr.rotation = nextRotation;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004D1C File Offset: 0x00002F1C
		private Quaternion SimulateRotationTowards(Vector3 direction, float deltaTime)
		{
			if (direction != Vector3.zero)
			{
				Quaternion quaternion = Quaternion.LookRotation(direction, (this.orientation == OrientationMode.YAxisForward) ? Vector3.back : Vector3.up);
				if (this.orientation == OrientationMode.YAxisForward)
				{
					quaternion *= Quaternion.Euler(90f, 0f, 0f);
				}
				return Quaternion.Slerp(this.simulatedRotation, quaternion, deltaTime * this.rotationSpeed);
			}
			return this.simulatedRotation;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004D94 File Offset: 0x00002F94
		protected virtual Vector3 CalculateNextPosition(out Vector3 direction, float deltaTime)
		{
			if (!this.interpolator.valid)
			{
				direction = Vector3.zero;
				return this.simulatedPosition;
			}
			this.interpolator.distance += deltaTime * this.speed;
			if (this.interpolator.remainingDistance < 0.0001f && !this.reachedEndOfPath)
			{
				this.reachedEndOfPath = true;
				this.OnTargetReached();
			}
			direction = this.interpolator.tangent;
			this.pathSwitchInterpolationTime += deltaTime;
			float num = this.switchPathInterpolationSpeed * this.pathSwitchInterpolationTime;
			if (this.interpolatePathSwitches && num < 1f)
			{
				return Vector3.Lerp(this.previousMovementOrigin + Vector3.ClampMagnitude(this.previousMovementDirection, this.speed * this.pathSwitchInterpolationTime), this.interpolator.position, num);
			}
			return this.interpolator.position;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004E7E File Offset: 0x0000307E
		protected override int OnUpgradeSerializedData(int version, bool unityThread)
		{
			if (unityThread && this.targetCompatibility != null)
			{
				this.target = this.targetCompatibility;
			}
			if (version <= 3)
			{
				this.repathRate = this.repathRateCompatibility;
				this.canSearch = this.canSearchCompability;
			}
			return 4;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004EBA File Offset: 0x000030BA
		public virtual void OnDrawGizmos()
		{
			this.tr = base.transform;
			this.autoRepath.DrawGizmos(this.position, 0f);
		}

		// Token: 0x0400006B RID: 107
		public AutoRepathPolicy autoRepath = new AutoRepathPolicy();

		// Token: 0x0400006C RID: 108
		public bool canMove = true;

		// Token: 0x0400006D RID: 109
		public float speed = 3f;

		// Token: 0x0400006E RID: 110
		[FormerlySerializedAs("rotationIn2D")]
		public OrientationMode orientation;

		// Token: 0x0400006F RID: 111
		public bool enableRotation = true;

		// Token: 0x04000070 RID: 112
		public float rotationSpeed = 10f;

		// Token: 0x04000071 RID: 113
		public bool interpolatePathSwitches = true;

		// Token: 0x04000072 RID: 114
		public float switchPathInterpolationSpeed = 5f;

		// Token: 0x04000075 RID: 117
		[NonSerialized]
		public bool updatePosition = true;

		// Token: 0x04000076 RID: 118
		[NonSerialized]
		public bool updateRotation = true;

		// Token: 0x04000079 RID: 121
		protected Seeker seeker;

		// Token: 0x0400007A RID: 122
		protected Transform tr;

		// Token: 0x0400007B RID: 123
		protected ABPath path;

		// Token: 0x0400007C RID: 124
		protected bool canSearchAgain = true;

		// Token: 0x0400007D RID: 125
		protected Vector3 previousMovementOrigin;

		// Token: 0x0400007E RID: 126
		protected Vector3 previousMovementDirection;

		// Token: 0x0400007F RID: 127
		protected float pathSwitchInterpolationTime;

		// Token: 0x04000080 RID: 128
		protected PathInterpolator interpolator = new PathInterpolator();

		// Token: 0x04000081 RID: 129
		private bool startHasRun;

		// Token: 0x04000082 RID: 130
		private Vector3 previousPosition1;

		// Token: 0x04000083 RID: 131
		private Vector3 previousPosition2;

		// Token: 0x04000084 RID: 132
		private Vector3 simulatedPosition;

		// Token: 0x04000085 RID: 133
		private Quaternion simulatedRotation;

		// Token: 0x04000086 RID: 134
		[FormerlySerializedAs("target")]
		[SerializeField]
		[HideInInspector]
		private Transform targetCompatibility;

		// Token: 0x04000087 RID: 135
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("repathRate")]
		private float repathRateCompatibility = float.NaN;

		// Token: 0x04000088 RID: 136
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("canSearch")]
		private bool canSearchCompability;
	}
}
