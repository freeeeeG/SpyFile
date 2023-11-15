using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000057 RID: 87
	public class MBVersionConcrete : MBVersionInterface
	{
		// Token: 0x06000241 RID: 577 RVA: 0x0001A00B File Offset: 0x0001840B
		public string version()
		{
			return "3.23.1";
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0001A014 File Offset: 0x00018414
		public int GetMajorVersion()
		{
			string unityVersion = Application.unityVersion;
			string[] array = unityVersion.Split(new char[]
			{
				'.'
			});
			return int.Parse(array[0]);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0001A044 File Offset: 0x00018444
		public int GetMinorVersion()
		{
			string unityVersion = Application.unityVersion;
			string[] array = unityVersion.Split(new char[]
			{
				'.'
			});
			return int.Parse(array[1]);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0001A071 File Offset: 0x00018471
		public bool GetActive(GameObject go)
		{
			return go.activeInHierarchy;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0001A079 File Offset: 0x00018479
		public void SetActive(GameObject go, bool isActive)
		{
			go.SetActive(isActive);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0001A082 File Offset: 0x00018482
		public void SetActiveRecursively(GameObject go, bool isActive)
		{
			go.SetActive(isActive);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0001A08B File Offset: 0x0001848B
		public UnityEngine.Object[] FindSceneObjectsOfType(Type t)
		{
			return UnityEngine.Object.FindObjectsOfType(t);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0001A093 File Offset: 0x00018493
		public void OptimizeMesh(Mesh m)
		{
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0001A095 File Offset: 0x00018495
		public bool IsRunningAndMeshNotReadWriteable(Mesh m)
		{
			return Application.isPlaying && !m.isReadable;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0001A0AC File Offset: 0x000184AC
		public Vector2[] GetMeshUV1s(Mesh m, MB2_LogLevel LOG_LEVEL)
		{
			if (LOG_LEVEL >= MB2_LogLevel.warn)
			{
				MB2_Log.LogDebug("UV1 does not exist in Unity 5+", new object[0]);
			}
			Vector2[] array = m.uv;
			if (array.Length == 0)
			{
				if (LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug("Mesh " + m + " has no uv1s. Generating", new object[0]);
				}
				if (LOG_LEVEL >= MB2_LogLevel.warn)
				{
					Debug.LogWarning("Mesh " + m + " didn't have uv1s. Generating uv1s.");
				}
				array = new Vector2[m.vertexCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._HALF_UV;
				}
			}
			return array;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0001A154 File Offset: 0x00018554
		public Vector2[] GetMeshUV3orUV4(Mesh m, bool get3, MB2_LogLevel LOG_LEVEL)
		{
			Vector2[] array;
			if (get3)
			{
				array = m.uv3;
			}
			else
			{
				array = m.uv4;
			}
			if (array.Length == 0)
			{
				if (LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug(string.Concat(new object[]
					{
						"Mesh ",
						m,
						" has no uv",
						(!get3) ? "4" : "3",
						". Generating"
					}), new object[0]);
				}
				array = new Vector2[m.vertexCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._HALF_UV;
				}
			}
			return array;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0001A207 File Offset: 0x00018607
		public void MeshClear(Mesh m, bool t)
		{
			m.Clear(t);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0001A210 File Offset: 0x00018610
		public void MeshAssignUV3(Mesh m, Vector2[] uv3s)
		{
			m.uv3 = uv3s;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0001A219 File Offset: 0x00018619
		public void MeshAssignUV4(Mesh m, Vector2[] uv4s)
		{
			m.uv4 = uv4s;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0001A222 File Offset: 0x00018622
		public Vector4 GetLightmapTilingOffset(Renderer r)
		{
			return r.lightmapScaleOffset;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0001A22C File Offset: 0x0001862C
		public Transform[] GetBones(Renderer r)
		{
			if (r is SkinnedMeshRenderer)
			{
				return ((SkinnedMeshRenderer)r).bones;
			}
			if (r is MeshRenderer)
			{
				return new Transform[]
				{
					r.transform
				};
			}
			Debug.LogError("Could not getBones. Object does not have a renderer");
			return null;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0001A27A File Offset: 0x0001867A
		public int GetBlendShapeFrameCount(Mesh m, int shapeIndex)
		{
			return m.GetBlendShapeFrameCount(shapeIndex);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0001A283 File Offset: 0x00018683
		public float GetBlendShapeFrameWeight(Mesh m, int shapeIndex, int frameIndex)
		{
			return m.GetBlendShapeFrameWeight(shapeIndex, frameIndex);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0001A28D File Offset: 0x0001868D
		public void GetBlendShapeFrameVertices(Mesh m, int shapeIndex, int frameIndex, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
			m.GetBlendShapeFrameVertices(shapeIndex, frameIndex, vs, ns, ts);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0001A29D File Offset: 0x0001869D
		public void ClearBlendShapes(Mesh m)
		{
			m.ClearBlendShapes();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0001A2A5 File Offset: 0x000186A5
		public void AddBlendShapeFrame(Mesh m, string nm, float wt, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
			m.AddBlendShapeFrame(nm, wt, vs, ns, ts);
		}

		// Token: 0x04000186 RID: 390
		private Vector2 _HALF_UV = new Vector2(0.5f, 0.5f);
	}
}
