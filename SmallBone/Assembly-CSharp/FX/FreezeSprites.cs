using System;
using Singletons;
using UnityEngine;

namespace FX
{
	// Token: 0x02000239 RID: 569
	public class FreezeSprites : MonoBehaviour
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000B2D RID: 2861 RVA: 0x0001F0F9 File Offset: 0x0001D2F9
		public Vector2Int size
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0001F101 File Offset: 0x0001D301
		public void Initialize(SpriteRenderer spriteRenderer, int multiplier)
		{
			this.SetLayer(spriteRenderer.sortingLayerID, spriteRenderer.sortingOrder);
			base.transform.localScale = Vector3.one * (float)multiplier;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0001F12C File Offset: 0x0001D32C
		public void SetLayer(int sortingLayerID, int sortingOrder)
		{
			this._front.sortingLayerID = sortingLayerID;
			this._front.sortingOrder = sortingOrder + 1;
			this._back.sortingLayerID = sortingLayerID;
			this._back.sortingOrder = sortingOrder - 1;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0001F162 File Offset: 0x0001D362
		public void Enable()
		{
			base.gameObject.SetActive(true);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._freezeSound, base.transform.position);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x000075E7 File Offset: 0x000057E7
		public void Disable()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0400095A RID: 2394
		[SerializeField]
		private SpriteRenderer _front;

		// Token: 0x0400095B RID: 2395
		[SerializeField]
		private SpriteRenderer _back;

		// Token: 0x0400095C RID: 2396
		[SerializeField]
		private Vector2Int _size;

		// Token: 0x0400095D RID: 2397
		[SerializeField]
		protected SoundInfo _freezeSound;
	}
}
