using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000083 RID: 131
	[AddComponentMenu("Pathfinding/Navmesh/Navmesh Cut")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_navmesh_cut.php")]
	public class NavmeshCut : NavmeshClipper
	{
		// Token: 0x06000686 RID: 1670 RVA: 0x00027077 File Offset: 0x00025277
		protected override void Awake()
		{
			base.Awake();
			this.tr = base.transform;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0002708B File Offset: 0x0002528B
		protected override void OnEnable()
		{
			base.OnEnable();
			this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
			this.lastRotation = this.tr.rotation;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000270BE File Offset: 0x000252BE
		public override void ForceUpdate()
		{
			this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000270DC File Offset: 0x000252DC
		public override bool RequiresUpdate()
		{
			return (this.tr.position - this.lastPosition).sqrMagnitude > this.updateDistance * this.updateDistance || (this.useRotationAndScale && Quaternion.Angle(this.lastRotation, this.tr.rotation) > this.updateRotationDistance);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00027140 File Offset: 0x00025340
		public virtual void UsedForCut()
		{
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00027142 File Offset: 0x00025342
		internal override void NotifyUpdated()
		{
			this.lastPosition = this.tr.position;
			if (this.useRotationAndScale)
			{
				this.lastRotation = this.tr.rotation;
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00027170 File Offset: 0x00025370
		private void CalculateMeshContour()
		{
			if (this.mesh == null)
			{
				return;
			}
			NavmeshCut.edges.Clear();
			NavmeshCut.pointers.Clear();
			Vector3[] vertices = this.mesh.vertices;
			int[] triangles = this.mesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (VectorMath.IsClockwiseXZ(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]))
				{
					int num = triangles[i];
					triangles[i] = triangles[i + 2];
					triangles[i + 2] = num;
				}
				NavmeshCut.edges[new Int2(triangles[i], triangles[i + 1])] = i;
				NavmeshCut.edges[new Int2(triangles[i + 1], triangles[i + 2])] = i;
				NavmeshCut.edges[new Int2(triangles[i + 2], triangles[i])] = i;
			}
			for (int j = 0; j < triangles.Length; j += 3)
			{
				for (int k = 0; k < 3; k++)
				{
					if (!NavmeshCut.edges.ContainsKey(new Int2(triangles[j + (k + 1) % 3], triangles[j + k % 3])))
					{
						NavmeshCut.pointers[triangles[j + k % 3]] = triangles[j + (k + 1) % 3];
					}
				}
			}
			List<Vector3[]> list = new List<Vector3[]>();
			List<Vector3> list2 = ListPool<Vector3>.Claim();
			for (int l = 0; l < vertices.Length; l++)
			{
				if (NavmeshCut.pointers.ContainsKey(l))
				{
					list2.Clear();
					int num2 = l;
					do
					{
						int num3 = NavmeshCut.pointers[num2];
						if (num3 == -1)
						{
							break;
						}
						NavmeshCut.pointers[num2] = -1;
						list2.Add(vertices[num2]);
						num2 = num3;
						if (num2 == -1)
						{
							goto Block_9;
						}
					}
					while (num2 != l);
					IL_1E4:
					if (list2.Count > 0)
					{
						list.Add(list2.ToArray());
						goto IL_1F9;
					}
					goto IL_1F9;
					Block_9:
					Debug.LogError("Invalid Mesh '" + this.mesh.name + " in " + base.gameObject.name);
					goto IL_1E4;
				}
				IL_1F9:;
			}
			ListPool<Vector3>.Release(ref list2);
			this.contours = list.ToArray();
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0002739C File Offset: 0x0002559C
		public override Rect GetBounds(GraphTransform inverseTransform)
		{
			List<List<Vector3>> list = ListPool<List<Vector3>>.Claim();
			this.GetContour(list);
			Rect result = default(Rect);
			for (int i = 0; i < list.Count; i++)
			{
				List<Vector3> list2 = list[i];
				for (int j = 0; j < list2.Count; j++)
				{
					Vector3 vector = inverseTransform.InverseTransform(list2[j]);
					if (j == 0)
					{
						result = new Rect(vector.x, vector.z, 0f, 0f);
					}
					else
					{
						result.xMax = Math.Max(result.xMax, vector.x);
						result.yMax = Math.Max(result.yMax, vector.z);
						result.xMin = Math.Min(result.xMin, vector.x);
						result.yMin = Math.Min(result.yMin, vector.z);
					}
				}
			}
			ListPool<List<Vector3>>.Release(ref list);
			return result;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000274A0 File Offset: 0x000256A0
		public void GetContour(List<List<Vector3>> buffer)
		{
			if (this.circleResolution < 3)
			{
				this.circleResolution = 3;
			}
			switch (this.type)
			{
			case NavmeshCut.MeshType.Rectangle:
			{
				List<Vector3> list = ListPool<Vector3>.Claim();
				list.Add(new Vector3(-this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f);
				list.Add(new Vector3(this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f);
				list.Add(new Vector3(this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f);
				list.Add(new Vector3(-this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f);
				bool reverse = this.rectangleSize.x < 0f ^ this.rectangleSize.y < 0f;
				this.TransformBuffer(list, reverse);
				buffer.Add(list);
				return;
			}
			case NavmeshCut.MeshType.Circle:
			{
				List<Vector3> list = ListPool<Vector3>.Claim(this.circleResolution);
				for (int i = 0; i < this.circleResolution; i++)
				{
					list.Add(new Vector3(Mathf.Cos((float)(i * 2) * 3.1415927f / (float)this.circleResolution), 0f, Mathf.Sin((float)(i * 2) * 3.1415927f / (float)this.circleResolution)) * this.circleRadius);
				}
				bool reverse = this.circleRadius < 0f;
				this.TransformBuffer(list, reverse);
				buffer.Add(list);
				return;
			}
			case NavmeshCut.MeshType.CustomMesh:
				if (this.mesh != this.lastMesh || this.contours == null)
				{
					this.CalculateMeshContour();
					this.lastMesh = this.mesh;
				}
				if (this.contours != null)
				{
					bool reverse = this.meshScale < 0f;
					for (int j = 0; j < this.contours.Length; j++)
					{
						Vector3[] array = this.contours[j];
						List<Vector3> list = ListPool<Vector3>.Claim(array.Length);
						for (int k = 0; k < array.Length; k++)
						{
							list.Add(array[k] * this.meshScale);
						}
						this.TransformBuffer(list, reverse);
						buffer.Add(list);
					}
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00027708 File Offset: 0x00025908
		private void TransformBuffer(List<Vector3> buffer, bool reverse)
		{
			Vector3 vector = this.center;
			if (this.useRotationAndScale)
			{
				Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
				for (int i = 0; i < buffer.Count; i++)
				{
					buffer[i] = localToWorldMatrix.MultiplyPoint3x4(buffer[i] + vector);
				}
				reverse ^= VectorMath.ReversesFaceOrientationsXZ(localToWorldMatrix);
			}
			else
			{
				vector += this.tr.position;
				for (int j = 0; j < buffer.Count; j++)
				{
					int index = j;
					buffer[index] += vector;
				}
			}
			if (reverse)
			{
				buffer.Reverse();
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000277B4 File Offset: 0x000259B4
		public void OnDrawGizmos()
		{
			if (this.tr == null)
			{
				this.tr = base.transform;
			}
			List<List<Vector3>> list = ListPool<List<Vector3>>.Claim();
			this.GetContour(list);
			Gizmos.color = NavmeshCut.GizmoColor;
			for (int i = 0; i < list.Count; i++)
			{
				List<Vector3> list2 = list[i];
				for (int j = 0; j < list2.Count; j++)
				{
					Vector3 from = list2[j];
					Vector3 to = list2[(j + 1) % list2.Count];
					Gizmos.DrawLine(from, to);
				}
			}
			ListPool<List<Vector3>>.Release(ref list);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00027843 File Offset: 0x00025A43
		internal float GetY(GraphTransform transform)
		{
			return transform.InverseTransform(this.useRotationAndScale ? this.tr.TransformPoint(this.center) : (this.tr.position + this.center)).y;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00027884 File Offset: 0x00025A84
		public void OnDrawGizmosSelected()
		{
			List<List<Vector3>> list = ListPool<List<Vector3>>.Claim();
			this.GetContour(list);
			Color color = Color.Lerp(NavmeshCut.GizmoColor, Color.white, 0.5f);
			color.a *= 0.5f;
			Gizmos.color = color;
			NavmeshBase navmeshBase = (AstarPath.active != null) ? (AstarPath.active.data.recastGraph ?? AstarPath.active.data.navmesh) : null;
			GraphTransform graphTransform = (navmeshBase != null) ? navmeshBase.transform : GraphTransform.identityTransform;
			float y = this.GetY(graphTransform);
			float y2 = y - this.height * 0.5f;
			float y3 = y + this.height * 0.5f;
			for (int i = 0; i < list.Count; i++)
			{
				List<Vector3> list2 = list[i];
				for (int j = 0; j < list2.Count; j++)
				{
					Vector3 vector = graphTransform.InverseTransform(list2[j]);
					Vector3 vector2 = graphTransform.InverseTransform(list2[(j + 1) % list2.Count]);
					Vector3 point = vector;
					Vector3 point2 = vector2;
					Vector3 point3 = vector;
					Vector3 point4 = vector2;
					point.y = (point2.y = y2);
					point3.y = (point4.y = y3);
					Gizmos.DrawLine(graphTransform.Transform(point), graphTransform.Transform(point2));
					Gizmos.DrawLine(graphTransform.Transform(point3), graphTransform.Transform(point4));
					Gizmos.DrawLine(graphTransform.Transform(point), graphTransform.Transform(point3));
				}
			}
			ListPool<List<Vector3>>.Release(ref list);
		}

		// Token: 0x040003C7 RID: 967
		[Tooltip("Shape of the cut")]
		public NavmeshCut.MeshType type;

		// Token: 0x040003C8 RID: 968
		[Tooltip("The contour(s) of the mesh will be extracted. This mesh should only be a 2D surface, not a volume (see documentation).")]
		public Mesh mesh;

		// Token: 0x040003C9 RID: 969
		public Vector2 rectangleSize = new Vector2(1f, 1f);

		// Token: 0x040003CA RID: 970
		public float circleRadius = 1f;

		// Token: 0x040003CB RID: 971
		public int circleResolution = 6;

		// Token: 0x040003CC RID: 972
		public float height = 1f;

		// Token: 0x040003CD RID: 973
		[Tooltip("Scale of the custom mesh")]
		public float meshScale = 1f;

		// Token: 0x040003CE RID: 974
		public Vector3 center;

		// Token: 0x040003CF RID: 975
		[Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
		public float updateDistance = 0.4f;

		// Token: 0x040003D0 RID: 976
		[Tooltip("Only makes a split in the navmesh, but does not remove the geometry to make a hole")]
		public bool isDual;

		// Token: 0x040003D1 RID: 977
		public bool cutsAddedGeom = true;

		// Token: 0x040003D2 RID: 978
		[Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
		public float updateRotationDistance = 10f;

		// Token: 0x040003D3 RID: 979
		[Tooltip("Includes rotation in calculations. This is slower since a lot more matrix multiplications are needed but gives more flexibility.")]
		[FormerlySerializedAs("useRotation")]
		public bool useRotationAndScale;

		// Token: 0x040003D4 RID: 980
		private Vector3[][] contours;

		// Token: 0x040003D5 RID: 981
		protected Transform tr;

		// Token: 0x040003D6 RID: 982
		private Mesh lastMesh;

		// Token: 0x040003D7 RID: 983
		private Vector3 lastPosition;

		// Token: 0x040003D8 RID: 984
		private Quaternion lastRotation;

		// Token: 0x040003D9 RID: 985
		private static readonly Dictionary<Int2, int> edges = new Dictionary<Int2, int>();

		// Token: 0x040003DA RID: 986
		private static readonly Dictionary<int, int> pointers = new Dictionary<int, int>();

		// Token: 0x040003DB RID: 987
		public static readonly Color GizmoColor = new Color(0.14509805f, 0.72156864f, 0.9372549f);

		// Token: 0x02000151 RID: 337
		public enum MeshType
		{
			// Token: 0x040007B7 RID: 1975
			Rectangle,
			// Token: 0x040007B8 RID: 1976
			Circle,
			// Token: 0x040007B9 RID: 1977
			CustomMesh
		}
	}
}
