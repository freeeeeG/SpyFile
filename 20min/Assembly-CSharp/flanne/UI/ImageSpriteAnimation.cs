using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000224 RID: 548
	public class ImageSpriteAnimation : MonoBehaviour
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x0002CAF3 File Offset: 0x0002ACF3
		private void OnEnable()
		{
			this._coroutine = this.Play();
			base.StartCoroutine(this._coroutine);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0002CB0E File Offset: 0x0002AD0E
		private void OnDisable()
		{
			base.StopCoroutine(this._coroutine);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0002CB1C File Offset: 0x0002AD1C
		private IEnumerator Play()
		{
			int num;
			for (int i = 0; i < this.sprites.Length; i = num + 1)
			{
				this.image.sprite = this.sprites[i];
				yield return new WaitForSecondsRealtime(this.secPerFrame);
				num = i;
			}
			while (this.isLooping)
			{
				yield return new WaitForSecondsRealtime(this.delayBetweenLoops);
				for (int i = 0; i < this.sprites.Length; i = num + 1)
				{
					this.image.sprite = this.sprites[i];
					yield return new WaitForSecondsRealtime(this.secPerFrame);
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x04000878 RID: 2168
		[SerializeField]
		private Image image;

		// Token: 0x04000879 RID: 2169
		[SerializeField]
		private float secPerFrame;

		// Token: 0x0400087A RID: 2170
		[SerializeField]
		private Sprite[] sprites;

		// Token: 0x0400087B RID: 2171
		[SerializeField]
		private bool isLooping;

		// Token: 0x0400087C RID: 2172
		[SerializeField]
		private float delayBetweenLoops;

		// Token: 0x0400087D RID: 2173
		private IEnumerator _coroutine;
	}
}
