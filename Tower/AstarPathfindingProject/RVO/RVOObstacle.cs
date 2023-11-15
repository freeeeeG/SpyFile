using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020000DC RID: 220
	public abstract class RVOObstacle : VersionedMonoBehaviour
	{
		// Token: 0x06000957 RID: 2391
		protected abstract void CreateObstacles();

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000958 RID: 2392
		protected abstract bool ExecuteInEditor { get; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000959 RID: 2393
		protected abstract bool LocalCoordinates { get; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600095A RID: 2394
		protected abstract bool StaticObstacle { get; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600095B RID: 2395
		protected abstract float Height { get; }

		// Token: 0x0600095C RID: 2396
		protected abstract bool AreGizmosDirty();

		// Token: 0x0600095D RID: 2397 RVA: 0x0003D58D File Offset: 0x0003B78D
		public void OnDrawGizmos()
		{
			this.OnDrawGizmos(false);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0003D596 File Offset: 0x0003B796
		public void OnDrawGizmosSelected()
		{
			this.OnDrawGizmos(true);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0003D5A0 File Offset: 0x0003B7A0
		public void OnDrawGizmos(bool selected)
		{
			this.gizmoDrawing = true;
			Gizmos.color = new Color(0.615f, 1f, 0.06f, selected ? 1f : 0.7f);
			MovementPlane movementPlane = (RVOSimulator.active != null) ? RVOSimulator.active.movementPlane : MovementPlane.XZ;
			Vector3 vector = (movementPlane == MovementPlane.XZ) ? Vector3.up : (-Vector3.forward);
			if (this.gizmoVerts == null || this.AreGizmosDirty() || this._obstacleMode != this.obstacleMode)
			{
				this._obstacleMode = this.obstacleMode;
				if (this.gizmoVerts == null)
				{
					this.gizmoVerts = new List<Vector3[]>();
				}
				else
				{
					this.gizmoVerts.Clear();
				}
				this.CreateObstacles();
			}
			Matrix4x4 matrix = this.GetMatrix();
			for (int i = 0; i < this.gizmoVerts.Count; i++)
			{
				Vector3[] array = this.gizmoVerts[i];
				int j = 0;
				int num = array.Length - 1;
				while (j < array.Length)
				{
					Gizmos.DrawLine(matrix.MultiplyPoint3x4(array[j]), matrix.MultiplyPoint3x4(array[num]));
					num = j++;
				}
				if (selected)
				{
					int k = 0;
					int num2 = array.Length - 1;
					while (k < array.Length)
					{
						Vector3 vector2 = matrix.MultiplyPoint3x4(array[num2]);
						Vector3 vector3 = matrix.MultiplyPoint3x4(array[k]);
						if (movementPlane != MovementPlane.XY)
						{
							Gizmos.DrawLine(vector2 + vector * this.Height, vector3 + vector * this.Height);
							Gizmos.DrawLine(vector2, vector2 + vector * this.Height);
						}
						Vector3 vector4 = (vector2 + vector3) * 0.5f;
						Vector3 normalized = (vector3 - vector2).normalized;
						if (!(normalized == Vector3.zero))
						{
							Vector3 vector5 = Vector3.Cross(vector, normalized);
							Gizmos.DrawLine(vector4, vector4 + vector5);
							Gizmos.DrawLine(vector4 + vector5, vector4 + vector5 * 0.5f + normalized * 0.5f);
							Gizmos.DrawLine(vector4 + vector5, vector4 + vector5 * 0.5f - normalized * 0.5f);
						}
						num2 = k++;
					}
				}
			}
			this.gizmoDrawing = false;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0003D823 File Offset: 0x0003BA23
		protected virtual Matrix4x4 GetMatrix()
		{
			if (!this.LocalCoordinates)
			{
				return Matrix4x4.identity;
			}
			return base.transform.localToWorldMatrix;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0003D840 File Offset: 0x0003BA40
		public void OnDisable()
		{
			if (this.addedObstacles != null)
			{
				if (this.sim == null)
				{
					throw new Exception("This should not happen! Make sure you are not overriding the OnEnable function");
				}
				for (int i = 0; i < this.addedObstacles.Count; i++)
				{
					this.sim.RemoveObstacle(this.addedObstacles[i]);
				}
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0003D898 File Offset: 0x0003BA98
		public void OnEnable()
		{
			if (this.addedObstacles != null)
			{
				if (this.sim == null)
				{
					throw new Exception("This should not happen! Make sure you are not overriding the OnDisable function");
				}
				for (int i = 0; i < this.addedObstacles.Count; i++)
				{
					ObstacleVertex obstacleVertex = this.addedObstacles[i];
					ObstacleVertex obstacleVertex2 = obstacleVertex;
					do
					{
						obstacleVertex.layer = this.layer;
						obstacleVertex = obstacleVertex.next;
					}
					while (obstacleVertex != obstacleVertex2);
					this.sim.AddObstacle(this.addedObstacles[i]);
				}
			}
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0003D914 File Offset: 0x0003BB14
		public void Start()
		{
			this.addedObstacles = new List<ObstacleVertex>();
			this.sourceObstacles = new List<Vector3[]>();
			this.prevUpdateMatrix = this.GetMatrix();
			this.CreateObstacles();
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0003D940 File Offset: 0x0003BB40
		public void Update()
		{
			Matrix4x4 matrix = this.GetMatrix();
			if (matrix != this.prevUpdateMatrix)
			{
				for (int i = 0; i < this.addedObstacles.Count; i++)
				{
					this.sim.UpdateObstacle(this.addedObstacles[i], this.sourceObstacles[i], matrix);
				}
				this.prevUpdateMatrix = matrix;
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0003D9A3 File Offset: 0x0003BBA3
		protected void FindSimulator()
		{
			if (RVOSimulator.active == null)
			{
				throw new InvalidOperationException("No RVOSimulator could be found in the scene. Please add one to any GameObject");
			}
			this.sim = RVOSimulator.active.GetSimulator();
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0003D9D0 File Offset: 0x0003BBD0
		protected void AddObstacle(Vector3[] vertices, float height)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException("Vertices Must Not Be Null");
			}
			if (height < 0f)
			{
				throw new ArgumentOutOfRangeException("Height must be non-negative");
			}
			if (vertices.Length < 2)
			{
				throw new ArgumentException("An obstacle must have at least two vertices");
			}
			if (this.sim == null)
			{
				this.FindSimulator();
			}
			if (this.gizmoDrawing)
			{
				Vector3[] array = new Vector3[vertices.Length];
				this.WindCorrectly(vertices);
				Array.Copy(vertices, array, vertices.Length);
				this.gizmoVerts.Add(array);
				return;
			}
			if (vertices.Length == 2)
			{
				this.AddObstacleInternal(vertices, height);
				return;
			}
			this.WindCorrectly(vertices);
			this.AddObstacleInternal(vertices, height);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0003DA6A File Offset: 0x0003BC6A
		private void AddObstacleInternal(Vector3[] vertices, float height)
		{
			this.addedObstacles.Add(this.sim.AddObstacle(vertices, height, this.GetMatrix(), this.layer, true));
			this.sourceObstacles.Add(vertices);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0003DAA0 File Offset: 0x0003BCA0
		private void WindCorrectly(Vector3[] vertices)
		{
			int num = 0;
			float num2 = float.PositiveInfinity;
			Matrix4x4 matrix = this.GetMatrix();
			for (int i = 0; i < vertices.Length; i++)
			{
				float x = matrix.MultiplyPoint3x4(vertices[i]).x;
				if (x < num2)
				{
					num = i;
					num2 = x;
				}
			}
			Vector3 vector = matrix.MultiplyPoint3x4(vertices[(num - 1 + vertices.Length) % vertices.Length]);
			Vector3 vector2 = matrix.MultiplyPoint3x4(vertices[num]);
			Vector3 vector3 = matrix.MultiplyPoint3x4(vertices[(num + 1) % vertices.Length]);
			MovementPlane movementPlane;
			if (this.sim != null)
			{
				movementPlane = this.sim.movementPlane;
			}
			else if (RVOSimulator.active)
			{
				movementPlane = RVOSimulator.active.movementPlane;
			}
			else
			{
				movementPlane = MovementPlane.XZ;
			}
			if (movementPlane == MovementPlane.XY)
			{
				vector.z = vector.y;
				vector2.z = vector2.y;
				vector3.z = vector3.y;
			}
			if (VectorMath.IsClockwiseXZ(vector, vector2, vector3) != (this.obstacleMode == RVOObstacle.ObstacleVertexWinding.KeepIn))
			{
				Array.Reverse<Vector3>(vertices);
			}
		}

		// Token: 0x04000577 RID: 1399
		public RVOObstacle.ObstacleVertexWinding obstacleMode;

		// Token: 0x04000578 RID: 1400
		public RVOLayer layer = RVOLayer.DefaultObstacle;

		// Token: 0x04000579 RID: 1401
		protected Simulator sim;

		// Token: 0x0400057A RID: 1402
		private List<ObstacleVertex> addedObstacles;

		// Token: 0x0400057B RID: 1403
		private List<Vector3[]> sourceObstacles;

		// Token: 0x0400057C RID: 1404
		private bool gizmoDrawing;

		// Token: 0x0400057D RID: 1405
		private List<Vector3[]> gizmoVerts;

		// Token: 0x0400057E RID: 1406
		private RVOObstacle.ObstacleVertexWinding _obstacleMode;

		// Token: 0x0400057F RID: 1407
		private Matrix4x4 prevUpdateMatrix;

		// Token: 0x02000178 RID: 376
		public enum ObstacleVertexWinding
		{
			// Token: 0x0400085C RID: 2140
			KeepOut,
			// Token: 0x0400085D RID: 2141
			KeepIn
		}
	}
}
