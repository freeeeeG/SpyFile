using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000033 RID: 51
	[Serializable]
	public abstract class MB3_MeshCombiner
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00008916 File Offset: 0x00006D16
		public static bool EVAL_VERSION
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00008919 File Offset: 0x00006D19
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00008921 File Offset: 0x00006D21
		public virtual MB2_LogLevel LOG_LEVEL
		{
			get
			{
				return this._LOG_LEVEL;
			}
			set
			{
				this._LOG_LEVEL = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x0000892A File Offset: 0x00006D2A
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00008932 File Offset: 0x00006D32
		public virtual MB2_ValidationLevel validationLevel
		{
			get
			{
				return this._validationLevel;
			}
			set
			{
				this._validationLevel = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000893B File Offset: 0x00006D3B
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00008943 File Offset: 0x00006D43
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x0000894C File Offset: 0x00006D4C
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00008954 File Offset: 0x00006D54
		public virtual MB2_TextureBakeResults textureBakeResults
		{
			get
			{
				return this._textureBakeResults;
			}
			set
			{
				this._textureBakeResults = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000895D File Offset: 0x00006D5D
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00008965 File Offset: 0x00006D65
		public virtual GameObject resultSceneObject
		{
			get
			{
				return this._resultSceneObject;
			}
			set
			{
				this._resultSceneObject = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000DB RID: 219 RVA: 0x0000896E File Offset: 0x00006D6E
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00008976 File Offset: 0x00006D76
		public virtual Renderer targetRenderer
		{
			get
			{
				return this._targetRenderer;
			}
			set
			{
				if (this._targetRenderer != null && this._targetRenderer != value)
				{
					Debug.LogWarning("Previous targetRenderer was not null. Combined mesh may be being used by more than one Renderer");
				}
				this._targetRenderer = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000089AB File Offset: 0x00006DAB
		// (set) Token: 0x060000DE RID: 222 RVA: 0x000089B3 File Offset: 0x00006DB3
		public virtual MB_RenderType renderType
		{
			get
			{
				return this._renderType;
			}
			set
			{
				this._renderType = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000089BC File Offset: 0x00006DBC
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x000089C4 File Offset: 0x00006DC4
		public virtual MB2_OutputOptions outputOption
		{
			get
			{
				return this._outputOption;
			}
			set
			{
				this._outputOption = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000089CD File Offset: 0x00006DCD
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x000089D5 File Offset: 0x00006DD5
		public virtual MB2_LightmapOptions lightmapOption
		{
			get
			{
				return this._lightmapOption;
			}
			set
			{
				this._lightmapOption = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x000089DE File Offset: 0x00006DDE
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x000089E6 File Offset: 0x00006DE6
		public virtual bool doNorm
		{
			get
			{
				return this._doNorm;
			}
			set
			{
				this._doNorm = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000089EF File Offset: 0x00006DEF
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x000089F7 File Offset: 0x00006DF7
		public virtual bool doTan
		{
			get
			{
				return this._doTan;
			}
			set
			{
				this._doTan = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00008A00 File Offset: 0x00006E00
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00008A08 File Offset: 0x00006E08
		public virtual bool doCol
		{
			get
			{
				return this._doCol;
			}
			set
			{
				this._doCol = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00008A11 File Offset: 0x00006E11
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00008A19 File Offset: 0x00006E19
		public virtual bool doUV
		{
			get
			{
				return this._doUV;
			}
			set
			{
				this._doUV = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00008A22 File Offset: 0x00006E22
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00008A25 File Offset: 0x00006E25
		public virtual bool doUV1
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00008A27 File Offset: 0x00006E27
		public virtual bool doUV2()
		{
			return this._lightmapOption == MB2_LightmapOptions.copy_UV2_unchanged || this._lightmapOption == MB2_LightmapOptions.preserve_current_lightmapping || this._lightmapOption == MB2_LightmapOptions.copy_UV2_unchanged_to_separate_rects;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00008A4C File Offset: 0x00006E4C
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00008A54 File Offset: 0x00006E54
		public virtual bool doUV3
		{
			get
			{
				return this._doUV3;
			}
			set
			{
				this._doUV3 = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00008A5D File Offset: 0x00006E5D
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00008A65 File Offset: 0x00006E65
		public virtual bool doUV4
		{
			get
			{
				return this._doUV4;
			}
			set
			{
				this._doUV4 = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00008A6E File Offset: 0x00006E6E
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00008A76 File Offset: 0x00006E76
		public virtual bool doBlendShapes
		{
			get
			{
				return this._doBlendShapes;
			}
			set
			{
				this._doBlendShapes = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00008A7F File Offset: 0x00006E7F
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00008A87 File Offset: 0x00006E87
		public virtual bool recenterVertsToBoundsCenter
		{
			get
			{
				return this._recenterVertsToBoundsCenter;
			}
			set
			{
				this._recenterVertsToBoundsCenter = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00008A90 File Offset: 0x00006E90
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00008A98 File Offset: 0x00006E98
		public bool optimizeAfterBake
		{
			get
			{
				return this._optimizeAfterBake;
			}
			set
			{
				this._optimizeAfterBake = value;
			}
		}

		// Token: 0x060000F8 RID: 248
		public abstract int GetLightmapIndex();

		// Token: 0x060000F9 RID: 249
		public abstract void ClearBuffers();

		// Token: 0x060000FA RID: 250
		public abstract void ClearMesh();

		// Token: 0x060000FB RID: 251
		public abstract void DestroyMesh();

		// Token: 0x060000FC RID: 252
		public abstract void DestroyMeshEditor(MB2_EditorMethodsInterface editorMethods);

		// Token: 0x060000FD RID: 253
		public abstract List<GameObject> GetObjectsInCombined();

		// Token: 0x060000FE RID: 254
		public abstract int GetNumObjectsInCombined();

		// Token: 0x060000FF RID: 255
		public abstract int GetNumVerticesFor(GameObject go);

		// Token: 0x06000100 RID: 256
		public abstract int GetNumVerticesFor(int instanceID);

		// Token: 0x06000101 RID: 257
		public abstract Dictionary<MB3_MeshCombiner.MBBlendShapeKey, MB3_MeshCombiner.MBBlendShapeValue> BuildSourceBlendShapeToCombinedIndexMap();

		// Token: 0x06000102 RID: 258 RVA: 0x00008AA1 File Offset: 0x00006EA1
		public virtual void Apply()
		{
			this.Apply(null);
		}

		// Token: 0x06000103 RID: 259
		public abstract void Apply(MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod);

		// Token: 0x06000104 RID: 260
		public abstract void Apply(bool triangles, bool vertices, bool normals, bool tangents, bool uvs, bool uv2, bool uv3, bool uv4, bool colors, bool bones = false, bool blendShapeFlag = false, MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod = null);

		// Token: 0x06000105 RID: 261
		public abstract void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true, bool updateVertices = true, bool updateNormals = true, bool updateTangents = true, bool updateUV = false, bool updateUV2 = false, bool updateUV3 = false, bool updateUV4 = false, bool updateColors = false, bool updateSkinningInfo = false);

		// Token: 0x06000106 RID: 262
		public abstract bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource = true);

		// Token: 0x06000107 RID: 263
		public abstract bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource);

		// Token: 0x06000108 RID: 264
		public abstract bool CombinedMeshContains(GameObject go);

		// Token: 0x06000109 RID: 265
		public abstract void UpdateSkinnedMeshApproximateBounds();

		// Token: 0x0600010A RID: 266
		public abstract void UpdateSkinnedMeshApproximateBoundsFromBones();

		// Token: 0x0600010B RID: 267
		public abstract void CheckIntegrity();

		// Token: 0x0600010C RID: 268
		public abstract void UpdateSkinnedMeshApproximateBoundsFromBounds();

		// Token: 0x0600010D RID: 269 RVA: 0x00008AAC File Offset: 0x00006EAC
		public static void UpdateSkinnedMeshApproximateBoundsFromBonesStatic(Transform[] bs, SkinnedMeshRenderer smr)
		{
			Vector3 position = bs[0].position;
			Vector3 position2 = bs[0].position;
			for (int i = 1; i < bs.Length; i++)
			{
				Vector3 position3 = bs[i].position;
				if (position3.x < position2.x)
				{
					position2.x = position3.x;
				}
				if (position3.y < position2.y)
				{
					position2.y = position3.y;
				}
				if (position3.z < position2.z)
				{
					position2.z = position3.z;
				}
				if (position3.x > position.x)
				{
					position.x = position3.x;
				}
				if (position3.y > position.y)
				{
					position.y = position3.y;
				}
				if (position3.z > position.z)
				{
					position.z = position3.z;
				}
			}
			Vector3 v = (position + position2) / 2f;
			Vector3 v2 = position - position2;
			Matrix4x4 worldToLocalMatrix = smr.worldToLocalMatrix;
			Bounds localBounds = new Bounds(worldToLocalMatrix * v, worldToLocalMatrix * v2);
			smr.localBounds = localBounds;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008C08 File Offset: 0x00007008
		public static void UpdateSkinnedMeshApproximateBoundsFromBoundsStatic(List<GameObject> objectsInCombined, SkinnedMeshRenderer smr)
		{
			Bounds bounds = default(Bounds);
			Bounds localBounds = default(Bounds);
			if (MB_Utility.GetBounds(objectsInCombined[0], out bounds))
			{
				localBounds = bounds;
				for (int i = 1; i < objectsInCombined.Count; i++)
				{
					if (!MB_Utility.GetBounds(objectsInCombined[i], out bounds))
					{
						Debug.LogError("Could not get bounds. Not updating skinned mesh bounds");
						return;
					}
					localBounds.Encapsulate(bounds);
				}
				smr.localBounds = localBounds;
				return;
			}
			Debug.LogError("Could not get bounds. Not updating skinned mesh bounds");
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008C93 File Offset: 0x00007093
		protected virtual bool _CreateTemporaryTextrueBakeResult(GameObject[] gos, List<Material> matsOnTargetRenderer)
		{
			if (this.GetNumObjectsInCombined() > 0)
			{
				Debug.LogError("Can't add objects if there are already objects in combined mesh when 'Texture Bake Result' is not set. Perhaps enable 'Clear Buffers After Bake'");
				return false;
			}
			this._usingTemporaryTextureBakeResult = true;
			this._textureBakeResults = MB2_TextureBakeResults.CreateForMaterialsOnRenderer(gos, matsOnTargetRenderer);
			return true;
		}

		// Token: 0x06000110 RID: 272
		public abstract List<Material> GetMaterialsOnTargetRenderer();

		// Token: 0x040000C1 RID: 193
		[SerializeField]
		protected MB2_LogLevel _LOG_LEVEL = MB2_LogLevel.info;

		// Token: 0x040000C2 RID: 194
		[SerializeField]
		protected MB2_ValidationLevel _validationLevel = MB2_ValidationLevel.robust;

		// Token: 0x040000C3 RID: 195
		[SerializeField]
		protected string _name;

		// Token: 0x040000C4 RID: 196
		[SerializeField]
		protected MB2_TextureBakeResults _textureBakeResults;

		// Token: 0x040000C5 RID: 197
		[SerializeField]
		protected GameObject _resultSceneObject;

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		protected Renderer _targetRenderer;

		// Token: 0x040000C7 RID: 199
		[SerializeField]
		protected MB_RenderType _renderType;

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		protected MB2_OutputOptions _outputOption;

		// Token: 0x040000C9 RID: 201
		[SerializeField]
		protected MB2_LightmapOptions _lightmapOption = MB2_LightmapOptions.ignore_UV2;

		// Token: 0x040000CA RID: 202
		[SerializeField]
		protected bool _doNorm = true;

		// Token: 0x040000CB RID: 203
		[SerializeField]
		protected bool _doTan = true;

		// Token: 0x040000CC RID: 204
		[SerializeField]
		protected bool _doCol;

		// Token: 0x040000CD RID: 205
		[SerializeField]
		protected bool _doUV = true;

		// Token: 0x040000CE RID: 206
		[SerializeField]
		protected bool _doUV3;

		// Token: 0x040000CF RID: 207
		[SerializeField]
		protected bool _doUV4;

		// Token: 0x040000D0 RID: 208
		[SerializeField]
		protected bool _doBlendShapes;

		// Token: 0x040000D1 RID: 209
		[SerializeField]
		protected bool _recenterVertsToBoundsCenter;

		// Token: 0x040000D2 RID: 210
		[SerializeField]
		public bool _optimizeAfterBake = true;

		// Token: 0x040000D3 RID: 211
		[SerializeField]
		public float uv2UnwrappingParamsHardAngle = 60f;

		// Token: 0x040000D4 RID: 212
		[SerializeField]
		public float uv2UnwrappingParamsPackMargin = 0.005f;

		// Token: 0x040000D5 RID: 213
		protected bool _usingTemporaryTextureBakeResult;

		// Token: 0x02000034 RID: 52
		// (Invoke) Token: 0x06000112 RID: 274
		public delegate void GenerateUV2Delegate(Mesh m, float hardAngle, float packMargin);

		// Token: 0x02000035 RID: 53
		public class MBBlendShapeKey
		{
			// Token: 0x06000115 RID: 277 RVA: 0x00008CC2 File Offset: 0x000070C2
			public MBBlendShapeKey(int srcSkinnedMeshRenderGameObjectID, int blendShapeIndexInSource)
			{
				this.gameObjecID = srcSkinnedMeshRenderGameObjectID;
				this.blendShapeIndexInSrc = blendShapeIndexInSource;
			}

			// Token: 0x06000116 RID: 278 RVA: 0x00008CD8 File Offset: 0x000070D8
			public override bool Equals(object obj)
			{
				if (!(obj is MB3_MeshCombiner.MBBlendShapeKey) || obj == null)
				{
					return false;
				}
				MB3_MeshCombiner.MBBlendShapeKey mbblendShapeKey = (MB3_MeshCombiner.MBBlendShapeKey)obj;
				return this.gameObjecID == mbblendShapeKey.gameObjecID && this.blendShapeIndexInSrc == mbblendShapeKey.blendShapeIndexInSrc;
			}

			// Token: 0x06000117 RID: 279 RVA: 0x00008D24 File Offset: 0x00007124
			public override int GetHashCode()
			{
				int num = 23;
				num = num * 31 + this.gameObjecID;
				return num * 31 + this.blendShapeIndexInSrc;
			}

			// Token: 0x040000D6 RID: 214
			public int gameObjecID;

			// Token: 0x040000D7 RID: 215
			public int blendShapeIndexInSrc;
		}

		// Token: 0x02000036 RID: 54
		public class MBBlendShapeValue
		{
			// Token: 0x040000D8 RID: 216
			public GameObject combinedMeshGameObject;

			// Token: 0x040000D9 RID: 217
			public int blendShapeIndex;
		}
	}
}
