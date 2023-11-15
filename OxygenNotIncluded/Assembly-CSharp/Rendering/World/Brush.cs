using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000CB6 RID: 3254
	public class Brush
	{
		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06006820 RID: 26656 RVA: 0x002763F8 File Offset: 0x002745F8
		// (set) Token: 0x06006821 RID: 26657 RVA: 0x00276400 File Offset: 0x00274600
		public int Id { get; private set; }

		// Token: 0x06006822 RID: 26658 RVA: 0x0027640C File Offset: 0x0027460C
		public Brush(int id, string name, Material material, Mask mask, List<Brush> active_brushes, List<Brush> dirty_brushes, int width_in_tiles, MaterialPropertyBlock property_block)
		{
			this.Id = id;
			this.material = material;
			this.mask = mask;
			this.mesh = new DynamicMesh(name, new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0f)));
			this.activeBrushes = active_brushes;
			this.dirtyBrushes = dirty_brushes;
			this.layer = LayerMask.NameToLayer("World");
			this.widthInTiles = width_in_tiles;
			this.propertyBlock = property_block;
		}

		// Token: 0x06006823 RID: 26659 RVA: 0x0027649A File Offset: 0x0027469A
		public void Add(int tile_idx)
		{
			this.tiles.Add(tile_idx);
			if (!this.dirty)
			{
				this.dirtyBrushes.Add(this);
				this.dirty = true;
			}
		}

		// Token: 0x06006824 RID: 26660 RVA: 0x002764C4 File Offset: 0x002746C4
		public void Remove(int tile_idx)
		{
			this.tiles.Remove(tile_idx);
			if (!this.dirty)
			{
				this.dirtyBrushes.Add(this);
				this.dirty = true;
			}
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x002764EE File Offset: 0x002746EE
		public void SetMaskOffset(int offset)
		{
			this.mask.SetOffset(offset);
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x002764FC File Offset: 0x002746FC
		public void Refresh()
		{
			bool flag = this.mesh.Meshes.Length != 0;
			int count = this.tiles.Count;
			int vertex_count = count * 4;
			int triangle_count = count * 6;
			this.mesh.Reserve(vertex_count, triangle_count);
			if (this.mesh.SetTriangles)
			{
				int num = 0;
				for (int i = 0; i < count; i++)
				{
					this.mesh.AddTriangle(num);
					this.mesh.AddTriangle(2 + num);
					this.mesh.AddTriangle(1 + num);
					this.mesh.AddTriangle(1 + num);
					this.mesh.AddTriangle(2 + num);
					this.mesh.AddTriangle(3 + num);
					num += 4;
				}
			}
			foreach (int num2 in this.tiles)
			{
				float num3 = (float)(num2 % this.widthInTiles);
				float num4 = (float)(num2 / this.widthInTiles);
				float z = 0f;
				this.mesh.AddVertex(new Vector3(num3 - 0.5f, num4 - 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 + 0.5f, num4 - 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 - 0.5f, num4 + 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 + 0.5f, num4 + 0.5f, z));
			}
			if (this.mesh.SetUVs)
			{
				for (int j = 0; j < count; j++)
				{
					this.mesh.AddUV(this.mask.UV0);
					this.mesh.AddUV(this.mask.UV1);
					this.mesh.AddUV(this.mask.UV2);
					this.mesh.AddUV(this.mask.UV3);
				}
			}
			this.dirty = false;
			this.mesh.Commit();
			if (this.mesh.Meshes.Length != 0)
			{
				if (!flag)
				{
					this.activeBrushes.Add(this);
					return;
				}
			}
			else if (flag)
			{
				this.activeBrushes.Remove(this);
			}
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x00276758 File Offset: 0x00274958
		public void Render()
		{
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Ground));
			this.mesh.Render(position, Quaternion.identity, this.material, this.layer, this.propertyBlock);
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x002767A0 File Offset: 0x002749A0
		public void SetMaterial(Material material, MaterialPropertyBlock property_block)
		{
			this.material = material;
			this.propertyBlock = property_block;
		}

		// Token: 0x040047CE RID: 18382
		private bool dirty;

		// Token: 0x040047CF RID: 18383
		private Material material;

		// Token: 0x040047D0 RID: 18384
		private int layer;

		// Token: 0x040047D1 RID: 18385
		private HashSet<int> tiles = new HashSet<int>();

		// Token: 0x040047D2 RID: 18386
		private List<Brush> activeBrushes;

		// Token: 0x040047D3 RID: 18387
		private List<Brush> dirtyBrushes;

		// Token: 0x040047D4 RID: 18388
		private int widthInTiles;

		// Token: 0x040047D5 RID: 18389
		private Mask mask;

		// Token: 0x040047D6 RID: 18390
		private DynamicMesh mesh;

		// Token: 0x040047D7 RID: 18391
		private MaterialPropertyBlock propertyBlock;
	}
}
