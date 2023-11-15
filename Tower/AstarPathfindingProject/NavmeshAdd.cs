using System;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000081 RID: 129
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_navmesh_add.php")]
	public class NavmeshAdd : NavmeshClipper
	{
		// Token: 0x06000671 RID: 1649 RVA: 0x000269F8 File Offset: 0x00024BF8
		public override bool RequiresUpdate()
		{
			return (this.tr.position - this.lastPosition).sqrMagnitude > this.updateDistance * this.updateDistance || (this.useRotationAndScale && Quaternion.Angle(this.lastRotation, this.tr.rotation) > this.updateRotationDistance);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00026A5C File Offset: 0x00024C5C
		public override void ForceUpdate()
		{
			this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00026A78 File Offset: 0x00024C78
		protected override void Awake()
		{
			base.Awake();
			this.tr = base.transform;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00026A8C File Offset: 0x00024C8C
		internal override void NotifyUpdated()
		{
			this.lastPosition = this.tr.position;
			if (this.useRotationAndScale)
			{
				this.lastRotation = this.tr.rotation;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x00026AB8 File Offset: 0x00024CB8
		public Vector3 Center
		{
			get
			{
				return this.tr.position + (this.useRotationAndScale ? this.tr.TransformPoint(this.center) : this.center);
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00026AEC File Offset: 0x00024CEC
		[ContextMenu("Rebuild Mesh")]
		public void RebuildMesh()
		{
			if (this.type != NavmeshAdd.MeshType.CustomMesh)
			{
				if (this.verts == null || this.verts.Length != 4 || this.tris == null || this.tris.Length != 6)
				{
					this.verts = new Vector3[4];
					this.tris = new int[6];
				}
				this.tris[0] = 0;
				this.tris[1] = 1;
				this.tris[2] = 2;
				this.tris[3] = 0;
				this.tris[4] = 2;
				this.tris[5] = 3;
				this.verts[0] = new Vector3(-this.rectangleSize.x * 0.5f, 0f, -this.rectangleSize.y * 0.5f);
				this.verts[1] = new Vector3(this.rectangleSize.x * 0.5f, 0f, -this.rectangleSize.y * 0.5f);
				this.verts[2] = new Vector3(this.rectangleSize.x * 0.5f, 0f, this.rectangleSize.y * 0.5f);
				this.verts[3] = new Vector3(-this.rectangleSize.x * 0.5f, 0f, this.rectangleSize.y * 0.5f);
				return;
			}
			if (this.mesh == null)
			{
				this.verts = null;
				this.tris = null;
				return;
			}
			this.verts = this.mesh.vertices;
			this.tris = this.mesh.triangles;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00026C9C File Offset: 0x00024E9C
		public override Rect GetBounds(GraphTransform inverseTransform)
		{
			if (this.verts == null)
			{
				this.RebuildMesh();
			}
			Int3[] array = ArrayPool<Int3>.Claim((this.verts != null) ? this.verts.Length : 0);
			int[] array2;
			this.GetMesh(ref array, out array2, inverseTransform);
			Rect result = default(Rect);
			for (int i = 0; i < array2.Length; i++)
			{
				Vector3 vector = (Vector3)array[array2[i]];
				if (i == 0)
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
			ArrayPool<Int3>.Release(ref array, false);
			return result;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00026D9C File Offset: 0x00024F9C
		public void GetMesh(ref Int3[] vbuffer, out int[] tbuffer, GraphTransform inverseTransform = null)
		{
			if (this.verts == null)
			{
				this.RebuildMesh();
			}
			if (this.verts == null)
			{
				tbuffer = ArrayPool<int>.Claim(0);
				return;
			}
			if (vbuffer == null || vbuffer.Length < this.verts.Length)
			{
				if (vbuffer != null)
				{
					ArrayPool<Int3>.Release(ref vbuffer, false);
				}
				vbuffer = ArrayPool<Int3>.Claim(this.verts.Length);
			}
			tbuffer = this.tris;
			if (this.useRotationAndScale)
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(this.tr.position + this.center, this.tr.rotation, this.tr.localScale * this.meshScale);
				for (int i = 0; i < this.verts.Length; i++)
				{
					Vector3 vector = matrix4x.MultiplyPoint3x4(this.verts[i]);
					if (inverseTransform != null)
					{
						vector = inverseTransform.InverseTransform(vector);
					}
					vbuffer[i] = (Int3)vector;
				}
				return;
			}
			Vector3 a = this.tr.position + this.center;
			for (int j = 0; j < this.verts.Length; j++)
			{
				Vector3 vector2 = a + this.verts[j] * this.meshScale;
				if (inverseTransform != null)
				{
					vector2 = inverseTransform.InverseTransform(vector2);
				}
				vbuffer[j] = (Int3)vector2;
			}
		}

		// Token: 0x040003B4 RID: 948
		public NavmeshAdd.MeshType type;

		// Token: 0x040003B5 RID: 949
		public Mesh mesh;

		// Token: 0x040003B6 RID: 950
		private Vector3[] verts;

		// Token: 0x040003B7 RID: 951
		private int[] tris;

		// Token: 0x040003B8 RID: 952
		public Vector2 rectangleSize = new Vector2(1f, 1f);

		// Token: 0x040003B9 RID: 953
		public float meshScale = 1f;

		// Token: 0x040003BA RID: 954
		public Vector3 center;

		// Token: 0x040003BB RID: 955
		[FormerlySerializedAs("useRotation")]
		public bool useRotationAndScale;

		// Token: 0x040003BC RID: 956
		[Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
		public float updateDistance = 0.4f;

		// Token: 0x040003BD RID: 957
		[Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
		public float updateRotationDistance = 10f;

		// Token: 0x040003BE RID: 958
		protected Transform tr;

		// Token: 0x040003BF RID: 959
		private Vector3 lastPosition;

		// Token: 0x040003C0 RID: 960
		private Quaternion lastRotation;

		// Token: 0x040003C1 RID: 961
		public static readonly Color GizmoColor = new Color(0.6039216f, 0.13725491f, 0.9372549f);

		// Token: 0x02000150 RID: 336
		public enum MeshType
		{
			// Token: 0x040007B4 RID: 1972
			Rectangle,
			// Token: 0x040007B5 RID: 1973
			CustomMesh
		}
	}
}
