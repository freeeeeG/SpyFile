using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000119 RID: 281
	public class SpriteFXRealtime : MonoBehaviour
	{
		// Token: 0x060007C5 RID: 1989 RVA: 0x00021534 File Offset: 0x0001F734
		private void OnEnable()
		{
			this._coroutine = this.Play();
			base.StartCoroutine(this._coroutine);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0002154F File Offset: 0x0001F74F
		private void OnDisable()
		{
			base.StopCoroutine(this._coroutine);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0002155D File Offset: 0x0001F75D
		private IEnumerator Play()
		{
			int num;
			for (int i = 0; i < this.sprites.Length; i = num + 1)
			{
				this.spriteRenderer.sprite = this.sprites[i];
				yield return new WaitForSecondsRealtime(this.secPerFrame);
				num = i;
			}
			while (this.loop)
			{
				for (int i = 0; i < this.sprites.Length; i = num + 1)
				{
					this.spriteRenderer.sprite = this.sprites[i];
					yield return new WaitForSecondsRealtime(this.secPerFrame);
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x0400059C RID: 1436
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x0400059D RID: 1437
		[SerializeField]
		private float secPerFrame;

		// Token: 0x0400059E RID: 1438
		[SerializeField]
		private bool loop;

		// Token: 0x0400059F RID: 1439
		[SerializeField]
		private Sprite[] sprites;

		// Token: 0x040005A0 RID: 1440
		private IEnumerator _coroutine;
	}
}
