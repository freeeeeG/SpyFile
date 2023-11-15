using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000A3E RID: 2622
[AddComponentMenu("KMonoBehaviour/scripts/GroundRenderer")]
public class GroundRenderer : KMonoBehaviour
{
	// Token: 0x06004EF8 RID: 20216 RVA: 0x001BDCD0 File Offset: 0x001BBED0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
		this.OnShadersReloaded();
		this.masks.Initialize();
		SubWorld.ZoneType[] array = (SubWorld.ZoneType[])Enum.GetValues(typeof(SubWorld.ZoneType));
		this.biomeMasks = new GroundMasks.BiomeMaskData[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			SubWorld.ZoneType zone_type = array[i];
			this.biomeMasks[i] = this.GetBiomeMask(zone_type);
		}
	}

	// Token: 0x06004EF9 RID: 20217 RVA: 0x001BDD4C File Offset: 0x001BBF4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.size = new Vector2I((Grid.WidthInCells + 16 - 1) / 16, (Grid.HeightInCells + 16 - 1) / 16);
		this.dirtyChunks = new bool[this.size.x, this.size.y];
		this.worldChunks = new GroundRenderer.WorldChunk[this.size.x, this.size.y];
		for (int i = 0; i < this.size.y; i++)
		{
			for (int j = 0; j < this.size.x; j++)
			{
				this.worldChunks[j, i] = new GroundRenderer.WorldChunk(j, i);
				this.dirtyChunks[j, i] = true;
			}
		}
	}

	// Token: 0x06004EFA RID: 20218 RVA: 0x001BDE14 File Offset: 0x001BC014
	public void Render(Vector2I vis_min, Vector2I vis_max, bool forceVisibleRebuild = false)
	{
		if (!base.enabled)
		{
			return;
		}
		int layer = LayerMask.NameToLayer("World");
		Vector2I vector2I = new Vector2I(vis_min.x / 16, vis_min.y / 16);
		Vector2I vector2I2 = new Vector2I((vis_max.x + 16 - 1) / 16, (vis_max.y + 16 - 1) / 16);
		for (int i = vector2I.y; i < vector2I2.y; i++)
		{
			for (int j = vector2I.x; j < vector2I2.x; j++)
			{
				GroundRenderer.WorldChunk worldChunk = this.worldChunks[j, i];
				if (this.dirtyChunks[j, i] || forceVisibleRebuild)
				{
					this.dirtyChunks[j, i] = false;
					worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
				}
				worldChunk.Render(layer);
			}
		}
		this.RebuildDirtyChunks();
	}

	// Token: 0x06004EFB RID: 20219 RVA: 0x001BDEF3 File Offset: 0x001BC0F3
	public void RenderAll()
	{
		this.Render(new Vector2I(0, 0), new Vector2I(this.worldChunks.GetLength(0) * 16, this.worldChunks.GetLength(1) * 16), true);
	}

	// Token: 0x06004EFC RID: 20220 RVA: 0x001BDF28 File Offset: 0x001BC128
	private void RebuildDirtyChunks()
	{
		for (int i = 0; i < this.dirtyChunks.GetLength(1); i++)
		{
			for (int j = 0; j < this.dirtyChunks.GetLength(0); j++)
			{
				if (this.dirtyChunks[j, i])
				{
					this.dirtyChunks[j, i] = false;
					this.worldChunks[j, i].Rebuild(this.biomeMasks, this.elementMaterials);
				}
			}
		}
	}

	// Token: 0x06004EFD RID: 20221 RVA: 0x001BDFA0 File Offset: 0x001BC1A0
	public void MarkDirty(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = new Vector2I(vector2I.x / 16, vector2I.y / 16);
		this.dirtyChunks[vector2I2.x, vector2I2.y] = true;
		bool flag = vector2I.x % 16 == 0 && vector2I2.x > 0;
		bool flag2 = vector2I.x % 16 == 15 && vector2I2.x < this.size.x - 1;
		bool flag3 = vector2I.y % 16 == 0 && vector2I2.y > 0;
		bool flag4 = vector2I.y % 16 == 15 && vector2I2.y < this.size.y - 1;
		if (flag)
		{
			this.dirtyChunks[vector2I2.x - 1, vector2I2.y] = true;
			if (flag3)
			{
				this.dirtyChunks[vector2I2.x - 1, vector2I2.y - 1] = true;
			}
			if (flag4)
			{
				this.dirtyChunks[vector2I2.x - 1, vector2I2.y + 1] = true;
			}
		}
		if (flag3)
		{
			this.dirtyChunks[vector2I2.x, vector2I2.y - 1] = true;
		}
		if (flag4)
		{
			this.dirtyChunks[vector2I2.x, vector2I2.y + 1] = true;
		}
		if (flag2)
		{
			this.dirtyChunks[vector2I2.x + 1, vector2I2.y] = true;
			if (flag3)
			{
				this.dirtyChunks[vector2I2.x + 1, vector2I2.y - 1] = true;
			}
			if (flag4)
			{
				this.dirtyChunks[vector2I2.x + 1, vector2I2.y + 1] = true;
			}
		}
	}

	// Token: 0x06004EFE RID: 20222 RVA: 0x001BE154 File Offset: 0x001BC354
	private Vector2I GetChunkIdx(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		return new Vector2I(vector2I.x / 16, vector2I.y / 16);
	}

	// Token: 0x06004EFF RID: 20223 RVA: 0x001BE180 File Offset: 0x001BC380
	private GroundMasks.BiomeMaskData GetBiomeMask(SubWorld.ZoneType zone_type)
	{
		GroundMasks.BiomeMaskData result = null;
		string key = zone_type.ToString().ToLower();
		this.masks.biomeMasks.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x06004F00 RID: 20224 RVA: 0x001BE1B8 File Offset: 0x001BC3B8
	private void InitOpaqueMaterial(Material material, Element element)
	{
		material.name = element.id.ToString() + "_opaque";
		material.renderQueue = RenderQueues.WorldOpaque;
		material.EnableKeyword("OPAQUE");
		material.DisableKeyword("ALPHA");
		this.ConfigureMaterialShine(material);
		material.SetInt("_SrcAlpha", 1);
		material.SetInt("_DstAlpha", 0);
		material.SetInt("_ZWrite", 1);
		material.SetTexture("_AlphaTestMap", Texture2D.whiteTexture);
	}

	// Token: 0x06004F01 RID: 20225 RVA: 0x001BE244 File Offset: 0x001BC444
	private void InitAlphaMaterial(Material material, Element element)
	{
		material.name = element.id.ToString() + "_alpha";
		material.renderQueue = RenderQueues.WorldTransparent;
		material.EnableKeyword("ALPHA");
		material.DisableKeyword("OPAQUE");
		this.ConfigureMaterialShine(material);
		material.SetTexture("_AlphaTestMap", this.masks.maskAtlas.texture);
		material.SetInt("_SrcAlpha", 5);
		material.SetInt("_DstAlpha", 10);
		material.SetInt("_ZWrite", 0);
	}

	// Token: 0x06004F02 RID: 20226 RVA: 0x001BE2DC File Offset: 0x001BC4DC
	private void ConfigureMaterialShine(Material material)
	{
		if (material.GetTexture("_ShineMask") != null)
		{
			material.DisableKeyword("MATTE");
			material.EnableKeyword("SHINY");
			return;
		}
		material.EnableKeyword("MATTE");
		material.DisableKeyword("SHINY");
	}

	// Token: 0x06004F03 RID: 20227 RVA: 0x001BE32C File Offset: 0x001BC52C
	[ContextMenu("Reload Shaders")]
	public void OnShadersReloaded()
	{
		this.FreeMaterials();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid)
			{
				if (element.substance.material == null)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						element.name,
						"must have material associated with it in the substance table"
					});
				}
				Material material = new Material(element.substance.material);
				this.InitOpaqueMaterial(material, element);
				Material material2 = new Material(material);
				this.InitAlphaMaterial(material2, element);
				GroundRenderer.Materials value = new GroundRenderer.Materials(material, material2);
				this.elementMaterials[element.id] = value;
			}
		}
		if (this.worldChunks != null)
		{
			for (int i = 0; i < this.dirtyChunks.GetLength(1); i++)
			{
				for (int j = 0; j < this.dirtyChunks.GetLength(0); j++)
				{
					this.dirtyChunks[j, i] = true;
				}
			}
			GroundRenderer.WorldChunk[,] array = this.worldChunks;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int k = array.GetLowerBound(0); k <= upperBound; k++)
			{
				for (int l = array.GetLowerBound(1); l <= upperBound2; l++)
				{
					GroundRenderer.WorldChunk worldChunk = array[k, l];
					worldChunk.Clear();
					worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
				}
			}
		}
	}

	// Token: 0x06004F04 RID: 20228 RVA: 0x001BE4C4 File Offset: 0x001BC6C4
	public void FreeResources()
	{
		this.FreeMaterials();
		this.elementMaterials.Clear();
		this.elementMaterials = null;
		if (this.worldChunks != null)
		{
			GroundRenderer.WorldChunk[,] array = this.worldChunks;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int i = array.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = array.GetLowerBound(1); j <= upperBound2; j++)
				{
					GroundRenderer.WorldChunk worldChunk = array[i, j];
					worldChunk.FreeResources();
				}
			}
			this.worldChunks = null;
		}
	}

	// Token: 0x06004F05 RID: 20229 RVA: 0x001BE54C File Offset: 0x001BC74C
	private void FreeMaterials()
	{
		foreach (GroundRenderer.Materials materials in this.elementMaterials.Values)
		{
			UnityEngine.Object.Destroy(materials.opaque);
			UnityEngine.Object.Destroy(materials.alpha);
		}
		this.elementMaterials.Clear();
	}

	// Token: 0x04003360 RID: 13152
	[SerializeField]
	private GroundMasks masks;

	// Token: 0x04003361 RID: 13153
	private GroundMasks.BiomeMaskData[] biomeMasks;

	// Token: 0x04003362 RID: 13154
	private Dictionary<SimHashes, GroundRenderer.Materials> elementMaterials = new Dictionary<SimHashes, GroundRenderer.Materials>();

	// Token: 0x04003363 RID: 13155
	private bool[,] dirtyChunks;

	// Token: 0x04003364 RID: 13156
	private GroundRenderer.WorldChunk[,] worldChunks;

	// Token: 0x04003365 RID: 13157
	private const int ChunkEdgeSize = 16;

	// Token: 0x04003366 RID: 13158
	private Vector2I size;

	// Token: 0x020018D4 RID: 6356
	[Serializable]
	private struct Materials
	{
		// Token: 0x06009301 RID: 37633 RVA: 0x0032D9C4 File Offset: 0x0032BBC4
		public Materials(Material opaque, Material alpha)
		{
			this.opaque = opaque;
			this.alpha = alpha;
		}

		// Token: 0x0400730E RID: 29454
		public Material opaque;

		// Token: 0x0400730F RID: 29455
		public Material alpha;
	}

	// Token: 0x020018D5 RID: 6357
	private class ElementChunk
	{
		// Token: 0x06009302 RID: 37634 RVA: 0x0032D9D4 File Offset: 0x0032BBD4
		public ElementChunk(SimHashes element, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			this.element = element;
			GroundRenderer.Materials materials2 = materials[element];
			this.alpha = new GroundRenderer.ElementChunk.RenderData(materials2.alpha);
			this.opaque = new GroundRenderer.ElementChunk.RenderData(materials2.opaque);
			this.Clear();
		}

		// Token: 0x06009303 RID: 37635 RVA: 0x0032DA1E File Offset: 0x0032BC1E
		public void Clear()
		{
			this.opaque.Clear();
			this.alpha.Clear();
			this.tileCount = 0;
		}

		// Token: 0x06009304 RID: 37636 RVA: 0x0032DA3D File Offset: 0x0032BC3D
		public void AddOpaqueQuad(int x, int y, GroundMasks.UVData uvs)
		{
			this.opaque.AddQuad(x, y, uvs);
			this.tileCount++;
		}

		// Token: 0x06009305 RID: 37637 RVA: 0x0032DA5B File Offset: 0x0032BC5B
		public void AddAlphaQuad(int x, int y, GroundMasks.UVData uvs)
		{
			this.alpha.AddQuad(x, y, uvs);
			this.tileCount++;
		}

		// Token: 0x06009306 RID: 37638 RVA: 0x0032DA79 File Offset: 0x0032BC79
		public void Build()
		{
			this.opaque.Build();
			this.alpha.Build();
		}

		// Token: 0x06009307 RID: 37639 RVA: 0x0032DA94 File Offset: 0x0032BC94
		public void Render(int layer, int element_idx)
		{
			float num = Grid.GetLayerZ(Grid.SceneLayer.Ground);
			num -= 0.0001f * (float)element_idx;
			this.opaque.Render(new Vector3(0f, 0f, num), layer);
			this.alpha.Render(new Vector3(0f, 0f, num), layer);
		}

		// Token: 0x06009308 RID: 37640 RVA: 0x0032DAEC File Offset: 0x0032BCEC
		public void FreeResources()
		{
			this.alpha.FreeResources();
			this.opaque.FreeResources();
			this.alpha = null;
			this.opaque = null;
		}

		// Token: 0x04007310 RID: 29456
		public SimHashes element;

		// Token: 0x04007311 RID: 29457
		private GroundRenderer.ElementChunk.RenderData alpha;

		// Token: 0x04007312 RID: 29458
		private GroundRenderer.ElementChunk.RenderData opaque;

		// Token: 0x04007313 RID: 29459
		public int tileCount;

		// Token: 0x0200220C RID: 8716
		private class RenderData
		{
			// Token: 0x0600ACA0 RID: 44192 RVA: 0x003779E0 File Offset: 0x00375BE0
			public RenderData(Material material)
			{
				this.material = material;
				this.mesh = new Mesh();
				this.mesh.MarkDynamic();
				this.mesh.name = "ElementChunk";
				this.pos = new List<Vector3>();
				this.uv = new List<Vector2>();
				this.indices = new List<int>();
			}

			// Token: 0x0600ACA1 RID: 44193 RVA: 0x00377A41 File Offset: 0x00375C41
			public void ClearMesh()
			{
				if (this.mesh != null)
				{
					this.mesh.Clear();
					UnityEngine.Object.DestroyImmediate(this.mesh);
					this.mesh = null;
				}
			}

			// Token: 0x0600ACA2 RID: 44194 RVA: 0x00377A70 File Offset: 0x00375C70
			public void Clear()
			{
				if (this.mesh != null)
				{
					this.mesh.Clear();
				}
				if (this.pos != null)
				{
					this.pos.Clear();
				}
				if (this.uv != null)
				{
					this.uv.Clear();
				}
				if (this.indices != null)
				{
					this.indices.Clear();
				}
			}

			// Token: 0x0600ACA3 RID: 44195 RVA: 0x00377ACF File Offset: 0x00375CCF
			public void FreeResources()
			{
				this.ClearMesh();
				this.Clear();
				this.pos = null;
				this.uv = null;
				this.indices = null;
				this.material = null;
			}

			// Token: 0x0600ACA4 RID: 44196 RVA: 0x00377AF9 File Offset: 0x00375CF9
			public void Build()
			{
				this.mesh.SetVertices(this.pos);
				this.mesh.SetUVs(0, this.uv);
				this.mesh.SetTriangles(this.indices, 0);
			}

			// Token: 0x0600ACA5 RID: 44197 RVA: 0x00377B30 File Offset: 0x00375D30
			public void AddQuad(int x, int y, GroundMasks.UVData uvs)
			{
				int count = this.pos.Count;
				this.indices.Add(count);
				this.indices.Add(count + 1);
				this.indices.Add(count + 3);
				this.indices.Add(count);
				this.indices.Add(count + 3);
				this.indices.Add(count + 2);
				this.pos.Add(new Vector3((float)x + -0.5f, (float)y + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + 1f + -0.5f, (float)y + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + -0.5f, (float)y + 1f + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + 1f + -0.5f, (float)y + 1f + -0.5f, 0f));
				this.uv.Add(uvs.bl);
				this.uv.Add(uvs.br);
				this.uv.Add(uvs.tl);
				this.uv.Add(uvs.tr);
			}

			// Token: 0x0600ACA6 RID: 44198 RVA: 0x00377C8C File Offset: 0x00375E8C
			public void Render(Vector3 position, int layer)
			{
				if (this.pos.Count != 0)
				{
					Graphics.DrawMesh(this.mesh, position, Quaternion.identity, this.material, layer, null, 0, null, ShadowCastingMode.Off, false, null, false);
				}
			}

			// Token: 0x0400986F RID: 39023
			public Material material;

			// Token: 0x04009870 RID: 39024
			public Mesh mesh;

			// Token: 0x04009871 RID: 39025
			public List<Vector3> pos;

			// Token: 0x04009872 RID: 39026
			public List<Vector2> uv;

			// Token: 0x04009873 RID: 39027
			public List<int> indices;
		}
	}

	// Token: 0x020018D6 RID: 6358
	private struct WorldChunk
	{
		// Token: 0x06009309 RID: 37641 RVA: 0x0032DB12 File Offset: 0x0032BD12
		public WorldChunk(int x, int y)
		{
			this.chunkX = x;
			this.chunkY = y;
			this.elementChunks = new List<GroundRenderer.ElementChunk>();
		}

		// Token: 0x0600930A RID: 37642 RVA: 0x0032DB2D File Offset: 0x0032BD2D
		public void Clear()
		{
			this.elementChunks.Clear();
		}

		// Token: 0x0600930B RID: 37643 RVA: 0x0032DB3C File Offset: 0x0032BD3C
		private static void InsertSorted(Element element, Element[] array, int size)
		{
			int id = (int)element.id;
			for (int i = 0; i < size; i++)
			{
				Element element2 = array[i];
				if (element2.id > (SimHashes)id)
				{
					array[i] = element;
					element = element2;
					id = (int)element2.id;
				}
			}
			array[size] = element;
		}

		// Token: 0x0600930C RID: 37644 RVA: 0x0032DB7C File Offset: 0x0032BD7C
		public void Rebuild(GroundMasks.BiomeMaskData[] biomeMasks, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
			{
				elementChunk.Clear();
			}
			Vector2I vector2I = new Vector2I(this.chunkX * 16, this.chunkY * 16);
			Vector2I vector2I2 = new Vector2I(Math.Min(Grid.WidthInCells, (this.chunkX + 1) * 16), Math.Min(Grid.HeightInCells, (this.chunkY + 1) * 16));
			for (int i = vector2I.y; i < vector2I2.y; i++)
			{
				int num = Math.Max(0, i - 1);
				int num2 = i;
				for (int j = vector2I.x; j < vector2I2.x; j++)
				{
					int num3 = Math.Max(0, j - 1);
					int num4 = j;
					int num5 = num * Grid.WidthInCells + num3;
					int num6 = num * Grid.WidthInCells + num4;
					int num7 = num2 * Grid.WidthInCells + num3;
					int num8 = num2 * Grid.WidthInCells + num4;
					GroundRenderer.WorldChunk.elements[0] = Grid.Element[num5];
					GroundRenderer.WorldChunk.elements[1] = Grid.Element[num6];
					GroundRenderer.WorldChunk.elements[2] = Grid.Element[num7];
					GroundRenderer.WorldChunk.elements[3] = Grid.Element[num8];
					GroundRenderer.WorldChunk.substances[0] = ((Grid.RenderedByWorld[num5] && GroundRenderer.WorldChunk.elements[0].IsSolid) ? GroundRenderer.WorldChunk.elements[0].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[1] = ((Grid.RenderedByWorld[num6] && GroundRenderer.WorldChunk.elements[1].IsSolid) ? GroundRenderer.WorldChunk.elements[1].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[2] = ((Grid.RenderedByWorld[num7] && GroundRenderer.WorldChunk.elements[2].IsSolid) ? GroundRenderer.WorldChunk.elements[2].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[3] = ((Grid.RenderedByWorld[num8] && GroundRenderer.WorldChunk.elements[3].IsSolid) ? GroundRenderer.WorldChunk.elements[3].substance.idx : -1);
					GroundRenderer.WorldChunk.uniqueElements[0] = GroundRenderer.WorldChunk.elements[0];
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[1], GroundRenderer.WorldChunk.uniqueElements, 1);
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[2], GroundRenderer.WorldChunk.uniqueElements, 2);
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[3], GroundRenderer.WorldChunk.uniqueElements, 3);
					int num9 = -1;
					int biomeIdx = GroundRenderer.WorldChunk.GetBiomeIdx(i * Grid.WidthInCells + j);
					GroundMasks.BiomeMaskData biomeMaskData = biomeMasks[biomeIdx];
					if (biomeMaskData == null)
					{
						biomeMaskData = biomeMasks[3];
					}
					for (int k = 0; k < GroundRenderer.WorldChunk.uniqueElements.Length; k++)
					{
						Element element = GroundRenderer.WorldChunk.uniqueElements[k];
						if (element.IsSolid)
						{
							int idx = element.substance.idx;
							if (idx != num9)
							{
								num9 = idx;
								int num10 = ((GroundRenderer.WorldChunk.substances[2] >= idx) ? 1 : 0) << 3 | ((GroundRenderer.WorldChunk.substances[3] >= idx) ? 1 : 0) << 2 | ((GroundRenderer.WorldChunk.substances[0] >= idx) ? 1 : 0) << 1 | ((GroundRenderer.WorldChunk.substances[1] >= idx) ? 1 : 0);
								if (num10 > 0)
								{
									GroundMasks.UVData[] variationUVs = biomeMaskData.tiles[num10].variationUVs;
									float staticRandom = GroundRenderer.WorldChunk.GetStaticRandom(j, i);
									int num11 = Mathf.Min(variationUVs.Length - 1, (int)((float)variationUVs.Length * staticRandom));
									GroundMasks.UVData uvs = variationUVs[num11 % variationUVs.Length];
									GroundRenderer.ElementChunk elementChunk2 = this.GetElementChunk(element.id, materials);
									if (num10 == 15)
									{
										elementChunk2.AddOpaqueQuad(j, i, uvs);
									}
									else
									{
										elementChunk2.AddAlphaQuad(j, i, uvs);
									}
								}
							}
						}
					}
				}
			}
			foreach (GroundRenderer.ElementChunk elementChunk3 in this.elementChunks)
			{
				elementChunk3.Build();
			}
			for (int l = this.elementChunks.Count - 1; l >= 0; l--)
			{
				if (this.elementChunks[l].tileCount == 0)
				{
					int index = this.elementChunks.Count - 1;
					this.elementChunks[l] = this.elementChunks[index];
					this.elementChunks.RemoveAt(index);
				}
			}
		}

		// Token: 0x0600930D RID: 37645 RVA: 0x0032DFD8 File Offset: 0x0032C1D8
		private GroundRenderer.ElementChunk GetElementChunk(SimHashes elementID, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			GroundRenderer.ElementChunk elementChunk = null;
			for (int i = 0; i < this.elementChunks.Count; i++)
			{
				if (this.elementChunks[i].element == elementID)
				{
					elementChunk = this.elementChunks[i];
					break;
				}
			}
			if (elementChunk == null)
			{
				elementChunk = new GroundRenderer.ElementChunk(elementID, materials);
				this.elementChunks.Add(elementChunk);
			}
			return elementChunk;
		}

		// Token: 0x0600930E RID: 37646 RVA: 0x0032E038 File Offset: 0x0032C238
		private static int GetBiomeIdx(int cell)
		{
			if (!Grid.IsValidCell(cell))
			{
				return 0;
			}
			SubWorld.ZoneType result = SubWorld.ZoneType.Sandstone;
			if (global::World.Instance != null && global::World.Instance.zoneRenderData != null)
			{
				result = global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell);
			}
			return (int)result;
		}

		// Token: 0x0600930F RID: 37647 RVA: 0x0032E082 File Offset: 0x0032C282
		private static float GetStaticRandom(int x, int y)
		{
			return PerlinSimplexNoise.noise((float)x * GroundRenderer.WorldChunk.NoiseScale.x, (float)y * GroundRenderer.WorldChunk.NoiseScale.y);
		}

		// Token: 0x06009310 RID: 37648 RVA: 0x0032E0A4 File Offset: 0x0032C2A4
		public void Render(int layer)
		{
			for (int i = 0; i < this.elementChunks.Count; i++)
			{
				GroundRenderer.ElementChunk elementChunk = this.elementChunks[i];
				elementChunk.Render(layer, ElementLoader.FindElementByHash(elementChunk.element).substance.idx);
			}
		}

		// Token: 0x06009311 RID: 37649 RVA: 0x0032E0F0 File Offset: 0x0032C2F0
		public void FreeResources()
		{
			foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
			{
				elementChunk.FreeResources();
			}
			this.elementChunks.Clear();
			this.elementChunks = null;
		}

		// Token: 0x04007314 RID: 29460
		public readonly int chunkX;

		// Token: 0x04007315 RID: 29461
		public readonly int chunkY;

		// Token: 0x04007316 RID: 29462
		private List<GroundRenderer.ElementChunk> elementChunks;

		// Token: 0x04007317 RID: 29463
		private static Element[] elements = new Element[4];

		// Token: 0x04007318 RID: 29464
		private static Element[] uniqueElements = new Element[4];

		// Token: 0x04007319 RID: 29465
		private static int[] substances = new int[4];

		// Token: 0x0400731A RID: 29466
		private static Vector2 NoiseScale = new Vector3(1f, 1f);
	}
}
