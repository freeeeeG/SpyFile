﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000043 RID: 67
	[Serializable]
	public class MB3_TextureCombiner
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00011368 File Offset: 0x0000F768
		// (set) Token: 0x060001BE RID: 446 RVA: 0x00011370 File Offset: 0x0000F770
		public MB2_TextureBakeResults textureBakeResults
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

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00011379 File Offset: 0x0000F779
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x00011381 File Offset: 0x0000F781
		public int atlasPadding
		{
			get
			{
				return this._atlasPadding;
			}
			set
			{
				this._atlasPadding = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0001138A File Offset: 0x0000F78A
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00011392 File Offset: 0x0000F792
		public int maxAtlasSize
		{
			get
			{
				return this._maxAtlasSize;
			}
			set
			{
				this._maxAtlasSize = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0001139B File Offset: 0x0000F79B
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x000113A3 File Offset: 0x0000F7A3
		public bool resizePowerOfTwoTextures
		{
			get
			{
				return this._resizePowerOfTwoTextures;
			}
			set
			{
				this._resizePowerOfTwoTextures = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x000113AC File Offset: 0x0000F7AC
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x000113B4 File Offset: 0x0000F7B4
		public bool fixOutOfBoundsUVs
		{
			get
			{
				return this._fixOutOfBoundsUVs;
			}
			set
			{
				this._fixOutOfBoundsUVs = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x000113BD File Offset: 0x0000F7BD
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x000113C5 File Offset: 0x0000F7C5
		public int maxTilingBakeSize
		{
			get
			{
				return this._maxTilingBakeSize;
			}
			set
			{
				this._maxTilingBakeSize = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x000113CE File Offset: 0x0000F7CE
		// (set) Token: 0x060001CA RID: 458 RVA: 0x000113D6 File Offset: 0x0000F7D6
		public bool saveAtlasesAsAssets
		{
			get
			{
				return this._saveAtlasesAsAssets;
			}
			set
			{
				this._saveAtlasesAsAssets = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000113DF File Offset: 0x0000F7DF
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000113E7 File Offset: 0x0000F7E7
		public MB2_PackingAlgorithmEnum packingAlgorithm
		{
			get
			{
				return this._packingAlgorithm;
			}
			set
			{
				this._packingAlgorithm = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000113F0 File Offset: 0x0000F7F0
		// (set) Token: 0x060001CE RID: 462 RVA: 0x000113F8 File Offset: 0x0000F7F8
		public bool meshBakerTexturePackerForcePowerOfTwo
		{
			get
			{
				return this._meshBakerTexturePackerForcePowerOfTwo;
			}
			set
			{
				this._meshBakerTexturePackerForcePowerOfTwo = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00011401 File Offset: 0x0000F801
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00011409 File Offset: 0x0000F809
		public List<ShaderTextureProperty> customShaderPropNames
		{
			get
			{
				return this._customShaderPropNames;
			}
			set
			{
				this._customShaderPropNames = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00011412 File Offset: 0x0000F812
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0001141A File Offset: 0x0000F81A
		public bool considerNonTextureProperties
		{
			get
			{
				return this._considerNonTextureProperties;
			}
			set
			{
				this._considerNonTextureProperties = value;
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00011424 File Offset: 0x0000F824
		public static void RunCorutineWithoutPause(IEnumerator cor, int recursionDepth)
		{
			if (recursionDepth == 0)
			{
				MB3_TextureCombiner._RunCorutineWithoutPauseIsRunning = true;
			}
			if (recursionDepth > 20)
			{
				UnityEngine.Debug.LogError("Recursion Depth Exceeded.");
				return;
			}
			while (cor.MoveNext())
			{
				object obj = cor.Current;
				if (!(obj is YieldInstruction))
				{
					if (obj != null)
					{
						if (obj is IEnumerator)
						{
							MB3_TextureCombiner.RunCorutineWithoutPause((IEnumerator)cor.Current, recursionDepth + 1);
						}
					}
				}
			}
			if (recursionDepth == 0)
			{
				MB3_TextureCombiner._RunCorutineWithoutPauseIsRunning = false;
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000114AC File Offset: 0x0000F8AC
		public bool CombineTexturesIntoAtlases(ProgressUpdateDelegate progressInfo, MB_AtlasesAndRects resultAtlasesAndRects, Material resultMaterial, List<GameObject> objsToMesh, List<Material> allowedMaterialsFilter, MB2_EditorMethodsInterface textureEditorMethods = null, List<AtlasPackingResult> packingResults = null, bool onlyPackRects = false)
		{
			MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult combineTexturesIntoAtlasesCoroutineResult = new MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult();
			MB3_TextureCombiner.RunCorutineWithoutPause(this._CombineTexturesIntoAtlases(progressInfo, combineTexturesIntoAtlasesCoroutineResult, resultAtlasesAndRects, resultMaterial, objsToMesh, allowedMaterialsFilter, textureEditorMethods, packingResults, onlyPackRects), 0);
			return combineTexturesIntoAtlasesCoroutineResult.success;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000114E0 File Offset: 0x0000F8E0
		public IEnumerator CombineTexturesIntoAtlasesCoroutine(ProgressUpdateDelegate progressInfo, MB_AtlasesAndRects resultAtlasesAndRects, Material resultMaterial, List<GameObject> objsToMesh, List<Material> allowedMaterialsFilter, MB2_EditorMethodsInterface textureEditorMethods = null, MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult coroutineResult = null, float maxTimePerFrame = 0.01f, List<AtlasPackingResult> packingResults = null, bool onlyPackRects = false)
		{
			if (!MB3_TextureCombiner._RunCorutineWithoutPauseIsRunning && (MBVersion.GetMajorVersion() < 5 || (MBVersion.GetMajorVersion() == 5 && MBVersion.GetMinorVersion() < 3)))
			{
				UnityEngine.Debug.LogError("Running the texture combiner as a coroutine only works in Unity 5.3 and higher");
				yield return null;
			}
			coroutineResult.success = true;
			coroutineResult.isFinished = false;
			if (maxTimePerFrame <= 0f)
			{
				UnityEngine.Debug.LogError("maxTimePerFrame must be a value greater than zero");
				coroutineResult.isFinished = true;
				yield break;
			}
			yield return this._CombineTexturesIntoAtlases(progressInfo, coroutineResult, resultAtlasesAndRects, resultMaterial, objsToMesh, allowedMaterialsFilter, textureEditorMethods, packingResults, onlyPackRects);
			coroutineResult.isFinished = true;
			yield break;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00011548 File Offset: 0x0000F948
		private static bool InterfaceFilter(Type typeObj, object criteriaObj)
		{
			return typeObj.ToString() == criteriaObj.ToString();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0001155C File Offset: 0x0000F95C
		private void _LoadTextureBlenders()
		{
			string filterCriteria = "DigitalOpus.MB.Core.TextureBlender";
			TypeFilter filter = new TypeFilter(MB3_TextureCombiner.InterfaceFilter);
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				IEnumerable enumerable = null;
				try
				{
					enumerable = assembly.GetTypes();
				}
				catch (Exception ex)
				{
					ex.Equals(null);
				}
				if (enumerable != null)
				{
					foreach (Type type in assembly.GetTypes())
					{
						Type[] array = type.FindInterfaces(filter, filterCriteria);
						if (array.Length > 0)
						{
							list.Add(type);
						}
					}
				}
			}
			TextureBlender textureBlender = null;
			List<TextureBlender> list2 = new List<TextureBlender>();
			foreach (Type type2 in list)
			{
				if (!type2.IsAbstract && !type2.IsInterface)
				{
					TextureBlender textureBlender2 = (TextureBlender)Activator.CreateInstance(type2);
					if (textureBlender2 is TextureBlenderFallback)
					{
						textureBlender = textureBlender2;
					}
					else
					{
						list2.Add(textureBlender2);
					}
				}
			}
			if (textureBlender != null)
			{
				list2.Add(textureBlender);
			}
			this.textureBlenders = list2.ToArray();
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log(string.Format("Loaded {0} TextureBlenders.", this.textureBlenders.Length));
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00011700 File Offset: 0x0000FB00
		private bool _CollectPropertyNames(Material resultMaterial, List<ShaderTextureProperty> texPropertyNames)
		{
			MB3_TextureCombiner.<_CollectPropertyNames>c__AnonStorey9 <_CollectPropertyNames>c__AnonStorey = new MB3_TextureCombiner.<_CollectPropertyNames>c__AnonStorey9();
			<_CollectPropertyNames>c__AnonStorey.texPropertyNames = texPropertyNames;
			int i;
			for (i = 0; i < <_CollectPropertyNames>c__AnonStorey.texPropertyNames.Count; i++)
			{
				ShaderTextureProperty shaderTextureProperty = this._customShaderPropNames.Find((ShaderTextureProperty x) => x.name.Equals(<_CollectPropertyNames>c__AnonStorey.texPropertyNames[i].name));
				if (shaderTextureProperty != null)
				{
					this._customShaderPropNames.Remove(shaderTextureProperty);
				}
			}
			if (resultMaterial == null)
			{
				UnityEngine.Debug.LogError("Please assign a result material. The combined mesh will use this material.");
				return false;
			}
			string str = string.Empty;
			for (int k = 0; k < MB3_TextureCombiner.shaderTexPropertyNames.Length; k++)
			{
				if (resultMaterial.HasProperty(MB3_TextureCombiner.shaderTexPropertyNames[k].name))
				{
					str = str + ", " + MB3_TextureCombiner.shaderTexPropertyNames[k].name;
					if (!<_CollectPropertyNames>c__AnonStorey.texPropertyNames.Contains(MB3_TextureCombiner.shaderTexPropertyNames[k]))
					{
						<_CollectPropertyNames>c__AnonStorey.texPropertyNames.Add(MB3_TextureCombiner.shaderTexPropertyNames[k]);
					}
					if (resultMaterial.GetTextureOffset(MB3_TextureCombiner.shaderTexPropertyNames[k].name) != new Vector2(0f, 0f) && this.LOG_LEVEL >= MB2_LogLevel.warn)
					{
						UnityEngine.Debug.LogWarning("Result material has non-zero offset. This is may be incorrect.");
					}
					if (resultMaterial.GetTextureScale(MB3_TextureCombiner.shaderTexPropertyNames[k].name) != new Vector2(1f, 1f) && this.LOG_LEVEL >= MB2_LogLevel.warn)
					{
						UnityEngine.Debug.LogWarning("Result material should have tiling of 1,1");
					}
				}
			}
			for (int j = 0; j < this._customShaderPropNames.Count; j++)
			{
				if (resultMaterial.HasProperty(this._customShaderPropNames[j].name))
				{
					str = str + ", " + this._customShaderPropNames[j].name;
					<_CollectPropertyNames>c__AnonStorey.texPropertyNames.Add(this._customShaderPropNames[j]);
					if (resultMaterial.GetTextureOffset(this._customShaderPropNames[j].name) != new Vector2(0f, 0f) && this.LOG_LEVEL >= MB2_LogLevel.warn)
					{
						UnityEngine.Debug.LogWarning("Result material has non-zero offset. This is probably incorrect.");
					}
					if (resultMaterial.GetTextureScale(this._customShaderPropNames[j].name) != new Vector2(1f, 1f) && this.LOG_LEVEL >= MB2_LogLevel.warn)
					{
						UnityEngine.Debug.LogWarning("Result material should probably have tiling of 1,1.");
					}
				}
				else if (this.LOG_LEVEL >= MB2_LogLevel.warn)
				{
					UnityEngine.Debug.LogWarning("Result material shader does not use property " + this._customShaderPropNames[j].name + " in the list of custom shader property names");
				}
			}
			return true;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000119DC File Offset: 0x0000FDDC
		private IEnumerator _CombineTexturesIntoAtlases(ProgressUpdateDelegate progressInfo, MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult result, MB_AtlasesAndRects resultAtlasesAndRects, Material resultMaterial, List<GameObject> objsToMesh, List<Material> allowedMaterialsFilter, MB2_EditorMethodsInterface textureEditorMethods, List<AtlasPackingResult> atlasPackingResult, bool onlyPackRects)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			try
			{
				this._temporaryTextures.Clear();
				if (textureEditorMethods != null)
				{
					textureEditorMethods.Clear();
				}
				if (objsToMesh == null || objsToMesh.Count == 0)
				{
					UnityEngine.Debug.LogError("No meshes to combine. Please assign some meshes to combine.");
					result.success = false;
					yield break;
				}
				if (this._atlasPadding < 0)
				{
					UnityEngine.Debug.LogError("Atlas padding must be zero or greater.");
					result.success = false;
					yield break;
				}
				if (this._maxTilingBakeSize < 2 || this._maxTilingBakeSize > 4096)
				{
					UnityEngine.Debug.LogError("Invalid value for max tiling bake size.");
					result.success = false;
					yield break;
				}
				for (int i = 0; i < objsToMesh.Count; i++)
				{
					foreach (Material x in MB_Utility.GetGOMaterials(objsToMesh[i]))
					{
						if (x == null)
						{
							UnityEngine.Debug.LogError("Game object " + objsToMesh[i] + " has a null material");
							result.success = false;
							yield break;
						}
					}
				}
				if (progressInfo != null)
				{
					progressInfo("Collecting textures for " + objsToMesh.Count + " meshes.", 0.01f);
				}
				List<ShaderTextureProperty> texPropertyNames = new List<ShaderTextureProperty>();
				if (!this._CollectPropertyNames(resultMaterial, texPropertyNames))
				{
					result.success = false;
					yield break;
				}
				if (this._considerNonTextureProperties)
				{
					this._LoadTextureBlenders();
					this.resultMaterialTextureBlender = this.FindMatchingTextureBlender(resultMaterial.shader.name);
					if (this.resultMaterialTextureBlender != null)
					{
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							UnityEngine.Debug.Log("Using _considerNonTextureProperties found a TextureBlender for result material. Using: " + this.resultMaterialTextureBlender);
						}
					}
					else
					{
						if (this.LOG_LEVEL >= MB2_LogLevel.error)
						{
							UnityEngine.Debug.LogWarning("Using _considerNonTextureProperties could not find a TextureBlender that matches the shader on the result material. Using the Fallback Texture Blender.");
						}
						this.resultMaterialTextureBlender = new TextureBlenderFallback();
					}
				}
				if (onlyPackRects)
				{
					yield return this.__RunTexturePacker(result, texPropertyNames, objsToMesh, allowedMaterialsFilter, textureEditorMethods, atlasPackingResult);
				}
				else
				{
					yield return this.__CombineTexturesIntoAtlases(progressInfo, result, resultAtlasesAndRects, resultMaterial, texPropertyNames, objsToMesh, allowedMaterialsFilter, textureEditorMethods);
				}
			}
			finally
			{
				this._destroyTemporaryTextures();
				if (textureEditorMethods != null)
				{
					textureEditorMethods.RestoreReadFlagsAndFormats(progressInfo);
				}
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"===== Done creating atlases for ",
						resultMaterial,
						" Total time to create atlases ",
						sw.Elapsed.ToString()
					}));
				}
			}
			yield break;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00011A3C File Offset: 0x0000FE3C
		private IEnumerator __CombineTexturesIntoAtlases(ProgressUpdateDelegate progressInfo, MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult result, MB_AtlasesAndRects resultAtlasesAndRects, Material resultMaterial, List<ShaderTextureProperty> texPropertyNames, List<GameObject> objsToMesh, List<Material> allowedMaterialsFilter, MB2_EditorMethodsInterface textureEditorMethods)
		{
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"__CombineTexturesIntoAtlases texture properties in shader:",
					texPropertyNames.Count,
					" objsToMesh:",
					objsToMesh.Count,
					" _fixOutOfBoundsUVs:",
					this._fixOutOfBoundsUVs
				}));
			}
			if (progressInfo != null)
			{
				progressInfo("Collecting textures ", 0.01f);
			}
			List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures = new List<MB3_TextureCombiner.MB_TexSet>();
			List<GameObject> usedObjsToMesh = new List<GameObject>();
			yield return this.__Step1_CollectDistinctMatTexturesAndUsedObjects(progressInfo, result, objsToMesh, allowedMaterialsFilter, texPropertyNames, textureEditorMethods, distinctMaterialTextures, usedObjsToMesh);
			if (!result.success)
			{
				yield break;
			}
			if (MB3_MeshCombiner.EVAL_VERSION)
			{
				bool flag = true;
				for (int i = 0; i < distinctMaterialTextures.Count; i++)
				{
					for (int j = 0; j < distinctMaterialTextures[i].matsAndGOs.mats.Count; j++)
					{
						if (!distinctMaterialTextures[i].matsAndGOs.mats[j].mat.shader.name.EndsWith("Diffuse") && !distinctMaterialTextures[i].matsAndGOs.mats[j].mat.shader.name.EndsWith("Bumped Diffuse"))
						{
							UnityEngine.Debug.LogError("The free version of Mesh Baker only works with Diffuse and Bumped Diffuse Shaders. The full version can be used with any shader. Material " + distinctMaterialTextures[i].matsAndGOs.mats[j].mat.name + " uses shader " + distinctMaterialTextures[i].matsAndGOs.mats[j].mat.shader.name);
							flag = false;
						}
					}
				}
				if (!flag)
				{
					result.success = false;
					yield break;
				}
			}
			bool[] allTexturesAreNullAndSameColor = new bool[texPropertyNames.Count];
			yield return this.__Step2_CalculateIdealSizesForTexturesInAtlasAndPadding(progressInfo, result, distinctMaterialTextures, texPropertyNames, allTexturesAreNullAndSameColor, textureEditorMethods);
			if (!result.success)
			{
				yield break;
			}
			int _padding = this.__step2_CalculateIdealSizesForTexturesInAtlasAndPadding;
			yield return this.__Step3_BuildAndSaveAtlasesAndStoreResults(result, progressInfo, distinctMaterialTextures, texPropertyNames, allTexturesAreNullAndSameColor, _padding, textureEditorMethods, resultAtlasesAndRects, resultMaterial);
			yield break;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00011A94 File Offset: 0x0000FE94
		private IEnumerator __RunTexturePacker(MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult result, List<ShaderTextureProperty> texPropertyNames, List<GameObject> objsToMesh, List<Material> allowedMaterialsFilter, MB2_EditorMethodsInterface textureEditorMethods, List<AtlasPackingResult> packingResult)
		{
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"__RunTexturePacker texture properties in shader:",
					texPropertyNames.Count,
					" objsToMesh:",
					objsToMesh.Count,
					" _fixOutOfBoundsUVs:",
					this._fixOutOfBoundsUVs
				}));
			}
			List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures = new List<MB3_TextureCombiner.MB_TexSet>();
			List<GameObject> usedObjsToMesh = new List<GameObject>();
			yield return this.__Step1_CollectDistinctMatTexturesAndUsedObjects(null, result, objsToMesh, allowedMaterialsFilter, texPropertyNames, textureEditorMethods, distinctMaterialTextures, usedObjsToMesh);
			if (!result.success)
			{
				yield break;
			}
			bool[] allTexturesAreNullAndSameColor = new bool[texPropertyNames.Count];
			yield return this.__Step2_CalculateIdealSizesForTexturesInAtlasAndPadding(null, result, distinctMaterialTextures, texPropertyNames, allTexturesAreNullAndSameColor, textureEditorMethods);
			if (!result.success)
			{
				yield break;
			}
			int _padding = this.__step2_CalculateIdealSizesForTexturesInAtlasAndPadding;
			AtlasPackingResult[] aprs = this.__Step3_RunTexturePacker(distinctMaterialTextures, _padding);
			for (int i = 0; i < aprs.Length; i++)
			{
				packingResult.Add(aprs[i]);
			}
			yield break;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00011ADC File Offset: 0x0000FEDC
		private IEnumerator __Step1_CollectDistinctMatTexturesAndUsedObjects(ProgressUpdateDelegate progressInfo, MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult result, List<GameObject> allObjsToMesh, List<Material> allowedMaterialsFilter, List<ShaderTextureProperty> texPropertyNames, MB2_EditorMethodsInterface textureEditorMethods, List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures, List<GameObject> usedObjsToMesh)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			bool flag = false;
			Dictionary<int, MB_Utility.MeshAnalysisResult[]> dictionary = new Dictionary<int, MB_Utility.MeshAnalysisResult[]>();
			for (int i = 0; i < allObjsToMesh.Count; i++)
			{
				GameObject gameObject = allObjsToMesh[i];
				if (progressInfo != null)
				{
					progressInfo("Collecting textures for " + gameObject, (float)i / (float)allObjsToMesh.Count / 2f);
				}
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					UnityEngine.Debug.Log("Collecting textures for object " + gameObject);
				}
				if (gameObject == null)
				{
					UnityEngine.Debug.LogError("The list of objects to mesh contained nulls.");
					result.success = false;
					yield break;
				}
				Mesh mesh = MB_Utility.GetMesh(gameObject);
				if (mesh == null)
				{
					UnityEngine.Debug.LogError("Object " + gameObject.name + " in the list of objects to mesh has no mesh.");
					result.success = false;
					yield break;
				}
				Material[] gomaterials = MB_Utility.GetGOMaterials(gameObject);
				if (gomaterials.Length == 0)
				{
					UnityEngine.Debug.LogError("Object " + gameObject.name + " in the list of objects has no materials.");
					result.success = false;
					yield break;
				}
				MB_Utility.MeshAnalysisResult[] array;
				if (!dictionary.TryGetValue(mesh.GetInstanceID(), out array))
				{
					array = new MB_Utility.MeshAnalysisResult[mesh.subMeshCount];
					for (int j = 0; j < mesh.subMeshCount; j++)
					{
						MB_Utility.hasOutOfBoundsUVs(mesh, ref array[j], j, 0);
						if (this._normalizeTexelDensity)
						{
							array[j].submeshArea = this.GetSubmeshArea(mesh, j);
						}
						if (this._fixOutOfBoundsUVs && !array[j].hasUVs)
						{
							array[j].uvRect = new Rect(0f, 0f, 1f, 1f);
							UnityEngine.Debug.LogWarning("Mesh for object " + gameObject + " has no UV channel but 'consider UVs' is enabled. Assuming UVs will be generated filling 0,0,1,1 rectangle.");
						}
					}
					dictionary.Add(mesh.GetInstanceID(), array);
				}
				if (this._fixOutOfBoundsUVs && this.LOG_LEVEL >= MB2_LogLevel.trace)
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Mesh Analysis for object ",
						gameObject,
						" numSubmesh=",
						array.Length,
						" HasOBUV=",
						array[0].hasOutOfBoundsUVs,
						" UVrectSubmesh0=",
						array[0].uvRect
					}));
				}
				for (int k = 0; k < gomaterials.Length; k++)
				{
					if (progressInfo != null)
					{
						progressInfo(string.Format("Collecting textures for {0} submesh {1}", gameObject, k), (float)i / (float)allObjsToMesh.Count / 2f);
					}
					Material material = gomaterials[k];
					if (allowedMaterialsFilter == null || allowedMaterialsFilter.Contains(material))
					{
						flag = (flag || array[k].hasOutOfBoundsUVs);
						if (material.name.Contains("(Instance)"))
						{
							UnityEngine.Debug.LogError("The sharedMaterial on object " + gameObject.name + " has been 'Instanced'. This was probably caused by a script accessing the meshRender.material property in the editor.  The material to UV Rectangle mapping will be incorrect. To fix this recreate the object from its prefab or re-assign its material from the correct asset.");
							result.success = false;
							yield break;
						}
						if (this._fixOutOfBoundsUVs && !MB_Utility.AreAllSharedMaterialsDistinct(gomaterials) && this.LOG_LEVEL >= MB2_LogLevel.warn)
						{
							UnityEngine.Debug.LogWarning("Object " + gameObject.name + " uses the same material on multiple submeshes. This may generate strange resultAtlasesAndRects especially when used with fix out of bounds uvs. Try duplicating the material.");
						}
						MB3_TextureCombiner.MeshBakerMaterialTexture[] array2 = new MB3_TextureCombiner.MeshBakerMaterialTexture[texPropertyNames.Count];
						for (int l = 0; l < texPropertyNames.Count; l++)
						{
							Texture2D texture2D = null;
							Vector2 s = Vector2.one;
							Vector2 o = Vector2.zero;
							float texelDens = 0f;
							if (material.HasProperty(texPropertyNames[l].name))
							{
								Texture texture = material.GetTexture(texPropertyNames[l].name);
								if (texture != null)
								{
									if (!(texture is Texture2D))
									{
										UnityEngine.Debug.LogError("Object " + gameObject.name + " in the list of objects to mesh uses a Texture that is not a Texture2D. Cannot build atlases.");
										result.success = false;
										yield break;
									}
									texture2D = (Texture2D)texture;
									TextureFormat format = texture2D.format;
									bool flag2 = false;
									if (!Application.isPlaying && textureEditorMethods != null)
									{
										flag2 = textureEditorMethods.IsNormalMap(texture2D);
									}
									if ((format != TextureFormat.ARGB32 && format != TextureFormat.RGBA32 && format != TextureFormat.BGRA32 && format != TextureFormat.RGB24 && format != TextureFormat.Alpha8) || flag2)
									{
										if (Application.isPlaying && this._packingAlgorithm != MB2_PackingAlgorithmEnum.MeshBakerTexturePacker_Fast)
										{
											UnityEngine.Debug.LogError(string.Concat(new object[]
											{
												"Object ",
												gameObject.name,
												" in the list of objects to mesh uses Texture ",
												texture2D.name,
												" uses format ",
												format,
												" that is not in: ARGB32, RGBA32, BGRA32, RGB24, Alpha8 or DXT. These textures cannot be resized at runtime. Try changing texture format. If format says 'compressed' try changing it to 'truecolor'"
											}));
											result.success = false;
											yield break;
										}
										texture2D = (Texture2D)material.GetTexture(texPropertyNames[l].name);
									}
								}
								if (texture2D != null && this._normalizeTexelDensity)
								{
									if (array[l].submeshArea == 0f)
									{
										texelDens = 0f;
									}
									else
									{
										texelDens = (float)(texture2D.width * texture2D.height) / array[l].submeshArea;
									}
								}
								s = material.GetTextureScale(texPropertyNames[l].name);
								o = material.GetTextureOffset(texPropertyNames[l].name);
							}
							array2[l] = new MB3_TextureCombiner.MeshBakerMaterialTexture(texture2D, o, s, texelDens);
						}
						Vector2 uvScale = new Vector2(array[k].uvRect.width, array[k].uvRect.height);
						Vector2 uvOffset = new Vector2(array[k].uvRect.x, array[k].uvRect.y);
						MB3_TextureCombiner.MB_TexSet setOfTexs = new MB3_TextureCombiner.MB_TexSet(array2, uvOffset, uvScale);
						MB3_TextureCombiner.MatAndTransformToMerged item = new MB3_TextureCombiner.MatAndTransformToMerged(material);
						setOfTexs.matsAndGOs.mats.Add(item);
						MB3_TextureCombiner.MB_TexSet mb_TexSet = distinctMaterialTextures.Find((MB3_TextureCombiner.MB_TexSet x) => x.IsEqual(setOfTexs, this._fixOutOfBoundsUVs, this._considerNonTextureProperties, this.resultMaterialTextureBlender));
						if (mb_TexSet != null)
						{
							setOfTexs = mb_TexSet;
						}
						else
						{
							distinctMaterialTextures.Add(setOfTexs);
						}
						if (!setOfTexs.matsAndGOs.mats.Contains(item))
						{
							setOfTexs.matsAndGOs.mats.Add(item);
						}
						if (!setOfTexs.matsAndGOs.gos.Contains(gameObject))
						{
							setOfTexs.matsAndGOs.gos.Add(gameObject);
							if (!usedObjsToMesh.Contains(gameObject))
							{
								usedObjsToMesh.Add(gameObject);
							}
						}
					}
				}
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log(string.Format("Step1_CollectDistinctTextures collected {0} sets of textures fixOutOfBoundsUV={1} considerNonTextureProperties={2}", distinctMaterialTextures.Count, this._fixOutOfBoundsUVs, this._considerNonTextureProperties));
			}
			if (distinctMaterialTextures.Count == 0)
			{
				UnityEngine.Debug.LogError("None of the source object materials matched any of the allowed materials for this submesh.");
				result.success = false;
				yield break;
			}
			this.MergeOverlappingDistinctMaterialTexturesAndCalcMaterialSubrects(distinctMaterialTextures, this.fixOutOfBoundsUVs);
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log("Total time Step1_CollectDistinctTextures " + stopwatch.ElapsedMilliseconds.ToString("f5"));
			}
			yield break;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00011B34 File Offset: 0x0000FF34
		private IEnumerator __Step2_CalculateIdealSizesForTexturesInAtlasAndPadding(ProgressUpdateDelegate progressInfo, MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult result, List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures, List<ShaderTextureProperty> texPropertyNames, bool[] allTexturesAreNullAndSameColor, MB2_EditorMethodsInterface textureEditorMethods)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (int i = 0; i < texPropertyNames.Count; i++)
			{
				bool flag = true;
				bool flag2 = true;
				for (int j = 0; j < distinctMaterialTextures.Count; j++)
				{
					if (distinctMaterialTextures[j].ts[i].t != null)
					{
						flag = false;
						break;
					}
					if (this._considerNonTextureProperties)
					{
						for (int k = j + 1; k < distinctMaterialTextures.Count; k++)
						{
							Color colorIfNoTexture = this.resultMaterialTextureBlender.GetColorIfNoTexture(distinctMaterialTextures[j].matsAndGOs.mats[0].mat, texPropertyNames[i]);
							Color colorIfNoTexture2 = this.resultMaterialTextureBlender.GetColorIfNoTexture(distinctMaterialTextures[k].matsAndGOs.mats[0].mat, texPropertyNames[i]);
							if (colorIfNoTexture != colorIfNoTexture2)
							{
								flag2 = false;
								break;
							}
						}
					}
				}
				allTexturesAreNullAndSameColor[i] = (flag && flag2);
				if (this.LOG_LEVEL >= MB2_LogLevel.trace)
				{
					UnityEngine.Debug.Log(string.Format("AllTexturesAreNullAndSameColor prop: {0} val:{1}", texPropertyNames[i].name, allTexturesAreNullAndSameColor[i]));
				}
			}
			int num = this._atlasPadding;
			if (distinctMaterialTextures.Count == 1 && !this._fixOutOfBoundsUVs)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.info)
				{
					UnityEngine.Debug.Log("All objects use the same textures in this set of atlases. Original textures will be reused instead of creating atlases.");
				}
				num = 0;
			}
			else
			{
				if (allTexturesAreNullAndSameColor.Length != texPropertyNames.Count)
				{
					UnityEngine.Debug.LogError("allTexturesAreNullAndSameColor array must be the same length of texPropertyNames.");
				}
				for (int l = 0; l < distinctMaterialTextures.Count; l++)
				{
					if (this.LOG_LEVEL >= MB2_LogLevel.debug)
					{
						UnityEngine.Debug.Log(string.Concat(new object[]
						{
							"Calculating ideal sizes for texSet TexSet ",
							l,
							" of ",
							distinctMaterialTextures.Count
						}));
					}
					MB3_TextureCombiner.MB_TexSet mb_TexSet = distinctMaterialTextures[l];
					mb_TexSet.idealWidth = 1;
					mb_TexSet.idealHeight = 1;
					int num2 = 1;
					int num3 = 1;
					if (mb_TexSet.ts.Length != texPropertyNames.Count)
					{
						UnityEngine.Debug.LogError("length of arrays in each element of distinctMaterialTextures must be texPropertyNames.Count");
					}
					for (int m = 0; m < texPropertyNames.Count; m++)
					{
						MB3_TextureCombiner.MeshBakerMaterialTexture meshBakerMaterialTexture = mb_TexSet.ts[m];
						if (!meshBakerMaterialTexture.matTilingRect.size.Equals(Vector2.one) && distinctMaterialTextures.Count > 1 && this.LOG_LEVEL >= MB2_LogLevel.warn)
						{
							UnityEngine.Debug.LogWarning(string.Concat(new object[]
							{
								"Texture ",
								meshBakerMaterialTexture.t,
								"is tiled by ",
								meshBakerMaterialTexture.matTilingRect.size,
								" tiling will be baked into a texture with maxSize:",
								this._maxTilingBakeSize
							}));
						}
						if (!mb_TexSet.obUVscale.Equals(Vector2.one) && distinctMaterialTextures.Count > 1 && this._fixOutOfBoundsUVs && this.LOG_LEVEL >= MB2_LogLevel.warn)
						{
							UnityEngine.Debug.LogWarning(string.Concat(new object[]
							{
								"Texture ",
								meshBakerMaterialTexture.t,
								"has out of bounds UVs that effectively tile by ",
								mb_TexSet.obUVscale,
								" tiling will be baked into a texture with maxSize:",
								this._maxTilingBakeSize
							}));
						}
						if (!allTexturesAreNullAndSameColor[m] && meshBakerMaterialTexture.t == null)
						{
							if (this.LOG_LEVEL >= MB2_LogLevel.trace)
							{
								UnityEngine.Debug.Log("No source texture creating a 16x16 texture.");
							}
							meshBakerMaterialTexture.t = this._createTemporaryTexture(16, 16, TextureFormat.ARGB32, true);
							if (this._considerNonTextureProperties && this.resultMaterialTextureBlender != null)
							{
								Color colorIfNoTexture3 = this.resultMaterialTextureBlender.GetColorIfNoTexture(mb_TexSet.matsAndGOs.mats[0].mat, texPropertyNames[m]);
								if (this.LOG_LEVEL >= MB2_LogLevel.trace)
								{
									UnityEngine.Debug.Log("Setting texture to solid color " + colorIfNoTexture3);
								}
								MB_Utility.setSolidColor(meshBakerMaterialTexture.t, colorIfNoTexture3);
							}
							else
							{
								Color colorIfNoTexture4 = MB3_TextureCombiner.GetColorIfNoTexture(texPropertyNames[m]);
								MB_Utility.setSolidColor(meshBakerMaterialTexture.t, colorIfNoTexture4);
							}
							if (this.fixOutOfBoundsUVs)
							{
								meshBakerMaterialTexture.encapsulatingSamplingRect = mb_TexSet.obUVrect;
							}
							else
							{
								meshBakerMaterialTexture.encapsulatingSamplingRect = new DRect(0f, 0f, 1f, 1f);
							}
						}
						if (meshBakerMaterialTexture.t != null)
						{
							Vector2 adjustedForScaleAndOffset2Dimensions = this.GetAdjustedForScaleAndOffset2Dimensions(meshBakerMaterialTexture, mb_TexSet.obUVoffset, mb_TexSet.obUVscale);
							if ((int)(adjustedForScaleAndOffset2Dimensions.x * adjustedForScaleAndOffset2Dimensions.y) > num2 * num3)
							{
								if (this.LOG_LEVEL >= MB2_LogLevel.trace)
								{
									UnityEngine.Debug.Log(string.Concat(new object[]
									{
										"    matTex ",
										meshBakerMaterialTexture.t,
										" ",
										adjustedForScaleAndOffset2Dimensions,
										" has a bigger size than ",
										num2,
										" ",
										num3
									}));
								}
								num2 = (int)adjustedForScaleAndOffset2Dimensions.x;
								num3 = (int)adjustedForScaleAndOffset2Dimensions.y;
							}
						}
					}
					if (this._resizePowerOfTwoTextures)
					{
						if (num2 <= num * 5)
						{
							UnityEngine.Debug.LogWarning(string.Format("Some of the textures have widths close to the size of the padding. It is not recommended to use _resizePowerOfTwoTextures with widths this small.", mb_TexSet.ToString()));
						}
						if (num3 <= num * 5)
						{
							UnityEngine.Debug.LogWarning(string.Format("Some of the textures have heights close to the size of the padding. It is not recommended to use _resizePowerOfTwoTextures with heights this small.", mb_TexSet.ToString()));
						}
						if (this.IsPowerOfTwo(num2))
						{
							num2 -= num * 2;
						}
						if (this.IsPowerOfTwo(num3))
						{
							num3 -= num * 2;
						}
						if (num2 < 1)
						{
							num2 = 1;
						}
						if (num3 < 1)
						{
							num3 = 1;
						}
					}
					if (this.LOG_LEVEL >= MB2_LogLevel.trace)
					{
						UnityEngine.Debug.Log(string.Concat(new object[]
						{
							"    Ideal size is ",
							num2,
							" ",
							num3
						}));
					}
					mb_TexSet.idealWidth = num2;
					mb_TexSet.idealHeight = num3;
				}
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log("Total time Step2 Calculate Ideal Sizes part1: " + stopwatch.Elapsed.ToString());
			}
			if (distinctMaterialTextures.Count > 1 && this._packingAlgorithm != MB2_PackingAlgorithmEnum.MeshBakerTexturePacker_Fast)
			{
				for (int n = 0; n < distinctMaterialTextures.Count; n++)
				{
					for (int num4 = 0; num4 < texPropertyNames.Count; num4++)
					{
						Texture2D t = distinctMaterialTextures[n].ts[num4].t;
						if (t != null && textureEditorMethods != null)
						{
							if (progressInfo != null)
							{
								progressInfo(string.Format("Convert texture {0} to readable format ", t), 0.5f);
							}
							textureEditorMethods.AddTextureFormat(t, texPropertyNames[num4].isNormalMap);
						}
					}
				}
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log("Total time Step2 Calculate Ideal Sizes part2: " + stopwatch.Elapsed.ToString());
			}
			this.__step2_CalculateIdealSizesForTexturesInAtlasAndPadding = num;
			yield break;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00011B78 File Offset: 0x0000FF78
		private AtlasPackingResult[] __Step3_RunTexturePacker(List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures, int _padding)
		{
			AtlasPackingResult[] array = this.__RuntTexturePackerOnly(distinctMaterialTextures, _padding);
			for (int i = 0; i < array.Length; i++)
			{
				List<MB3_TextureCombiner.MatsAndGOs> list = new List<MB3_TextureCombiner.MatsAndGOs>();
				array[i].data = list;
				for (int j = 0; j < array[i].srcImgIdxs.Length; j++)
				{
					MB3_TextureCombiner.MB_TexSet mb_TexSet = distinctMaterialTextures[array[i].srcImgIdxs[j]];
					list.Add(mb_TexSet.matsAndGOs);
				}
			}
			return array;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00011BEC File Offset: 0x0000FFEC
		private IEnumerator __Step3_BuildAndSaveAtlasesAndStoreResults(MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult result, ProgressUpdateDelegate progressInfo, List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures, List<ShaderTextureProperty> texPropertyNames, bool[] allTexturesAreNullAndSameColor, int _padding, MB2_EditorMethodsInterface textureEditorMethods, MB_AtlasesAndRects resultAtlasesAndRects, Material resultMaterial)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			int numAtlases = texPropertyNames.Count;
			StringBuilder report = new StringBuilder();
			if (numAtlases > 0)
			{
				report = new StringBuilder();
				report.AppendLine("Report");
				for (int i = 0; i < distinctMaterialTextures.Count; i++)
				{
					MB3_TextureCombiner.MB_TexSet mb_TexSet = distinctMaterialTextures[i];
					report.AppendLine("----------");
					report.Append(string.Concat(new object[]
					{
						"This set of textures will be resized to:",
						mb_TexSet.idealWidth,
						"x",
						mb_TexSet.idealHeight,
						"\n"
					}));
					for (int j = 0; j < mb_TexSet.ts.Length; j++)
					{
						if (mb_TexSet.ts[j].t != null)
						{
							report.Append(string.Concat(new object[]
							{
								"   [",
								texPropertyNames[j].name,
								" ",
								mb_TexSet.ts[j].t.name,
								" ",
								mb_TexSet.ts[j].t.width,
								"x",
								mb_TexSet.ts[j].t.height,
								"]"
							}));
							if (mb_TexSet.ts[j].matTilingRect.size != Vector2.one || mb_TexSet.ts[j].matTilingRect.min != Vector2.zero)
							{
								report.AppendFormat(" material scale {0} offset{1} ", mb_TexSet.ts[j].matTilingRect.size.ToString("G4"), mb_TexSet.ts[j].matTilingRect.min.ToString("G4"));
							}
							if (mb_TexSet.obUVscale != Vector2.one || mb_TexSet.obUVoffset != Vector2.zero)
							{
								report.AppendFormat(" obUV scale {0} offset{1} ", mb_TexSet.obUVscale.ToString("G4"), mb_TexSet.obUVoffset.ToString("G4"));
							}
							report.AppendLine(string.Empty);
						}
						else
						{
							report.Append("   [" + texPropertyNames[j].name + " null ");
							if (allTexturesAreNullAndSameColor[j])
							{
								report.Append("no atlas will be created all textures null]\n");
							}
							else
							{
								report.AppendFormat("a 16x16 texture will be created]\n", new object[0]);
							}
						}
					}
					report.AppendLine(string.Empty);
					report.Append("Materials using:");
					for (int k = 0; k < mb_TexSet.matsAndGOs.mats.Count; k++)
					{
						report.Append(mb_TexSet.matsAndGOs.mats[k].mat.name + ", ");
					}
					report.AppendLine(string.Empty);
				}
			}
			GC.Collect();
			Texture2D[] atlases = new Texture2D[numAtlases];
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log("time Step 3 Create And Save Atlases part 1 " + sw.Elapsed.ToString());
			}
			Rect[] rectsInAtlas;
			if (this._packingAlgorithm == MB2_PackingAlgorithmEnum.UnitysPackTextures)
			{
				rectsInAtlas = this.__CreateAtlasesUnityTexturePacker(progressInfo, numAtlases, distinctMaterialTextures, texPropertyNames, allTexturesAreNullAndSameColor, resultMaterial, atlases, textureEditorMethods, _padding);
			}
			else if (this._packingAlgorithm == MB2_PackingAlgorithmEnum.MeshBakerTexturePacker)
			{
				yield return this.__CreateAtlasesMBTexturePacker(progressInfo, numAtlases, distinctMaterialTextures, texPropertyNames, allTexturesAreNullAndSameColor, resultMaterial, atlases, textureEditorMethods, _padding);
				rectsInAtlas = this.__createAtlasesMBTexturePacker;
			}
			else
			{
				rectsInAtlas = this.__CreateAtlasesMBTexturePackerFast(progressInfo, numAtlases, distinctMaterialTextures, texPropertyNames, allTexturesAreNullAndSameColor, resultMaterial, atlases, textureEditorMethods, _padding);
			}
			float t3 = (float)sw.ElapsedMilliseconds;
			this.AdjustNonTextureProperties(resultMaterial, texPropertyNames, distinctMaterialTextures, this._considerNonTextureProperties, textureEditorMethods);
			if (progressInfo != null)
			{
				progressInfo("Building Report", 0.7f);
			}
			StringBuilder atlasMessage = new StringBuilder();
			atlasMessage.AppendLine("---- Atlases ------");
			for (int l = 0; l < numAtlases; l++)
			{
				if (atlases[l] != null)
				{
					atlasMessage.AppendLine(string.Concat(new object[]
					{
						"Created Atlas For: ",
						texPropertyNames[l].name,
						" h=",
						atlases[l].height,
						" w=",
						atlases[l].width
					}));
				}
				else if (allTexturesAreNullAndSameColor[l])
				{
					atlasMessage.AppendLine("Did not create atlas for " + texPropertyNames[l].name + " because all source textures were null.");
				}
			}
			report.Append(atlasMessage.ToString());
			List<MB_MaterialAndUVRect> mat2rect_map = new List<MB_MaterialAndUVRect>();
			for (int m = 0; m < distinctMaterialTextures.Count; m++)
			{
				List<MB3_TextureCombiner.MatAndTransformToMerged> mats = distinctMaterialTextures[m].matsAndGOs.mats;
				Rect rect = new Rect(0f, 0f, 1f, 1f);
				if (distinctMaterialTextures[m].ts.Length > 0)
				{
					rect = distinctMaterialTextures[m].ts[0].encapsulatingSamplingRect.GetRect();
				}
				for (int n = 0; n < mats.Count; n++)
				{
					MB_MaterialAndUVRect item = new MB_MaterialAndUVRect(mats[n].mat, rectsInAtlas[m], mats[n].samplingRectMatAndUVTiling.GetRect(), mats[n].materialTiling.GetRect(), rect, mats[n].objName);
					if (!mat2rect_map.Contains(item))
					{
						mat2rect_map.Add(item);
					}
				}
			}
			resultAtlasesAndRects.atlases = atlases;
			resultAtlasesAndRects.texPropertyNames = ShaderTextureProperty.GetNames(texPropertyNames);
			resultAtlasesAndRects.mat2rect_map = mat2rect_map;
			if (progressInfo != null)
			{
				progressInfo("Restoring Texture Formats & Read Flags", 0.8f);
			}
			this._destroyTemporaryTextures();
			if (textureEditorMethods != null)
			{
				textureEditorMethods.RestoreReadFlagsAndFormats(progressInfo);
			}
			if (report != null && this.LOG_LEVEL >= MB2_LogLevel.info)
			{
				UnityEngine.Debug.Log(report.ToString());
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log("Time Step 3 Create And Save Atlases part 3 " + ((float)sw.ElapsedMilliseconds - t3).ToString("f5"));
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log("Total time Step 3 Create And Save Atlases " + sw.Elapsed.ToString());
			}
			yield break;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00011C48 File Offset: 0x00010048
		private AtlasPackingResult[] __RuntTexturePackerOnly(List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures, int _padding)
		{
			AtlasPackingResult[] array;
			if (distinctMaterialTextures.Count == 1 && !this._fixOutOfBoundsUVs)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					UnityEngine.Debug.Log("Only one image per atlas. Will re-use original texture");
				}
				array = new AtlasPackingResult[]
				{
					new AtlasPackingResult()
				};
				array[0].rects = new Rect[1];
				array[0].srcImgIdxs = new int[1];
				array[0].rects[0] = new Rect(0f, 0f, 1f, 1f);
				Texture2D x = null;
				MB3_TextureCombiner.MeshBakerMaterialTexture meshBakerMaterialTexture = null;
				if (distinctMaterialTextures[0].ts.Length > 0)
				{
					meshBakerMaterialTexture = distinctMaterialTextures[0].ts[0];
					x = meshBakerMaterialTexture.t;
				}
				array[0].atlasX = ((!(x == null)) ? meshBakerMaterialTexture.t.width : 16);
				array[0].atlasY = ((!(x == null)) ? meshBakerMaterialTexture.t.height : 16);
				array[0].usedW = ((!(x == null)) ? meshBakerMaterialTexture.t.width : 16);
				array[0].usedH = ((!(x == null)) ? meshBakerMaterialTexture.t.height : 16);
			}
			else
			{
				List<Vector2> list = new List<Vector2>();
				for (int i = 0; i < distinctMaterialTextures.Count; i++)
				{
					list.Add(new Vector2((float)distinctMaterialTextures[i].idealWidth, (float)distinctMaterialTextures[i].idealHeight));
				}
				array = new MB2_TexturePacker
				{
					doPowerOfTwoTextures = this._meshBakerTexturePackerForcePowerOfTwo
				}.GetRects(list, this._maxAtlasSize, _padding, true);
			}
			return array;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00011E14 File Offset: 0x00010214
		private IEnumerator __CreateAtlasesMBTexturePacker(ProgressUpdateDelegate progressInfo, int numAtlases, List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures, List<ShaderTextureProperty> texPropertyNames, bool[] allTexturesAreNullAndSameColor, Material resultMaterial, Texture2D[] atlases, MB2_EditorMethodsInterface textureEditorMethods, int _padding)
		{
			Rect[] uvRects;
			if (distinctMaterialTextures.Count == 1 && !this._fixOutOfBoundsUVs)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					UnityEngine.Debug.Log("Only one image per atlas. Will re-use original texture");
				}
				uvRects = new Rect[]
				{
					new Rect(0f, 0f, 1f, 1f)
				};
				for (int i = 0; i < numAtlases; i++)
				{
					MB3_TextureCombiner.MeshBakerMaterialTexture meshBakerMaterialTexture = distinctMaterialTextures[0].ts[i];
					atlases[i] = meshBakerMaterialTexture.t;
					resultMaterial.SetTexture(texPropertyNames[i].name, atlases[i]);
					resultMaterial.SetTextureScale(texPropertyNames[i].name, meshBakerMaterialTexture.matTilingRect.size);
					resultMaterial.SetTextureOffset(texPropertyNames[i].name, meshBakerMaterialTexture.matTilingRect.min);
				}
			}
			else
			{
				List<Vector2> imageSizes = new List<Vector2>();
				for (int j = 0; j < distinctMaterialTextures.Count; j++)
				{
					imageSizes.Add(new Vector2((float)distinctMaterialTextures[j].idealWidth, (float)distinctMaterialTextures[j].idealHeight));
				}
				MB2_TexturePacker tp = new MB2_TexturePacker();
				tp.doPowerOfTwoTextures = this._meshBakerTexturePackerForcePowerOfTwo;
				int atlasSizeX = 1;
				int atlasSizeY = 1;
				int atlasMaxDimension = this._maxAtlasSize;
				AtlasPackingResult[] packerRects = tp.GetRects(imageSizes, atlasMaxDimension, _padding);
				atlasSizeX = packerRects[0].atlasX;
				atlasSizeY = packerRects[0].atlasY;
				uvRects = packerRects[0].rects;
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Generated atlas will be ",
						atlasSizeX,
						"x",
						atlasSizeY,
						" (Max atlas size for platform: ",
						atlasMaxDimension,
						")"
					}));
				}
				for (int propIdx = 0; propIdx < numAtlases; propIdx++)
				{
					Texture2D atlas = null;
					if (allTexturesAreNullAndSameColor[propIdx])
					{
						atlas = null;
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							UnityEngine.Debug.Log("=== Not creating atlas for " + texPropertyNames[propIdx].name + " because textures are null and default value parameters are the same.");
						}
					}
					else
					{
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							UnityEngine.Debug.Log("=== Creating atlas for " + texPropertyNames[propIdx].name);
						}
						GC.Collect();
						Color[][] atlasPixels = new Color[atlasSizeY][];
						for (int k = 0; k < atlasPixels.Length; k++)
						{
							atlasPixels[k] = new Color[atlasSizeX];
						}
						bool isNormalMap = false;
						if (texPropertyNames[propIdx].isNormalMap)
						{
							isNormalMap = true;
						}
						for (int texSetIdx = 0; texSetIdx < distinctMaterialTextures.Count; texSetIdx++)
						{
							string s = string.Concat(new object[]
							{
								"Creating Atlas '",
								texPropertyNames[propIdx].name,
								"' texture ",
								distinctMaterialTextures[texSetIdx]
							});
							if (progressInfo != null)
							{
								progressInfo(s, 0.01f);
							}
							MB3_TextureCombiner.MB_TexSet texSet = distinctMaterialTextures[texSetIdx];
							if (this.LOG_LEVEL >= MB2_LogLevel.trace)
							{
								UnityEngine.Debug.Log(string.Format("Adding texture {0} to atlas {1}", (!(texSet.ts[propIdx].t == null)) ? texSet.ts[propIdx].t.ToString() : "null", texPropertyNames[propIdx]));
							}
							Rect r = uvRects[texSetIdx];
							Texture2D t = texSet.ts[propIdx].t;
							int x = Mathf.RoundToInt(r.x * (float)atlasSizeX);
							int y = Mathf.RoundToInt(r.y * (float)atlasSizeY);
							int ww = Mathf.RoundToInt(r.width * (float)atlasSizeX);
							int hh = Mathf.RoundToInt(r.height * (float)atlasSizeY);
							if (ww == 0 || hh == 0)
							{
								UnityEngine.Debug.LogError("Image in atlas has no height or width");
							}
							if (progressInfo != null)
							{
								progressInfo(s + " set ReadWrite flag", 0.01f);
							}
							if (textureEditorMethods != null)
							{
								textureEditorMethods.SetReadWriteFlag(t, true, true);
							}
							if (progressInfo != null)
							{
								progressInfo(string.Concat(new object[]
								{
									s,
									"Copying to atlas: '",
									texSet.ts[propIdx].t,
									"'"
								}), 0.02f);
							}
							DRect samplingRect = texSet.ts[propIdx].encapsulatingSamplingRect;
							yield return this.CopyScaledAndTiledToAtlas(texSet.ts[propIdx], texSet, texPropertyNames[propIdx], samplingRect, x, y, ww, hh, this._fixOutOfBoundsUVs, this._maxTilingBakeSize, atlasPixels, atlasSizeX, isNormalMap, progressInfo);
						}
						yield return numAtlases;
						if (progressInfo != null)
						{
							progressInfo("Applying changes to atlas: '" + texPropertyNames[propIdx].name + "'", 0.03f);
						}
						atlas = new Texture2D(atlasSizeX, atlasSizeY, TextureFormat.ARGB32, true);
						for (int l = 0; l < atlasPixels.Length; l++)
						{
							atlas.SetPixels(0, l, atlasSizeX, 1, atlasPixels[l]);
						}
						atlas.Apply();
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							UnityEngine.Debug.Log(string.Concat(new object[]
							{
								"Saving atlas ",
								texPropertyNames[propIdx].name,
								" w=",
								atlas.width,
								" h=",
								atlas.height
							}));
						}
					}
					atlases[propIdx] = atlas;
					if (progressInfo != null)
					{
						progressInfo("Saving atlas: '" + texPropertyNames[propIdx].name + "'", 0.04f);
					}
					if (this._saveAtlasesAsAssets && textureEditorMethods != null)
					{
						textureEditorMethods.SaveAtlasToAssetDatabase(atlases[propIdx], texPropertyNames[propIdx], propIdx, resultMaterial);
					}
					else
					{
						resultMaterial.SetTexture(texPropertyNames[propIdx].name, atlases[propIdx]);
					}
					resultMaterial.SetTextureOffset(texPropertyNames[propIdx].name, Vector2.zero);
					resultMaterial.SetTextureScale(texPropertyNames[propIdx].name, Vector2.one);
					this._destroyTemporaryTextures();
				}
			}
			this.__createAtlasesMBTexturePacker = uvRects;
			yield break;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00011E74 File Offset: 0x00010274
		private Rect[] __CreateAtlasesMBTexturePackerFast(ProgressUpdateDelegate progressInfo, int numAtlases, List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures, List<ShaderTextureProperty> texPropertyNames, bool[] allTexturesAreNullAndSameColor, Material resultMaterial, Texture2D[] atlases, MB2_EditorMethodsInterface textureEditorMethods, int _padding)
		{
			Rect[] array;
			if (distinctMaterialTextures.Count == 1 && !this._fixOutOfBoundsUVs)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					UnityEngine.Debug.Log("Only one image per atlas. Will re-use original texture");
				}
				array = new Rect[]
				{
					new Rect(0f, 0f, 1f, 1f)
				};
				for (int i = 0; i < numAtlases; i++)
				{
					MB3_TextureCombiner.MeshBakerMaterialTexture meshBakerMaterialTexture = distinctMaterialTextures[0].ts[i];
					atlases[i] = meshBakerMaterialTexture.t;
					resultMaterial.SetTexture(texPropertyNames[i].name, atlases[i]);
					resultMaterial.SetTextureScale(texPropertyNames[i].name, meshBakerMaterialTexture.matTilingRect.size);
					resultMaterial.SetTextureOffset(texPropertyNames[i].name, meshBakerMaterialTexture.matTilingRect.min);
				}
			}
			else
			{
				List<Vector2> list = new List<Vector2>();
				for (int j = 0; j < distinctMaterialTextures.Count; j++)
				{
					list.Add(new Vector2((float)distinctMaterialTextures[j].idealWidth, (float)distinctMaterialTextures[j].idealHeight));
				}
				MB2_TexturePacker mb2_TexturePacker = new MB2_TexturePacker();
				mb2_TexturePacker.doPowerOfTwoTextures = this._meshBakerTexturePackerForcePowerOfTwo;
				int maxAtlasSize = this._maxAtlasSize;
				AtlasPackingResult[] rects = mb2_TexturePacker.GetRects(list, maxAtlasSize, _padding);
				int atlasX = rects[0].atlasX;
				int atlasY = rects[0].atlasY;
				array = rects[0].rects;
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Generated atlas will be ",
						atlasX,
						"x",
						atlasY,
						" (Max atlas size for platform: ",
						maxAtlasSize,
						")"
					}));
				}
				GameObject gameObject = null;
				try
				{
					gameObject = new GameObject("MBrenderAtlasesGO");
					MB3_AtlasPackerRenderTexture mb3_AtlasPackerRenderTexture = gameObject.AddComponent<MB3_AtlasPackerRenderTexture>();
					gameObject.AddComponent<Camera>();
					if (this._considerNonTextureProperties && this.LOG_LEVEL >= MB2_LogLevel.warn)
					{
						UnityEngine.Debug.LogWarning("Blend Non-Texture Properties has limited functionality when used with Mesh Baker Texture Packer Fast.");
					}
					for (int k = 0; k < numAtlases; k++)
					{
						Texture2D texture2D;
						if (allTexturesAreNullAndSameColor[k])
						{
							texture2D = null;
							if (this.LOG_LEVEL >= MB2_LogLevel.debug)
							{
								UnityEngine.Debug.Log("Not creating atlas for " + texPropertyNames[k].name + " because textures are null and default value parameters are the same.");
							}
						}
						else
						{
							GC.Collect();
							if (progressInfo != null)
							{
								progressInfo("Creating Atlas '" + texPropertyNames[k].name + "'", 0.01f);
							}
							if (this.LOG_LEVEL >= MB2_LogLevel.debug)
							{
								UnityEngine.Debug.Log(string.Concat(new object[]
								{
									"About to render ",
									texPropertyNames[k].name,
									" isNormal=",
									texPropertyNames[k].isNormalMap
								}));
							}
							mb3_AtlasPackerRenderTexture.LOG_LEVEL = this.LOG_LEVEL;
							mb3_AtlasPackerRenderTexture.width = atlasX;
							mb3_AtlasPackerRenderTexture.height = atlasY;
							mb3_AtlasPackerRenderTexture.padding = _padding;
							mb3_AtlasPackerRenderTexture.rects = array;
							mb3_AtlasPackerRenderTexture.textureSets = distinctMaterialTextures;
							mb3_AtlasPackerRenderTexture.indexOfTexSetToRender = k;
							mb3_AtlasPackerRenderTexture.texPropertyName = texPropertyNames[k];
							mb3_AtlasPackerRenderTexture.isNormalMap = texPropertyNames[k].isNormalMap;
							mb3_AtlasPackerRenderTexture.fixOutOfBoundsUVs = this._fixOutOfBoundsUVs;
							mb3_AtlasPackerRenderTexture.considerNonTextureProperties = this._considerNonTextureProperties;
							mb3_AtlasPackerRenderTexture.resultMaterialTextureBlender = this.resultMaterialTextureBlender;
							texture2D = mb3_AtlasPackerRenderTexture.OnRenderAtlas(this);
							if (this.LOG_LEVEL >= MB2_LogLevel.debug)
							{
								UnityEngine.Debug.Log(string.Concat(new object[]
								{
									"Saving atlas ",
									texPropertyNames[k].name,
									" w=",
									texture2D.width,
									" h=",
									texture2D.height,
									" id=",
									texture2D.GetInstanceID()
								}));
							}
						}
						atlases[k] = texture2D;
						if (progressInfo != null)
						{
							progressInfo("Saving atlas: '" + texPropertyNames[k].name + "'", 0.04f);
						}
						if (this._saveAtlasesAsAssets && textureEditorMethods != null)
						{
							textureEditorMethods.SaveAtlasToAssetDatabase(atlases[k], texPropertyNames[k], k, resultMaterial);
						}
						else
						{
							resultMaterial.SetTexture(texPropertyNames[k].name, atlases[k]);
						}
						resultMaterial.SetTextureOffset(texPropertyNames[k].name, Vector2.zero);
						resultMaterial.SetTextureScale(texPropertyNames[k].name, Vector2.one);
						this._destroyTemporaryTextures();
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
				finally
				{
					if (gameObject != null)
					{
						MB_Utility.Destroy(gameObject);
					}
				}
			}
			return array;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0001239C File Offset: 0x0001079C
		private Rect[] __CreateAtlasesUnityTexturePacker(ProgressUpdateDelegate progressInfo, int numAtlases, List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures, List<ShaderTextureProperty> texPropertyNames, bool[] allTexturesAreNullAndSameColor, Material resultMaterial, Texture2D[] atlases, MB2_EditorMethodsInterface textureEditorMethods, int _padding)
		{
			Rect[] array;
			if (distinctMaterialTextures.Count == 1 && !this._fixOutOfBoundsUVs)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					UnityEngine.Debug.Log("Only one image per atlas. Will re-use original texture");
				}
				array = new Rect[]
				{
					new Rect(0f, 0f, 1f, 1f)
				};
				for (int i = 0; i < numAtlases; i++)
				{
					MB3_TextureCombiner.MeshBakerMaterialTexture meshBakerMaterialTexture = distinctMaterialTextures[0].ts[i];
					atlases[i] = meshBakerMaterialTexture.t;
					resultMaterial.SetTexture(texPropertyNames[i].name, atlases[i]);
					resultMaterial.SetTextureScale(texPropertyNames[i].name, meshBakerMaterialTexture.matTilingRect.size);
					resultMaterial.SetTextureOffset(texPropertyNames[i].name, meshBakerMaterialTexture.matTilingRect.min);
				}
			}
			else
			{
				long num = 0L;
				int w = 1;
				int h = 1;
				array = null;
				for (int j = 0; j < numAtlases; j++)
				{
					Texture2D texture2D;
					if (allTexturesAreNullAndSameColor[j])
					{
						texture2D = null;
					}
					else
					{
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							UnityEngine.Debug.LogWarning(string.Concat(new object[]
							{
								"Beginning loop ",
								j,
								" num temporary textures ",
								this._temporaryTextures.Count
							}));
						}
						for (int k = 0; k < distinctMaterialTextures.Count; k++)
						{
							MB3_TextureCombiner.MB_TexSet mb_TexSet = distinctMaterialTextures[k];
							int idealWidth = mb_TexSet.idealWidth;
							int idealHeight = mb_TexSet.idealHeight;
							Texture2D texture2D2 = mb_TexSet.ts[j].t;
							if (texture2D2 == null)
							{
								texture2D2 = (mb_TexSet.ts[j].t = this._createTemporaryTexture(idealWidth, idealHeight, TextureFormat.ARGB32, true));
								if (this._considerNonTextureProperties && this.resultMaterialTextureBlender != null)
								{
									Color colorIfNoTexture = this.resultMaterialTextureBlender.GetColorIfNoTexture(mb_TexSet.matsAndGOs.mats[0].mat, texPropertyNames[j]);
									if (this.LOG_LEVEL >= MB2_LogLevel.trace)
									{
										UnityEngine.Debug.Log("Setting texture to solid color " + colorIfNoTexture);
									}
									MB_Utility.setSolidColor(texture2D2, colorIfNoTexture);
								}
								else
								{
									Color colorIfNoTexture2 = MB3_TextureCombiner.GetColorIfNoTexture(texPropertyNames[j]);
									MB_Utility.setSolidColor(texture2D2, colorIfNoTexture2);
								}
							}
							if (progressInfo != null)
							{
								progressInfo("Adjusting for scale and offset " + texture2D2, 0.01f);
							}
							if (textureEditorMethods != null)
							{
								textureEditorMethods.SetReadWriteFlag(texture2D2, true, true);
							}
							texture2D2 = this.GetAdjustedForScaleAndOffset2(mb_TexSet.ts[j], mb_TexSet.obUVoffset, mb_TexSet.obUVscale);
							if (texture2D2.width != idealWidth || texture2D2.height != idealHeight)
							{
								if (progressInfo != null)
								{
									progressInfo("Resizing texture '" + texture2D2 + "'", 0.01f);
								}
								if (this.LOG_LEVEL >= MB2_LogLevel.debug)
								{
									UnityEngine.Debug.LogWarning(string.Concat(new object[]
									{
										"Copying and resizing texture ",
										texPropertyNames[j].name,
										" from ",
										texture2D2.width,
										"x",
										texture2D2.height,
										" to ",
										idealWidth,
										"x",
										idealHeight
									}));
								}
								texture2D2 = this._resizeTexture(texture2D2, idealWidth, idealHeight);
							}
							mb_TexSet.ts[j].t = texture2D2;
						}
						Texture2D[] array2 = new Texture2D[distinctMaterialTextures.Count];
						for (int l = 0; l < distinctMaterialTextures.Count; l++)
						{
							Texture2D texture2D3 = distinctMaterialTextures[l].ts[j].t;
							num += (long)(texture2D3.width * texture2D3.height);
							if (this._considerNonTextureProperties)
							{
								texture2D3 = this.TintTextureWithTextureCombiner(texture2D3, distinctMaterialTextures[l], texPropertyNames[j]);
							}
							array2[l] = texture2D3;
						}
						if (textureEditorMethods != null)
						{
							textureEditorMethods.CheckBuildSettings(num);
						}
						if (Math.Sqrt((double)num) > 3500.0 && this.LOG_LEVEL >= MB2_LogLevel.warn)
						{
							UnityEngine.Debug.LogWarning("The maximum possible atlas size is 4096. Textures may be shrunk");
						}
						texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, true);
						if (progressInfo != null)
						{
							progressInfo("Packing texture atlas " + texPropertyNames[j].name, 0.25f);
						}
						if (j == 0)
						{
							if (progressInfo != null)
							{
								progressInfo("Estimated min size of atlases: " + Math.Sqrt((double)num).ToString("F0"), 0.1f);
							}
							if (this.LOG_LEVEL >= MB2_LogLevel.info)
							{
								UnityEngine.Debug.Log("Estimated atlas minimum size:" + Math.Sqrt((double)num).ToString("F0"));
							}
							this._addWatermark(array2);
							if (distinctMaterialTextures.Count == 1 && !this._fixOutOfBoundsUVs)
							{
								array = new Rect[]
								{
									new Rect(0f, 0f, 1f, 1f)
								};
								texture2D = this._copyTexturesIntoAtlas(array2, _padding, array, array2[0].width, array2[0].height);
							}
							else
							{
								int maximumAtlasSize = 4096;
								array = texture2D.PackTextures(array2, _padding, maximumAtlasSize, false);
							}
							if (this.LOG_LEVEL >= MB2_LogLevel.info)
							{
								UnityEngine.Debug.Log(string.Concat(new object[]
								{
									"After pack textures atlas size ",
									texture2D.width,
									" ",
									texture2D.height
								}));
							}
							w = texture2D.width;
							h = texture2D.height;
							texture2D.Apply();
						}
						else
						{
							if (progressInfo != null)
							{
								progressInfo("Copying Textures Into: " + texPropertyNames[j].name, 0.1f);
							}
							texture2D = this._copyTexturesIntoAtlas(array2, _padding, array, w, h);
						}
					}
					atlases[j] = texture2D;
					if (this._saveAtlasesAsAssets && textureEditorMethods != null)
					{
						textureEditorMethods.SaveAtlasToAssetDatabase(atlases[j], texPropertyNames[j], j, resultMaterial);
					}
					resultMaterial.SetTextureOffset(texPropertyNames[j].name, Vector2.zero);
					resultMaterial.SetTextureScale(texPropertyNames[j].name, Vector2.one);
					this._destroyTemporaryTextures();
					GC.Collect();
				}
			}
			return array;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00012A2C File Offset: 0x00010E2C
		private void _addWatermark(Texture2D[] texToPack)
		{
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00012A2E File Offset: 0x00010E2E
		private Texture2D _addWatermark(Texture2D texToPack)
		{
			return texToPack;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00012A34 File Offset: 0x00010E34
		private Texture2D _copyTexturesIntoAtlas(Texture2D[] texToPack, int padding, Rect[] rs, int w, int h)
		{
			Texture2D texture2D = new Texture2D(w, h, TextureFormat.ARGB32, true);
			MB_Utility.setSolidColor(texture2D, Color.clear);
			for (int i = 0; i < rs.Length; i++)
			{
				Rect rect = rs[i];
				Texture2D texture2D2 = texToPack[i];
				int x = Mathf.RoundToInt(rect.x * (float)w);
				int y = Mathf.RoundToInt(rect.y * (float)h);
				int num = Mathf.RoundToInt(rect.width * (float)w);
				int num2 = Mathf.RoundToInt(rect.height * (float)h);
				if (texture2D2.width != num && texture2D2.height != num2)
				{
					texture2D2 = MB_Utility.resampleTexture(texture2D2, num, num2);
					this._temporaryTextures.Add(texture2D2);
				}
				texture2D.SetPixels(x, y, num, num2, texture2D2.GetPixels());
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00012B11 File Offset: 0x00010F11
		private bool IsPowerOfTwo(int x)
		{
			return (x & x - 1) == 0;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00012B1C File Offset: 0x00010F1C
		private void MergeOverlappingDistinctMaterialTexturesAndCalcMaterialSubrects(List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures, bool fixOutOfBoundsUVs)
		{
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log("MergeOverlappingDistinctMaterialTexturesAndCalcMaterialSubrects");
			}
			int num = 0;
			for (int i = 0; i < distinctMaterialTextures.Count; i++)
			{
				MB3_TextureCombiner.MB_TexSet mb_TexSet = distinctMaterialTextures[i];
				int num2 = -1;
				bool flag = true;
				DRect a = default(DRect);
				for (int j = 0; j < mb_TexSet.ts.Length; j++)
				{
					if (num2 != -1)
					{
						if (mb_TexSet.ts[j].t != null && a != mb_TexSet.ts[j].matTilingRect)
						{
							flag = false;
						}
					}
					else if (mb_TexSet.ts[j].t != null)
					{
						num2 = j;
						a = mb_TexSet.ts[j].matTilingRect;
					}
				}
				if (flag)
				{
					mb_TexSet.allTexturesUseSameMatTiling = true;
				}
				else
				{
					if (this.LOG_LEVEL <= MB2_LogLevel.info)
					{
						UnityEngine.Debug.Log(string.Format("Textures in material(s) do not all use the same material tiling. This set of textures will not be considered for merge: {0} ", mb_TexSet.GetDescription()));
					}
					mb_TexSet.allTexturesUseSameMatTiling = false;
				}
			}
			for (int k = 0; k < distinctMaterialTextures.Count; k++)
			{
				MB3_TextureCombiner.MB_TexSet mb_TexSet2 = distinctMaterialTextures[k];
				DRect obUVRectIfTilingSame;
				if (fixOutOfBoundsUVs)
				{
					obUVRectIfTilingSame = new DRect(mb_TexSet2.obUVoffset, mb_TexSet2.obUVscale);
				}
				else
				{
					obUVRectIfTilingSame = new DRect(0.0, 0.0, 1.0, 1.0);
				}
				for (int l = 0; l < mb_TexSet2.matsAndGOs.mats.Count; l++)
				{
					mb_TexSet2.matsAndGOs.mats[l].obUVRectIfTilingSame = obUVRectIfTilingSame;
					mb_TexSet2.matsAndGOs.mats[l].objName = distinctMaterialTextures[k].matsAndGOs.gos[0].name;
				}
				mb_TexSet2.CalcInitialFullSamplingRects(fixOutOfBoundsUVs);
				if (mb_TexSet2.allTexturesUseSameMatTiling)
				{
					mb_TexSet2.CalcMatAndUVSamplingRectsIfAllMatTilingSame();
				}
			}
			List<int> list = new List<int>();
			for (int m = 0; m < distinctMaterialTextures.Count; m++)
			{
				MB3_TextureCombiner.MB_TexSet mb_TexSet3 = distinctMaterialTextures[m];
				for (int n = m + 1; n < distinctMaterialTextures.Count; n++)
				{
					MB3_TextureCombiner.MB_TexSet mb_TexSet4 = distinctMaterialTextures[n];
					if (mb_TexSet4.AllTexturesAreSameForMerge(mb_TexSet3, this._considerNonTextureProperties, this.resultMaterialTextureBlender))
					{
						double num3 = 0.0;
						double num4 = 0.0;
						DRect encapsulatingSamplingRect = default(DRect);
						int num5 = -1;
						for (int num6 = 0; num6 < mb_TexSet3.ts.Length; num6++)
						{
							if (mb_TexSet3.ts[num6].t != null && num5 == -1)
							{
								num5 = num6;
							}
						}
						if (num5 != -1)
						{
							DRect drect = mb_TexSet4.matsAndGOs.mats[0].samplingRectMatAndUVTiling;
							for (int num7 = 1; num7 < mb_TexSet4.matsAndGOs.mats.Count; num7++)
							{
								drect = MB3_UVTransformUtility.GetEncapsulatingRect(ref drect, ref mb_TexSet4.matsAndGOs.mats[num7].samplingRectMatAndUVTiling);
							}
							DRect drect2 = mb_TexSet3.matsAndGOs.mats[0].samplingRectMatAndUVTiling;
							for (int num8 = 1; num8 < mb_TexSet3.matsAndGOs.mats.Count; num8++)
							{
								drect2 = MB3_UVTransformUtility.GetEncapsulatingRect(ref drect2, ref mb_TexSet3.matsAndGOs.mats[num8].samplingRectMatAndUVTiling);
							}
							encapsulatingSamplingRect = MB3_UVTransformUtility.GetEncapsulatingRect(ref drect, ref drect2);
							num3 += encapsulatingSamplingRect.width * encapsulatingSamplingRect.height;
							num4 += drect.width * drect.height + drect2.width * drect2.height;
						}
						else
						{
							encapsulatingSamplingRect = new DRect(0f, 0f, 1f, 1f);
						}
						if (num3 < num4)
						{
							num++;
							StringBuilder stringBuilder = null;
							if (this.LOG_LEVEL >= MB2_LogLevel.info)
							{
								stringBuilder = new StringBuilder();
								stringBuilder.AppendFormat("About To Merge:\n   TextureSet1 {0}\n   TextureSet2 {1}\n", mb_TexSet4.GetDescription(), mb_TexSet3.GetDescription());
								if (this.LOG_LEVEL >= MB2_LogLevel.trace)
								{
									for (int num9 = 0; num9 < mb_TexSet4.matsAndGOs.mats.Count; num9++)
									{
										stringBuilder.AppendFormat("tx1 Mat {0} matAndMeshUVRect {1} fullSamplingRect {2}\n", mb_TexSet4.matsAndGOs.mats[num9].mat, mb_TexSet4.matsAndGOs.mats[num9].samplingRectMatAndUVTiling, mb_TexSet4.ts[0].encapsulatingSamplingRect);
									}
									for (int num10 = 0; num10 < mb_TexSet3.matsAndGOs.mats.Count; num10++)
									{
										stringBuilder.AppendFormat("tx2 Mat {0} matAndMeshUVRect {1} fullSamplingRect {2}\n", mb_TexSet3.matsAndGOs.mats[num10].mat, mb_TexSet3.matsAndGOs.mats[num10].samplingRectMatAndUVTiling, mb_TexSet3.ts[0].encapsulatingSamplingRect);
									}
								}
							}
							for (int num11 = 0; num11 < mb_TexSet3.matsAndGOs.gos.Count; num11++)
							{
								if (!mb_TexSet4.matsAndGOs.gos.Contains(mb_TexSet3.matsAndGOs.gos[num11]))
								{
									mb_TexSet4.matsAndGOs.gos.Add(mb_TexSet3.matsAndGOs.gos[num11]);
								}
							}
							for (int num12 = 0; num12 < mb_TexSet3.matsAndGOs.mats.Count; num12++)
							{
								mb_TexSet4.matsAndGOs.mats.Add(mb_TexSet3.matsAndGOs.mats[num12]);
							}
							mb_TexSet4.matsAndGOs.mats.Sort(new MB3_TextureCombiner.SamplingRectEnclosesComparer());
							for (int num13 = 0; num13 < mb_TexSet4.ts.Length; num13++)
							{
								mb_TexSet4.ts[num13].encapsulatingSamplingRect = encapsulatingSamplingRect;
							}
							if (!list.Contains(m))
							{
								list.Add(m);
							}
							if (this.LOG_LEVEL >= MB2_LogLevel.debug)
							{
								if (this.LOG_LEVEL >= MB2_LogLevel.trace)
								{
									stringBuilder.AppendFormat("=== After Merge TextureSet {0}\n", mb_TexSet4.GetDescription());
									for (int num14 = 0; num14 < mb_TexSet4.matsAndGOs.mats.Count; num14++)
									{
										stringBuilder.AppendFormat("tx1 Mat {0} matAndMeshUVRect {1} fullSamplingRect {2}\n", mb_TexSet4.matsAndGOs.mats[num14].mat, mb_TexSet4.matsAndGOs.mats[num14].samplingRectMatAndUVTiling, mb_TexSet4.ts[0].encapsulatingSamplingRect);
									}
									DRect encapsulatingSamplingRect2 = mb_TexSet4.ts[0].encapsulatingSamplingRect;
									MB3_UVTransformUtility.Canonicalize(ref encapsulatingSamplingRect2, 0.0, 0.0);
									for (int num15 = 0; num15 < mb_TexSet4.matsAndGOs.mats.Count; num15++)
									{
										DRect samplingRectMatAndUVTiling = mb_TexSet4.matsAndGOs.mats[num15].samplingRectMatAndUVTiling;
										MB3_UVTransformUtility.Canonicalize(ref samplingRectMatAndUVTiling, encapsulatingSamplingRect2.x, encapsulatingSamplingRect2.y);
										Rect rect = default(Rect);
										DRect obUVRectIfTilingSame2 = mb_TexSet4.matsAndGOs.mats[num15].obUVRectIfTilingSame;
										DRect materialTiling = mb_TexSet4.matsAndGOs.mats[num15].materialTiling;
										Rect rect2 = encapsulatingSamplingRect2.GetRect();
										rect = MB3_UVTransformUtility.CombineTransforms(ref obUVRectIfTilingSame2, ref materialTiling).GetRect();
										MB3_UVTransformUtility.Canonicalize(ref rect, (float)encapsulatingSamplingRect2.x, (float)encapsulatingSamplingRect2.y);
										if (!mb_TexSet4.ts[0].encapsulatingSamplingRect.Encloses(mb_TexSet4.matsAndGOs.mats[num15].samplingRectMatAndUVTiling))
										{
											stringBuilder.AppendFormat(string.Concat(new object[]
											{
												"mesh ",
												mb_TexSet4.matsAndGOs.mats[num15].objName,
												"\n uv=",
												obUVRectIfTilingSame2,
												"\n mat=",
												materialTiling.GetRect().ToString("f5"),
												"\n samplingRect=",
												mb_TexSet4.matsAndGOs.mats[num15].samplingRectMatAndUVTiling.GetRect().ToString("f4"),
												"\n samplingRectCannonical=",
												samplingRectMatAndUVTiling.GetRect().ToString("f4"),
												"\n potentialRect (cannonicalized)=",
												rect.ToString("f4"),
												"\n encapsulatingRect ",
												mb_TexSet4.ts[0].encapsulatingSamplingRect.GetRect().ToString("f4"),
												"\n encapsulatingRectCannonical=",
												rect2.ToString("f4"),
												"\n\n"
											}), new object[0]);
											stringBuilder.AppendFormat(string.Format("Integrity check failed. " + mb_TexSet4.matsAndGOs.mats[num15].objName + " Encapsulating rect cannonical failed to contain samplingRectMatAndUVTiling cannonical\n", new object[0]), new object[0]);
										}
									}
								}
								UnityEngine.Debug.Log(stringBuilder.ToString());
							}
							break;
						}
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							UnityEngine.Debug.Log(string.Format("Considered merging {0} and {1} but there was not enough overlap. It is more efficient to bake these to separate rectangles.", mb_TexSet4.GetDescription(), mb_TexSet3.GetDescription()));
						}
					}
				}
			}
			for (int num16 = list.Count - 1; num16 >= 0; num16--)
			{
				distinctMaterialTextures.RemoveAt(list[num16]);
			}
			list.Clear();
			if (this.LOG_LEVEL >= MB2_LogLevel.info)
			{
				UnityEngine.Debug.Log(string.Format("MergeOverlappingDistinctMaterialTexturesAndCalcMaterialSubrects complete merged {0}", num));
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00013550 File Offset: 0x00011950
		private Vector2 GetAdjustedForScaleAndOffset2Dimensions(MB3_TextureCombiner.MeshBakerMaterialTexture source, Vector2 obUVoffset, Vector2 obUVscale)
		{
			if (source.matTilingRect.x == 0.0 && source.matTilingRect.y == 0.0 && source.matTilingRect.width == 1.0 && source.matTilingRect.height == 1.0)
			{
				if (!this._fixOutOfBoundsUVs)
				{
					return new Vector2((float)source.t.width, (float)source.t.height);
				}
				if (obUVoffset.x == 0f && obUVoffset.y == 0f && obUVscale.x == 1f && obUVscale.y == 1f)
				{
					return new Vector2((float)source.t.width, (float)source.t.height);
				}
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"GetAdjustedForScaleAndOffset2Dimensions: ",
					source.t,
					" ",
					obUVoffset,
					" ",
					obUVscale
				}));
			}
			float num = (float)source.encapsulatingSamplingRect.width * (float)source.t.width;
			float num2 = (float)source.encapsulatingSamplingRect.height * (float)source.t.height;
			if (num > (float)this._maxTilingBakeSize)
			{
				num = (float)this._maxTilingBakeSize;
			}
			if (num2 > (float)this._maxTilingBakeSize)
			{
				num2 = (float)this._maxTilingBakeSize;
			}
			if (num < 1f)
			{
				num = 1f;
			}
			if (num2 < 1f)
			{
				num2 = 1f;
			}
			return new Vector2(num, num2);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00013728 File Offset: 0x00011B28
		public Texture2D GetAdjustedForScaleAndOffset2(MB3_TextureCombiner.MeshBakerMaterialTexture source, Vector2 obUVoffset, Vector2 obUVscale)
		{
			if (source.matTilingRect.x == 0.0 && source.matTilingRect.y == 0.0 && source.matTilingRect.width == 1.0 && source.matTilingRect.height == 1.0)
			{
				if (!this._fixOutOfBoundsUVs)
				{
					return source.t;
				}
				if (obUVoffset.x == 0f && obUVoffset.y == 0f && obUVscale.x == 1f && obUVscale.y == 1f)
				{
					return source.t;
				}
			}
			Vector2 adjustedForScaleAndOffset2Dimensions = this.GetAdjustedForScaleAndOffset2Dimensions(source, obUVoffset, obUVscale);
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.LogWarning(string.Concat(new object[]
				{
					"GetAdjustedForScaleAndOffset2: ",
					source.t,
					" ",
					obUVoffset,
					" ",
					obUVscale
				}));
			}
			float x = adjustedForScaleAndOffset2Dimensions.x;
			float y = adjustedForScaleAndOffset2Dimensions.y;
			float num = (float)source.matTilingRect.width;
			float num2 = (float)source.matTilingRect.height;
			float num3 = (float)source.matTilingRect.x;
			float num4 = (float)source.matTilingRect.y;
			if (this._fixOutOfBoundsUVs)
			{
				num *= obUVscale.x;
				num2 *= obUVscale.y;
				num3 = (float)(source.matTilingRect.x * (double)obUVscale.x + (double)obUVoffset.x);
				num4 = (float)(source.matTilingRect.y * (double)obUVscale.y + (double)obUVoffset.y);
			}
			Texture2D texture2D = this._createTemporaryTexture((int)x, (int)y, TextureFormat.ARGB32, true);
			for (int i = 0; i < texture2D.width; i++)
			{
				for (int j = 0; j < texture2D.height; j++)
				{
					float u = (float)i / x * num + num3;
					float v = (float)j / y * num2 + num4;
					texture2D.SetPixel(i, j, source.t.GetPixelBilinear(u, v));
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00013980 File Offset: 0x00011D80
		internal static DRect GetSourceSamplingRect(MB3_TextureCombiner.MeshBakerMaterialTexture source, Vector2 obUVoffset, Vector2 obUVscale)
		{
			DRect matTilingRect = source.matTilingRect;
			DRect drect = new DRect(obUVoffset, obUVscale);
			return MB3_UVTransformUtility.CombineTransforms(ref matTilingRect, ref drect);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000139A8 File Offset: 0x00011DA8
		private Texture2D TintTextureWithTextureCombiner(Texture2D t, MB3_TextureCombiner.MB_TexSet sourceMaterial, ShaderTextureProperty shaderPropertyName)
		{
			if (this.LOG_LEVEL >= MB2_LogLevel.trace)
			{
				UnityEngine.Debug.Log(string.Format("Blending texture {0} mat {1} with non-texture properties using TextureBlender {2}", t.name, sourceMaterial.matsAndGOs.mats[0].mat, this.resultMaterialTextureBlender));
			}
			this.resultMaterialTextureBlender.OnBeforeTintTexture(sourceMaterial.matsAndGOs.mats[0].mat, shaderPropertyName.name);
			t = this._createTextureCopy(t);
			for (int i = 0; i < t.height; i++)
			{
				Color[] pixels = t.GetPixels(0, i, t.width, 1);
				for (int j = 0; j < pixels.Length; j++)
				{
					pixels[j] = this.resultMaterialTextureBlender.OnBlendTexturePixel(shaderPropertyName.name, pixels[j]);
				}
				t.SetPixels(0, i, t.width, 1, pixels);
			}
			t.Apply();
			return t;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00013AA0 File Offset: 0x00011EA0
		public IEnumerator CopyScaledAndTiledToAtlas(MB3_TextureCombiner.MeshBakerMaterialTexture source, MB3_TextureCombiner.MB_TexSet sourceMaterial, ShaderTextureProperty shaderPropertyName, DRect srcSamplingRect, int targX, int targY, int targW, int targH, bool _fixOutOfBoundsUVs, int maxSize, Color[][] atlasPixels, int atlasWidth, bool isNormalMap, ProgressUpdateDelegate progressInfo = null)
		{
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"CopyScaledAndTiledToAtlas: ",
					source.t,
					" inAtlasX=",
					targX,
					" inAtlasY=",
					targY,
					" inAtlasW=",
					targW,
					" inAtlasH=",
					targH
				}));
			}
			float newWidth = (float)targW;
			float newHeight = (float)targH;
			float scx = (float)srcSamplingRect.width;
			float scy = (float)srcSamplingRect.height;
			float ox = (float)srcSamplingRect.x;
			float oy = (float)srcSamplingRect.y;
			int w = (int)newWidth;
			int h = (int)newHeight;
			Texture2D t = source.t;
			if (t == null)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.trace)
				{
					UnityEngine.Debug.Log("No source texture creating a 16x16 texture.");
				}
				t = this._createTemporaryTexture(16, 16, TextureFormat.ARGB32, true);
				scx = 1f;
				scy = 1f;
				if (this._considerNonTextureProperties && this.resultMaterialTextureBlender != null)
				{
					Color colorIfNoTexture = this.resultMaterialTextureBlender.GetColorIfNoTexture(sourceMaterial.matsAndGOs.mats[0].mat, shaderPropertyName);
					if (this.LOG_LEVEL >= MB2_LogLevel.trace)
					{
						UnityEngine.Debug.Log("Setting texture to solid color " + colorIfNoTexture);
					}
					MB_Utility.setSolidColor(t, colorIfNoTexture);
				}
				else
				{
					Color colorIfNoTexture2 = MB3_TextureCombiner.GetColorIfNoTexture(shaderPropertyName);
					MB_Utility.setSolidColor(t, colorIfNoTexture2);
				}
			}
			if (this._considerNonTextureProperties && this.resultMaterialTextureBlender != null)
			{
				t = this.TintTextureWithTextureCombiner(t, sourceMaterial, shaderPropertyName);
			}
			t = this._addWatermark(t);
			for (int k = 0; k < w; k++)
			{
				if (progressInfo != null && w > 0)
				{
					progressInfo("CopyScaledAndTiledToAtlas " + ((float)k / (float)w * 100f).ToString("F0"), 0.2f);
				}
				for (int l = 0; l < h; l++)
				{
					float u = (float)k / newWidth * scx + ox;
					float v = (float)l / newHeight * scy + oy;
					atlasPixels[targY + l][targX + k] = t.GetPixelBilinear(u, v);
				}
			}
			for (int m = 0; m < w; m++)
			{
				for (int n = 1; n <= this.atlasPadding; n++)
				{
					atlasPixels[targY - n][targX + m] = atlasPixels[targY][targX + m];
					atlasPixels[targY + h - 1 + n][targX + m] = atlasPixels[targY + h - 1][targX + m];
				}
			}
			for (int num = 0; num < h; num++)
			{
				for (int num2 = 1; num2 <= this._atlasPadding; num2++)
				{
					atlasPixels[targY + num][targX - num2] = atlasPixels[targY + num][targX];
					atlasPixels[targY + num][targX + w + num2 - 1] = atlasPixels[targY + num][targX + w - 1];
				}
			}
			for (int i = 1; i <= this._atlasPadding; i++)
			{
				for (int j = 1; j <= this._atlasPadding; j++)
				{
					atlasPixels[targY - j][targX - i] = atlasPixels[targY][targX];
					atlasPixels[targY + h - 1 + j][targX - i] = atlasPixels[targY + h - 1][targX];
					atlasPixels[targY + h - 1 + j][targX + w + i - 1] = atlasPixels[targY + h - 1][targX + w - 1];
					atlasPixels[targY - j][targX + w + i - 1] = atlasPixels[targY][targX + w - 1];
					yield return null;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00013B08 File Offset: 0x00011F08
		public Texture2D _createTemporaryTexture(int w, int h, TextureFormat texFormat, bool mipMaps)
		{
			Texture2D texture2D = new Texture2D(w, h, texFormat, mipMaps);
			MB_Utility.setSolidColor(texture2D, Color.clear);
			this._temporaryTextures.Add(texture2D);
			return texture2D;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00013B38 File Offset: 0x00011F38
		internal Texture2D _createTextureCopy(Texture2D t)
		{
			Texture2D texture2D = MB_Utility.createTextureCopy(t);
			this._temporaryTextures.Add(texture2D);
			return texture2D;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00013B5C File Offset: 0x00011F5C
		private Texture2D _resizeTexture(Texture2D t, int w, int h)
		{
			Texture2D texture2D = MB_Utility.resampleTexture(t, w, h);
			this._temporaryTextures.Add(texture2D);
			return texture2D;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00013B80 File Offset: 0x00011F80
		private void _destroyTemporaryTextures()
		{
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log("Destroying " + this._temporaryTextures.Count + " temporary textures");
			}
			for (int i = 0; i < this._temporaryTextures.Count; i++)
			{
				MB_Utility.Destroy(this._temporaryTextures[i]);
			}
			this._temporaryTextures.Clear();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00013BF8 File Offset: 0x00011FF8
		public void SuggestTreatment(List<GameObject> objsToMesh, Material[] resultMaterials, List<ShaderTextureProperty> _customShaderPropNames)
		{
			this._customShaderPropNames = _customShaderPropNames;
			StringBuilder stringBuilder = new StringBuilder();
			Dictionary<int, MB_Utility.MeshAnalysisResult[]> dictionary = new Dictionary<int, MB_Utility.MeshAnalysisResult[]>();
			for (int i = 0; i < objsToMesh.Count; i++)
			{
				GameObject gameObject = objsToMesh[i];
				if (!(gameObject == null))
				{
					Material[] gomaterials = MB_Utility.GetGOMaterials(objsToMesh[i]);
					if (gomaterials.Length > 1)
					{
						stringBuilder.AppendFormat("\nObject {0} uses {1} materials. Possible treatments:\n", objsToMesh[i].name, gomaterials.Length);
						stringBuilder.AppendFormat("  1) Collapse the submeshes together into one submesh in the combined mesh. Each of the original submesh materials will map to a different UV rectangle in the atlas(es) used by the combined material.\n", new object[0]);
						stringBuilder.AppendFormat("  2) Use the multiple materials feature to map submeshes in the source mesh to submeshes in the combined mesh.\n", new object[0]);
					}
					Mesh mesh = MB_Utility.GetMesh(gameObject);
					MB_Utility.MeshAnalysisResult[] array;
					if (!dictionary.TryGetValue(mesh.GetInstanceID(), out array))
					{
						array = new MB_Utility.MeshAnalysisResult[mesh.subMeshCount];
						MB_Utility.doSubmeshesShareVertsOrTris(mesh, ref array[0]);
						for (int j = 0; j < mesh.subMeshCount; j++)
						{
							MB_Utility.hasOutOfBoundsUVs(mesh, ref array[j], j, 0);
							array[j].hasOverlappingSubmeshTris = array[0].hasOverlappingSubmeshTris;
							array[j].hasOverlappingSubmeshVerts = array[0].hasOverlappingSubmeshVerts;
						}
						dictionary.Add(mesh.GetInstanceID(), array);
					}
					for (int k = 0; k < gomaterials.Length; k++)
					{
						if (array[k].hasOutOfBoundsUVs)
						{
							DRect drect = new DRect(array[k].uvRect);
							stringBuilder.AppendFormat("\nObject {0} submesh={1} material={2} uses UVs outside the range 0,0 .. 1,1 to create tiling that tiles the box {3},{4} .. {5},{6}. This is a problem because the UVs outside the 0,0 .. 1,1 rectangle will pick up neighboring textures in the atlas. Possible Treatments:\n", new object[]
							{
								gameObject,
								k,
								gomaterials[k],
								drect.x.ToString("G4"),
								drect.y.ToString("G4"),
								(drect.x + drect.width).ToString("G4"),
								(drect.y + drect.height).ToString("G4")
							});
							stringBuilder.AppendFormat("    1) Ignore the problem. The tiling may not affect result significantly.\n", new object[0]);
							stringBuilder.AppendFormat("    2) Use the 'fix out of bounds UVs' feature to bake the tiling and scale the UVs to fit in the 0,0 .. 1,1 rectangle.\n", new object[0]);
							stringBuilder.AppendFormat("    3) Use the Multiple Materials feature to map the material on this submesh to its own submesh in the combined mesh. No other materials should map to this submesh. This will result in only one texture in the atlas(es) and the UVs should tile correctly.\n", new object[0]);
							stringBuilder.AppendFormat("    4) Combine only meshes that use the same (or subset of) the set of materials on this mesh. The original material(s) can be applied to the result\n", new object[0]);
						}
					}
					if (array[0].hasOverlappingSubmeshVerts)
					{
						stringBuilder.AppendFormat("\nObject {0} has submeshes that share vertices. This is a problem because each vertex can have only one UV coordinate and may be required to map to different positions in the various atlases that are generated. Possible treatments:\n", objsToMesh[i]);
						stringBuilder.AppendFormat(" 1) Ignore the problem. The vertices may not affect the result.\n", new object[0]);
						stringBuilder.AppendFormat(" 2) Use the Multiple Materials feature to map the submeshs that overlap to their own submeshs in the combined mesh. No other materials should map to this submesh. This will result in only one texture in the atlas(es) and the UVs should tile correctly.\n", new object[0]);
						stringBuilder.AppendFormat(" 3) Combine only meshes that use the same (or subset of) the set of materials on this mesh. The original material(s) can be applied to the result\n", new object[0]);
					}
				}
			}
			Dictionary<Material, List<GameObject>> dictionary2 = new Dictionary<Material, List<GameObject>>();
			for (int l = 0; l < objsToMesh.Count; l++)
			{
				if (objsToMesh[l] != null)
				{
					Material[] gomaterials2 = MB_Utility.GetGOMaterials(objsToMesh[l]);
					for (int m = 0; m < gomaterials2.Length; m++)
					{
						if (gomaterials2[m] != null)
						{
							List<GameObject> list;
							if (!dictionary2.TryGetValue(gomaterials2[m], out list))
							{
								list = new List<GameObject>();
								dictionary2.Add(gomaterials2[m], list);
							}
							if (!list.Contains(objsToMesh[l]))
							{
								list.Add(objsToMesh[l]);
							}
						}
					}
				}
			}
			List<ShaderTextureProperty> list2 = new List<ShaderTextureProperty>();
			for (int n = 0; n < resultMaterials.Length; n++)
			{
				this._CollectPropertyNames(resultMaterials[n], list2);
				foreach (Material material in dictionary2.Keys)
				{
					for (int num = 0; num < list2.Count; num++)
					{
						if (material.HasProperty(list2[num].name))
						{
							Texture texture = material.GetTexture(list2[num].name);
							if (texture != null)
							{
								Vector2 textureOffset = material.GetTextureOffset(list2[num].name);
								Vector3 vector = material.GetTextureScale(list2[num].name);
								if (textureOffset.x < 0f || textureOffset.x + vector.x > 1f || textureOffset.y < 0f || textureOffset.y + vector.y > 1f)
								{
									stringBuilder.AppendFormat("\nMaterial {0} used by objects {1} uses texture {2} that is tiled (scale={3} offset={4}). If there is more than one texture in the atlas  then Mesh Baker will bake the tiling into the atlas. If the baked tiling is large then quality can be lost. Possible treatments:\n", new object[]
									{
										material,
										this.PrintList(dictionary2[material]),
										texture,
										vector,
										textureOffset
									});
									stringBuilder.AppendFormat("  1) Use the baked tiling.\n", new object[0]);
									stringBuilder.AppendFormat("  2) Use the Multiple Materials feature to map the material on this object/submesh to its own submesh in the combined mesh. No other materials should map to this submesh. The original material can be applied to this submesh.\n", new object[0]);
									stringBuilder.AppendFormat("  3) Combine only meshes that use the same (or subset of) the set of textures on this mesh. The original material can be applied to the result.\n", new object[0]);
								}
							}
						}
					}
				}
			}
			string message = string.Empty;
			if (stringBuilder.Length == 0)
			{
				message = "====== No problems detected. These meshes should combine well ====\n  If there are problems with the combined meshes please report the problem to digitalOpus.ca so we can improve Mesh Baker.";
			}
			else
			{
				message = "====== There are possible problems with these meshes that may prevent them from combining well. TREATMENT SUGGESTIONS (copy and paste to text editor if too big) =====\n" + stringBuilder.ToString();
			}
			UnityEngine.Debug.Log(message);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00014194 File Offset: 0x00012594
		private TextureBlender FindMatchingTextureBlender(string shaderName)
		{
			for (int i = 0; i < this.textureBlenders.Length; i++)
			{
				if (this.textureBlenders[i].DoesShaderNameMatch(shaderName))
				{
					return this.textureBlenders[i];
				}
			}
			return null;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000141D8 File Offset: 0x000125D8
		private void AdjustNonTextureProperties(Material mat, List<ShaderTextureProperty> texPropertyNames, List<MB3_TextureCombiner.MB_TexSet> distinctMaterialTextures, bool considerTintColor, MB2_EditorMethodsInterface editorMethods)
		{
			if (mat == null || texPropertyNames == null)
			{
				return;
			}
			if (this._considerNonTextureProperties)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					UnityEngine.Debug.Log("Adjusting non texture properties using TextureBlender for shader: " + mat.shader.name);
				}
				this.resultMaterialTextureBlender.SetNonTexturePropertyValuesOnResultMaterial(mat);
				return;
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				UnityEngine.Debug.Log("Adjusting non texture properties on result material");
			}
			for (int i = 0; i < texPropertyNames.Count; i++)
			{
				string name = texPropertyNames[i].name;
				if (name.Equals("_MainTex") && mat.HasProperty("_Color"))
				{
					try
					{
						if (considerTintColor)
						{
							mat.SetColor("_Color", Color.white);
						}
					}
					catch (Exception)
					{
					}
				}
				if (name.Equals("_BumpMap") && mat.HasProperty("_BumpScale"))
				{
					try
					{
						mat.SetFloat("_BumpScale", 1f);
					}
					catch (Exception)
					{
					}
				}
				if (name.Equals("_ParallaxMap") && mat.HasProperty("_Parallax"))
				{
					try
					{
						mat.SetFloat("_Parallax", 0.02f);
					}
					catch (Exception)
					{
					}
				}
				if (name.Equals("_OcclusionMap") && mat.HasProperty("_OcclusionStrength"))
				{
					try
					{
						mat.SetFloat("_OcclusionStrength", 1f);
					}
					catch (Exception)
					{
					}
				}
				if (name.Equals("_EmissionMap"))
				{
					if (mat.HasProperty("_EmissionColor"))
					{
						try
						{
							mat.SetColor("_EmissionColor", new Color(0f, 0f, 0f, 0f));
						}
						catch (Exception)
						{
						}
					}
					if (mat.HasProperty("_EmissionScaleUI"))
					{
						try
						{
							mat.SetFloat("_EmissionScaleUI", 1f);
						}
						catch (Exception)
						{
						}
					}
				}
			}
			if (editorMethods != null)
			{
				editorMethods.CommitChangesToAssets();
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0001443C File Offset: 0x0001283C
		public static Color GetColorIfNoTexture(ShaderTextureProperty texProperty)
		{
			if (texProperty.isNormalMap)
			{
				return new Color(0.5f, 0.5f, 1f);
			}
			if (texProperty.name.Equals("_MetallicGlossMap"))
			{
				return new Color(0f, 0f, 0f, 1f);
			}
			if (texProperty.name.Equals("_ParallaxMap"))
			{
				return new Color(0f, 0f, 0f, 0f);
			}
			if (texProperty.name.Equals("_OcclusionMap"))
			{
				return new Color(1f, 1f, 1f, 1f);
			}
			if (texProperty.name.Equals("_EmissionMap"))
			{
				return new Color(0f, 0f, 0f, 0f);
			}
			if (texProperty.name.Equals("_DetailMask"))
			{
				return new Color(0f, 0f, 0f, 0f);
			}
			return new Color(1f, 1f, 1f, 0f);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00014570 File Offset: 0x00012970
		private Color32 ConvertNormalFormatFromUnity_ToStandard(Color32 c)
		{
			Vector3 zero = Vector3.zero;
			zero.x = (float)c.a * 2f - 1f;
			zero.y = (float)c.g * 2f - 1f;
			zero.z = Mathf.Sqrt(1f - zero.x * zero.x - zero.y * zero.y);
			return new Color32
			{
				a = 1,
				r = (byte)((zero.x + 1f) * 0.5f),
				g = (byte)((zero.y + 1f) * 0.5f),
				b = (byte)((zero.z + 1f) * 0.5f)
			};
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0001464C File Offset: 0x00012A4C
		private float GetSubmeshArea(Mesh m, int submeshIdx)
		{
			if (submeshIdx >= m.subMeshCount || submeshIdx < 0)
			{
				return 0f;
			}
			Vector3[] vertices = m.vertices;
			int[] indices = m.GetIndices(submeshIdx);
			float num = 0f;
			for (int i = 0; i < indices.Length; i += 3)
			{
				Vector3 b = vertices[indices[i]];
				Vector3 a = vertices[indices[i + 1]];
				Vector3 a2 = vertices[indices[i + 2]];
				num += Vector3.Cross(a - b, a2 - b).magnitude / 2f;
			}
			return num;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000146FC File Offset: 0x00012AFC
		private string PrintList(List<GameObject> gos)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < gos.Count; i++)
			{
				stringBuilder.Append(gos[i] + ",");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000135 RID: 309
		public MB2_LogLevel LOG_LEVEL = MB2_LogLevel.info;

		// Token: 0x04000136 RID: 310
		public static ShaderTextureProperty[] shaderTexPropertyNames = new ShaderTextureProperty[]
		{
			new ShaderTextureProperty("_MainTex", false),
			new ShaderTextureProperty("_BumpMap", true),
			new ShaderTextureProperty("_Normal", true),
			new ShaderTextureProperty("_BumpSpecMap", false),
			new ShaderTextureProperty("_DecalTex", false),
			new ShaderTextureProperty("_Detail", false),
			new ShaderTextureProperty("_GlossMap", false),
			new ShaderTextureProperty("_Illum", false),
			new ShaderTextureProperty("_LightTextureB0", false),
			new ShaderTextureProperty("_ParallaxMap", false),
			new ShaderTextureProperty("_ShadowOffset", false),
			new ShaderTextureProperty("_TranslucencyMap", false),
			new ShaderTextureProperty("_SpecMap", false),
			new ShaderTextureProperty("_SpecGlossMap", false),
			new ShaderTextureProperty("_TranspMap", false),
			new ShaderTextureProperty("_MetallicGlossMap", false),
			new ShaderTextureProperty("_OcclusionMap", false),
			new ShaderTextureProperty("_EmissionMap", false),
			new ShaderTextureProperty("_DetailMask", false)
		};

		// Token: 0x04000137 RID: 311
		[SerializeField]
		protected MB2_TextureBakeResults _textureBakeResults;

		// Token: 0x04000138 RID: 312
		[SerializeField]
		protected int _atlasPadding = 1;

		// Token: 0x04000139 RID: 313
		[SerializeField]
		protected int _maxAtlasSize = 1;

		// Token: 0x0400013A RID: 314
		[SerializeField]
		protected bool _resizePowerOfTwoTextures;

		// Token: 0x0400013B RID: 315
		[SerializeField]
		protected bool _fixOutOfBoundsUVs;

		// Token: 0x0400013C RID: 316
		[SerializeField]
		protected int _maxTilingBakeSize = 1024;

		// Token: 0x0400013D RID: 317
		[SerializeField]
		protected bool _saveAtlasesAsAssets;

		// Token: 0x0400013E RID: 318
		[SerializeField]
		protected MB2_PackingAlgorithmEnum _packingAlgorithm;

		// Token: 0x0400013F RID: 319
		[SerializeField]
		protected bool _meshBakerTexturePackerForcePowerOfTwo = true;

		// Token: 0x04000140 RID: 320
		[SerializeField]
		protected List<ShaderTextureProperty> _customShaderPropNames = new List<ShaderTextureProperty>();

		// Token: 0x04000141 RID: 321
		[SerializeField]
		protected bool _normalizeTexelDensity;

		// Token: 0x04000142 RID: 322
		[SerializeField]
		protected bool _considerNonTextureProperties;

		// Token: 0x04000143 RID: 323
		protected TextureBlender resultMaterialTextureBlender;

		// Token: 0x04000144 RID: 324
		protected TextureBlender[] textureBlenders = new TextureBlender[0];

		// Token: 0x04000145 RID: 325
		protected List<Texture2D> _temporaryTextures = new List<Texture2D>();

		// Token: 0x04000146 RID: 326
		public static bool _RunCorutineWithoutPauseIsRunning = false;

		// Token: 0x04000147 RID: 327
		private int __step2_CalculateIdealSizesForTexturesInAtlasAndPadding;

		// Token: 0x04000148 RID: 328
		private Rect[] __createAtlasesMBTexturePacker;

		// Token: 0x02000044 RID: 68
		public class MeshBakerMaterialTexture
		{
			// Token: 0x060001FA RID: 506 RVA: 0x00014877 File Offset: 0x00012C77
			public MeshBakerMaterialTexture()
			{
			}

			// Token: 0x060001FB RID: 507 RVA: 0x0001487F File Offset: 0x00012C7F
			public MeshBakerMaterialTexture(Texture2D tx)
			{
				this.t = tx;
			}

			// Token: 0x060001FC RID: 508 RVA: 0x0001488E File Offset: 0x00012C8E
			public MeshBakerMaterialTexture(Texture2D tx, Vector2 o, Vector2 s, float texelDens)
			{
				this.t = tx;
				this.matTilingRect = new DRect(o, s);
				this.texelDensity = texelDens;
			}

			// Token: 0x04000149 RID: 329
			public Texture2D t;

			// Token: 0x0400014A RID: 330
			public float texelDensity;

			// Token: 0x0400014B RID: 331
			public DRect encapsulatingSamplingRect;

			// Token: 0x0400014C RID: 332
			public DRect matTilingRect;
		}

		// Token: 0x02000045 RID: 69
		public class MatAndTransformToMerged
		{
			// Token: 0x060001FD RID: 509 RVA: 0x000148B4 File Offset: 0x00012CB4
			public MatAndTransformToMerged(Material m)
			{
				this.mat = m;
			}

			// Token: 0x060001FE RID: 510 RVA: 0x0001490C File Offset: 0x00012D0C
			public override bool Equals(object obj)
			{
				if (obj is MB3_TextureCombiner.MatAndTransformToMerged)
				{
					MB3_TextureCombiner.MatAndTransformToMerged matAndTransformToMerged = (MB3_TextureCombiner.MatAndTransformToMerged)obj;
					if (matAndTransformToMerged.mat == this.mat && matAndTransformToMerged.obUVRectIfTilingSame == this.obUVRectIfTilingSame)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060001FF RID: 511 RVA: 0x0001495A File Offset: 0x00012D5A
			public override int GetHashCode()
			{
				return this.mat.GetHashCode() ^ this.obUVRectIfTilingSame.GetHashCode() ^ this.samplingRectMatAndUVTiling.GetHashCode();
			}

			// Token: 0x0400014D RID: 333
			public Material mat;

			// Token: 0x0400014E RID: 334
			public DRect obUVRectIfTilingSame = new DRect(0f, 0f, 1f, 1f);

			// Token: 0x0400014F RID: 335
			public DRect samplingRectMatAndUVTiling = default(DRect);

			// Token: 0x04000150 RID: 336
			public DRect materialTiling = default(DRect);

			// Token: 0x04000151 RID: 337
			public string objName;
		}

		// Token: 0x02000046 RID: 70
		public class SamplingRectEnclosesComparer : IComparer<MB3_TextureCombiner.MatAndTransformToMerged>
		{
			// Token: 0x06000201 RID: 513 RVA: 0x00014993 File Offset: 0x00012D93
			public int Compare(MB3_TextureCombiner.MatAndTransformToMerged x, MB3_TextureCombiner.MatAndTransformToMerged y)
			{
				if (x.samplingRectMatAndUVTiling.Equals(y.samplingRectMatAndUVTiling))
				{
					return 0;
				}
				if (x.samplingRectMatAndUVTiling.Encloses(y.samplingRectMatAndUVTiling))
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x02000047 RID: 71
		public class MatsAndGOs
		{
			// Token: 0x04000152 RID: 338
			public List<MB3_TextureCombiner.MatAndTransformToMerged> mats;

			// Token: 0x04000153 RID: 339
			public List<GameObject> gos;
		}

		// Token: 0x02000048 RID: 72
		public class MB_TexSet
		{
			// Token: 0x06000203 RID: 515 RVA: 0x000149DC File Offset: 0x00012DDC
			public MB_TexSet(MB3_TextureCombiner.MeshBakerMaterialTexture[] tss, Vector2 uvOffset, Vector2 uvScale)
			{
				this.ts = tss;
				this.obUVoffset = uvOffset;
				this.obUVscale = uvScale;
				this.allTexturesUseSameMatTiling = false;
				this.matsAndGOs = new MB3_TextureCombiner.MatsAndGOs();
				this.matsAndGOs.mats = new List<MB3_TextureCombiner.MatAndTransformToMerged>();
				this.matsAndGOs.gos = new List<GameObject>();
			}

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x06000204 RID: 516 RVA: 0x00014A60 File Offset: 0x00012E60
			public DRect obUVrect
			{
				get
				{
					return new DRect(this.obUVoffset, this.obUVscale);
				}
			}

			// Token: 0x06000205 RID: 517 RVA: 0x00014A74 File Offset: 0x00012E74
			public bool IsEqual(object obj, bool fixOutOfBoundsUVs, bool considerNonTextureProperties, TextureBlender resultMaterialTextureBlender)
			{
				if (!(obj is MB3_TextureCombiner.MB_TexSet))
				{
					return false;
				}
				MB3_TextureCombiner.MB_TexSet mb_TexSet = (MB3_TextureCombiner.MB_TexSet)obj;
				if (mb_TexSet.ts.Length != this.ts.Length)
				{
					return false;
				}
				for (int i = 0; i < this.ts.Length; i++)
				{
					if (this.ts[i].matTilingRect != mb_TexSet.ts[i].matTilingRect)
					{
						return false;
					}
					if (this.ts[i].t != mb_TexSet.ts[i].t)
					{
						return false;
					}
					if (considerNonTextureProperties && resultMaterialTextureBlender != null && !resultMaterialTextureBlender.NonTexturePropertiesAreEqual(this.matsAndGOs.mats[0].mat, mb_TexSet.matsAndGOs.mats[0].mat))
					{
						return false;
					}
				}
				return (!fixOutOfBoundsUVs || (this.obUVoffset.x == mb_TexSet.obUVoffset.x && this.obUVoffset.y == mb_TexSet.obUVoffset.y)) && (!fixOutOfBoundsUVs || (this.obUVscale.x == mb_TexSet.obUVscale.x && this.obUVscale.y == mb_TexSet.obUVscale.y));
			}

			// Token: 0x06000206 RID: 518 RVA: 0x00014BD8 File Offset: 0x00012FD8
			public void CalcInitialFullSamplingRects(bool fixOutOfBoundsUVs)
			{
				DRect encapsulatingSamplingRect = new DRect(0f, 0f, 1f, 1f);
				for (int i = 0; i < this.ts.Length; i++)
				{
					if (this.ts[i].t != null)
					{
						DRect matTilingRect = this.ts[i].matTilingRect;
						DRect obUVrect;
						if (fixOutOfBoundsUVs)
						{
							obUVrect = this.obUVrect;
						}
						else
						{
							obUVrect = new DRect(0.0, 0.0, 1.0, 1.0);
						}
						this.ts[i].encapsulatingSamplingRect = MB3_UVTransformUtility.CombineTransforms(ref obUVrect, ref matTilingRect);
						encapsulatingSamplingRect = this.ts[i].encapsulatingSamplingRect;
					}
				}
				for (int j = 0; j < this.ts.Length; j++)
				{
					if (this.ts[j].t == null)
					{
						this.ts[j].encapsulatingSamplingRect = encapsulatingSamplingRect;
					}
				}
			}

			// Token: 0x06000207 RID: 519 RVA: 0x00014CE8 File Offset: 0x000130E8
			public void CalcMatAndUVSamplingRectsIfAllMatTilingSame()
			{
				if (!this.allTexturesUseSameMatTiling)
				{
					UnityEngine.Debug.LogError("All textures must use same material tiling to calc full sampling rects");
				}
				DRect matTilingRect = new DRect(0f, 0f, 1f, 1f);
				for (int i = 0; i < this.ts.Length; i++)
				{
					if (this.ts[i].t != null)
					{
						matTilingRect = this.ts[i].matTilingRect;
					}
				}
				for (int j = 0; j < this.matsAndGOs.mats.Count; j++)
				{
					this.matsAndGOs.mats[j].materialTiling = matTilingRect;
					this.matsAndGOs.mats[j].samplingRectMatAndUVTiling = MB3_UVTransformUtility.CombineTransforms(ref this.matsAndGOs.mats[j].obUVRectIfTilingSame, ref matTilingRect);
				}
			}

			// Token: 0x06000208 RID: 520 RVA: 0x00014DD0 File Offset: 0x000131D0
			public bool AllTexturesAreSameForMerge(MB3_TextureCombiner.MB_TexSet other, bool considerNonTextureProperties, TextureBlender resultMaterialTextureBlender)
			{
				if (other.ts.Length != this.ts.Length)
				{
					return false;
				}
				if (!other.allTexturesUseSameMatTiling || !this.allTexturesUseSameMatTiling)
				{
					return false;
				}
				int num = -1;
				for (int i = 0; i < this.ts.Length; i++)
				{
					if (this.ts[i].t != other.ts[i].t)
					{
						return false;
					}
					if (num == -1 && this.ts[i].t != null)
					{
						num = i;
					}
					if (considerNonTextureProperties && resultMaterialTextureBlender != null && !resultMaterialTextureBlender.NonTexturePropertiesAreEqual(this.matsAndGOs.mats[0].mat, other.matsAndGOs.mats[0].mat))
					{
						return false;
					}
				}
				if (num != -1)
				{
					for (int j = 0; j < this.ts.Length; j++)
					{
						if (this.ts[j].t != other.ts[j].t)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x06000209 RID: 521 RVA: 0x00014EFC File Offset: 0x000132FC
			internal string GetDescription()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[GAME_OBJS=", new object[0]);
				for (int i = 0; i < this.matsAndGOs.gos.Count; i++)
				{
					stringBuilder.AppendFormat("{0},", this.matsAndGOs.gos[i].name);
				}
				stringBuilder.AppendFormat("MATS=", new object[0]);
				for (int j = 0; j < this.matsAndGOs.mats.Count; j++)
				{
					stringBuilder.AppendFormat("{0},", this.matsAndGOs.mats[j].mat.name);
				}
				stringBuilder.Append("]");
				return stringBuilder.ToString();
			}

			// Token: 0x0600020A RID: 522 RVA: 0x00014FD0 File Offset: 0x000133D0
			internal string GetMatSubrectDescriptions()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.matsAndGOs.mats.Count; i++)
				{
					stringBuilder.AppendFormat("\n    {0}={1},", this.matsAndGOs.mats[i].mat.name, this.matsAndGOs.mats[i].samplingRectMatAndUVTiling);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x04000154 RID: 340
			public MB3_TextureCombiner.MeshBakerMaterialTexture[] ts;

			// Token: 0x04000155 RID: 341
			public MB3_TextureCombiner.MatsAndGOs matsAndGOs;

			// Token: 0x04000156 RID: 342
			public bool allTexturesUseSameMatTiling;

			// Token: 0x04000157 RID: 343
			public Vector2 obUVoffset = new Vector2(0f, 0f);

			// Token: 0x04000158 RID: 344
			public Vector2 obUVscale = new Vector2(1f, 1f);

			// Token: 0x04000159 RID: 345
			public int idealWidth;

			// Token: 0x0400015A RID: 346
			public int idealHeight;
		}

		// Token: 0x02000049 RID: 73
		public class CombineTexturesIntoAtlasesCoroutineResult
		{
			// Token: 0x0400015B RID: 347
			public bool success = true;

			// Token: 0x0400015C RID: 348
			public bool isFinished;
		}
	}
}
