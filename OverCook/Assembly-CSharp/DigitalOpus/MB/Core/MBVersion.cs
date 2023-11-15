using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200001E RID: 30
	public class MBVersion
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00004370 File Offset: 0x00002770
		private static MBVersionInterface _CreateMBVersionConcrete()
		{
			Type typeFromHandle = typeof(MBVersionConcrete);
			return (MBVersionInterface)Activator.CreateInstance(typeFromHandle);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004395 File Offset: 0x00002795
		public static string version()
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.version();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000043B5 File Offset: 0x000027B5
		public static int GetMajorVersion()
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetMajorVersion();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000043D5 File Offset: 0x000027D5
		public static int GetMinorVersion()
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetMinorVersion();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000043F5 File Offset: 0x000027F5
		public static bool GetActive(GameObject go)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetActive(go);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004416 File Offset: 0x00002816
		public static void SetActive(GameObject go, bool isActive)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.SetActive(go, isActive);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004438 File Offset: 0x00002838
		public static void SetActiveRecursively(GameObject go, bool isActive)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.SetActiveRecursively(go, isActive);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000445A File Offset: 0x0000285A
		public static UnityEngine.Object[] FindSceneObjectsOfType(Type t)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.FindSceneObjectsOfType(t);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000447B File Offset: 0x0000287B
		public static bool IsRunningAndMeshNotReadWriteable(Mesh m)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.IsRunningAndMeshNotReadWriteable(m);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000449C File Offset: 0x0000289C
		public static Vector2[] GetMeshUV3orUV4(Mesh m, bool get3, MB2_LogLevel LOG_LEVEL)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetMeshUV3orUV4(m, get3, LOG_LEVEL);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000044BF File Offset: 0x000028BF
		public static void MeshClear(Mesh m, bool t)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.MeshClear(m, t);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000044E1 File Offset: 0x000028E1
		public static void MeshAssignUV3(Mesh m, Vector2[] uv3s)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.MeshAssignUV3(m, uv3s);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004503 File Offset: 0x00002903
		public static void MeshAssignUV4(Mesh m, Vector2[] uv4s)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.MeshAssignUV4(m, uv4s);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004525 File Offset: 0x00002925
		public static Vector4 GetLightmapTilingOffset(Renderer r)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetLightmapTilingOffset(r);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004546 File Offset: 0x00002946
		public static Transform[] GetBones(Renderer r)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetBones(r);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004567 File Offset: 0x00002967
		public static void OptimizeMesh(Mesh m)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.OptimizeMesh(m);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004588 File Offset: 0x00002988
		public static int GetBlendShapeFrameCount(Mesh m, int shapeIndex)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetBlendShapeFrameCount(m, shapeIndex);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000045AA File Offset: 0x000029AA
		public static float GetBlendShapeFrameWeight(Mesh m, int shapeIndex, int frameIndex)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetBlendShapeFrameWeight(m, shapeIndex, frameIndex);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000045CD File Offset: 0x000029CD
		public static void GetBlendShapeFrameVertices(Mesh m, int shapeIndex, int frameIndex, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.GetBlendShapeFrameVertices(m, shapeIndex, frameIndex, vs, ns, ts);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000045F5 File Offset: 0x000029F5
		public static void ClearBlendShapes(Mesh m)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.ClearBlendShapes(m);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004616 File Offset: 0x00002A16
		public static void AddBlendShapeFrame(Mesh m, string nm, float wt, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.AddBlendShapeFrame(m, nm, wt, vs, ns, ts);
		}

		// Token: 0x04000063 RID: 99
		private static MBVersionInterface _MBVersion;
	}
}
