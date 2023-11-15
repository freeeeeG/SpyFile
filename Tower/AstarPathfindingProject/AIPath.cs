using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000008 RID: 8
	[AddComponentMenu("Pathfinding/AI/AIPath (2D,3D)")]
	public class AIPath : AIBase, IAstarAI
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00004EE6 File Offset: 0x000030E6
		public override void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			this.reachedEndOfPath = false;
			base.Teleport(newPosition, clearPath);
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00004EF8 File Offset: 0x000030F8
		public float remainingDistance
		{
			get
			{
				if (!this.interpolator.valid)
				{
					return float.PositiveInfinity;
				}
				return this.interpolator.remainingDistance + this.movementPlane.ToPlane(this.interpolator.position - base.position).magnitude;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004F50 File Offset: 0x00003150
		public bool reachedDestination
		{
			get
			{
				if (!this.reachedEndOfPath)
				{
					return false;
				}
				if (!this.interpolator.valid || this.remainingDistance + this.movementPlane.ToPlane(base.destination - this.interpolator.endPoint).magnitude > this.endReachedDistance)
				{
					return false;
				}
				if (this.orientation != OrientationMode.YAxisForward)
				{
					float num;
					this.movementPlane.ToPlane(base.destination - base.position, out num);
					float num2 = this.tr.localScale.y * this.height;
					if (num > num2 || (double)num < (double)(-(double)num2) * 0.5)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005006 File Offset: 0x00003206
		// (set) Token: 0x060000DA RID: 218 RVA: 0x0000500E File Offset: 0x0000320E
		public bool reachedEndOfPath { get; protected set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00005017 File Offset: 0x00003217
		public bool hasPath
		{
			get
			{
				return this.interpolator.valid;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00005024 File Offset: 0x00003224
		public bool pathPending
		{
			get
			{
				return this.waitingForPathCalculation;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000DD RID: 221 RVA: 0x0000502C File Offset: 0x0000322C
		public Vector3 steeringTarget
		{
			get
			{
				if (!this.interpolator.valid)
				{
					return base.position;
				}
				return this.interpolator.position;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000DE RID: 222 RVA: 0x0000504D File Offset: 0x0000324D
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00005055 File Offset: 0x00003255
		float IAstarAI.radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				this.radius = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x0000505E File Offset: 0x0000325E
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00005066 File Offset: 0x00003266
		float IAstarAI.height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000506F File Offset: 0x0000326F
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00005077 File Offset: 0x00003277
		float IAstarAI.maxSpeed
		{
			get
			{
				return this.maxSpeed;
			}
			set
			{
				this.maxSpeed = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00005080 File Offset: 0x00003280
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00005088 File Offset: 0x00003288
		bool IAstarAI.canSearch
		{
			get
			{
				return base.canSearch;
			}
			set
			{
				base.canSearch = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00005091 File Offset: 0x00003291
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00005099 File Offset: 0x00003299
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

		// Token: 0x060000E8 RID: 232 RVA: 0x000050A2 File Offset: 0x000032A2
		public void GetRemainingPath(List<Vector3> buffer, out bool stale)
		{
			buffer.Clear();
			buffer.Add(base.position);
			if (!this.interpolator.valid)
			{
				stale = true;
				return;
			}
			stale = false;
			this.interpolator.GetRemainingPath(buffer);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000050D6 File Offset: 0x000032D6
		protected override void OnDisable()
		{
			base.OnDisable();
			if (this.path != null)
			{
				this.path.Release(this, false);
			}
			this.path = null;
			this.interpolator.SetPath(null);
			this.reachedEndOfPath = false;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000510D File Offset: 0x0000330D
		public virtual void OnTargetReached()
		{
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005110 File Offset: 0x00003310
		protected override void OnPathComplete(Path newPath)
		{
			ABPath abpath = newPath as ABPath;
			if (abpath == null)
			{
				throw new Exception("This function only handles ABPaths, do not use special path types");
			}
			this.waitingForPathCalculation = false;
			abpath.Claim(this);
			if (abpath.error)
			{
				abpath.Release(this, false);
				base.SetPath(null, true);
				return;
			}
			if (this.path != null)
			{
				this.path.Release(this, false);
			}
			this.path = abpath;
			RandomPath randomPath = this.path as RandomPath;
			if (randomPath != null)
			{
				base.destination = randomPath.originalEndPoint;
			}
			else
			{
				MultiTargetPath multiTargetPath = this.path as MultiTargetPath;
				if (multiTargetPath != null)
				{
					base.destination = multiTargetPath.originalEndPoint;
				}
			}
			if (this.path.vectorPath.Count == 1)
			{
				this.path.vectorPath.Add(this.path.vectorPath[0]);
			}
			this.interpolator.SetPath(this.path.vectorPath);
			ITransformedGraph transformedGraph = (this.path.path.Count > 0) ? (AstarData.GetGraph(this.path.path[0]) as ITransformedGraph) : null;
			this.movementPlane = ((transformedGraph != null) ? transformedGraph.transform : ((this.orientation == OrientationMode.YAxisForward) ? new GraphTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90f, 270f, 90f), Vector3.one)) : GraphTransform.identityTransform));
			this.reachedEndOfPath = false;
			this.interpolator.MoveToLocallyClosestPoint((this.GetFeetPosition() + abpath.originalStartPoint) * 0.5f, true, true);
			this.interpolator.MoveToLocallyClosestPoint(this.GetFeetPosition(), true, true);
			this.interpolator.MoveToCircleIntersection2D(base.position, this.pickNextWaypointDist, this.movementPlane);
			if (this.remainingDistance <= this.endReachedDistance)
			{
				this.reachedEndOfPath = true;
				this.OnTargetReached();
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000052EC File Offset: 0x000034EC
		protected override void ClearPath()
		{
			base.CancelCurrentPathRequest();
			if (this.path != null)
			{
				this.path.Release(this, false);
			}
			this.path = null;
			this.interpolator.SetPath(null);
			this.reachedEndOfPath = false;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005324 File Offset: 0x00003524
		protected override void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			float num = this.maxAcceleration;
			if (num < 0f)
			{
				num *= -this.maxSpeed;
			}
			if (this.updatePosition)
			{
				this.simulatedPosition = this.tr.position;
			}
			if (this.updateRotation)
			{
				this.simulatedRotation = this.tr.rotation;
			}
			Vector3 simulatedPosition = this.simulatedPosition;
			this.interpolator.MoveToCircleIntersection2D(simulatedPosition, this.pickNextWaypointDist, this.movementPlane);
			Vector2 deltaPosition = this.movementPlane.ToPlane(this.steeringTarget - simulatedPosition);
			float num2 = deltaPosition.magnitude + Mathf.Max(0f, this.interpolator.remainingDistance);
			bool reachedEndOfPath = this.reachedEndOfPath;
			this.reachedEndOfPath = (num2 <= this.endReachedDistance && this.interpolator.valid);
			if (!reachedEndOfPath && this.reachedEndOfPath)
			{
				this.OnTargetReached();
			}
			Vector2 vector = this.movementPlane.ToPlane(this.simulatedRotation * ((this.orientation == OrientationMode.YAxisForward) ? Vector3.up : Vector3.forward));
			bool flag = base.isStopped || (this.reachedDestination && this.whenCloseToDestination == CloseToDestinationMode.Stop);
			float num3;
			if (this.interpolator.valid && !flag)
			{
				num3 = ((num2 < this.slowdownDistance) ? Mathf.Sqrt(num2 / this.slowdownDistance) : 1f);
				if (this.reachedEndOfPath && this.whenCloseToDestination == CloseToDestinationMode.Stop)
				{
					this.velocity2D -= Vector2.ClampMagnitude(this.velocity2D, num * deltaTime);
				}
				else
				{
					this.velocity2D += MovementUtilities.CalculateAccelerationToReachPoint(deltaPosition, deltaPosition.normalized * this.maxSpeed, this.velocity2D, num, this.rotationSpeed, this.maxSpeed, vector) * deltaTime;
				}
			}
			else
			{
				num3 = 1f;
				this.velocity2D -= Vector2.ClampMagnitude(this.velocity2D, num * deltaTime);
			}
			this.velocity2D = MovementUtilities.ClampVelocity(this.velocity2D, this.maxSpeed, num3, this.slowWhenNotFacingTarget && this.enableRotation, vector);
			base.ApplyGravity(deltaTime);
			if (this.rvoController != null && this.rvoController.enabled)
			{
				Vector3 pos = simulatedPosition + this.movementPlane.ToWorld(Vector2.ClampMagnitude(this.velocity2D, num2), 0f);
				this.rvoController.SetTarget(pos, this.velocity2D.magnitude, this.maxSpeed);
			}
			Vector2 p = this.lastDeltaPosition = base.CalculateDeltaToMoveThisFrame(this.movementPlane.ToPlane(simulatedPosition), num2, deltaTime);
			nextPosition = simulatedPosition + this.movementPlane.ToWorld(p, this.verticalVelocity * this.lastDeltaTime);
			this.CalculateNextRotation(num3, out nextRotation);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005608 File Offset: 0x00003808
		protected virtual void CalculateNextRotation(float slowdown, out Quaternion nextRotation)
		{
			if (this.lastDeltaTime > 1E-05f && this.enableRotation)
			{
				Vector2 direction;
				if (this.rvoController != null && this.rvoController.enabled)
				{
					Vector2 b = this.lastDeltaPosition / this.lastDeltaTime;
					direction = Vector2.Lerp(this.velocity2D, b, 4f * b.magnitude / (this.maxSpeed + 0.0001f));
				}
				else
				{
					direction = this.velocity2D;
				}
				float num = this.rotationSpeed * Mathf.Max(0f, (slowdown - 0.3f) / 0.7f);
				nextRotation = base.SimulateRotationTowards(direction, num * this.lastDeltaTime);
				return;
			}
			nextRotation = base.rotation;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000056D0 File Offset: 0x000038D0
		protected override Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
		{
			if (this.constrainInsideGraph)
			{
				AIPath.cachedNNConstraint.tags = this.seeker.traversableTags;
				AIPath.cachedNNConstraint.graphMask = this.seeker.graphMask;
				AIPath.cachedNNConstraint.distanceXZ = true;
				Vector3 position2 = AstarPath.active.GetNearest(position, AIPath.cachedNNConstraint).position;
				Vector2 vector = this.movementPlane.ToPlane(position2 - position);
				float sqrMagnitude = vector.sqrMagnitude;
				if (sqrMagnitude > 1.0000001E-06f)
				{
					this.velocity2D -= vector * Vector2.Dot(vector, this.velocity2D) / sqrMagnitude;
					if (this.rvoController != null && this.rvoController.enabled)
					{
						this.rvoController.SetCollisionNormal(vector);
					}
					positionChanged = true;
					return position + this.movementPlane.ToWorld(vector, 0f);
				}
			}
			positionChanged = false;
			return position;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000057CA File Offset: 0x000039CA
		protected override int OnUpgradeSerializedData(int version, bool unityThread)
		{
			if (version < 1)
			{
				this.rotationSpeed *= 90f;
			}
			return base.OnUpgradeSerializedData(version, unityThread);
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x000057EA File Offset: 0x000039EA
		[Obsolete("When unifying the interfaces for different movement scripts, this property has been renamed to reachedEndOfPath.  [AstarUpgradable: 'TargetReached' -> 'reachedEndOfPath']")]
		public bool TargetReached
		{
			get
			{
				return this.reachedEndOfPath;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000057F2 File Offset: 0x000039F2
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00005800 File Offset: 0x00003A00
		[Obsolete("This field has been renamed to #rotationSpeed and is now in degrees per second instead of a damping factor")]
		public float turningSpeed
		{
			get
			{
				return this.rotationSpeed / 90f;
			}
			set
			{
				this.rotationSpeed = value * 90f;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000580F File Offset: 0x00003A0F
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00005817 File Offset: 0x00003A17
		[Obsolete("This member has been deprecated. Use 'maxSpeed' instead. [AstarUpgradable: 'speed' -> 'maxSpeed']")]
		public float speed
		{
			get
			{
				return this.maxSpeed;
			}
			set
			{
				this.maxSpeed = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005820 File Offset: 0x00003A20
		[Obsolete("Only exists for compatibility reasons. Use desiredVelocity or steeringTarget instead.")]
		public Vector3 targetDirection
		{
			get
			{
				return (this.steeringTarget - this.tr.position).normalized;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000584B File Offset: 0x00003A4B
		[Obsolete("This method no longer calculates the velocity. Use the desiredVelocity property instead")]
		public Vector3 CalculateVelocity(Vector3 position)
		{
			return base.desiredVelocity;
		}

		// Token: 0x04000089 RID: 137
		public float maxAcceleration = -2.5f;

		// Token: 0x0400008A RID: 138
		[FormerlySerializedAs("turningSpeed")]
		public float rotationSpeed = 360f;

		// Token: 0x0400008B RID: 139
		public float slowdownDistance = 0.6f;

		// Token: 0x0400008C RID: 140
		public float pickNextWaypointDist = 2f;

		// Token: 0x0400008D RID: 141
		public float endReachedDistance = 0.2f;

		// Token: 0x0400008E RID: 142
		public bool alwaysDrawGizmos;

		// Token: 0x0400008F RID: 143
		public bool slowWhenNotFacingTarget = true;

		// Token: 0x04000090 RID: 144
		public CloseToDestinationMode whenCloseToDestination;

		// Token: 0x04000091 RID: 145
		public bool constrainInsideGraph;

		// Token: 0x04000092 RID: 146
		protected Path path;

		// Token: 0x04000093 RID: 147
		protected PathInterpolator interpolator = new PathInterpolator();

		// Token: 0x04000095 RID: 149
		private static NNConstraint cachedNNConstraint = NNConstraint.Default;
	}
}
