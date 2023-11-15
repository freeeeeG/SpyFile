using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000C3 RID: 195
	public class LineAnimator : MonoBehaviour
	{
		// Token: 0x06000640 RID: 1600 RVA: 0x0001CB13 File Offset: 0x0001AD13
		private void Start()
		{
			base.StartCoroutine(this.PlayCR());
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001CB22 File Offset: 0x0001AD22
		private IEnumerator PlayCR()
		{
			int num;
			for (int i = 0; i < this.textures.Length; i = num + 1)
			{
				this.lineRenderer.material.SetTexture("_MainTex", this.textures[i]);
				yield return new WaitForSeconds(this.secPerFrame);
				num = i;
			}
			while (this.loop)
			{
				for (int i = 0; i < this.textures.Length; i = num + 1)
				{
					this.lineRenderer.material.SetTexture("_MainTex", this.textures[i]);
					yield return new WaitForSeconds(this.secPerFrame);
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x04000404 RID: 1028
		[SerializeField]
		private LineRenderer lineRenderer;

		// Token: 0x04000405 RID: 1029
		[SerializeField]
		private Texture[] textures;

		// Token: 0x04000406 RID: 1030
		[SerializeField]
		private float secPerFrame;

		// Token: 0x04000407 RID: 1031
		[SerializeField]
		private bool loop;
	}
}
