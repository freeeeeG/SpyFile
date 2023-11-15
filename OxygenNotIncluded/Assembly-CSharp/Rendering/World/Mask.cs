using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000CBA RID: 3258
	public struct Mask
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x0600683B RID: 26683 RVA: 0x00276C77 File Offset: 0x00274E77
		// (set) Token: 0x0600683C RID: 26684 RVA: 0x00276C7F File Offset: 0x00274E7F
		public Vector2 UV0 { readonly get; private set; }

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600683D RID: 26685 RVA: 0x00276C88 File Offset: 0x00274E88
		// (set) Token: 0x0600683E RID: 26686 RVA: 0x00276C90 File Offset: 0x00274E90
		public Vector2 UV1 { readonly get; private set; }

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x0600683F RID: 26687 RVA: 0x00276C99 File Offset: 0x00274E99
		// (set) Token: 0x06006840 RID: 26688 RVA: 0x00276CA1 File Offset: 0x00274EA1
		public Vector2 UV2 { readonly get; private set; }

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06006841 RID: 26689 RVA: 0x00276CAA File Offset: 0x00274EAA
		// (set) Token: 0x06006842 RID: 26690 RVA: 0x00276CB2 File Offset: 0x00274EB2
		public Vector2 UV3 { readonly get; private set; }

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06006843 RID: 26691 RVA: 0x00276CBB File Offset: 0x00274EBB
		// (set) Token: 0x06006844 RID: 26692 RVA: 0x00276CC3 File Offset: 0x00274EC3
		public bool IsOpaque { readonly get; private set; }

		// Token: 0x06006845 RID: 26693 RVA: 0x00276CCC File Offset: 0x00274ECC
		public Mask(TextureAtlas atlas, int texture_idx, bool transpose, bool flip_x, bool flip_y, bool is_opaque)
		{
			this = default(Mask);
			this.atlas = atlas;
			this.texture_idx = texture_idx;
			this.transpose = transpose;
			this.flip_x = flip_x;
			this.flip_y = flip_y;
			this.atlas_offset = 0;
			this.IsOpaque = is_opaque;
			this.Refresh();
		}

		// Token: 0x06006846 RID: 26694 RVA: 0x00276D1A File Offset: 0x00274F1A
		public void SetOffset(int offset)
		{
			this.atlas_offset = offset;
			this.Refresh();
		}

		// Token: 0x06006847 RID: 26695 RVA: 0x00276D2C File Offset: 0x00274F2C
		public void Refresh()
		{
			int num = this.atlas_offset * 4 + this.atlas_offset;
			if (num + this.texture_idx >= this.atlas.items.Length)
			{
				num = 0;
			}
			Vector4 uvBox = this.atlas.items[num + this.texture_idx].uvBox;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 zero3 = Vector2.zero;
			Vector2 zero4 = Vector2.zero;
			if (this.transpose)
			{
				float x = uvBox.x;
				float x2 = uvBox.z;
				if (this.flip_x)
				{
					x = uvBox.z;
					x2 = uvBox.x;
				}
				zero.x = x;
				zero2.x = x;
				zero3.x = x2;
				zero4.x = x2;
				float y = uvBox.y;
				float y2 = uvBox.w;
				if (this.flip_y)
				{
					y = uvBox.w;
					y2 = uvBox.y;
				}
				zero.y = y;
				zero2.y = y2;
				zero3.y = y;
				zero4.y = y2;
			}
			else
			{
				float x3 = uvBox.x;
				float x4 = uvBox.z;
				if (this.flip_x)
				{
					x3 = uvBox.z;
					x4 = uvBox.x;
				}
				zero.x = x3;
				zero2.x = x4;
				zero3.x = x3;
				zero4.x = x4;
				float y3 = uvBox.y;
				float y4 = uvBox.w;
				if (this.flip_y)
				{
					y3 = uvBox.w;
					y4 = uvBox.y;
				}
				zero.y = y4;
				zero2.y = y4;
				zero3.y = y3;
				zero4.y = y3;
			}
			this.UV0 = zero;
			this.UV1 = zero2;
			this.UV2 = zero3;
			this.UV3 = zero4;
		}

		// Token: 0x040047FD RID: 18429
		private TextureAtlas atlas;

		// Token: 0x040047FE RID: 18430
		private int texture_idx;

		// Token: 0x040047FF RID: 18431
		private bool transpose;

		// Token: 0x04004800 RID: 18432
		private bool flip_x;

		// Token: 0x04004801 RID: 18433
		private bool flip_y;

		// Token: 0x04004802 RID: 18434
		private int atlas_offset;

		// Token: 0x04004803 RID: 18435
		private const int TILES_PER_SET = 4;
	}
}
