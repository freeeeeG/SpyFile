using System;
using UnityEngine;

namespace FX
{
	// Token: 0x0200025D RID: 605
	public struct SpriteRendererValues
	{
		// Token: 0x06000BE6 RID: 3046 RVA: 0x00020A84 File Offset: 0x0001EC84
		public SpriteRendererValues(Sprite sprite, Color color, bool flipX, bool flipY, Material sharedMaterial, SpriteDrawMode drawMode, int sortingLayerID, int sortingOrder, SpriteMaskInteraction maskInteraction, SpriteSortPoint spriteSortPoint, uint renderingLayerMask)
		{
			this.sprite = sprite;
			this.color = color;
			this.flipX = flipX;
			this.flipY = flipY;
			this.sharedMaterial = sharedMaterial;
			this.drawMode = drawMode;
			this.sortingLayerID = sortingLayerID;
			this.sortingOrder = sortingOrder;
			this.maskInteraction = maskInteraction;
			this.spriteSortPoint = spriteSortPoint;
			this.renderingLayerMask = renderingLayerMask;
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00020AE8 File Offset: 0x0001ECE8
		public SpriteRendererValues(SpriteRenderer from)
		{
			this.sprite = from.sprite;
			this.color = from.color;
			this.flipX = from.flipX;
			this.flipY = from.flipY;
			this.sharedMaterial = from.sharedMaterial;
			this.drawMode = from.drawMode;
			this.sortingLayerID = from.sortingLayerID;
			this.sortingOrder = from.sortingOrder;
			this.maskInteraction = from.maskInteraction;
			this.spriteSortPoint = from.spriteSortPoint;
			this.renderingLayerMask = from.renderingLayerMask;
		}

		// Token: 0x040009E4 RID: 2532
		public static readonly SpriteRendererValues @default = new SpriteRendererValues(Effects.sprite.spriteRenderer);

		// Token: 0x040009E5 RID: 2533
		public Sprite sprite;

		// Token: 0x040009E6 RID: 2534
		public Color color;

		// Token: 0x040009E7 RID: 2535
		public bool flipX;

		// Token: 0x040009E8 RID: 2536
		public bool flipY;

		// Token: 0x040009E9 RID: 2537
		public Material sharedMaterial;

		// Token: 0x040009EA RID: 2538
		public SpriteDrawMode drawMode;

		// Token: 0x040009EB RID: 2539
		public int sortingLayerID;

		// Token: 0x040009EC RID: 2540
		public int sortingOrder;

		// Token: 0x040009ED RID: 2541
		public SpriteMaskInteraction maskInteraction;

		// Token: 0x040009EE RID: 2542
		public SpriteSortPoint spriteSortPoint;

		// Token: 0x040009EF RID: 2543
		public uint renderingLayerMask;
	}
}
