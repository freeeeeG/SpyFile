using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200005D RID: 93
	[Serializable]
	public abstract class MB3_MeshBakerGrouperCore
	{
		// Token: 0x06000278 RID: 632
		public abstract Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection);

		// Token: 0x06000279 RID: 633
		public abstract void DrawGizmos(Bounds sourceObjectBounds);

		// Token: 0x0600027A RID: 634 RVA: 0x0001AC8C File Offset: 0x0001908C
		public void DoClustering(MB3_TextureBaker tb, MB3_MeshBakerGrouper grouper)
		{
			Dictionary<string, List<Renderer>> dictionary = this.FilterIntoGroups(tb.GetObjectsToCombine());
			if (this.d.clusterOnLMIndex)
			{
				Dictionary<string, List<Renderer>> dictionary2 = new Dictionary<string, List<Renderer>>();
				foreach (string text in dictionary.Keys)
				{
					List<Renderer> gaws = dictionary[text];
					Dictionary<int, List<Renderer>> dictionary3 = this.GroupByLightmapIndex(gaws);
					foreach (int num in dictionary3.Keys)
					{
						string key = text + "-LM-" + num;
						dictionary2.Add(key, dictionary3[num]);
					}
				}
				dictionary = dictionary2;
			}
			if (this.d.clusterByLODLevel)
			{
				Dictionary<string, List<Renderer>> dictionary4 = new Dictionary<string, List<Renderer>>();
				foreach (string text2 in dictionary.Keys)
				{
					List<Renderer> list = dictionary[text2];
					using (List<Renderer>.Enumerator enumerator4 = list.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							Renderer r = enumerator4.Current;
							if (!(r == null))
							{
								bool flag = false;
								LODGroup componentInParent = r.GetComponentInParent<LODGroup>();
								if (componentInParent != null)
								{
									LOD[] lods = componentInParent.GetLODs();
									for (int i = 0; i < lods.Length; i++)
									{
										LOD lod = lods[i];
										if (Array.Find<Renderer>(lod.renderers, (Renderer x) => x == r) != null)
										{
											flag = true;
											string key2 = string.Format("{0}_LOD{1}", text2, i);
											List<Renderer> list2;
											if (!dictionary4.TryGetValue(key2, out list2))
											{
												list2 = new List<Renderer>();
												dictionary4.Add(key2, list2);
											}
											if (!list2.Contains(r))
											{
												list2.Add(r);
											}
										}
									}
								}
								if (!flag)
								{
									string key3 = string.Format("{0}_LOD0", text2);
									List<Renderer> list3;
									if (!dictionary4.TryGetValue(key3, out list3))
									{
										list3 = new List<Renderer>();
										dictionary4.Add(key3, list3);
									}
									if (!list3.Contains(r))
									{
										list3.Add(r);
									}
								}
							}
						}
					}
				}
				dictionary = dictionary4;
			}
			int num2 = 0;
			foreach (string key4 in dictionary.Keys)
			{
				List<Renderer> list4 = dictionary[key4];
				if (list4.Count > 1 || grouper.data.includeCellsWithOnlyOneRenderer)
				{
					this.AddMeshBaker(tb, key4, list4);
				}
				else
				{
					num2++;
				}
			}
			Debug.Log(string.Format("Found {0} cells with Renderers. Not creating bakers for {1} because there is only one mesh in the cell. Creating {2} bakers.", dictionary.Count, num2, dictionary.Count - num2));
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0001B06C File Offset: 0x0001946C
		private Dictionary<int, List<Renderer>> GroupByLightmapIndex(List<Renderer> gaws)
		{
			Dictionary<int, List<Renderer>> dictionary = new Dictionary<int, List<Renderer>>();
			for (int i = 0; i < gaws.Count; i++)
			{
				List<Renderer> list;
				if (dictionary.ContainsKey(gaws[i].lightmapIndex))
				{
					list = dictionary[gaws[i].lightmapIndex];
				}
				else
				{
					list = new List<Renderer>();
					dictionary.Add(gaws[i].lightmapIndex, list);
				}
				list.Add(gaws[i]);
			}
			return dictionary;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0001B0F0 File Offset: 0x000194F0
		private void AddMeshBaker(MB3_TextureBaker tb, string key, List<Renderer> gaws)
		{
			int num = 0;
			for (int i = 0; i < gaws.Count; i++)
			{
				Mesh mesh = MB_Utility.GetMesh(gaws[i].gameObject);
				if (mesh != null)
				{
					num += mesh.vertexCount;
				}
			}
			GameObject gameObject = new GameObject("MeshBaker-" + key);
			gameObject.transform.position = Vector3.zero;
			MB3_MeshBakerCommon mb3_MeshBakerCommon;
			if (num >= 65535)
			{
				mb3_MeshBakerCommon = gameObject.AddComponent<MB3_MultiMeshBaker>();
				mb3_MeshBakerCommon.useObjsToMeshFromTexBaker = false;
			}
			else
			{
				mb3_MeshBakerCommon = gameObject.AddComponent<MB3_MeshBaker>();
				mb3_MeshBakerCommon.useObjsToMeshFromTexBaker = false;
			}
			mb3_MeshBakerCommon.textureBakeResults = tb.textureBakeResults;
			mb3_MeshBakerCommon.transform.parent = tb.transform;
			for (int j = 0; j < gaws.Count; j++)
			{
				mb3_MeshBakerCommon.GetObjectsToCombine().Add(gaws[j].gameObject);
			}
		}

		// Token: 0x0400019F RID: 415
		public GrouperData d;
	}
}
