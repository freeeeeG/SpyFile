using System;
using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x02000059 RID: 89
public abstract class MB3_MeshBakerCommon : MB3_MeshBakerRoot
{
	// Token: 0x1700002E RID: 46
	// (get) Token: 0x0600025E RID: 606
	public abstract MB3_MeshCombiner meshCombiner { get; }

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x0600025F RID: 607 RVA: 0x0001A6B2 File Offset: 0x00018AB2
	// (set) Token: 0x06000260 RID: 608 RVA: 0x0001A6BF File Offset: 0x00018ABF
	public override MB2_TextureBakeResults textureBakeResults
	{
		get
		{
			return this.meshCombiner.textureBakeResults;
		}
		set
		{
			this.meshCombiner.textureBakeResults = value;
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0001A6D0 File Offset: 0x00018AD0
	public override List<GameObject> GetObjectsToCombine()
	{
		if (!this.useObjsToMeshFromTexBaker)
		{
			if (this.objsToMesh == null)
			{
				this.objsToMesh = new List<GameObject>();
			}
			return this.objsToMesh;
		}
		MB3_TextureBaker component = base.gameObject.GetComponent<MB3_TextureBaker>();
		if (component == null)
		{
			component = base.gameObject.transform.parent.GetComponent<MB3_TextureBaker>();
		}
		if (component != null)
		{
			return component.GetObjectsToCombine();
		}
		Debug.LogWarning("Use Objects To Mesh From Texture Baker was checked but no texture baker");
		return new List<GameObject>();
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0001A758 File Offset: 0x00018B58
	public void EnableDisableSourceObjectRenderers(bool show)
	{
		for (int i = 0; i < this.GetObjectsToCombine().Count; i++)
		{
			GameObject gameObject = this.GetObjectsToCombine()[i];
			if (gameObject != null)
			{
				Renderer renderer = MB_Utility.GetRenderer(gameObject);
				if (renderer != null)
				{
					renderer.enabled = show;
				}
				if (renderer == null)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Object: ",
						gameObject.name,
						" at index ",
						i,
						" has no renderer."
					}));
				}
				else
				{
					LODGroup componentInParent = renderer.GetComponentInParent<LODGroup>();
					if (componentInParent != null)
					{
						bool flag = true;
						LOD[] lods = componentInParent.GetLODs();
						for (int j = 0; j < lods.Length; j++)
						{
							for (int k = 0; k < lods[j].renderers.Length; k++)
							{
								if (lods[j].renderers[k] != renderer)
								{
									flag = false;
									break;
								}
							}
						}
						if (flag)
						{
							componentInParent.enabled = show;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0001A88D File Offset: 0x00018C8D
	public virtual void ClearMesh()
	{
		this.meshCombiner.ClearMesh();
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0001A89A File Offset: 0x00018C9A
	public virtual void DestroyMesh()
	{
		this.meshCombiner.DestroyMesh();
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0001A8A7 File Offset: 0x00018CA7
	public virtual void DestroyMeshEditor(MB2_EditorMethodsInterface editorMethods)
	{
		this.meshCombiner.DestroyMeshEditor(editorMethods);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0001A8B5 File Offset: 0x00018CB5
	public virtual int GetNumObjectsInCombined()
	{
		return this.meshCombiner.GetNumObjectsInCombined();
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0001A8C2 File Offset: 0x00018CC2
	public virtual int GetNumVerticesFor(GameObject go)
	{
		return this.meshCombiner.GetNumVerticesFor(go);
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0001A8D0 File Offset: 0x00018CD0
	public MB3_TextureBaker GetTextureBaker()
	{
		MB3_TextureBaker component = base.GetComponent<MB3_TextureBaker>();
		if (component != null)
		{
			return component;
		}
		if (base.transform.parent != null)
		{
			return base.transform.parent.GetComponent<MB3_TextureBaker>();
		}
		return null;
	}

	// Token: 0x06000269 RID: 617
	public abstract bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource = true);

	// Token: 0x0600026A RID: 618
	public abstract bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource = true);

	// Token: 0x0600026B RID: 619 RVA: 0x0001A91A File Offset: 0x00018D1A
	public virtual void Apply(MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod = null)
	{
		this.meshCombiner.name = base.name + "-mesh";
		this.meshCombiner.Apply(uv2GenerationMethod);
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0001A944 File Offset: 0x00018D44
	public virtual void Apply(bool triangles, bool vertices, bool normals, bool tangents, bool uvs, bool uv2, bool uv3, bool uv4, bool colors, bool bones = false, bool blendShapesFlag = false, MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod = null)
	{
		this.meshCombiner.name = base.name + "-mesh";
		this.meshCombiner.Apply(triangles, vertices, normals, tangents, uvs, uv2, uv3, uv4, colors, bones, blendShapesFlag, uv2GenerationMethod);
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0001A98C File Offset: 0x00018D8C
	public virtual bool CombinedMeshContains(GameObject go)
	{
		return this.meshCombiner.CombinedMeshContains(go);
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0001A99C File Offset: 0x00018D9C
	public virtual void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true, bool updateVertices = true, bool updateNormals = true, bool updateTangents = true, bool updateUV = false, bool updateUV1 = false, bool updateUV2 = false, bool updateColors = false, bool updateSkinningInfo = false)
	{
		this.meshCombiner.name = base.name + "-mesh";
		this.meshCombiner.UpdateGameObjects(gos, recalcBounds, updateVertices, updateNormals, updateTangents, updateUV, updateUV1, updateUV2, updateColors, updateSkinningInfo, false);
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0001A9E1 File Offset: 0x00018DE1
	public virtual void UpdateSkinnedMeshApproximateBounds()
	{
		if (this._ValidateForUpdateSkinnedMeshBounds())
		{
			this.meshCombiner.UpdateSkinnedMeshApproximateBounds();
		}
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0001A9F9 File Offset: 0x00018DF9
	public virtual void UpdateSkinnedMeshApproximateBoundsFromBones()
	{
		if (this._ValidateForUpdateSkinnedMeshBounds())
		{
			this.meshCombiner.UpdateSkinnedMeshApproximateBoundsFromBones();
		}
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0001AA11 File Offset: 0x00018E11
	public virtual void UpdateSkinnedMeshApproximateBoundsFromBounds()
	{
		if (this._ValidateForUpdateSkinnedMeshBounds())
		{
			this.meshCombiner.UpdateSkinnedMeshApproximateBoundsFromBounds();
		}
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0001AA2C File Offset: 0x00018E2C
	protected virtual bool _ValidateForUpdateSkinnedMeshBounds()
	{
		if (this.meshCombiner.outputOption == MB2_OutputOptions.bakeMeshAssetsInPlace)
		{
			Debug.LogWarning("Can't UpdateSkinnedMeshApproximateBounds when output type is bakeMeshAssetsInPlace");
			return false;
		}
		if (this.meshCombiner.resultSceneObject == null)
		{
			Debug.LogWarning("Result Scene Object does not exist. No point in calling UpdateSkinnedMeshApproximateBounds.");
			return false;
		}
		SkinnedMeshRenderer componentInChildren = this.meshCombiner.resultSceneObject.GetComponentInChildren<SkinnedMeshRenderer>();
		if (componentInChildren == null)
		{
			Debug.LogWarning("No SkinnedMeshRenderer on result scene object.");
			return false;
		}
		return true;
	}

	// Token: 0x04000188 RID: 392
	public List<GameObject> objsToMesh;

	// Token: 0x04000189 RID: 393
	public bool useObjsToMeshFromTexBaker = true;

	// Token: 0x0400018A RID: 394
	public bool clearBuffersAfterBake = true;

	// Token: 0x0400018B RID: 395
	public string bakeAssetsInPlaceFolderPath;

	// Token: 0x0400018C RID: 396
	[HideInInspector]
	public GameObject resultPrefab;
}
