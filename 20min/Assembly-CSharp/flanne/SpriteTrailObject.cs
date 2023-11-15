using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200011B RID: 283
	public class SpriteTrailObject : MonoBehaviour
	{
		// Token: 0x060007CE RID: 1998 RVA: 0x000216D0 File Offset: 0x0001F8D0
		private void Start()
		{
			this.mRenderer.enabled = false;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x000216E0 File Offset: 0x0001F8E0
		private void Update()
		{
			if (this.mbInUse)
			{
				base.transform.position = this.mPosition;
				this.mTimeDisplayed += Time.deltaTime;
				this.mRenderer.color = Color.Lerp(this.mStartColor, this.mEndColor, this.mTimeDisplayed / this.mDisplayTime);
				if (this.mTimeDisplayed >= this.mDisplayTime)
				{
					this.mSpawner.RemoveTrailObject(base.gameObject);
					this.mbInUse = false;
					this.mRenderer.enabled = false;
				}
			}
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002177C File Offset: 0x0001F97C
		public void Initiate(float displayTime, Sprite sprite, Vector2 position, SpriteTrail trail)
		{
			this.mDisplayTime = displayTime;
			this.mRenderer.sprite = sprite;
			this.mRenderer.enabled = true;
			this.mPosition = position;
			this.mTimeDisplayed = 0f;
			this.mSpawner = trail;
			this.mbInUse = true;
		}

		// Token: 0x040005AA RID: 1450
		public SpriteRenderer mRenderer;

		// Token: 0x040005AB RID: 1451
		public Color mStartColor;

		// Token: 0x040005AC RID: 1452
		public Color mEndColor;

		// Token: 0x040005AD RID: 1453
		private bool mbInUse;

		// Token: 0x040005AE RID: 1454
		private Vector2 mPosition;

		// Token: 0x040005AF RID: 1455
		private float mDisplayTime;

		// Token: 0x040005B0 RID: 1456
		private float mTimeDisplayed;

		// Token: 0x040005B1 RID: 1457
		private SpriteTrail mSpawner;
	}
}
