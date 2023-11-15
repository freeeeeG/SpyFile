using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000118 RID: 280
	public class SpriteFX : MonoBehaviour
	{
		// Token: 0x060007C1 RID: 1985 RVA: 0x000214FC File Offset: 0x0001F6FC
		private void OnEnable()
		{
			this._coroutine = this.Play();
			base.StartCoroutine(this._coroutine);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00021517 File Offset: 0x0001F717
		private void OnDisable()
		{
			base.StopCoroutine(this._coroutine);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00021525 File Offset: 0x0001F725
		private IEnumerator Play()
		{
			int num;
			for (int i = 0; i < this.sprites.Length; i = num + 1)
			{
				this.spriteRenderer.sprite = this.sprites[i];
				yield return new WaitForSeconds(this.secPerFrame);
				num = i;
			}
			while (this.loop)
			{
				for (int i = 0; i < this.sprites.Length; i = num + 1)
				{
					this.spriteRenderer.sprite = this.sprites[i];
					yield return new WaitForSeconds(this.secPerFrame);
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x04000597 RID: 1431
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x04000598 RID: 1432
		[SerializeField]
		private float secPerFrame;

		// Token: 0x04000599 RID: 1433
		[SerializeField]
		private bool loop;

		// Token: 0x0400059A RID: 1434
		[SerializeField]
		private Sprite[] sprites;

		// Token: 0x0400059B RID: 1435
		private IEnumerator _coroutine;
	}
}
