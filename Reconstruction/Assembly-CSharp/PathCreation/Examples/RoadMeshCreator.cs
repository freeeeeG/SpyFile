using System;
using UnityEngine;

namespace PathCreation.Examples
{
	// Token: 0x020002B9 RID: 697
	public class RoadMeshCreator : PathSceneTool
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x00030407 File Offset: 0x0002E607
		protected override void PathUpdated()
		{
			if (this.pathCreator != null)
			{
				this.AssignMeshComponents();
				this.AssignMaterials();
				this.CreateRoadMesh();
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0003042C File Offset: 0x0002E62C
		private void CreateRoadMesh()
		{
			Vector3[] array = new Vector3[base.path.NumPoints * 8];
			Vector2[] array2 = new Vector2[array.Length];
			Vector3[] array3 = new Vector3[array.Length];
			int num = 2 * (base.path.NumPoints - 1) + (base.path.isClosedLoop ? 2 : 0);
			int[] array4 = new int[num * 3];
			int[] array5 = new int[num * 3];
			int[] array6 = new int[num * 2 * 3];
			int num2 = 0;
			int num3 = 0;
			int[] array7 = new int[]
			{
				0,
				8,
				1,
				1,
				8,
				9
			};
			int[] array8 = new int[]
			{
				4,
				6,
				14,
				12,
				4,
				14,
				5,
				15,
				7,
				13,
				15,
				5
			};
			bool flag = base.path.space != PathSpace.xyz || !this.flattenSurface;
			for (int i = 0; i < base.path.NumPoints; i++)
			{
				Vector3 vector = flag ? Vector3.Cross(base.path.GetTangent(i), base.path.GetNormal(i)) : base.path.up;
				Vector3 vector2 = flag ? base.path.GetNormal(i) : Vector3.Cross(vector, base.path.GetTangent(i));
				Vector3 vector3 = base.path.GetPoint(i) - vector2 * Mathf.Abs(this.roadWidth);
				Vector3 vector4 = base.path.GetPoint(i) + vector2 * Mathf.Abs(this.roadWidth);
				array[num2] = vector3;
				array[num2 + 1] = vector4;
				array[num2 + 2] = vector3 - vector * this.thickness;
				array[num2 + 3] = vector4 - vector * this.thickness;
				array[num2 + 4] = array[num2];
				array[num2 + 5] = array[num2 + 1];
				array[num2 + 6] = array[num2 + 2];
				array[num2 + 7] = array[num2 + 3];
				array2[num2] = new Vector2(0f, base.path.times[i]);
				array2[num2 + 1] = new Vector2(1f, base.path.times[i]);
				array3[num2] = vector;
				array3[num2 + 1] = vector;
				array3[num2 + 2] = -vector;
				array3[num2 + 3] = -vector;
				array3[num2 + 4] = -vector2;
				array3[num2 + 5] = vector2;
				array3[num2 + 6] = -vector2;
				array3[num2 + 7] = vector2;
				if (i < base.path.NumPoints - 1 || base.path.isClosedLoop)
				{
					for (int j = 0; j < array7.Length; j++)
					{
						array4[num3 + j] = (num2 + array7[j]) % array.Length;
						array5[num3 + j] = (num2 + array7[array7.Length - 1 - j] + 2) % array.Length;
					}
					for (int k = 0; k < array8.Length; k++)
					{
						array6[num3 * 2 + k] = (num2 + array8[k]) % array.Length;
					}
				}
				num2 += 8;
				num3 += 6;
			}
			this.mesh.Clear();
			this.mesh.vertices = array;
			this.mesh.uv = array2;
			this.mesh.normals = array3;
			this.mesh.subMeshCount = 3;
			this.mesh.SetTriangles(array4, 0);
			this.mesh.SetTriangles(array5, 1);
			this.mesh.SetTriangles(array6, 2);
			this.mesh.RecalculateBounds();
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00030808 File Offset: 0x0002EA08
		private void AssignMeshComponents()
		{
			if (this.meshHolder == null)
			{
				this.meshHolder = new GameObject("Road Mesh Holder");
			}
			this.meshHolder.transform.rotation = Quaternion.identity;
			this.meshHolder.transform.position = Vector3.zero;
			this.meshHolder.transform.localScale = Vector3.one;
			if (!this.meshHolder.gameObject.GetComponent<MeshFilter>())
			{
				this.meshHolder.gameObject.AddComponent<MeshFilter>();
			}
			if (!this.meshHolder.GetComponent<MeshRenderer>())
			{
				this.meshHolder.gameObject.AddComponent<MeshRenderer>();
			}
			this.meshRenderer = this.meshHolder.GetComponent<MeshRenderer>();
			this.meshFilter = this.meshHolder.GetComponent<MeshFilter>();
			if (this.mesh == null)
			{
				this.mesh = new Mesh();
			}
			this.meshFilter.sharedMesh = this.mesh;
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0003090C File Offset: 0x0002EB0C
		private void AssignMaterials()
		{
			if (this.roadMaterial != null && this.undersideMaterial != null)
			{
				this.meshRenderer.sharedMaterials = new Material[]
				{
					this.roadMaterial,
					this.undersideMaterial,
					this.undersideMaterial
				};
				this.meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1f, this.textureTiling);
			}
		}

		// Token: 0x04000952 RID: 2386
		[Header("Road settings")]
		public float roadWidth = 0.4f;

		// Token: 0x04000953 RID: 2387
		[Range(0f, 0.5f)]
		public float thickness = 0.15f;

		// Token: 0x04000954 RID: 2388
		public bool flattenSurface;

		// Token: 0x04000955 RID: 2389
		[Header("Material settings")]
		public Material roadMaterial;

		// Token: 0x04000956 RID: 2390
		public Material undersideMaterial;

		// Token: 0x04000957 RID: 2391
		public float textureTiling = 1f;

		// Token: 0x04000958 RID: 2392
		[SerializeField]
		[HideInInspector]
		private GameObject meshHolder;

		// Token: 0x04000959 RID: 2393
		private MeshFilter meshFilter;

		// Token: 0x0400095A RID: 2394
		private MeshRenderer meshRenderer;

		// Token: 0x0400095B RID: 2395
		private Mesh mesh;
	}
}
