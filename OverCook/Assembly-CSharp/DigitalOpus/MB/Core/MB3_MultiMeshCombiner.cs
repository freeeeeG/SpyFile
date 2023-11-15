﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public class MB3_MultiMeshCombiner : MB3_MeshCombiner
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000F6D6 File Offset: 0x0000DAD6
		// (set) Token: 0x0600017B RID: 379 RVA: 0x0000F6E0 File Offset: 0x0000DAE0
		public override MB2_LogLevel LOG_LEVEL
		{
			get
			{
				return this._LOG_LEVEL;
			}
			set
			{
				this._LOG_LEVEL = value;
				for (int i = 0; i < this.meshCombiners.Count; i++)
				{
					this.meshCombiners[i].combinedMesh.LOG_LEVEL = value;
				}
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000F774 File Offset: 0x0000DB74
		// (set) Token: 0x0600017C RID: 380 RVA: 0x0000F728 File Offset: 0x0000DB28
		public override MB2_ValidationLevel validationLevel
		{
			get
			{
				return this._validationLevel;
			}
			set
			{
				this._validationLevel = value;
				for (int i = 0; i < this.meshCombiners.Count; i++)
				{
					this.meshCombiners[i].combinedMesh.validationLevel = this._validationLevel;
				}
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000F77C File Offset: 0x0000DB7C
		// (set) Token: 0x0600017F RID: 383 RVA: 0x0000F784 File Offset: 0x0000DB84
		public int maxVertsInMesh
		{
			get
			{
				return this._maxVertsInMesh;
			}
			set
			{
				if (this.obj2MeshCombinerMap.Count > 0)
				{
					return;
				}
				if (value < 3)
				{
					Debug.LogError("Max verts in mesh must be greater than three.");
				}
				else if (value > 65535)
				{
					Debug.LogError("Meshes in unity cannot have more than 65535 vertices.");
				}
				else
				{
					this._maxVertsInMesh = value;
				}
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000F7DA File Offset: 0x0000DBDA
		public override int GetNumObjectsInCombined()
		{
			return this.obj2MeshCombinerMap.Count;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000F7E8 File Offset: 0x0000DBE8
		public override int GetNumVerticesFor(GameObject go)
		{
			MB3_MultiMeshCombiner.CombinedMesh combinedMesh = null;
			if (this.obj2MeshCombinerMap.TryGetValue(go.GetInstanceID(), out combinedMesh))
			{
				return combinedMesh.combinedMesh.GetNumVerticesFor(go);
			}
			return -1;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000F820 File Offset: 0x0000DC20
		public override int GetNumVerticesFor(int gameObjectID)
		{
			MB3_MultiMeshCombiner.CombinedMesh combinedMesh = null;
			if (this.obj2MeshCombinerMap.TryGetValue(gameObjectID, out combinedMesh))
			{
				return combinedMesh.combinedMesh.GetNumVerticesFor(gameObjectID);
			}
			return -1;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000F850 File Offset: 0x0000DC50
		public override List<GameObject> GetObjectsInCombined()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				list.AddRange(this.meshCombiners[i].combinedMesh.GetObjectsInCombined());
			}
			return list;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000F89C File Offset: 0x0000DC9C
		public override int GetLightmapIndex()
		{
			if (this.meshCombiners.Count > 0)
			{
				return this.meshCombiners[0].combinedMesh.GetLightmapIndex();
			}
			return -1;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000F8C7 File Offset: 0x0000DCC7
		public override bool CombinedMeshContains(GameObject go)
		{
			return this.obj2MeshCombinerMap.ContainsKey(go.GetInstanceID());
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000F8DC File Offset: 0x0000DCDC
		private bool _validateTextureBakeResults()
		{
			if (this._textureBakeResults == null)
			{
				Debug.LogError("Texture Bake Results is null. Can't combine meshes.");
				return false;
			}
			if (this._textureBakeResults.materialsAndUVRects == null || this._textureBakeResults.materialsAndUVRects.Length == 0)
			{
				Debug.LogError("Texture Bake Results has no materials in material to sourceUVRect map. Try baking materials. Can't combine meshes.");
				return false;
			}
			if (this._textureBakeResults.resultMaterials == null || this._textureBakeResults.resultMaterials.Length == 0)
			{
				if (this._textureBakeResults.materialsAndUVRects == null || this._textureBakeResults.materialsAndUVRects.Length <= 0 || this._textureBakeResults.doMultiMaterial || !(this._textureBakeResults.resultMaterial != null))
				{
					Debug.LogError("Texture Bake Results has no result materials. Try baking materials. Can't combine meshes.");
					return false;
				}
				MB_MultiMaterial[] array = this._textureBakeResults.resultMaterials = new MB_MultiMaterial[1];
				array[0] = new MB_MultiMaterial();
				array[0].combinedMaterial = this._textureBakeResults.resultMaterial;
				array[0].considerMeshUVs = this._textureBakeResults.fixOutOfBoundsUVs;
				List<Material> list = array[0].sourceMaterials = new List<Material>();
				for (int i = 0; i < this._textureBakeResults.materialsAndUVRects.Length; i++)
				{
					if (!list.Contains(this._textureBakeResults.materialsAndUVRects[i].material))
					{
						list.Add(this._textureBakeResults.materialsAndUVRects[i].material);
					}
				}
			}
			return true;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000FA60 File Offset: 0x0000DE60
		public override void Apply(MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod)
		{
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				if (this.meshCombiners[i].isDirty)
				{
					this.meshCombiners[i].combinedMesh.Apply(uv2GenerationMethod);
					this.meshCombiners[i].isDirty = false;
				}
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000FAC8 File Offset: 0x0000DEC8
		public override void Apply(bool triangles, bool vertices, bool normals, bool tangents, bool uvs, bool uv2, bool uv3, bool uv4, bool colors, bool bones = false, bool blendShapesFlag = false, MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod = null)
		{
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				if (this.meshCombiners[i].isDirty)
				{
					this.meshCombiners[i].combinedMesh.Apply(triangles, vertices, normals, tangents, uvs, uv2, uv3, uv4, colors, bones, blendShapesFlag, uv2GenerationMethod);
					this.meshCombiners[i].isDirty = false;
				}
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000FB44 File Offset: 0x0000DF44
		public override void UpdateSkinnedMeshApproximateBounds()
		{
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				this.meshCombiners[i].combinedMesh.UpdateSkinnedMeshApproximateBounds();
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000FB84 File Offset: 0x0000DF84
		public override void UpdateSkinnedMeshApproximateBoundsFromBones()
		{
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				this.meshCombiners[i].combinedMesh.UpdateSkinnedMeshApproximateBoundsFromBones();
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000FBC4 File Offset: 0x0000DFC4
		public override void UpdateSkinnedMeshApproximateBoundsFromBounds()
		{
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				this.meshCombiners[i].combinedMesh.UpdateSkinnedMeshApproximateBoundsFromBounds();
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000FC04 File Offset: 0x0000E004
		public override void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true, bool updateVertices = true, bool updateNormals = true, bool updateTangents = true, bool updateUV = false, bool updateUV2 = false, bool updateUV3 = false, bool updateUV4 = false, bool updateColors = false, bool updateSkinningInfo = false)
		{
			if (gos == null)
			{
				Debug.LogError("list of game objects cannot be null");
				return;
			}
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				this.meshCombiners[i].gosToUpdate.Clear();
			}
			for (int j = 0; j < gos.Length; j++)
			{
				MB3_MultiMeshCombiner.CombinedMesh combinedMesh = null;
				this.obj2MeshCombinerMap.TryGetValue(gos[j].GetInstanceID(), out combinedMesh);
				if (combinedMesh != null)
				{
					combinedMesh.gosToUpdate.Add(gos[j]);
				}
				else
				{
					Debug.LogWarning("Object " + gos[j] + " is not in the combined mesh.");
				}
			}
			for (int k = 0; k < this.meshCombiners.Count; k++)
			{
				if (this.meshCombiners[k].gosToUpdate.Count > 0)
				{
					this.meshCombiners[k].isDirty = true;
					GameObject[] gos2 = this.meshCombiners[k].gosToUpdate.ToArray();
					this.meshCombiners[k].combinedMesh.UpdateGameObjects(gos2, recalcBounds, updateVertices, updateNormals, updateTangents, updateUV, updateUV2, updateUV3, updateUV4, updateColors, updateSkinningInfo);
				}
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000FD3C File Offset: 0x0000E13C
		public override bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource = true)
		{
			int[] array = null;
			if (deleteGOs != null)
			{
				array = new int[deleteGOs.Length];
				for (int i = 0; i < deleteGOs.Length; i++)
				{
					if (deleteGOs[i] == null)
					{
						Debug.LogError("The " + i + "th object on the list of objects to delete is 'Null'");
					}
					else
					{
						array[i] = deleteGOs[i].GetInstanceID();
					}
				}
			}
			return this.AddDeleteGameObjectsByID(gos, array, disableRendererInSource);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000FDB0 File Offset: 0x0000E1B0
		public override bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource = true)
		{
			if (this._usingTemporaryTextureBakeResult && gos != null && gos.Length > 0)
			{
				MB_Utility.Destroy(this._textureBakeResults);
				this._textureBakeResults = null;
				this._usingTemporaryTextureBakeResult = false;
			}
			if (this._textureBakeResults == null && gos != null && gos.Length > 0 && gos[0] != null && !this._CreateTemporaryTextrueBakeResult(gos, this.GetMaterialsOnTargetRenderer()))
			{
				return false;
			}
			if (!this._validate(gos, deleteGOinstanceIDs))
			{
				return false;
			}
			this._distributeAmongBakers(gos, deleteGOinstanceIDs);
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				MB2_Log.LogDebug(string.Concat(new object[]
				{
					"MB2_MultiMeshCombiner.AddDeleteGameObjects numCombinedMeshes: ",
					this.meshCombiners.Count,
					" added:",
					gos,
					" deleted:",
					deleteGOinstanceIDs,
					" disableRendererInSource:",
					disableRendererInSource,
					" maxVertsPerCombined:",
					this._maxVertsInMesh
				}), new object[0]);
			}
			return this._bakeStep1(gos, deleteGOinstanceIDs, disableRendererInSource);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000FED4 File Offset: 0x0000E2D4
		private bool _validate(GameObject[] gos, int[] deleteGOinstanceIDs)
		{
			if (this._validationLevel == MB2_ValidationLevel.none)
			{
				return true;
			}
			if (this._maxVertsInMesh < 3)
			{
				Debug.LogError("Invalid value for maxVertsInMesh=" + this._maxVertsInMesh);
			}
			this._validateTextureBakeResults();
			if (gos != null)
			{
				for (int i = 0; i < gos.Length; i++)
				{
					if (gos[i] == null)
					{
						Debug.LogError("The " + i + "th object on the list of objects to combine is 'None'. Use Command-Delete on Mac OS X; Delete or Shift-Delete on Windows to remove this one element.");
						return false;
					}
					if (this._validationLevel >= MB2_ValidationLevel.robust)
					{
						for (int j = i + 1; j < gos.Length; j++)
						{
							if (gos[i] == gos[j])
							{
								Debug.LogError("GameObject " + gos[i] + "appears twice in list of game objects to add");
								return false;
							}
						}
						if (this.obj2MeshCombinerMap.ContainsKey(gos[i].GetInstanceID()))
						{
							bool flag = false;
							if (deleteGOinstanceIDs != null)
							{
								for (int k = 0; k < deleteGOinstanceIDs.Length; k++)
								{
									if (deleteGOinstanceIDs[k] == gos[i].GetInstanceID())
									{
										flag = true;
									}
								}
							}
							if (!flag)
							{
								Debug.LogError(string.Concat(new object[]
								{
									"GameObject ",
									gos[i],
									" is already in the combined mesh ",
									gos[i].GetInstanceID()
								}));
								return false;
							}
						}
					}
				}
			}
			if (deleteGOinstanceIDs != null && this._validationLevel >= MB2_ValidationLevel.robust)
			{
				for (int l = 0; l < deleteGOinstanceIDs.Length; l++)
				{
					for (int m = l + 1; m < deleteGOinstanceIDs.Length; m++)
					{
						if (deleteGOinstanceIDs[l] == deleteGOinstanceIDs[m])
						{
							Debug.LogError("GameObject " + deleteGOinstanceIDs[l] + "appears twice in list of game objects to delete");
							return false;
						}
					}
					if (!this.obj2MeshCombinerMap.ContainsKey(deleteGOinstanceIDs[l]))
					{
						Debug.LogWarning("GameObject with instance ID " + deleteGOinstanceIDs[l] + " on the list of objects to delete is not in the combined mesh.");
					}
				}
			}
			return true;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000100D4 File Offset: 0x0000E4D4
		private void _distributeAmongBakers(GameObject[] gos, int[] deleteGOinstanceIDs)
		{
			if (gos == null)
			{
				gos = MB3_MultiMeshCombiner.empty;
			}
			if (deleteGOinstanceIDs == null)
			{
				deleteGOinstanceIDs = MB3_MultiMeshCombiner.emptyIDs;
			}
			if (this.resultSceneObject == null)
			{
				this.resultSceneObject = new GameObject("CombinedMesh-" + base.name);
			}
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				this.meshCombiners[i].extraSpace = this._maxVertsInMesh - this.meshCombiners[i].combinedMesh.GetMesh().vertexCount;
			}
			for (int j = 0; j < deleteGOinstanceIDs.Length; j++)
			{
				MB3_MultiMeshCombiner.CombinedMesh combinedMesh = null;
				if (this.obj2MeshCombinerMap.TryGetValue(deleteGOinstanceIDs[j], out combinedMesh))
				{
					if (this.LOG_LEVEL >= MB2_LogLevel.debug)
					{
						MB2_Log.LogDebug(string.Concat(new object[]
						{
							"MB2_MultiMeshCombiner.Removing ",
							deleteGOinstanceIDs[j],
							" from meshCombiner ",
							this.meshCombiners.IndexOf(combinedMesh)
						}), new object[0]);
					}
					combinedMesh.numVertsInListToDelete += combinedMesh.combinedMesh.GetNumVerticesFor(deleteGOinstanceIDs[j]);
					combinedMesh.gosToDelete.Add(deleteGOinstanceIDs[j]);
				}
				else
				{
					Debug.LogWarning("Object " + deleteGOinstanceIDs[j] + " in the list of objects to delete is not in the combined mesh.");
				}
			}
			for (int k = 0; k < gos.Length; k++)
			{
				GameObject gameObject = gos[k];
				int vertexCount = MB_Utility.GetMesh(gameObject).vertexCount;
				MB3_MultiMeshCombiner.CombinedMesh combinedMesh2 = null;
				for (int l = 0; l < this.meshCombiners.Count; l++)
				{
					if (this.meshCombiners[l].extraSpace + this.meshCombiners[l].numVertsInListToDelete - this.meshCombiners[l].numVertsInListToAdd > vertexCount)
					{
						combinedMesh2 = this.meshCombiners[l];
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							MB2_Log.LogDebug(string.Concat(new object[]
							{
								"MB2_MultiMeshCombiner.Added ",
								gos[k],
								" to combinedMesh ",
								l
							}), new object[]
							{
								this.LOG_LEVEL
							});
						}
						break;
					}
				}
				if (combinedMesh2 == null)
				{
					combinedMesh2 = new MB3_MultiMeshCombiner.CombinedMesh(this.maxVertsInMesh, this._resultSceneObject, this._LOG_LEVEL);
					this._setMBValues(combinedMesh2.combinedMesh);
					this.meshCombiners.Add(combinedMesh2);
					if (this.LOG_LEVEL >= MB2_LogLevel.debug)
					{
						MB2_Log.LogDebug("MB2_MultiMeshCombiner.Created new combinedMesh", new object[0]);
					}
				}
				combinedMesh2.gosToAdd.Add(gameObject);
				combinedMesh2.numVertsInListToAdd += vertexCount;
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000103A0 File Offset: 0x0000E7A0
		private bool _bakeStep1(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource)
		{
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				MB3_MultiMeshCombiner.CombinedMesh combinedMesh = this.meshCombiners[i];
				if (combinedMesh.combinedMesh.targetRenderer == null)
				{
					combinedMesh.combinedMesh.resultSceneObject = this._resultSceneObject;
					combinedMesh.combinedMesh.BuildSceneMeshObject(gos, true);
					if (this._LOG_LEVEL >= MB2_LogLevel.debug)
					{
						MB2_Log.LogDebug("BuildSO combiner {0} goID {1} targetRenID {2} meshID {3}", new object[]
						{
							i,
							combinedMesh.combinedMesh.targetRenderer.gameObject.GetInstanceID(),
							combinedMesh.combinedMesh.targetRenderer.GetInstanceID(),
							combinedMesh.combinedMesh.GetMesh().GetInstanceID()
						});
					}
				}
				else if (combinedMesh.combinedMesh.targetRenderer.transform.parent != this.resultSceneObject.transform)
				{
					Debug.LogError("targetRender objects must be children of resultSceneObject");
					return false;
				}
				if (combinedMesh.gosToAdd.Count > 0 || combinedMesh.gosToDelete.Count > 0)
				{
					combinedMesh.combinedMesh.AddDeleteGameObjectsByID(combinedMesh.gosToAdd.ToArray(), combinedMesh.gosToDelete.ToArray(), disableRendererInSource);
					if (this._LOG_LEVEL >= MB2_LogLevel.debug)
					{
						MB2_Log.LogDebug("Baked combiner {0} obsAdded {1} objsRemoved {2} goID {3} targetRenID {4} meshID {5}", new object[]
						{
							i,
							combinedMesh.gosToAdd.Count,
							combinedMesh.gosToDelete.Count,
							combinedMesh.combinedMesh.targetRenderer.gameObject.GetInstanceID(),
							combinedMesh.combinedMesh.targetRenderer.GetInstanceID(),
							combinedMesh.combinedMesh.GetMesh().GetInstanceID()
						});
					}
				}
				Renderer targetRenderer = combinedMesh.combinedMesh.targetRenderer;
				Mesh mesh = combinedMesh.combinedMesh.GetMesh();
				if (targetRenderer is MeshRenderer)
				{
					MeshFilter component = targetRenderer.gameObject.GetComponent<MeshFilter>();
					component.sharedMesh = mesh;
				}
				else
				{
					SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)targetRenderer;
					skinnedMeshRenderer.sharedMesh = mesh;
				}
			}
			for (int j = 0; j < this.meshCombiners.Count; j++)
			{
				MB3_MultiMeshCombiner.CombinedMesh combinedMesh2 = this.meshCombiners[j];
				for (int k = 0; k < combinedMesh2.gosToDelete.Count; k++)
				{
					this.obj2MeshCombinerMap.Remove(combinedMesh2.gosToDelete[k]);
				}
			}
			for (int l = 0; l < this.meshCombiners.Count; l++)
			{
				MB3_MultiMeshCombiner.CombinedMesh combinedMesh3 = this.meshCombiners[l];
				for (int m = 0; m < combinedMesh3.gosToAdd.Count; m++)
				{
					this.obj2MeshCombinerMap.Add(combinedMesh3.gosToAdd[m].GetInstanceID(), combinedMesh3);
				}
				if (combinedMesh3.gosToAdd.Count > 0 || combinedMesh3.gosToDelete.Count > 0)
				{
					combinedMesh3.gosToDelete.Clear();
					combinedMesh3.gosToAdd.Clear();
					combinedMesh3.numVertsInListToDelete = 0;
					combinedMesh3.numVertsInListToAdd = 0;
					combinedMesh3.isDirty = true;
				}
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				string text = "Meshes in combined:";
				for (int n = 0; n < this.meshCombiners.Count; n++)
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						" mesh",
						n,
						"(",
						this.meshCombiners[n].combinedMesh.GetObjectsInCombined().Count,
						")\n"
					});
				}
				text = text + "children in result: " + this.resultSceneObject.transform.childCount;
				MB2_Log.LogDebug(text, new object[]
				{
					this.LOG_LEVEL
				});
			}
			return this.meshCombiners.Count > 0;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000107F4 File Offset: 0x0000EBF4
		public override Dictionary<MB3_MeshCombiner.MBBlendShapeKey, MB3_MeshCombiner.MBBlendShapeValue> BuildSourceBlendShapeToCombinedIndexMap()
		{
			Dictionary<MB3_MeshCombiner.MBBlendShapeKey, MB3_MeshCombiner.MBBlendShapeValue> dictionary = new Dictionary<MB3_MeshCombiner.MBBlendShapeKey, MB3_MeshCombiner.MBBlendShapeValue>();
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				for (int j = 0; j < this.meshCombiners[i].combinedMesh.blendShapes.Length; j++)
				{
					MB3_MeshCombinerSingle.MBBlendShape mbblendShape = this.meshCombiners[i].combinedMesh.blendShapes[j];
					MB3_MeshCombiner.MBBlendShapeValue mbblendShapeValue = new MB3_MeshCombiner.MBBlendShapeValue();
					mbblendShapeValue.combinedMeshGameObject = this.meshCombiners[i].combinedMesh.targetRenderer.gameObject;
					mbblendShapeValue.blendShapeIndex = j;
					dictionary.Add(new MB3_MeshCombiner.MBBlendShapeKey(mbblendShape.gameObjectID, mbblendShape.indexInSource), mbblendShapeValue);
				}
			}
			return dictionary;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000108B0 File Offset: 0x0000ECB0
		public override void ClearBuffers()
		{
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				this.meshCombiners[i].combinedMesh.ClearBuffers();
			}
			this.obj2MeshCombinerMap.Clear();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000108FA File Offset: 0x0000ECFA
		public override void ClearMesh()
		{
			this.DestroyMesh();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00010904 File Offset: 0x0000ED04
		public override void DestroyMesh()
		{
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				if (this.meshCombiners[i].combinedMesh.targetRenderer != null)
				{
					MB_Utility.Destroy(this.meshCombiners[i].combinedMesh.targetRenderer.gameObject);
				}
				this.meshCombiners[i].combinedMesh.ClearMesh();
			}
			this.obj2MeshCombinerMap.Clear();
			this.meshCombiners.Clear();
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0001099C File Offset: 0x0000ED9C
		public override void DestroyMeshEditor(MB2_EditorMethodsInterface editorMethods)
		{
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				if (this.meshCombiners[i].combinedMesh.targetRenderer != null)
				{
					editorMethods.Destroy(this.meshCombiners[i].combinedMesh.targetRenderer.gameObject);
				}
				this.meshCombiners[i].combinedMesh.ClearMesh();
			}
			this.obj2MeshCombinerMap.Clear();
			this.meshCombiners.Clear();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00010A34 File Offset: 0x0000EE34
		private void _setMBValues(MB3_MeshCombinerSingle targ)
		{
			targ.validationLevel = this._validationLevel;
			targ.renderType = this.renderType;
			targ.outputOption = MB2_OutputOptions.bakeIntoSceneObject;
			targ.lightmapOption = this.lightmapOption;
			targ.textureBakeResults = this.textureBakeResults;
			targ.doNorm = this.doNorm;
			targ.doTan = this.doTan;
			targ.doCol = this.doCol;
			targ.doUV = this.doUV;
			targ.doUV3 = this.doUV3;
			targ.doUV4 = this.doUV4;
			targ.doBlendShapes = this.doBlendShapes;
			targ.optimizeAfterBake = base.optimizeAfterBake;
			targ.recenterVertsToBoundsCenter = this.recenterVertsToBoundsCenter;
			targ.uv2UnwrappingParamsHardAngle = this.uv2UnwrappingParamsHardAngle;
			targ.uv2UnwrappingParamsPackMargin = this.uv2UnwrappingParamsPackMargin;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00010AFC File Offset: 0x0000EEFC
		public override List<Material> GetMaterialsOnTargetRenderer()
		{
			HashSet<Material> hashSet = new HashSet<Material>();
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				hashSet.UnionWith(this.meshCombiners[i].combinedMesh.GetMaterialsOnTargetRenderer());
			}
			return new List<Material>(hashSet);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00010B50 File Offset: 0x0000EF50
		public override void CheckIntegrity()
		{
			if (!MB3_MeshBakerRoot.DO_INTEGRITY_CHECKS)
			{
				return;
			}
			for (int i = 0; i < this.meshCombiners.Count; i++)
			{
				this.meshCombiners[i].combinedMesh.CheckIntegrity();
			}
		}

		// Token: 0x04000124 RID: 292
		private static GameObject[] empty = new GameObject[0];

		// Token: 0x04000125 RID: 293
		private static int[] emptyIDs = new int[0];

		// Token: 0x04000126 RID: 294
		public Dictionary<int, MB3_MultiMeshCombiner.CombinedMesh> obj2MeshCombinerMap = new Dictionary<int, MB3_MultiMeshCombiner.CombinedMesh>();

		// Token: 0x04000127 RID: 295
		[SerializeField]
		public List<MB3_MultiMeshCombiner.CombinedMesh> meshCombiners = new List<MB3_MultiMeshCombiner.CombinedMesh>();

		// Token: 0x04000128 RID: 296
		[SerializeField]
		private int _maxVertsInMesh = 65535;

		// Token: 0x02000040 RID: 64
		[Serializable]
		public class CombinedMesh
		{
			// Token: 0x0600019B RID: 411 RVA: 0x00010BB4 File Offset: 0x0000EFB4
			public CombinedMesh(int maxNumVertsInMesh, GameObject resultSceneObject, MB2_LogLevel ll)
			{
				this.combinedMesh = new MB3_MeshCombinerSingle();
				this.combinedMesh.resultSceneObject = resultSceneObject;
				this.combinedMesh.LOG_LEVEL = ll;
				this.extraSpace = maxNumVertsInMesh;
				this.numVertsInListToDelete = 0;
				this.numVertsInListToAdd = 0;
				this.gosToAdd = new List<GameObject>();
				this.gosToDelete = new List<int>();
				this.gosToUpdate = new List<GameObject>();
			}

			// Token: 0x0600019C RID: 412 RVA: 0x00010C28 File Offset: 0x0000F028
			public bool isEmpty()
			{
				List<GameObject> list = new List<GameObject>();
				list.AddRange(this.combinedMesh.GetObjectsInCombined());
				for (int i = 0; i < this.gosToDelete.Count; i++)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].GetInstanceID() == this.gosToDelete[i])
						{
							list.RemoveAt(j);
							break;
						}
					}
				}
				return list.Count == 0;
			}

			// Token: 0x04000129 RID: 297
			public MB3_MeshCombinerSingle combinedMesh;

			// Token: 0x0400012A RID: 298
			public int extraSpace = -1;

			// Token: 0x0400012B RID: 299
			public int numVertsInListToDelete;

			// Token: 0x0400012C RID: 300
			public int numVertsInListToAdd;

			// Token: 0x0400012D RID: 301
			public List<GameObject> gosToAdd;

			// Token: 0x0400012E RID: 302
			public List<int> gosToDelete;

			// Token: 0x0400012F RID: 303
			public List<GameObject> gosToUpdate;

			// Token: 0x04000130 RID: 304
			public bool isDirty;
		}
	}
}
