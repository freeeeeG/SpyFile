using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200001D RID: 29
	public interface MBVersionInterface
	{
		// Token: 0x0600005A RID: 90
		string version();

		// Token: 0x0600005B RID: 91
		int GetMajorVersion();

		// Token: 0x0600005C RID: 92
		int GetMinorVersion();

		// Token: 0x0600005D RID: 93
		bool GetActive(GameObject go);

		// Token: 0x0600005E RID: 94
		void SetActive(GameObject go, bool isActive);

		// Token: 0x0600005F RID: 95
		void SetActiveRecursively(GameObject go, bool isActive);

		// Token: 0x06000060 RID: 96
		UnityEngine.Object[] FindSceneObjectsOfType(Type t);

		// Token: 0x06000061 RID: 97
		bool IsRunningAndMeshNotReadWriteable(Mesh m);

		// Token: 0x06000062 RID: 98
		Vector2[] GetMeshUV3orUV4(Mesh m, bool get3, MB2_LogLevel LOG_LEVEL);

		// Token: 0x06000063 RID: 99
		void MeshClear(Mesh m, bool t);

		// Token: 0x06000064 RID: 100
		void MeshAssignUV3(Mesh m, Vector2[] uv3s);

		// Token: 0x06000065 RID: 101
		void MeshAssignUV4(Mesh m, Vector2[] uv4s);

		// Token: 0x06000066 RID: 102
		Vector4 GetLightmapTilingOffset(Renderer r);

		// Token: 0x06000067 RID: 103
		Transform[] GetBones(Renderer r);

		// Token: 0x06000068 RID: 104
		void OptimizeMesh(Mesh m);

		// Token: 0x06000069 RID: 105
		int GetBlendShapeFrameCount(Mesh m, int shapeIndex);

		// Token: 0x0600006A RID: 106
		float GetBlendShapeFrameWeight(Mesh m, int shapeIndex, int frameIndex);

		// Token: 0x0600006B RID: 107
		void GetBlendShapeFrameVertices(Mesh m, int shapeIndex, int frameIndex, Vector3[] vs, Vector3[] ns, Vector3[] ts);

		// Token: 0x0600006C RID: 108
		void ClearBlendShapes(Mesh m);

		// Token: 0x0600006D RID: 109
		void AddBlendShapeFrame(Mesh m, string nm, float wt, Vector3[] vs, Vector3[] ns, Vector3[] ts);
	}
}
