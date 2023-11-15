using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Examples;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x0200000A RID: 10
	[AddComponentMenu("Pathfinding/AI/RichAI (3D, for navmesh)")]
	public class RichAI : AIBase, IAstarAI
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000058BC File Offset: 0x00003ABC
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000058C4 File Offset: 0x00003AC4
		public bool traversingOffMeshLink { get; protected set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000058CD File Offset: 0x00003ACD
		public float remainingDistance
		{
			get
			{
				return this.distanceToSteeringTarget + Vector3.Distance(this.steeringTarget, this.richPath.Endpoint);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600011F RID: 287 RVA: 0x000058EC File Offset: 0x00003AEC
		public bool reachedEndOfPath
		{
			get
			{
				return this.approachingPathEndpoint && this.distanceToSteeringTarget < this.endReachedDistance;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005908 File Offset: 0x00003B08
		public bool reachedDestination
		{
			get
			{
				if (!this.reachedEndOfPath)
				{
					return false;
				}
				if (this.approachingPathEndpoint && this.distanceToSteeringTarget + this.movementPlane.ToPlane(base.destination - this.richPath.Endpoint).magnitude > this.endReachedDistance)
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000059B9 File Offset: 0x00003BB9
		public bool hasPath
		{
			get
			{
				return this.richPath.GetCurrentPart() != null;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000122 RID: 290 RVA: 0x000059C9 File Offset: 0x00003BC9
		public bool pathPending
		{
			get
			{
				return this.waitingForPathCalculation || this.delayUpdatePath;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000123 RID: 291 RVA: 0x000059DB File Offset: 0x00003BDB
		// (set) Token: 0x06000124 RID: 292 RVA: 0x000059E3 File Offset: 0x00003BE3
		public Vector3 steeringTarget { get; protected set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000059EC File Offset: 0x00003BEC
		// (set) Token: 0x06000126 RID: 294 RVA: 0x000059F4 File Offset: 0x00003BF4
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

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000059FD File Offset: 0x00003BFD
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00005A05 File Offset: 0x00003C05
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

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00005A0E File Offset: 0x00003C0E
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00005A16 File Offset: 0x00003C16
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

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00005A1F File Offset: 0x00003C1F
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00005A27 File Offset: 0x00003C27
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00005A30 File Offset: 0x00003C30
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00005A38 File Offset: 0x00003C38
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

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00005A41 File Offset: 0x00003C41
		public bool approachingPartEndpoint
		{
			get
			{
				return this.lastCorner && this.nextCorners.Count == 1;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00005A5B File Offset: 0x00003C5B
		public bool approachingPathEndpoint
		{
			get
			{
				return this.approachingPartEndpoint && this.richPath.IsLastPart;
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005A74 File Offset: 0x00003C74
		public override void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			NNInfo nninfo = (AstarPath.active != null) ? AstarPath.active.GetNearest(newPosition) : default(NNInfo);
			float elevation;
			this.movementPlane.ToPlane(newPosition, out elevation);
			newPosition = this.movementPlane.ToWorld(this.movementPlane.ToPlane((nninfo.node != null) ? nninfo.position : newPosition), elevation);
			base.Teleport(newPosition, clearPath);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005AE6 File Offset: 0x00003CE6
		protected override void OnDisable()
		{
			base.OnDisable();
			this.traversingOffMeshLink = false;
			base.StopAllCoroutines();
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00005AFB File Offset: 0x00003CFB
		protected override bool shouldRecalculatePath
		{
			get
			{
				return base.shouldRecalculatePath && !this.traversingOffMeshLink;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005B10 File Offset: 0x00003D10
		public override void SearchPath()
		{
			if (this.traversingOffMeshLink)
			{
				this.delayUpdatePath = true;
				return;
			}
			base.SearchPath();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005B28 File Offset: 0x00003D28
		protected override void OnPathComplete(Path p)
		{
			this.waitingForPathCalculation = false;
			p.Claim(this);
			if (p.error)
			{
				p.Release(this, false);
				return;
			}
			if (this.traversingOffMeshLink)
			{
				this.delayUpdatePath = true;
			}
			else
			{
				RandomPath randomPath = p as RandomPath;
				if (randomPath != null)
				{
					base.destination = randomPath.originalEndPoint;
				}
				else
				{
					MultiTargetPath multiTargetPath = p as MultiTargetPath;
					if (multiTargetPath != null)
					{
						base.destination = multiTargetPath.originalEndPoint;
					}
				}
				this.richPath.Initialize(this.seeker, p, true, this.funnelSimplification);
				RichFunnel richFunnel = this.richPath.GetCurrentPart() as RichFunnel;
				if (richFunnel != null)
				{
					if (this.updatePosition)
					{
						this.simulatedPosition = this.tr.position;
					}
					Vector2 b = this.movementPlane.ToPlane(this.UpdateTarget(richFunnel));
					this.steeringTarget = this.nextCorners[0];
					Vector2 a = this.movementPlane.ToPlane(this.steeringTarget);
					this.distanceToSteeringTarget = (a - b).magnitude;
					if (this.lastCorner && this.nextCorners.Count == 1 && this.distanceToSteeringTarget <= this.endReachedDistance)
					{
						this.NextPart();
					}
				}
			}
			p.Release(this, false);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005C60 File Offset: 0x00003E60
		protected override void ClearPath()
		{
			base.CancelCurrentPathRequest();
			this.richPath.Clear();
			this.lastCorner = false;
			this.delayUpdatePath = false;
			this.distanceToSteeringTarget = float.PositiveInfinity;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005C8C File Offset: 0x00003E8C
		protected void NextPart()
		{
			if (!this.richPath.CompletedAllParts)
			{
				if (!this.richPath.IsLastPart)
				{
					this.lastCorner = false;
				}
				this.richPath.NextPart();
				if (this.richPath.CompletedAllParts)
				{
					this.OnTargetReached();
				}
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005CD8 File Offset: 0x00003ED8
		public void GetRemainingPath(List<Vector3> buffer, out bool stale)
		{
			this.richPath.GetRemainingPath(buffer, this.simulatedPosition, out stale);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005CED File Offset: 0x00003EED
		protected virtual void OnTargetReached()
		{
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005CF0 File Offset: 0x00003EF0
		protected virtual Vector3 UpdateTarget(RichFunnel fn)
		{
			this.nextCorners.Clear();
			bool flag;
			Vector3 result = fn.Update(this.simulatedPosition, this.nextCorners, 2, out this.lastCorner, out flag);
			if (flag && !this.waitingForPathCalculation && base.canSearch)
			{
				this.SearchPath();
			}
			return result;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00005D3C File Offset: 0x00003F3C
		protected override void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			if (this.updatePosition)
			{
				this.simulatedPosition = this.tr.position;
			}
			if (this.updateRotation)
			{
				this.simulatedRotation = this.tr.rotation;
			}
			RichPathPart currentPart = this.richPath.GetCurrentPart();
			if (currentPart is RichSpecial)
			{
				if (!this.traversingOffMeshLink && !this.richPath.CompletedAllParts)
				{
					base.StartCoroutine(this.TraverseSpecial(currentPart as RichSpecial));
				}
				nextPosition = (this.steeringTarget = this.simulatedPosition);
				nextRotation = base.rotation;
				return;
			}
			RichFunnel richFunnel = currentPart as RichFunnel;
			if (richFunnel != null && !base.isStopped)
			{
				this.TraverseFunnel(richFunnel, deltaTime, out nextPosition, out nextRotation);
				return;
			}
			this.velocity2D -= Vector2.ClampMagnitude(this.velocity2D, this.acceleration * deltaTime);
			this.FinalMovement(this.simulatedPosition, deltaTime, float.PositiveInfinity, 1f, out nextPosition, out nextRotation);
			this.steeringTarget = this.simulatedPosition;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00005E40 File Offset: 0x00004040
		private void TraverseFunnel(RichFunnel fn, float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			Vector3 vector = this.UpdateTarget(fn);
			float elevation;
			Vector2 vector2 = this.movementPlane.ToPlane(vector, out elevation);
			if (Time.frameCount % 5 == 0 && this.wallForce > 0f && this.wallDist > 0f)
			{
				this.wallBuffer.Clear();
				fn.FindWalls(this.wallBuffer, this.wallDist);
			}
			this.steeringTarget = this.nextCorners[0];
			Vector2 vector3 = this.movementPlane.ToPlane(this.steeringTarget);
			Vector2 vector4 = vector3 - vector2;
			Vector2 vector5 = VectorMath.Normalize(vector4, out this.distanceToSteeringTarget);
			Vector2 a = this.CalculateWallForce(vector2, elevation, vector5);
			Vector2 targetVelocity;
			if (this.approachingPartEndpoint)
			{
				targetVelocity = ((this.slowdownTime > 0f) ? Vector2.zero : (vector5 * this.maxSpeed));
				a *= Math.Min(this.distanceToSteeringTarget / 0.5f, 1f);
				if (this.distanceToSteeringTarget <= this.endReachedDistance)
				{
					this.NextPart();
				}
			}
			else
			{
				targetVelocity = (((this.nextCorners.Count > 1) ? this.movementPlane.ToPlane(this.nextCorners[1]) : (vector2 + 2f * vector4)) - vector3).normalized * this.maxSpeed;
			}
			Vector2 forwardsVector = this.movementPlane.ToPlane(this.simulatedRotation * ((this.orientation == OrientationMode.YAxisForward) ? Vector3.up : Vector3.forward));
			Vector2 a2 = MovementUtilities.CalculateAccelerationToReachPoint(vector3 - vector2, targetVelocity, this.velocity2D, this.acceleration, this.rotationSpeed, this.maxSpeed, forwardsVector);
			this.velocity2D += (a2 + a * this.wallForce) * deltaTime;
			float num = this.distanceToSteeringTarget + Vector3.Distance(this.steeringTarget, fn.exactEnd);
			float slowdownFactor = (num < this.maxSpeed * this.slowdownTime) ? Mathf.Sqrt(num / (this.maxSpeed * this.slowdownTime)) : 1f;
			this.FinalMovement(vector, deltaTime, num, slowdownFactor, out nextPosition, out nextRotation);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000607C File Offset: 0x0000427C
		private void FinalMovement(Vector3 position3D, float deltaTime, float distanceToEndOfPath, float slowdownFactor, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			Vector2 forward = this.movementPlane.ToPlane(this.simulatedRotation * ((this.orientation == OrientationMode.YAxisForward) ? Vector3.up : Vector3.forward));
			this.velocity2D = MovementUtilities.ClampVelocity(this.velocity2D, this.maxSpeed, slowdownFactor, this.slowWhenNotFacingTarget && this.enableRotation, forward);
			base.ApplyGravity(deltaTime);
			if (this.rvoController != null && this.rvoController.enabled)
			{
				Vector3 pos = position3D + this.movementPlane.ToWorld(Vector2.ClampMagnitude(this.velocity2D, distanceToEndOfPath), 0f);
				this.rvoController.SetTarget(pos, this.velocity2D.magnitude, this.maxSpeed);
			}
			Vector2 vector = this.lastDeltaPosition = base.CalculateDeltaToMoveThisFrame(this.movementPlane.ToPlane(position3D), distanceToEndOfPath, deltaTime);
			float num = this.approachingPartEndpoint ? Mathf.Clamp01(1.1f * slowdownFactor - 0.1f) : 1f;
			nextRotation = (this.enableRotation ? base.SimulateRotationTowards(vector, this.rotationSpeed * num * deltaTime) : this.simulatedRotation);
			nextPosition = position3D + this.movementPlane.ToWorld(vector, this.verticalVelocity * deltaTime);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000061D0 File Offset: 0x000043D0
		protected override Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
		{
			if (this.richPath != null)
			{
				RichFunnel richFunnel = this.richPath.GetCurrentPart() as RichFunnel;
				if (richFunnel != null)
				{
					Vector3 a = richFunnel.ClampToNavmesh(position);
					Vector2 vector = this.movementPlane.ToPlane(a - position);
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
			}
			positionChanged = false;
			return position;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000062A0 File Offset: 0x000044A0
		private Vector2 CalculateWallForce(Vector2 position, float elevation, Vector2 directionToTarget)
		{
			if (this.wallForce <= 0f || this.wallDist <= 0f)
			{
				return Vector2.zero;
			}
			float num = 0f;
			float num2 = 0f;
			Vector3 vector = this.movementPlane.ToWorld(position, elevation);
			for (int i = 0; i < this.wallBuffer.Count; i += 2)
			{
				float sqrMagnitude = (VectorMath.ClosestPointOnSegment(this.wallBuffer[i], this.wallBuffer[i + 1], vector) - vector).sqrMagnitude;
				if (sqrMagnitude <= this.wallDist * this.wallDist)
				{
					Vector2 normalized = this.movementPlane.ToPlane(this.wallBuffer[i + 1] - this.wallBuffer[i]).normalized;
					float num3 = Vector2.Dot(directionToTarget, normalized);
					float num4 = 1f - Math.Max(0f, 2f * (sqrMagnitude / (this.wallDist * this.wallDist)) - 1f);
					if (num3 > 0f)
					{
						num2 = Math.Max(num2, num3 * num4);
					}
					else
					{
						num = Math.Max(num, -num3 * num4);
					}
				}
			}
			return new Vector2(directionToTarget.y, -directionToTarget.x) * (num2 - num);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000063F3 File Offset: 0x000045F3
		protected virtual IEnumerator TraverseSpecial(RichSpecial link)
		{
			this.traversingOffMeshLink = true;
			this.velocity2D = Vector3.zero;
			IEnumerator routine = (this.onTraverseOffMeshLink != null) ? this.onTraverseOffMeshLink(link) : this.TraverseOffMeshLinkFallback(link);
			yield return base.StartCoroutine(routine);
			this.traversingOffMeshLink = false;
			this.NextPart();
			if (this.delayUpdatePath)
			{
				this.delayUpdatePath = false;
				if (base.canSearch)
				{
					this.SearchPath();
				}
			}
			yield break;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006409 File Offset: 0x00004609
		protected IEnumerator TraverseOffMeshLinkFallback(RichSpecial link)
		{
			float duration = (this.maxSpeed > 0f) ? (Vector3.Distance(link.second.position, link.first.position) / this.maxSpeed) : 1f;
			float startTime = Time.time;
			for (;;)
			{
				Vector3 vector = Vector3.Lerp(link.first.position, link.second.position, Mathf.InverseLerp(startTime, startTime + duration, Time.time));
				if (this.updatePosition)
				{
					this.tr.position = vector;
				}
				else
				{
					this.simulatedPosition = vector;
				}
				if (Time.time >= startTime + duration)
				{
					break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006420 File Offset: 0x00004620
		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if (this.tr != null)
			{
				Gizmos.color = RichAI.GizmoColorPath;
				Vector3 from = base.position;
				for (int i = 0; i < this.nextCorners.Count; i++)
				{
					Gizmos.DrawLine(from, this.nextCorners[i]);
					from = this.nextCorners[i];
				}
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006487 File Offset: 0x00004687
		protected override int OnUpgradeSerializedData(int version, bool unityThread)
		{
			if (unityThread && this.animCompatibility != null)
			{
				this.anim = this.animCompatibility;
			}
			return base.OnUpgradeSerializedData(version, unityThread);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000064AE File Offset: 0x000046AE
		[Obsolete("Use SearchPath instead. [AstarUpgradable: 'UpdatePath' -> 'SearchPath']")]
		public void UpdatePath()
		{
			this.SearchPath();
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000064B6 File Offset: 0x000046B6
		[Obsolete("Use velocity instead (lowercase 'v'). [AstarUpgradable: 'Velocity' -> 'velocity']")]
		public Vector3 Velocity
		{
			get
			{
				return base.velocity;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000064BE File Offset: 0x000046BE
		[Obsolete("Use steeringTarget instead. [AstarUpgradable: 'NextWaypoint' -> 'steeringTarget']")]
		public Vector3 NextWaypoint
		{
			get
			{
				return this.steeringTarget;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000064C6 File Offset: 0x000046C6
		[Obsolete("Use Vector3.Distance(transform.position, ai.steeringTarget) instead.")]
		public float DistanceToNextWaypoint
		{
			get
			{
				return this.distanceToSteeringTarget;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000064CE File Offset: 0x000046CE
		// (set) Token: 0x06000149 RID: 329 RVA: 0x000064D6 File Offset: 0x000046D6
		[Obsolete("Use canSearch instead. [AstarUpgradable: 'repeatedlySearchPaths' -> 'canSearch']")]
		public bool repeatedlySearchPaths
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

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000064DF File Offset: 0x000046DF
		[Obsolete("When unifying the interfaces for different movement scripts, this property has been renamed to reachedEndOfPath (lowercase t).  [AstarUpgradable: 'TargetReached' -> 'reachedEndOfPath']")]
		public bool TargetReached
		{
			get
			{
				return this.reachedEndOfPath;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000064E7 File Offset: 0x000046E7
		[Obsolete("Use pathPending instead (lowercase 'p'). [AstarUpgradable: 'PathPending' -> 'pathPending']")]
		public bool PathPending
		{
			get
			{
				return this.pathPending;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000064EF File Offset: 0x000046EF
		[Obsolete("Use approachingPartEndpoint (lowercase 'a') instead")]
		public bool ApproachingPartEndpoint
		{
			get
			{
				return this.approachingPartEndpoint;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000064F7 File Offset: 0x000046F7
		[Obsolete("Use approachingPathEndpoint (lowercase 'a') instead")]
		public bool ApproachingPathEndpoint
		{
			get
			{
				return this.approachingPathEndpoint;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000064FF File Offset: 0x000046FF
		[Obsolete("This property has been renamed to 'traversingOffMeshLink'. [AstarUpgradable: 'TraversingSpecial' -> 'traversingOffMeshLink']")]
		public bool TraversingSpecial
		{
			get
			{
				return this.traversingOffMeshLink;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00006507 File Offset: 0x00004707
		[Obsolete("This property has been renamed to steeringTarget")]
		public Vector3 TargetPoint
		{
			get
			{
				return this.steeringTarget;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00006510 File Offset: 0x00004710
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00006538 File Offset: 0x00004738
		[Obsolete("Use the onTraverseOffMeshLink event or the ... component instead. Setting this value will add a ... component")]
		public Animation anim
		{
			get
			{
				AnimationLinkTraverser component = base.GetComponent<AnimationLinkTraverser>();
				if (!(component != null))
				{
					return null;
				}
				return component.anim;
			}
			set
			{
				this.animCompatibility = null;
				AnimationLinkTraverser animationLinkTraverser = base.GetComponent<AnimationLinkTraverser>();
				if (animationLinkTraverser == null)
				{
					animationLinkTraverser = base.gameObject.AddComponent<AnimationLinkTraverser>();
				}
				animationLinkTraverser.anim = value;
			}
		}

		// Token: 0x04000096 RID: 150
		public float acceleration = 5f;

		// Token: 0x04000097 RID: 151
		public float rotationSpeed = 360f;

		// Token: 0x04000098 RID: 152
		public float slowdownTime = 0.5f;

		// Token: 0x04000099 RID: 153
		public float endReachedDistance = 0.01f;

		// Token: 0x0400009A RID: 154
		public float wallForce = 3f;

		// Token: 0x0400009B RID: 155
		public float wallDist = 1f;

		// Token: 0x0400009C RID: 156
		public bool funnelSimplification;

		// Token: 0x0400009D RID: 157
		public bool slowWhenNotFacingTarget = true;

		// Token: 0x0400009E RID: 158
		public Func<RichSpecial, IEnumerator> onTraverseOffMeshLink;

		// Token: 0x0400009F RID: 159
		protected readonly RichPath richPath = new RichPath();

		// Token: 0x040000A0 RID: 160
		protected bool delayUpdatePath;

		// Token: 0x040000A1 RID: 161
		protected bool lastCorner;

		// Token: 0x040000A2 RID: 162
		protected float distanceToSteeringTarget = float.PositiveInfinity;

		// Token: 0x040000A3 RID: 163
		protected readonly List<Vector3> nextCorners = new List<Vector3>();

		// Token: 0x040000A4 RID: 164
		protected readonly List<Vector3> wallBuffer = new List<Vector3>();

		// Token: 0x040000A7 RID: 167
		protected static readonly Color GizmoColorPath = new Color(0.03137255f, 0.30588236f, 0.7607843f);

		// Token: 0x040000A8 RID: 168
		[FormerlySerializedAs("anim")]
		[SerializeField]
		[HideInInspector]
		private Animation animCompatibility;
	}
}
