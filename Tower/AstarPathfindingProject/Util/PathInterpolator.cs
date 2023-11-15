using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x020000C6 RID: 198
	public class PathInterpolator
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00037AE0 File Offset: 0x00035CE0
		public virtual Vector3 position
		{
			get
			{
				float t = (this.currentSegmentLength > 0.0001f) ? ((this.currentDistance - this.distanceToSegmentStart) / this.currentSegmentLength) : 0f;
				return Vector3.Lerp(this.path[this.segmentIndex], this.path[this.segmentIndex + 1], t);
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x00037B40 File Offset: 0x00035D40
		public Vector3 endPoint
		{
			get
			{
				return this.path[this.path.Count - 1];
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00037B5A File Offset: 0x00035D5A
		public Vector3 tangent
		{
			get
			{
				return this.path[this.segmentIndex + 1] - this.path[this.segmentIndex];
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x00037B85 File Offset: 0x00035D85
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x00037B94 File Offset: 0x00035D94
		public float remainingDistance
		{
			get
			{
				return this.totalDistance - this.distance;
			}
			set
			{
				this.distance = this.totalDistance - value;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x00037BA4 File Offset: 0x00035DA4
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x00037BAC File Offset: 0x00035DAC
		public float distance
		{
			get
			{
				return this.currentDistance;
			}
			set
			{
				this.currentDistance = value;
				while (this.currentDistance < this.distanceToSegmentStart)
				{
					if (this.segmentIndex <= 0)
					{
						break;
					}
					this.PrevSegment();
				}
				while (this.currentDistance > this.distanceToSegmentStart + this.currentSegmentLength && this.segmentIndex < this.path.Count - 2)
				{
					this.NextSegment();
				}
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x00037C11 File Offset: 0x00035E11
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x00037C19 File Offset: 0x00035E19
		public int segmentIndex { get; private set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x00037C22 File Offset: 0x00035E22
		public bool valid
		{
			get
			{
				return this.path != null;
			}
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00037C30 File Offset: 0x00035E30
		public void GetRemainingPath(List<Vector3> buffer)
		{
			if (!this.valid)
			{
				throw new Exception("PathInterpolator is not valid");
			}
			buffer.Add(this.position);
			for (int i = this.segmentIndex + 1; i < this.path.Count; i++)
			{
				buffer.Add(this.path[i]);
			}
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00037C8C File Offset: 0x00035E8C
		public void SetPath(List<Vector3> path)
		{
			this.path = path;
			this.currentDistance = 0f;
			this.segmentIndex = 0;
			this.distanceToSegmentStart = 0f;
			if (path == null)
			{
				this.totalDistance = float.PositiveInfinity;
				this.currentSegmentLength = float.PositiveInfinity;
				return;
			}
			if (path.Count < 2)
			{
				throw new ArgumentException("Path must have a length of at least 2");
			}
			this.currentSegmentLength = (path[1] - path[0]).magnitude;
			this.totalDistance = 0f;
			Vector3 b = path[0];
			for (int i = 1; i < path.Count; i++)
			{
				Vector3 vector = path[i];
				this.totalDistance += (vector - b).magnitude;
				b = vector;
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00037D58 File Offset: 0x00035F58
		public void MoveToSegment(int index, float fractionAlongSegment)
		{
			if (this.path == null)
			{
				return;
			}
			if (index < 0 || index >= this.path.Count - 1)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			while (this.segmentIndex > index)
			{
				this.PrevSegment();
			}
			while (this.segmentIndex < index)
			{
				this.NextSegment();
			}
			this.distance = this.distanceToSegmentStart + Mathf.Clamp01(fractionAlongSegment) * this.currentSegmentLength;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00037DC8 File Offset: 0x00035FC8
		public void MoveToClosestPoint(Vector3 point)
		{
			if (this.path == null)
			{
				return;
			}
			float num = float.PositiveInfinity;
			float fractionAlongSegment = 0f;
			int index = 0;
			for (int i = 0; i < this.path.Count - 1; i++)
			{
				float num2 = VectorMath.ClosestPointOnLineFactor(this.path[i], this.path[i + 1], point);
				Vector3 b = Vector3.Lerp(this.path[i], this.path[i + 1], num2);
				float sqrMagnitude = (point - b).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					fractionAlongSegment = num2;
					index = i;
				}
			}
			this.MoveToSegment(index, fractionAlongSegment);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00037E74 File Offset: 0x00036074
		public void MoveToLocallyClosestPoint(Vector3 point, bool allowForwards = true, bool allowBackwards = true)
		{
			if (this.path == null)
			{
				return;
			}
			while (allowForwards && this.segmentIndex < this.path.Count - 2)
			{
				if ((this.path[this.segmentIndex + 1] - point).sqrMagnitude > (this.path[this.segmentIndex] - point).sqrMagnitude)
				{
					break;
				}
				this.NextSegment();
			}
			while (allowBackwards && this.segmentIndex > 0 && (this.path[this.segmentIndex - 1] - point).sqrMagnitude <= (this.path[this.segmentIndex] - point).sqrMagnitude)
			{
				this.PrevSegment();
			}
			float num = 0f;
			float num2 = 0f;
			float num3 = float.PositiveInfinity;
			float num4 = float.PositiveInfinity;
			if (this.segmentIndex > 0)
			{
				num = VectorMath.ClosestPointOnLineFactor(this.path[this.segmentIndex - 1], this.path[this.segmentIndex], point);
				num3 = (Vector3.Lerp(this.path[this.segmentIndex - 1], this.path[this.segmentIndex], num) - point).sqrMagnitude;
			}
			if (this.segmentIndex < this.path.Count - 1)
			{
				num2 = VectorMath.ClosestPointOnLineFactor(this.path[this.segmentIndex], this.path[this.segmentIndex + 1], point);
				num4 = (Vector3.Lerp(this.path[this.segmentIndex], this.path[this.segmentIndex + 1], num2) - point).sqrMagnitude;
			}
			if (num3 < num4)
			{
				this.MoveToSegment(this.segmentIndex - 1, num);
				return;
			}
			this.MoveToSegment(this.segmentIndex, num2);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00038068 File Offset: 0x00036268
		public void MoveToCircleIntersection2D(Vector3 circleCenter3D, float radius, IMovementPlane transform)
		{
			if (this.path == null)
			{
				return;
			}
			while (this.segmentIndex < this.path.Count - 2 && VectorMath.ClosestPointOnLineFactor(this.path[this.segmentIndex], this.path[this.segmentIndex + 1], circleCenter3D) > 1f)
			{
				this.NextSegment();
			}
			Vector2 vector = transform.ToPlane(circleCenter3D);
			while (this.segmentIndex < this.path.Count - 2 && (transform.ToPlane(this.path[this.segmentIndex + 1]) - vector).sqrMagnitude <= radius * radius)
			{
				this.NextSegment();
			}
			float fractionAlongSegment = VectorMath.LineCircleIntersectionFactor(vector, transform.ToPlane(this.path[this.segmentIndex]), transform.ToPlane(this.path[this.segmentIndex + 1]), radius);
			this.MoveToSegment(this.segmentIndex, fractionAlongSegment);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00038170 File Offset: 0x00036370
		protected virtual void PrevSegment()
		{
			int segmentIndex = this.segmentIndex;
			this.segmentIndex = segmentIndex - 1;
			this.currentSegmentLength = (this.path[this.segmentIndex + 1] - this.path[this.segmentIndex]).magnitude;
			this.distanceToSegmentStart -= this.currentSegmentLength;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x000381D8 File Offset: 0x000363D8
		protected virtual void NextSegment()
		{
			int segmentIndex = this.segmentIndex;
			this.segmentIndex = segmentIndex + 1;
			this.distanceToSegmentStart += this.currentSegmentLength;
			this.currentSegmentLength = (this.path[this.segmentIndex + 1] - this.path[this.segmentIndex]).magnitude;
		}

		// Token: 0x040004E1 RID: 1249
		private List<Vector3> path;

		// Token: 0x040004E2 RID: 1250
		private float distanceToSegmentStart;

		// Token: 0x040004E3 RID: 1251
		private float currentDistance;

		// Token: 0x040004E4 RID: 1252
		private float currentSegmentLength = float.PositiveInfinity;

		// Token: 0x040004E5 RID: 1253
		private float totalDistance = float.PositiveInfinity;
	}
}
