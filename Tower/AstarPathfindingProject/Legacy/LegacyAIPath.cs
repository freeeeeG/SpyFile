using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Legacy
{
	// Token: 0x020000A2 RID: 162
	[RequireComponent(typeof(Seeker))]
	[AddComponentMenu("Pathfinding/Legacy/AI/Legacy AIPath (3D)")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_legacy_1_1_legacy_a_i_path.php")]
	public class LegacyAIPath : AIPath
	{
		// Token: 0x06000770 RID: 1904 RVA: 0x0002D4A1 File Offset: 0x0002B6A1
		protected override void Awake()
		{
			base.Awake();
			if (this.rvoController != null)
			{
				if (this.rvoController is LegacyRVOController)
				{
					(this.rvoController as LegacyRVOController).enableRotation = false;
					return;
				}
				Debug.LogError("The LegacyAIPath component only works with the legacy RVOController, not the latest one. Please upgrade this component", this);
			}
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0002D4E4 File Offset: 0x0002B6E4
		protected override void OnPathComplete(Path _p)
		{
			ABPath abpath = _p as ABPath;
			if (abpath == null)
			{
				throw new Exception("This function only handles ABPaths, do not use special path types");
			}
			this.waitingForPathCalculation = false;
			abpath.Claim(this);
			if (abpath.error)
			{
				abpath.Release(this, false);
				return;
			}
			if (this.path != null)
			{
				this.path.Release(this, false);
			}
			this.path = abpath;
			this.currentWaypointIndex = 0;
			base.reachedEndOfPath = false;
			if (this.closestOnPathCheck)
			{
				Vector3 vector = (Time.time - this.lastFoundWaypointTime < 0.3f) ? this.lastFoundWaypointPosition : abpath.originalStartPoint;
				Vector3 vector2 = this.GetFeetPosition() - vector;
				float magnitude = vector2.magnitude;
				vector2 /= magnitude;
				int num = (int)(magnitude / this.pickNextWaypointDist);
				for (int i = 0; i <= num; i++)
				{
					this.CalculateVelocity(vector);
					vector += vector2;
				}
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0002D5C4 File Offset: 0x0002B7C4
		protected override void Update()
		{
			if (!this.canMove)
			{
				return;
			}
			Vector3 vector = this.CalculateVelocity(this.GetFeetPosition());
			this.RotateTowards(this.targetDirection);
			if (this.rvoController != null)
			{
				this.rvoController.Move(vector);
				return;
			}
			if (this.controller != null)
			{
				this.controller.SimpleMove(vector);
				return;
			}
			if (this.rigid != null)
			{
				this.rigid.AddForce(vector);
				return;
			}
			this.tr.Translate(vector * Time.deltaTime, Space.World);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0002D65C File Offset: 0x0002B85C
		protected float XZSqrMagnitude(Vector3 a, Vector3 b)
		{
			float num = b.x - a.x;
			float num2 = b.z - a.z;
			return num * num + num2 * num2;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0002D68C File Offset: 0x0002B88C
		protected new Vector3 CalculateVelocity(Vector3 currentPosition)
		{
			if (this.path == null || this.path.vectorPath == null || this.path.vectorPath.Count == 0)
			{
				return Vector3.zero;
			}
			List<Vector3> vectorPath = this.path.vectorPath;
			if (vectorPath.Count == 1)
			{
				vectorPath.Insert(0, currentPosition);
			}
			if (this.currentWaypointIndex >= vectorPath.Count)
			{
				this.currentWaypointIndex = vectorPath.Count - 1;
			}
			if (this.currentWaypointIndex <= 1)
			{
				this.currentWaypointIndex = 1;
			}
			while (this.currentWaypointIndex < vectorPath.Count - 1 && this.XZSqrMagnitude(vectorPath[this.currentWaypointIndex], currentPosition) < this.pickNextWaypointDist * this.pickNextWaypointDist)
			{
				this.lastFoundWaypointPosition = currentPosition;
				this.lastFoundWaypointTime = Time.time;
				this.currentWaypointIndex++;
			}
			Vector3 vector = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
			vector = this.CalculateTargetPoint(currentPosition, vectorPath[this.currentWaypointIndex - 1], vectorPath[this.currentWaypointIndex]) - currentPosition;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			float num = Mathf.Clamp01(magnitude / this.slowdownDistance);
			this.targetDirection = vector;
			if (this.currentWaypointIndex == vectorPath.Count - 1 && magnitude <= this.endReachedDistance)
			{
				if (!base.reachedEndOfPath)
				{
					base.reachedEndOfPath = true;
					this.OnTargetReached();
				}
				return Vector3.zero;
			}
			Vector3 forward = this.tr.forward;
			float a = Vector3.Dot(vector.normalized, forward);
			float num2 = this.maxSpeed * Mathf.Max(a, this.minMoveScale) * num;
			if (Time.deltaTime > 0f)
			{
				num2 = Mathf.Clamp(num2, 0f, magnitude / (Time.deltaTime * 2f));
			}
			return forward * num2;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002D870 File Offset: 0x0002BA70
		protected void RotateTowards(Vector3 dir)
		{
			if (dir == Vector3.zero)
			{
				return;
			}
			Quaternion quaternion = this.tr.rotation;
			Quaternion b = Quaternion.LookRotation(dir);
			Vector3 eulerAngles = Quaternion.Slerp(quaternion, b, base.turningSpeed * Time.deltaTime).eulerAngles;
			eulerAngles.z = 0f;
			eulerAngles.x = 0f;
			quaternion = Quaternion.Euler(eulerAngles);
			this.tr.rotation = quaternion;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0002D8E8 File Offset: 0x0002BAE8
		protected Vector3 CalculateTargetPoint(Vector3 p, Vector3 a, Vector3 b)
		{
			a.y = p.y;
			b.y = p.y;
			float magnitude = (a - b).magnitude;
			if (magnitude == 0f)
			{
				return a;
			}
			float num = Mathf.Clamp01(VectorMath.ClosestPointOnLineFactor(a, b, p));
			float magnitude2 = ((b - a) * num + a - p).magnitude;
			float num2 = Mathf.Clamp(this.forwardLook - magnitude2, 0f, this.forwardLook) / magnitude;
			num2 = Mathf.Clamp(num2 + num, 0f, 1f);
			return (b - a) * num2 + a;
		}

		// Token: 0x0400043A RID: 1082
		public float forwardLook = 1f;

		// Token: 0x0400043B RID: 1083
		public bool closestOnPathCheck = true;

		// Token: 0x0400043C RID: 1084
		protected float minMoveScale = 0.05f;

		// Token: 0x0400043D RID: 1085
		protected int currentWaypointIndex;

		// Token: 0x0400043E RID: 1086
		protected Vector3 lastFoundWaypointPosition;

		// Token: 0x0400043F RID: 1087
		protected float lastFoundWaypointTime = -9999f;

		// Token: 0x04000440 RID: 1088
		protected new Vector3 targetDirection;
	}
}
