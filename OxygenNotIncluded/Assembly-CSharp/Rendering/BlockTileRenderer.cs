using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering
{
	// Token: 0x02000CB5 RID: 3253
	public class BlockTileRenderer : MonoBehaviour
	{
		// Token: 0x0600680D RID: 26637 RVA: 0x00275BBC File Offset: 0x00273DBC
		public static BlockTileRenderer.RenderInfoLayer GetRenderInfoLayer(bool isReplacement, SimHashes element)
		{
			if (isReplacement)
			{
				return BlockTileRenderer.RenderInfoLayer.Replacement;
			}
			if (element == SimHashes.Void)
			{
				return BlockTileRenderer.RenderInfoLayer.UnderConstruction;
			}
			return BlockTileRenderer.RenderInfoLayer.Built;
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x0600680E RID: 26638 RVA: 0x00275BCE File Offset: 0x00273DCE
		public bool ForceRebuild
		{
			get
			{
				return this.forceRebuild;
			}
		}

		// Token: 0x0600680F RID: 26639 RVA: 0x00275BD8 File Offset: 0x00273DD8
		public BlockTileRenderer()
		{
			this.forceRebuild = false;
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x00275C5C File Offset: 0x00273E5C
		public void FreeResources()
		{
			foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.FreeResources();
				}
			}
			this.renderInfo.Clear();
		}

		// Token: 0x06006811 RID: 26641 RVA: 0x00275CC8 File Offset: 0x00273EC8
		private static bool MatchesDef(GameObject go, BuildingDef def)
		{
			return go != null && go.GetComponent<Building>().Def == def;
		}

		// Token: 0x06006812 RID: 26642 RVA: 0x00275CE8 File Offset: 0x00273EE8
		public virtual BlockTileRenderer.Bits GetConnectionBits(int x, int y, int query_layer)
		{
			BlockTileRenderer.Bits bits = (BlockTileRenderer.Bits)0;
			GameObject gameObject = Grid.Objects[y * Grid.WidthInCells + x, query_layer];
			BuildingDef def = (gameObject != null) ? gameObject.GetComponent<Building>().Def : null;
			if (y > 0)
			{
				int num = (y - 1) * Grid.WidthInCells + x;
				if (x > 0 && BlockTileRenderer.MatchesDef(Grid.Objects[num - 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.DownLeft;
				}
				if (BlockTileRenderer.MatchesDef(Grid.Objects[num, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.Down;
				}
				if (x < Grid.WidthInCells - 1 && BlockTileRenderer.MatchesDef(Grid.Objects[num + 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.DownRight;
				}
			}
			int num2 = y * Grid.WidthInCells + x;
			if (x > 0 && BlockTileRenderer.MatchesDef(Grid.Objects[num2 - 1, query_layer], def))
			{
				bits |= BlockTileRenderer.Bits.Left;
			}
			if (x < Grid.WidthInCells - 1 && BlockTileRenderer.MatchesDef(Grid.Objects[num2 + 1, query_layer], def))
			{
				bits |= BlockTileRenderer.Bits.Right;
			}
			if (y < Grid.HeightInCells - 1)
			{
				int num3 = (y + 1) * Grid.WidthInCells + x;
				if (x > 0 && BlockTileRenderer.MatchesDef(Grid.Objects[num3 - 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.UpLeft;
				}
				if (BlockTileRenderer.MatchesDef(Grid.Objects[num3, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.Up;
				}
				if (x < Grid.WidthInCells + 1 && BlockTileRenderer.MatchesDef(Grid.Objects[num3 + 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.UpRight;
				}
			}
			return bits;
		}

		// Token: 0x06006813 RID: 26643 RVA: 0x00275E5C File Offset: 0x0027405C
		private bool IsDecorConnectable(GameObject src, GameObject target)
		{
			if (src != null && target != null)
			{
				IBlockTileInfo component = src.GetComponent<IBlockTileInfo>();
				IBlockTileInfo component2 = target.GetComponent<IBlockTileInfo>();
				if (component != null && component2 != null)
				{
					return component.GetBlockTileConnectorID() == component2.GetBlockTileConnectorID();
				}
			}
			return false;
		}

		// Token: 0x06006814 RID: 26644 RVA: 0x00275EA0 File Offset: 0x002740A0
		public virtual BlockTileRenderer.Bits GetDecorConnectionBits(int x, int y, int query_layer)
		{
			BlockTileRenderer.Bits bits = (BlockTileRenderer.Bits)0;
			GameObject src = Grid.Objects[y * Grid.WidthInCells + x, query_layer];
			if (y > 0)
			{
				int num = (y - 1) * Grid.WidthInCells + x;
				if (x > 0 && Grid.Objects[num - 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.DownLeft;
				}
				if (Grid.Objects[num, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.Down;
				}
				if (x < Grid.WidthInCells - 1 && Grid.Objects[num + 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.DownRight;
				}
			}
			int num2 = y * Grid.WidthInCells + x;
			if (x > 0 && this.IsDecorConnectable(src, Grid.Objects[num2 - 1, query_layer]))
			{
				bits |= BlockTileRenderer.Bits.Left;
			}
			if (x < Grid.WidthInCells - 1 && this.IsDecorConnectable(src, Grid.Objects[num2 + 1, query_layer]))
			{
				bits |= BlockTileRenderer.Bits.Right;
			}
			if (y < Grid.HeightInCells - 1)
			{
				int num3 = (y + 1) * Grid.WidthInCells + x;
				if (x > 0 && Grid.Objects[num3 - 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.UpLeft;
				}
				if (Grid.Objects[num3, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.Up;
				}
				if (x < Grid.WidthInCells + 1 && Grid.Objects[num3 + 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.UpRight;
				}
			}
			return bits;
		}

		// Token: 0x06006815 RID: 26645 RVA: 0x00275FF8 File Offset: 0x002741F8
		public void LateUpdate()
		{
			this.Render();
		}

		// Token: 0x06006816 RID: 26646 RVA: 0x00276000 File Offset: 0x00274200
		private void Render()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			if (GameUtil.IsCapturingTimeLapse())
			{
				vector2I = new Vector2I(0, 0);
				vector2I2 = new Vector2I(Grid.WidthInCells / 16, Grid.HeightInCells / 16);
			}
			else
			{
				GridArea visibleArea = GridVisibleArea.GetVisibleArea();
				vector2I = new Vector2I(visibleArea.Min.x / 16, visibleArea.Min.y / 16);
				vector2I2 = new Vector2I((visibleArea.Max.x + 16 - 1) / 16, (visibleArea.Max.y + 16 - 1) / 16);
			}
			foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
			{
				BlockTileRenderer.RenderInfo value = keyValuePair.Value;
				for (int i = vector2I.y; i < vector2I2.y; i++)
				{
					for (int j = vector2I.x; j < vector2I2.x; j++)
					{
						value.Rebuild(this, j, i, MeshUtil.vertices, MeshUtil.uvs, MeshUtil.indices, MeshUtil.colours);
						value.Render(j, i);
					}
				}
			}
		}

		// Token: 0x06006817 RID: 26647 RVA: 0x00276138 File Offset: 0x00274338
		public Color GetCellColour(int cell, SimHashes element)
		{
			Color white;
			if (cell == this.selectedCell)
			{
				white = this.selectColour;
			}
			else if (cell == this.invalidPlaceCell && element == SimHashes.Void)
			{
				white = this.invalidPlaceColour;
			}
			else if (cell == this.highlightCell)
			{
				white = this.highlightColour;
			}
			else
			{
				white = Color.white;
			}
			return white;
		}

		// Token: 0x06006818 RID: 26648 RVA: 0x0027618C File Offset: 0x0027438C
		public static Vector2I GetChunkIdx(int cell)
		{
			Vector2I vector2I = Grid.CellToXY(cell);
			return new Vector2I(vector2I.x / 16, vector2I.y / 16);
		}

		// Token: 0x06006819 RID: 26649 RVA: 0x002761B8 File Offset: 0x002743B8
		public void AddBlock(int renderLayer, BuildingDef def, bool isReplacement, SimHashes element, int cell)
		{
			KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer> key = new KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>(def, BlockTileRenderer.GetRenderInfoLayer(isReplacement, element));
			BlockTileRenderer.RenderInfo renderInfo;
			if (!this.renderInfo.TryGetValue(key, out renderInfo))
			{
				int queryLayer = (int)(isReplacement ? def.ReplacementLayer : def.TileLayer);
				renderInfo = new BlockTileRenderer.RenderInfo(this, queryLayer, renderLayer, def, element);
				this.renderInfo[key] = renderInfo;
			}
			renderInfo.AddCell(cell);
		}

		// Token: 0x0600681A RID: 26650 RVA: 0x00276218 File Offset: 0x00274418
		public void RemoveBlock(BuildingDef def, bool isReplacement, SimHashes element, int cell)
		{
			KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer> key = new KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>(def, BlockTileRenderer.GetRenderInfoLayer(isReplacement, element));
			BlockTileRenderer.RenderInfo renderInfo;
			if (this.renderInfo.TryGetValue(key, out renderInfo))
			{
				renderInfo.RemoveCell(cell);
			}
		}

		// Token: 0x0600681B RID: 26651 RVA: 0x0027624C File Offset: 0x0027444C
		public void Rebuild(ObjectLayer layer, int cell)
		{
			foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
			{
				if (keyValuePair.Key.Key.TileLayer == layer)
				{
					keyValuePair.Value.MarkDirty(cell);
				}
			}
		}

		// Token: 0x0600681C RID: 26652 RVA: 0x002762BC File Offset: 0x002744BC
		public void SelectCell(int cell, bool enabled)
		{
			this.UpdateCellStatus(ref this.selectedCell, cell, enabled);
		}

		// Token: 0x0600681D RID: 26653 RVA: 0x002762CC File Offset: 0x002744CC
		public void HighlightCell(int cell, bool enabled)
		{
			this.UpdateCellStatus(ref this.highlightCell, cell, enabled);
		}

		// Token: 0x0600681E RID: 26654 RVA: 0x002762DC File Offset: 0x002744DC
		public void SetInvalidPlaceCell(int cell, bool enabled)
		{
			this.UpdateCellStatus(ref this.invalidPlaceCell, cell, enabled);
		}

		// Token: 0x0600681F RID: 26655 RVA: 0x002762EC File Offset: 0x002744EC
		private void UpdateCellStatus(ref int cell_status, int cell, bool enabled)
		{
			if (enabled)
			{
				if (cell == cell_status)
				{
					return;
				}
				if (cell_status != -1)
				{
					foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
					{
						keyValuePair.Value.MarkDirtyIfOccupied(cell_status);
					}
				}
				cell_status = cell;
				using (Dictionary<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo>.Enumerator enumerator = this.renderInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair2 = enumerator.Current;
						keyValuePair2.Value.MarkDirtyIfOccupied(cell_status);
					}
					return;
				}
			}
			if (cell_status == cell)
			{
				foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair3 in this.renderInfo)
				{
					keyValuePair3.Value.MarkDirty(cell_status);
				}
				cell_status = -1;
			}
		}

		// Token: 0x040047C2 RID: 18370
		[SerializeField]
		private bool forceRebuild;

		// Token: 0x040047C3 RID: 18371
		[SerializeField]
		private Color highlightColour = new Color(1.25f, 1.25f, 1.25f, 1f);

		// Token: 0x040047C4 RID: 18372
		[SerializeField]
		private Color selectColour = new Color(1.5f, 1.5f, 1.5f, 1f);

		// Token: 0x040047C5 RID: 18373
		[SerializeField]
		private Color invalidPlaceColour = Color.red;

		// Token: 0x040047C6 RID: 18374
		private const float TILE_ATLAS_WIDTH = 2048f;

		// Token: 0x040047C7 RID: 18375
		private const float TILE_ATLAS_HEIGHT = 2048f;

		// Token: 0x040047C8 RID: 18376
		private const int chunkEdgeSize = 16;

		// Token: 0x040047C9 RID: 18377
		protected Dictionary<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> renderInfo = new Dictionary<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo>();

		// Token: 0x040047CA RID: 18378
		private int selectedCell = -1;

		// Token: 0x040047CB RID: 18379
		private int highlightCell = -1;

		// Token: 0x040047CC RID: 18380
		private int invalidPlaceCell = -1;

		// Token: 0x02001C05 RID: 7173
		public enum RenderInfoLayer
		{
			// Token: 0x04007E89 RID: 32393
			Built,
			// Token: 0x04007E8A RID: 32394
			UnderConstruction,
			// Token: 0x04007E8B RID: 32395
			Replacement
		}

		// Token: 0x02001C06 RID: 7174
		[Flags]
		public enum Bits
		{
			// Token: 0x04007E8D RID: 32397
			UpLeft = 128,
			// Token: 0x04007E8E RID: 32398
			Up = 64,
			// Token: 0x04007E8F RID: 32399
			UpRight = 32,
			// Token: 0x04007E90 RID: 32400
			Left = 16,
			// Token: 0x04007E91 RID: 32401
			Right = 8,
			// Token: 0x04007E92 RID: 32402
			DownLeft = 4,
			// Token: 0x04007E93 RID: 32403
			Down = 2,
			// Token: 0x04007E94 RID: 32404
			DownRight = 1
		}

		// Token: 0x02001C07 RID: 7175
		protected class RenderInfo
		{
			// Token: 0x06009B77 RID: 39799 RVA: 0x00348D60 File Offset: 0x00346F60
			public RenderInfo(BlockTileRenderer renderer, int queryLayer, int renderLayer, BuildingDef def, SimHashes element)
			{
				this.queryLayer = queryLayer;
				this.renderLayer = renderLayer;
				this.rootPosition = new Vector3(0f, 0f, Grid.GetLayerZ(def.SceneLayer));
				this.element = element;
				this.material = new Material(def.BlockTileMaterial);
				if (def.BlockTileIsTransparent)
				{
					this.material.renderQueue = RenderQueues.Liquid;
					this.decorZOffset = Grid.GetLayerZ(Grid.SceneLayer.TileFront) - Grid.GetLayerZ(Grid.SceneLayer.Liquid) - 1f;
				}
				else if (def.SceneLayer == Grid.SceneLayer.TileMain)
				{
					this.material.renderQueue = RenderQueues.BlockTiles;
				}
				this.material.DisableKeyword("ENABLE_SHINE");
				if (element != SimHashes.Void)
				{
					this.material.SetTexture("_MainTex", def.BlockTileAtlas.texture);
					this.material.name = def.BlockTileAtlas.name + "Mat";
					if (def.BlockTileShineAtlas != null)
					{
						this.material.SetTexture("_SpecularTex", def.BlockTileShineAtlas.texture);
						this.material.EnableKeyword("ENABLE_SHINE");
					}
				}
				else
				{
					this.material.SetTexture("_MainTex", def.BlockTilePlaceAtlas.texture);
					this.material.name = def.BlockTilePlaceAtlas.name + "Mat";
				}
				int num = Grid.WidthInCells / 16 + 1;
				int num2 = Grid.HeightInCells / 16 + 1;
				this.meshChunks = new Mesh[num, num2];
				this.dirtyChunks = new bool[num, num2];
				for (int i = 0; i < num2; i++)
				{
					for (int j = 0; j < num; j++)
					{
						this.dirtyChunks[j, i] = true;
					}
				}
				BlockTileDecorInfo blockTileDecorInfo = (element == SimHashes.Void) ? def.DecorPlaceBlockTileInfo : def.DecorBlockTileInfo;
				if (blockTileDecorInfo)
				{
					this.decorRenderInfo = new BlockTileRenderer.DecorRenderInfo(num, num2, queryLayer, def, blockTileDecorInfo);
				}
				int num3 = def.BlockTileAtlas.items[0].name.Length - 4 - 8;
				int startIndex = num3 - 1 - 8;
				this.atlasInfo = new BlockTileRenderer.RenderInfo.AtlasInfo[def.BlockTileAtlas.items.Length];
				for (int k = 0; k < this.atlasInfo.Length; k++)
				{
					TextureAtlas.Item item = def.BlockTileAtlas.items[k];
					string value = item.name.Substring(startIndex, 8);
					string value2 = item.name.Substring(num3, 8);
					int requiredConnections = Convert.ToInt32(value, 2);
					int forbiddenConnections = Convert.ToInt32(value2, 2);
					this.atlasInfo[k].requiredConnections = (BlockTileRenderer.Bits)requiredConnections;
					this.atlasInfo[k].forbiddenConnections = (BlockTileRenderer.Bits)forbiddenConnections;
					this.atlasInfo[k].uvBox = item.uvBox;
					this.atlasInfo[k].name = item.name;
				}
				this.trimUVSize = new Vector2(0.03125f, 0.03125f);
			}

			// Token: 0x06009B78 RID: 39800 RVA: 0x0034909C File Offset: 0x0034729C
			public void FreeResources()
			{
				UnityEngine.Object.DestroyImmediate(this.material);
				this.material = null;
				this.atlasInfo = null;
				for (int i = 0; i < this.meshChunks.GetLength(0); i++)
				{
					for (int j = 0; j < this.meshChunks.GetLength(1); j++)
					{
						if (this.meshChunks[i, j] != null)
						{
							UnityEngine.Object.DestroyImmediate(this.meshChunks[i, j]);
							this.meshChunks[i, j] = null;
						}
					}
				}
				this.meshChunks = null;
				this.decorRenderInfo = null;
				this.occupiedCells.Clear();
			}

			// Token: 0x06009B79 RID: 39801 RVA: 0x00349140 File Offset: 0x00347340
			public void AddCell(int cell)
			{
				int num = 0;
				this.occupiedCells.TryGetValue(cell, out num);
				this.occupiedCells[cell] = num + 1;
				this.MarkDirty(cell);
			}

			// Token: 0x06009B7A RID: 39802 RVA: 0x00349174 File Offset: 0x00347374
			public void RemoveCell(int cell)
			{
				int num = 0;
				this.occupiedCells.TryGetValue(cell, out num);
				if (num > 1)
				{
					this.occupiedCells[cell] = num - 1;
				}
				else
				{
					this.occupiedCells.Remove(cell);
				}
				this.MarkDirty(cell);
			}

			// Token: 0x06009B7B RID: 39803 RVA: 0x003491BC File Offset: 0x003473BC
			public void MarkDirty(int cell)
			{
				Vector2I chunkIdx = BlockTileRenderer.GetChunkIdx(cell);
				this.dirtyChunks[chunkIdx.x, chunkIdx.y] = true;
			}

			// Token: 0x06009B7C RID: 39804 RVA: 0x003491E8 File Offset: 0x003473E8
			public void MarkDirtyIfOccupied(int cell)
			{
				if (this.occupiedCells.ContainsKey(cell))
				{
					this.MarkDirty(cell);
				}
			}

			// Token: 0x06009B7D RID: 39805 RVA: 0x00349200 File Offset: 0x00347400
			public void Render(int x, int y)
			{
				if (this.meshChunks[x, y] != null)
				{
					Graphics.DrawMesh(this.meshChunks[x, y], this.rootPosition, Quaternion.identity, this.material, this.renderLayer);
				}
				if (this.decorRenderInfo != null)
				{
					this.decorRenderInfo.Render(x, y, this.rootPosition - new Vector3(0f, 0f, 0.5f), this.renderLayer);
				}
			}

			// Token: 0x06009B7E RID: 39806 RVA: 0x00349288 File Offset: 0x00347488
			public void Rebuild(BlockTileRenderer renderer, int chunk_x, int chunk_y, List<Vector3> vertices, List<Vector2> uvs, List<int> indices, List<Color> colours)
			{
				if (!this.dirtyChunks[chunk_x, chunk_y] && !renderer.ForceRebuild)
				{
					return;
				}
				this.dirtyChunks[chunk_x, chunk_y] = false;
				vertices.Clear();
				uvs.Clear();
				indices.Clear();
				colours.Clear();
				for (int i = chunk_y * 16; i < chunk_y * 16 + 16; i++)
				{
					for (int j = chunk_x * 16; j < chunk_x * 16 + 16; j++)
					{
						int num = i * Grid.WidthInCells + j;
						if (this.occupiedCells.ContainsKey(num))
						{
							BlockTileRenderer.Bits connectionBits = renderer.GetConnectionBits(j, i, this.queryLayer);
							for (int k = 0; k < this.atlasInfo.Length; k++)
							{
								bool flag = (this.atlasInfo[k].requiredConnections & connectionBits) == this.atlasInfo[k].requiredConnections;
								bool flag2 = (this.atlasInfo[k].forbiddenConnections & connectionBits) > (BlockTileRenderer.Bits)0;
								if (flag && !flag2)
								{
									Color cellColour = renderer.GetCellColour(num, this.element);
									this.AddVertexInfo(this.atlasInfo[k], this.trimUVSize, j, i, connectionBits, cellColour, vertices, uvs, indices, colours);
									break;
								}
							}
						}
					}
				}
				Mesh mesh = this.meshChunks[chunk_x, chunk_y];
				if (vertices.Count > 0)
				{
					if (mesh == null)
					{
						mesh = new Mesh();
						mesh.name = "BlockTile";
						this.meshChunks[chunk_x, chunk_y] = mesh;
					}
					mesh.Clear();
					mesh.SetVertices(vertices);
					mesh.SetUVs(0, uvs);
					mesh.SetColors(colours);
					mesh.SetTriangles(indices, 0);
				}
				else if (mesh != null)
				{
					this.meshChunks[chunk_x, chunk_y] = null;
				}
				if (this.decorRenderInfo != null)
				{
					this.decorRenderInfo.Rebuild(renderer, this.occupiedCells, chunk_x, chunk_y, this.decorZOffset, 16, vertices, uvs, colours, indices, this.element);
				}
			}

			// Token: 0x06009B7F RID: 39807 RVA: 0x0034948C File Offset: 0x0034768C
			private void AddVertexInfo(BlockTileRenderer.RenderInfo.AtlasInfo atlas_info, Vector2 uv_trim_size, int x, int y, BlockTileRenderer.Bits connection_bits, Color color, List<Vector3> vertices, List<Vector2> uvs, List<int> indices, List<Color> colours)
			{
				Vector2 vector = new Vector2((float)x, (float)y);
				Vector2 vector2 = vector + new Vector2(1f, 1f);
				Vector2 vector3 = new Vector2(atlas_info.uvBox.x, atlas_info.uvBox.w);
				Vector2 vector4 = new Vector2(atlas_info.uvBox.z, atlas_info.uvBox.y);
				if ((connection_bits & BlockTileRenderer.Bits.Left) == (BlockTileRenderer.Bits)0)
				{
					vector.x -= 0.25f;
				}
				else
				{
					vector3.x += uv_trim_size.x;
				}
				if ((connection_bits & BlockTileRenderer.Bits.Right) == (BlockTileRenderer.Bits)0)
				{
					vector2.x += 0.25f;
				}
				else
				{
					vector4.x -= uv_trim_size.x;
				}
				if ((connection_bits & BlockTileRenderer.Bits.Up) == (BlockTileRenderer.Bits)0)
				{
					vector2.y += 0.25f;
				}
				else
				{
					vector4.y -= uv_trim_size.y;
				}
				if ((connection_bits & BlockTileRenderer.Bits.Down) == (BlockTileRenderer.Bits)0)
				{
					vector.y -= 0.25f;
				}
				else
				{
					vector3.y += uv_trim_size.y;
				}
				int count = vertices.Count;
				vertices.Add(vector);
				vertices.Add(new Vector2(vector2.x, vector.y));
				vertices.Add(vector2);
				vertices.Add(new Vector2(vector.x, vector2.y));
				uvs.Add(vector3);
				uvs.Add(new Vector2(vector4.x, vector3.y));
				uvs.Add(vector4);
				uvs.Add(new Vector2(vector3.x, vector4.y));
				indices.Add(count);
				indices.Add(count + 1);
				indices.Add(count + 2);
				indices.Add(count);
				indices.Add(count + 2);
				indices.Add(count + 3);
				colours.Add(color);
				colours.Add(color);
				colours.Add(color);
				colours.Add(color);
			}

			// Token: 0x04007E95 RID: 32405
			private BlockTileRenderer.RenderInfo.AtlasInfo[] atlasInfo;

			// Token: 0x04007E96 RID: 32406
			private bool[,] dirtyChunks;

			// Token: 0x04007E97 RID: 32407
			private int queryLayer;

			// Token: 0x04007E98 RID: 32408
			private Material material;

			// Token: 0x04007E99 RID: 32409
			private int renderLayer;

			// Token: 0x04007E9A RID: 32410
			private Mesh[,] meshChunks;

			// Token: 0x04007E9B RID: 32411
			private BlockTileRenderer.DecorRenderInfo decorRenderInfo;

			// Token: 0x04007E9C RID: 32412
			private Vector2 trimUVSize;

			// Token: 0x04007E9D RID: 32413
			private Vector3 rootPosition;

			// Token: 0x04007E9E RID: 32414
			private Dictionary<int, int> occupiedCells = new Dictionary<int, int>();

			// Token: 0x04007E9F RID: 32415
			private SimHashes element;

			// Token: 0x04007EA0 RID: 32416
			private float decorZOffset = -1f;

			// Token: 0x04007EA1 RID: 32417
			private const float scale = 0.5f;

			// Token: 0x04007EA2 RID: 32418
			private const float core_size = 256f;

			// Token: 0x04007EA3 RID: 32419
			private const float trim_size = 64f;

			// Token: 0x04007EA4 RID: 32420
			private const float cell_size = 1f;

			// Token: 0x04007EA5 RID: 32421
			private const float world_trim_size = 0.25f;

			// Token: 0x0200223B RID: 8763
			private struct AtlasInfo
			{
				// Token: 0x040098F7 RID: 39159
				public BlockTileRenderer.Bits requiredConnections;

				// Token: 0x040098F8 RID: 39160
				public BlockTileRenderer.Bits forbiddenConnections;

				// Token: 0x040098F9 RID: 39161
				public Vector4 uvBox;

				// Token: 0x040098FA RID: 39162
				public string name;
			}
		}

		// Token: 0x02001C08 RID: 7176
		private class DecorRenderInfo
		{
			// Token: 0x06009B80 RID: 39808 RVA: 0x0034969C File Offset: 0x0034789C
			public DecorRenderInfo(int num_x_chunks, int num_y_chunks, int query_layer, BuildingDef def, BlockTileDecorInfo decorInfo)
			{
				this.decorInfo = decorInfo;
				this.queryLayer = query_layer;
				this.material = new Material(def.BlockTileMaterial);
				if (def.BlockTileIsTransparent)
				{
					this.material.renderQueue = RenderQueues.Liquid;
				}
				else if (def.SceneLayer == Grid.SceneLayer.TileMain)
				{
					this.material.renderQueue = RenderQueues.BlockTiles;
				}
				this.material.SetTexture("_MainTex", decorInfo.atlas.texture);
				if (decorInfo.atlasSpec != null)
				{
					this.material.SetTexture("_SpecularTex", decorInfo.atlasSpec.texture);
					this.material.EnableKeyword("ENABLE_SHINE");
				}
				else
				{
					this.material.DisableKeyword("ENABLE_SHINE");
				}
				this.meshChunks = new Mesh[num_x_chunks, num_y_chunks];
			}

			// Token: 0x06009B81 RID: 39809 RVA: 0x00349788 File Offset: 0x00347988
			public void FreeResources()
			{
				this.decorInfo = null;
				UnityEngine.Object.DestroyImmediate(this.material);
				this.material = null;
				for (int i = 0; i < this.meshChunks.GetLength(0); i++)
				{
					for (int j = 0; j < this.meshChunks.GetLength(1); j++)
					{
						if (this.meshChunks[i, j] != null)
						{
							UnityEngine.Object.DestroyImmediate(this.meshChunks[i, j]);
							this.meshChunks[i, j] = null;
						}
					}
				}
				this.meshChunks = null;
				this.triangles.Clear();
			}

			// Token: 0x06009B82 RID: 39810 RVA: 0x00349823 File Offset: 0x00347A23
			public void Render(int x, int y, Vector3 position, int renderLayer)
			{
				if (this.meshChunks[x, y] != null)
				{
					Graphics.DrawMesh(this.meshChunks[x, y], position, Quaternion.identity, this.material, renderLayer);
				}
			}

			// Token: 0x06009B83 RID: 39811 RVA: 0x0034985C File Offset: 0x00347A5C
			public void Rebuild(BlockTileRenderer renderer, Dictionary<int, int> occupiedCells, int chunk_x, int chunk_y, float z_offset, int chunkEdgeSize, List<Vector3> vertices, List<Vector2> uvs, List<Color> colours, List<int> indices, SimHashes element)
			{
				vertices.Clear();
				uvs.Clear();
				this.triangles.Clear();
				colours.Clear();
				indices.Clear();
				for (int i = chunk_y * chunkEdgeSize; i < chunk_y * chunkEdgeSize + chunkEdgeSize; i++)
				{
					for (int j = chunk_x * chunkEdgeSize; j < chunk_x * chunkEdgeSize + chunkEdgeSize; j++)
					{
						int num = i * Grid.WidthInCells + j;
						if (occupiedCells.ContainsKey(num))
						{
							Color cellColour = renderer.GetCellColour(num, element);
							BlockTileRenderer.Bits decorConnectionBits = renderer.GetDecorConnectionBits(j, i, this.queryLayer);
							this.AddDecor(j, i, z_offset, decorConnectionBits, cellColour, vertices, uvs, this.triangles, colours);
						}
					}
				}
				if (vertices.Count > 0)
				{
					Mesh mesh = this.meshChunks[chunk_x, chunk_y];
					if (mesh == null)
					{
						mesh = new Mesh();
						mesh.name = "DecorRender";
						this.meshChunks[chunk_x, chunk_y] = mesh;
					}
					this.triangles.Sort((BlockTileRenderer.DecorRenderInfo.TriangleInfo a, BlockTileRenderer.DecorRenderInfo.TriangleInfo b) => a.sortOrder.CompareTo(b.sortOrder));
					for (int k = 0; k < this.triangles.Count; k++)
					{
						indices.Add(this.triangles[k].i0);
						indices.Add(this.triangles[k].i1);
						indices.Add(this.triangles[k].i2);
					}
					mesh.Clear();
					mesh.SetVertices(vertices);
					mesh.SetUVs(0, uvs);
					mesh.SetColors(colours);
					mesh.SetTriangles(indices, 0);
					return;
				}
				this.meshChunks[chunk_x, chunk_y] = null;
			}

			// Token: 0x06009B84 RID: 39812 RVA: 0x00349A18 File Offset: 0x00347C18
			private void AddDecor(int x, int y, float z_offset, BlockTileRenderer.Bits connection_bits, Color colour, List<Vector3> vertices, List<Vector2> uvs, List<BlockTileRenderer.DecorRenderInfo.TriangleInfo> triangles, List<Color> colours)
			{
				for (int i = 0; i < this.decorInfo.decor.Length; i++)
				{
					BlockTileDecorInfo.Decor decor = this.decorInfo.decor[i];
					if (decor.variants != null && decor.variants.Length != 0)
					{
						bool flag = (connection_bits & decor.requiredConnections) == decor.requiredConnections;
						bool flag2 = (connection_bits & decor.forbiddenConnections) > (BlockTileRenderer.Bits)0;
						if (flag && !flag2)
						{
							float num = PerlinSimplexNoise.noise((float)(i + x + connection_bits) * BlockTileRenderer.DecorRenderInfo.simplex_scale.x, (float)(i + y + connection_bits) * BlockTileRenderer.DecorRenderInfo.simplex_scale.y);
							if (num >= decor.probabilityCutoff)
							{
								int num2 = (int)((float)(decor.variants.Length - 1) * num);
								int count = vertices.Count;
								Vector3 b = new Vector3((float)x, (float)y, z_offset) + decor.variants[num2].offset;
								foreach (Vector3 a in decor.variants[num2].atlasItem.vertices)
								{
									vertices.Add(a + b);
									colours.Add(colour);
								}
								uvs.AddRange(decor.variants[num2].atlasItem.uvs);
								int[] indices = decor.variants[num2].atlasItem.indices;
								for (int k = 0; k < indices.Length; k += 3)
								{
									triangles.Add(new BlockTileRenderer.DecorRenderInfo.TriangleInfo
									{
										sortOrder = decor.sortOrder,
										i0 = indices[k] + count,
										i1 = indices[k + 1] + count,
										i2 = indices[k + 2] + count
									});
								}
							}
						}
					}
				}
			}

			// Token: 0x04007EA6 RID: 32422
			private int queryLayer;

			// Token: 0x04007EA7 RID: 32423
			private BlockTileDecorInfo decorInfo;

			// Token: 0x04007EA8 RID: 32424
			private Mesh[,] meshChunks;

			// Token: 0x04007EA9 RID: 32425
			private Material material;

			// Token: 0x04007EAA RID: 32426
			private List<BlockTileRenderer.DecorRenderInfo.TriangleInfo> triangles = new List<BlockTileRenderer.DecorRenderInfo.TriangleInfo>();

			// Token: 0x04007EAB RID: 32427
			private static Vector2 simplex_scale = new Vector2(92.41f, 87.16f);

			// Token: 0x0200223C RID: 8764
			private struct TriangleInfo
			{
				// Token: 0x040098FB RID: 39163
				public int sortOrder;

				// Token: 0x040098FC RID: 39164
				public int i0;

				// Token: 0x040098FD RID: 39165
				public int i1;

				// Token: 0x040098FE RID: 39166
				public int i2;
			}
		}
	}
}
