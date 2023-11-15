using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class MB3_TextureBaker : MB3_MeshBakerRoot
{
	// Token: 0x17000032 RID: 50
	// (get) Token: 0x0600029F RID: 671 RVA: 0x0001C578 File Offset: 0x0001A978
	// (set) Token: 0x060002A0 RID: 672 RVA: 0x0001C580 File Offset: 0x0001A980
	public override MB2_TextureBakeResults textureBakeResults
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

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060002A1 RID: 673 RVA: 0x0001C589 File Offset: 0x0001A989
	// (set) Token: 0x060002A2 RID: 674 RVA: 0x0001C591 File Offset: 0x0001A991
	public virtual int atlasPadding
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

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060002A3 RID: 675 RVA: 0x0001C59A File Offset: 0x0001A99A
	// (set) Token: 0x060002A4 RID: 676 RVA: 0x0001C5A2 File Offset: 0x0001A9A2
	public virtual int maxAtlasSize
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

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060002A5 RID: 677 RVA: 0x0001C5AB File Offset: 0x0001A9AB
	// (set) Token: 0x060002A6 RID: 678 RVA: 0x0001C5B3 File Offset: 0x0001A9B3
	public virtual bool resizePowerOfTwoTextures
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

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060002A7 RID: 679 RVA: 0x0001C5BC File Offset: 0x0001A9BC
	// (set) Token: 0x060002A8 RID: 680 RVA: 0x0001C5C4 File Offset: 0x0001A9C4
	public virtual bool fixOutOfBoundsUVs
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

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060002A9 RID: 681 RVA: 0x0001C5CD File Offset: 0x0001A9CD
	// (set) Token: 0x060002AA RID: 682 RVA: 0x0001C5D5 File Offset: 0x0001A9D5
	public virtual int maxTilingBakeSize
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

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x060002AB RID: 683 RVA: 0x0001C5DE File Offset: 0x0001A9DE
	// (set) Token: 0x060002AC RID: 684 RVA: 0x0001C5E6 File Offset: 0x0001A9E6
	public virtual MB2_PackingAlgorithmEnum packingAlgorithm
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

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x060002AD RID: 685 RVA: 0x0001C5EF File Offset: 0x0001A9EF
	// (set) Token: 0x060002AE RID: 686 RVA: 0x0001C5F7 File Offset: 0x0001A9F7
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

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060002AF RID: 687 RVA: 0x0001C600 File Offset: 0x0001AA00
	// (set) Token: 0x060002B0 RID: 688 RVA: 0x0001C608 File Offset: 0x0001AA08
	public virtual List<ShaderTextureProperty> customShaderProperties
	{
		get
		{
			return this._customShaderProperties;
		}
		set
		{
			this._customShaderProperties = value;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060002B1 RID: 689 RVA: 0x0001C611 File Offset: 0x0001AA11
	// (set) Token: 0x060002B2 RID: 690 RVA: 0x0001C619 File Offset: 0x0001AA19
	public virtual List<string> customShaderPropNames
	{
		get
		{
			return this._customShaderPropNames_Depricated;
		}
		set
		{
			this._customShaderPropNames_Depricated = value;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060002B3 RID: 691 RVA: 0x0001C622 File Offset: 0x0001AA22
	// (set) Token: 0x060002B4 RID: 692 RVA: 0x0001C62A File Offset: 0x0001AA2A
	public virtual bool doMultiMaterial
	{
		get
		{
			return this._doMultiMaterial;
		}
		set
		{
			this._doMultiMaterial = value;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060002B5 RID: 693 RVA: 0x0001C633 File Offset: 0x0001AA33
	// (set) Token: 0x060002B6 RID: 694 RVA: 0x0001C63B File Offset: 0x0001AA3B
	public virtual bool doMultiMaterialSplitAtlasesIfTooBig
	{
		get
		{
			return this._doMultiMaterialSplitAtlasesIfTooBig;
		}
		set
		{
			this._doMultiMaterialSplitAtlasesIfTooBig = value;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060002B7 RID: 695 RVA: 0x0001C644 File Offset: 0x0001AA44
	// (set) Token: 0x060002B8 RID: 696 RVA: 0x0001C64C File Offset: 0x0001AA4C
	public virtual bool doMultiMaterialSplitAtlasesIfOBUVs
	{
		get
		{
			return this._doMultiMaterialSplitAtlasesIfOBUVs;
		}
		set
		{
			this._doMultiMaterialSplitAtlasesIfOBUVs = value;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060002B9 RID: 697 RVA: 0x0001C655 File Offset: 0x0001AA55
	// (set) Token: 0x060002BA RID: 698 RVA: 0x0001C65D File Offset: 0x0001AA5D
	public virtual Material resultMaterial
	{
		get
		{
			return this._resultMaterial;
		}
		set
		{
			this._resultMaterial = value;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060002BB RID: 699 RVA: 0x0001C666 File Offset: 0x0001AA66
	// (set) Token: 0x060002BC RID: 700 RVA: 0x0001C66E File Offset: 0x0001AA6E
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

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060002BD RID: 701 RVA: 0x0001C677 File Offset: 0x0001AA77
	// (set) Token: 0x060002BE RID: 702 RVA: 0x0001C67F File Offset: 0x0001AA7F
	public bool doSuggestTreatment
	{
		get
		{
			return this._doSuggestTreatment;
		}
		set
		{
			this._doSuggestTreatment = value;
		}
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0001C688 File Offset: 0x0001AA88
	public override List<GameObject> GetObjectsToCombine()
	{
		if (this.objsToMesh == null)
		{
			this.objsToMesh = new List<GameObject>();
		}
		return this.objsToMesh;
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x0001C6A6 File Offset: 0x0001AAA6
	public MB_AtlasesAndRects[] CreateAtlases()
	{
		return this.CreateAtlases(null, false, null);
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0001C6B4 File Offset: 0x0001AAB4
	public IEnumerator CreateAtlasesCoroutine(ProgressUpdateDelegate progressInfo, MB3_TextureBaker.CreateAtlasesCoroutineResult coroutineResult, bool saveAtlasesAsAssets = false, MB2_EditorMethodsInterface editorMethods = null, float maxTimePerFrame = 0.01f)
	{
		MBVersionConcrete mbv = new MBVersionConcrete();
		if (!MB3_TextureCombiner._RunCorutineWithoutPauseIsRunning && (mbv.GetMajorVersion() < 5 || (mbv.GetMajorVersion() == 5 && mbv.GetMinorVersion() < 3)))
		{
			Debug.LogError("Running the texture combiner as a coroutine only works in Unity 5.3 and higher");
			coroutineResult.success = false;
			yield break;
		}
		this.OnCombinedTexturesCoroutineAtlasesAndRects = null;
		if (maxTimePerFrame <= 0f)
		{
			Debug.LogError("maxTimePerFrame must be a value greater than zero");
			coroutineResult.isFinished = true;
			yield break;
		}
		MB2_ValidationLevel vl = (!Application.isPlaying) ? MB2_ValidationLevel.robust : MB2_ValidationLevel.quick;
		if (!MB3_MeshBakerRoot.DoCombinedValidate(this, MB_ObjsToCombineTypes.dontCare, null, vl))
		{
			coroutineResult.isFinished = true;
			yield break;
		}
		if (this._doMultiMaterial && !this._ValidateResultMaterials())
		{
			coroutineResult.isFinished = true;
			yield break;
		}
		if (!this._doMultiMaterial)
		{
			if (this._resultMaterial == null)
			{
				Debug.LogError("Combined Material is null please create and assign a result material.");
				coroutineResult.isFinished = true;
				yield break;
			}
			Shader shader = this._resultMaterial.shader;
			for (int j = 0; j < this.objsToMesh.Count; j++)
			{
				foreach (Material material in MB_Utility.GetGOMaterials(this.objsToMesh[j]))
				{
					if (material != null && material.shader != shader)
					{
						Debug.LogWarning(string.Concat(new object[]
						{
							"Game object ",
							this.objsToMesh[j],
							" does not use shader ",
							shader,
							" it may not have the required textures. If not small solid color textures will be generated."
						}));
					}
				}
			}
		}
		MB3_TextureCombiner combiner = this.CreateAndConfigureTextureCombiner();
		combiner.saveAtlasesAsAssets = saveAtlasesAsAssets;
		int numResults = 1;
		if (this._doMultiMaterial)
		{
			numResults = this.resultMaterials.Length;
		}
		this.OnCombinedTexturesCoroutineAtlasesAndRects = new MB_AtlasesAndRects[numResults];
		for (int l = 0; l < this.OnCombinedTexturesCoroutineAtlasesAndRects.Length; l++)
		{
			this.OnCombinedTexturesCoroutineAtlasesAndRects[l] = new MB_AtlasesAndRects();
		}
		for (int i = 0; i < this.OnCombinedTexturesCoroutineAtlasesAndRects.Length; i++)
		{
			Material resMatToPass = null;
			List<Material> sourceMats = null;
			if (this._doMultiMaterial)
			{
				sourceMats = this.resultMaterials[i].sourceMaterials;
				resMatToPass = this.resultMaterials[i].combinedMaterial;
				combiner.fixOutOfBoundsUVs = this.resultMaterials[i].considerMeshUVs;
			}
			else
			{
				resMatToPass = this._resultMaterial;
			}
			Debug.Log(string.Format("Creating atlases for result material {0} using shader {1}", resMatToPass, resMatToPass.shader));
			MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult coroutineResult2 = new MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult();
			yield return combiner.CombineTexturesIntoAtlasesCoroutine(progressInfo, this.OnCombinedTexturesCoroutineAtlasesAndRects[i], resMatToPass, this.objsToMesh, sourceMats, editorMethods, coroutineResult2, maxTimePerFrame, null, false);
			coroutineResult.success = coroutineResult2.success;
			if (!coroutineResult.success)
			{
				coroutineResult.isFinished = true;
				yield break;
			}
		}
		this.unpackMat2RectMap(this.textureBakeResults);
		this.textureBakeResults.doMultiMaterial = this._doMultiMaterial;
		if (this._doMultiMaterial)
		{
			this.textureBakeResults.resultMaterials = this.resultMaterials;
		}
		else
		{
			MB_MultiMaterial[] array = new MB_MultiMaterial[]
			{
				new MB_MultiMaterial()
			};
			array[0].combinedMaterial = this._resultMaterial;
			array[0].considerMeshUVs = this._fixOutOfBoundsUVs;
			array[0].sourceMaterials = new List<Material>();
			array[0].sourceMaterials.AddRange(this.textureBakeResults.materials);
			this.textureBakeResults.resultMaterials = array;
		}
		MB3_MeshBakerCommon[] mb = base.GetComponentsInChildren<MB3_MeshBakerCommon>();
		for (int m = 0; m < mb.Length; m++)
		{
			mb[m].textureBakeResults = this.textureBakeResults;
		}
		if (this.LOG_LEVEL >= MB2_LogLevel.info)
		{
			Debug.Log("Created Atlases");
		}
		coroutineResult.isFinished = true;
		if (coroutineResult.success && this.onBuiltAtlasesSuccess != null)
		{
			this.onBuiltAtlasesSuccess();
		}
		if (!coroutineResult.success && this.onBuiltAtlasesFail != null)
		{
			this.onBuiltAtlasesFail();
		}
		yield break;
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0001C6F4 File Offset: 0x0001AAF4
	public MB_AtlasesAndRects[] CreateAtlases(ProgressUpdateDelegate progressInfo, bool saveAtlasesAsAssets = false, MB2_EditorMethodsInterface editorMethods = null)
	{
		MB_AtlasesAndRects[] array = null;
		try
		{
			MB3_TextureBaker.CreateAtlasesCoroutineResult createAtlasesCoroutineResult = new MB3_TextureBaker.CreateAtlasesCoroutineResult();
			MB3_TextureCombiner.RunCorutineWithoutPause(this.CreateAtlasesCoroutine(progressInfo, createAtlasesCoroutineResult, saveAtlasesAsAssets, editorMethods, 1000f), 0);
			if (createAtlasesCoroutineResult.success && this.textureBakeResults != null)
			{
				array = this.OnCombinedTexturesCoroutineAtlasesAndRects;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		finally
		{
			if (saveAtlasesAsAssets && array != null)
			{
				foreach (MB_AtlasesAndRects mb_AtlasesAndRects in array)
				{
					if (mb_AtlasesAndRects != null && mb_AtlasesAndRects.atlases != null)
					{
						for (int j = 0; j < mb_AtlasesAndRects.atlases.Length; j++)
						{
							if (mb_AtlasesAndRects.atlases[j] != null)
							{
								if (editorMethods != null)
								{
									editorMethods.Destroy(mb_AtlasesAndRects.atlases[j]);
								}
								else
								{
									MB_Utility.Destroy(mb_AtlasesAndRects.atlases[j]);
								}
							}
						}
					}
				}
			}
		}
		return array;
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0001C808 File Offset: 0x0001AC08
	private void unpackMat2RectMap(MB2_TextureBakeResults tbr)
	{
		List<Material> list = new List<Material>();
		List<MB_MaterialAndUVRect> list2 = new List<MB_MaterialAndUVRect>();
		List<Rect> list3 = new List<Rect>();
		for (int i = 0; i < this.OnCombinedTexturesCoroutineAtlasesAndRects.Length; i++)
		{
			MB_AtlasesAndRects mb_AtlasesAndRects = this.OnCombinedTexturesCoroutineAtlasesAndRects[i];
			List<MB_MaterialAndUVRect> mat2rect_map = mb_AtlasesAndRects.mat2rect_map;
			if (mat2rect_map != null)
			{
				for (int j = 0; j < mat2rect_map.Count; j++)
				{
					list2.Add(mat2rect_map[j]);
					list.Add(mat2rect_map[j].material);
					list3.Add(mat2rect_map[j].atlasRect);
				}
			}
		}
		tbr.materials = list.ToArray();
		tbr.materialsAndUVRects = list2.ToArray();
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x0001C8C8 File Offset: 0x0001ACC8
	public MB3_TextureCombiner CreateAndConfigureTextureCombiner()
	{
		return new MB3_TextureCombiner
		{
			LOG_LEVEL = this.LOG_LEVEL,
			atlasPadding = this._atlasPadding,
			maxAtlasSize = this._maxAtlasSize,
			customShaderPropNames = this._customShaderProperties,
			fixOutOfBoundsUVs = this._fixOutOfBoundsUVs,
			maxTilingBakeSize = this._maxTilingBakeSize,
			packingAlgorithm = this._packingAlgorithm,
			meshBakerTexturePackerForcePowerOfTwo = this._meshBakerTexturePackerForcePowerOfTwo,
			resizePowerOfTwoTextures = this._resizePowerOfTwoTextures,
			considerNonTextureProperties = this._considerNonTextureProperties
		};
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0001C954 File Offset: 0x0001AD54
	public static void ConfigureNewMaterialToMatchOld(Material newMat, Material original)
	{
		if (original == null)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Original material is null, could not copy properties to ",
				newMat,
				". Setting shader to ",
				newMat.shader
			}));
			return;
		}
		newMat.shader = original.shader;
		newMat.CopyPropertiesFromMaterial(original);
		ShaderTextureProperty[] shaderTexPropertyNames = MB3_TextureCombiner.shaderTexPropertyNames;
		for (int i = 0; i < shaderTexPropertyNames.Length; i++)
		{
			Vector2 one = Vector2.one;
			Vector2 zero = Vector2.zero;
			if (newMat.HasProperty(shaderTexPropertyNames[i].name))
			{
				newMat.SetTextureOffset(shaderTexPropertyNames[i].name, zero);
				newMat.SetTextureScale(shaderTexPropertyNames[i].name, one);
			}
		}
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x0001CA08 File Offset: 0x0001AE08
	private string PrintSet(HashSet<Material> s)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Material arg in s)
		{
			stringBuilder.Append(arg + ",");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0001CA78 File Offset: 0x0001AE78
	private bool _ValidateResultMaterials()
	{
		HashSet<Material> hashSet = new HashSet<Material>();
		for (int i = 0; i < this.objsToMesh.Count; i++)
		{
			if (this.objsToMesh[i] != null)
			{
				Material[] gomaterials = MB_Utility.GetGOMaterials(this.objsToMesh[i]);
				for (int j = 0; j < gomaterials.Length; j++)
				{
					if (gomaterials[j] != null)
					{
						hashSet.Add(gomaterials[j]);
					}
				}
			}
		}
		HashSet<Material> hashSet2 = new HashSet<Material>();
		for (int k = 0; k < this.resultMaterials.Length; k++)
		{
			MB_MultiMaterial mb_MultiMaterial = this.resultMaterials[k];
			if (mb_MultiMaterial.combinedMaterial == null)
			{
				Debug.LogError("Combined Material is null please create and assign a result material.");
				return false;
			}
			Shader shader = mb_MultiMaterial.combinedMaterial.shader;
			for (int l = 0; l < mb_MultiMaterial.sourceMaterials.Count; l++)
			{
				if (mb_MultiMaterial.sourceMaterials[l] == null)
				{
					Debug.LogError("There are null entries in the list of Source Materials");
					return false;
				}
				if (shader != mb_MultiMaterial.sourceMaterials[l].shader)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"Source material ",
						mb_MultiMaterial.sourceMaterials[l],
						" does not use shader ",
						shader,
						" it may not have the required textures. If not empty textures will be generated."
					}));
				}
				if (hashSet2.Contains(mb_MultiMaterial.sourceMaterials[l]))
				{
					Debug.LogError("A Material " + mb_MultiMaterial.sourceMaterials[l] + " appears more than once in the list of source materials in the source material to combined mapping. Each source material must be unique.");
					return false;
				}
				hashSet2.Add(mb_MultiMaterial.sourceMaterials[l]);
			}
		}
		if (hashSet.IsProperSubsetOf(hashSet2))
		{
			hashSet2.ExceptWith(hashSet);
			Debug.LogWarning("There are materials in the mapping that are not used on your source objects: " + this.PrintSet(hashSet2));
		}
		if (this.resultMaterials != null && this.resultMaterials.Length > 0 && hashSet2.IsProperSubsetOf(hashSet))
		{
			hashSet.ExceptWith(hashSet2);
			Debug.LogError("There are materials on the objects to combine that are not in the mapping: " + this.PrintSet(hashSet));
			return false;
		}
		return true;
	}

	// Token: 0x040001AF RID: 431
	public MB2_LogLevel LOG_LEVEL = MB2_LogLevel.info;

	// Token: 0x040001B0 RID: 432
	[SerializeField]
	protected MB2_TextureBakeResults _textureBakeResults;

	// Token: 0x040001B1 RID: 433
	[SerializeField]
	protected int _atlasPadding = 1;

	// Token: 0x040001B2 RID: 434
	[SerializeField]
	protected int _maxAtlasSize = 4096;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	protected bool _resizePowerOfTwoTextures;

	// Token: 0x040001B4 RID: 436
	[SerializeField]
	protected bool _fixOutOfBoundsUVs;

	// Token: 0x040001B5 RID: 437
	[SerializeField]
	protected int _maxTilingBakeSize = 1024;

	// Token: 0x040001B6 RID: 438
	[SerializeField]
	protected MB2_PackingAlgorithmEnum _packingAlgorithm = MB2_PackingAlgorithmEnum.MeshBakerTexturePacker;

	// Token: 0x040001B7 RID: 439
	[SerializeField]
	protected bool _meshBakerTexturePackerForcePowerOfTwo = true;

	// Token: 0x040001B8 RID: 440
	[SerializeField]
	protected List<ShaderTextureProperty> _customShaderProperties = new List<ShaderTextureProperty>();

	// Token: 0x040001B9 RID: 441
	[SerializeField]
	protected List<string> _customShaderPropNames_Depricated = new List<string>();

	// Token: 0x040001BA RID: 442
	[SerializeField]
	protected bool _doMultiMaterial;

	// Token: 0x040001BB RID: 443
	[SerializeField]
	protected bool _doMultiMaterialSplitAtlasesIfTooBig = true;

	// Token: 0x040001BC RID: 444
	[SerializeField]
	protected bool _doMultiMaterialSplitAtlasesIfOBUVs = true;

	// Token: 0x040001BD RID: 445
	[SerializeField]
	protected Material _resultMaterial;

	// Token: 0x040001BE RID: 446
	[SerializeField]
	protected bool _considerNonTextureProperties;

	// Token: 0x040001BF RID: 447
	[SerializeField]
	protected bool _doSuggestTreatment = true;

	// Token: 0x040001C0 RID: 448
	public MB_MultiMaterial[] resultMaterials = new MB_MultiMaterial[0];

	// Token: 0x040001C1 RID: 449
	public List<GameObject> objsToMesh;

	// Token: 0x040001C2 RID: 450
	public MB3_TextureBaker.OnCombinedTexturesCoroutineSuccess onBuiltAtlasesSuccess;

	// Token: 0x040001C3 RID: 451
	public MB3_TextureBaker.OnCombinedTexturesCoroutineFail onBuiltAtlasesFail;

	// Token: 0x040001C4 RID: 452
	public MB_AtlasesAndRects[] OnCombinedTexturesCoroutineAtlasesAndRects;

	// Token: 0x02000069 RID: 105
	// (Invoke) Token: 0x060002C9 RID: 713
	public delegate void OnCombinedTexturesCoroutineSuccess();

	// Token: 0x0200006A RID: 106
	// (Invoke) Token: 0x060002CD RID: 717
	public delegate void OnCombinedTexturesCoroutineFail();

	// Token: 0x0200006B RID: 107
	public class CreateAtlasesCoroutineResult
	{
		// Token: 0x040001C5 RID: 453
		public bool success = true;

		// Token: 0x040001C6 RID: 454
		public bool isFinished;
	}
}
